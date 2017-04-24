using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Ultraviolet.Presentation.Styles
{
    /// <summary>
    /// Evaluates a trigger condition against the specified dependency object.
    /// </summary>
    /// <param name="dobj">The dependency object to evaluate.</param>
    /// <param name="dprop">The dependency property to evaluate.</param>
    /// <param name="refvalue">The comparison's reference value.</param>
    /// <returns>The result of the comparison operation.</returns>
    public delegate Boolean TriggerComparison(DependencyObject dobj, DependencyProperty dprop, Object refvalue);

    /// <summary>
    /// Contains cached type-specific instances of the <see cref="TriggerComparison"/> delegate.
    /// </summary>
    public static class TriggerComparisonCache
    {
        /// <summary>
        /// Initializes the <see cref="TriggerComparisonCache"/> type.
        /// </summary>
        static TriggerComparisonCache()
        {
            CreateComparisonDelegates(typeof(Boolean));
            CreateComparisonDelegates(typeof(Byte));
            CreateComparisonDelegates(typeof(Char));
            CreateComparisonDelegates(typeof(Int16));
            CreateComparisonDelegates(typeof(Int32));
            CreateComparisonDelegates(typeof(Int64));
            CreateComparisonDelegates(typeof(Single));
            CreateComparisonDelegates(typeof(Double));
        }

        /// <summary>
        /// Gets the cached delegate for the specified type. If the delegate does not exist in the cache, it
        /// will automatically be created when this method is called.
        /// </summary>
        /// <param name="type">The type for which to retrieve a comparison delegate.</param>
        /// <param name="op">A <see cref="TriggerComparisonOp"/> value which specifies the type of the comparison operation.</param>
        /// <returns>The cached <see cref="TriggerComparison"/> delegate for the specified type and operation, or <see langword="null"/>
        /// if the specified operation is not supported by the given type.</returns>
        public static TriggerComparison Get(Type type, TriggerComparisonOp op)
        {
            lock (cache)
            {
                Dictionary<Int32, TriggerComparison> cacheForType;
                if (!cache.TryGetValue(type, out cacheForType))
                {
                    cache[type] = cacheForType = new Dictionary<Int32, TriggerComparison>();
                    CreateComparisonDelegates(type);
                }

                TriggerComparison comparison;
                cacheForType.TryGetValue((Int32)op, out comparison);
                return comparison;
            }
        }

        /// <summary>
        /// Creates a full set of comparison delegates for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create comparison delegates.</param>
        private static void CreateComparisonDelegates(Type type)
        {
            var underlyingType = type;
            if (type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                underlyingType = type.GetGenericArguments()[0];
            }

            var miGetValue  = typeof(DependencyObject).GetMethod("GetValue").MakeGenericMethod(type);
            var miEquals    = underlyingType.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance, null, new[] { underlyingType }, null);
            var miCompareTo = underlyingType.GetMethod("CompareTo", BindingFlags.Public | BindingFlags.Instance, null, new[] { underlyingType }, null);

            if (miEquals == null && miCompareTo == null)
                throw new InvalidOperationException(PresentationStrings.TypeIsNotComparable.Format(type.Name));

            Dictionary<Int32, TriggerComparison> cacheForType;
            if (!cache.TryGetValue(type, out cacheForType))
                cache[type] = cacheForType = new Dictionary<Int32, TriggerComparison>();

            if (miEquals != null)
            {
                cacheForType[(Int32)TriggerComparisonOp.Equals]    = CreateEqualsComparison(type, miGetValue, miEquals);
                cacheForType[(Int32)TriggerComparisonOp.NotEquals] = CreateNotEqualsComparison(type, miGetValue, miEquals);
            }

            if (miCompareTo != null)
            {
                cacheForType[(Int32)TriggerComparisonOp.GreaterThan]          = CreateGreaterThanComparison(type, miGetValue, miCompareTo);
                cacheForType[(Int32)TriggerComparisonOp.GreaterThanOrEqualTo] = CreateGreaterThanEqualsComparison(type, miGetValue, miCompareTo);
                cacheForType[(Int32)TriggerComparisonOp.LessThan]             = CreateLessThanComparison(type, miGetValue, miCompareTo);
                cacheForType[(Int32)TriggerComparisonOp.LessThanOrEqualTo]    = CreateLessThanEqualsComparison(type, miGetValue, miCompareTo);
            }
        }

        /// <summary>
        /// If the specified expression is of type Nullable{T}, it is "lifted" by retrieving its underlying value.
        /// Otherwise, the input expression is unchanged.
        /// </summary>
        /// <param name="exp">The expression to lift.</param>
        /// <returns>The lifted expression.</returns>
        private static Expression Lift(Expression exp)
        {
            var nullable = exp.Type.IsValueType && exp.Type.IsGenericType && exp.Type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (nullable)
            {
                return Expression.Property(exp, "Value");
            }
            return exp;
        }

        /// <summary>
        /// Creates the trigger comparison for <see cref="TriggerComparisonOp.Equals"/>.
        /// </summary>
        private static TriggerComparison CreateEqualsComparison(Type type, MethodInfo miGetValue, MethodInfo miEquals)
        {
            /*
             * var currentValue = dobj.GetValue<T>(dprop);
             * if (currentValue == null && refval == null) return true;
             * if (currentValue == null && refval != null) return false;
             * if (currentValue != null && refval == null) return false;
             * return currentValue.Equals(refval);
             */

            var vars = new List<ParameterExpression>();
            var exps = new List<Expression>();

            var paramDobj   = Expression.Parameter(typeof(DependencyObject), "dobj");
            var paramDprop  = Expression.Parameter(typeof(DependencyProperty), "dprop");
            var paramRefval = Expression.Parameter(typeof(Object), "refval");

            var varCurrentValue = Expression.Variable(type, "currentValue");
            vars.Add(varCurrentValue);

            var expAssignCurrentValue = Expression.Assign(varCurrentValue, Expression.Call(paramDobj, miGetValue, paramDprop));
            exps.Add(expAssignCurrentValue);

            var expReturnTarget = Expression.Label(typeof(Boolean), "exit");
            var expReturnLabel  = Expression.Label(expReturnTarget, Expression.Constant(false));

            var includeNullChecks = !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
            if (includeNullChecks)
            {
                exps.Add(Expression.IfThen(
                    Expression.And(
                        Expression.Equal(varCurrentValue, Expression.Constant(null)),
                        Expression.Equal(paramRefval, Expression.Constant(null))),
                    Expression.Return(expReturnTarget, Expression.Constant(true))));

                exps.Add(Expression.IfThen(
                    Expression.And(
                        Expression.Equal(varCurrentValue, Expression.Constant(null)),
                        Expression.NotEqual(paramRefval, Expression.Constant(null))),
                    Expression.Return(expReturnTarget, Expression.Constant(false))));

                exps.Add(Expression.IfThen(
                    Expression.And(
                        Expression.NotEqual(varCurrentValue, Expression.Constant(null)),
                        Expression.Equal(paramRefval, Expression.Constant(null))),
                    Expression.Return(expReturnTarget, Expression.Constant(false))));
            }

            var callParamType = miEquals.GetParameters()[0].ParameterType;

            exps.Add(Expression.Return(expReturnTarget,
                Expression.Call(Lift(varCurrentValue), miEquals, Expression.Convert(Lift(paramRefval), callParamType))));

            exps.Add(expReturnLabel);

            return Expression.Lambda<TriggerComparison>(Expression.Block(vars, exps), paramDobj, paramDprop, paramRefval).Compile();
        }

        /// <summary>
        /// Creates the trigger comparison for <see cref="TriggerComparisonOp.NotEquals"/>.
        /// </summary>
        private static TriggerComparison CreateNotEqualsComparison(Type type, MethodInfo miGetValue, MethodInfo miEquals)
        {
            /*
             * var currentValue = dobj.GetValue<T>(dprop);
             * if (currentValue == null && refval == null) return false;
             * if (currentValue == null && refval != null) return true;
             * if (currentValue != null && refval == null) return true;
             * return !currentValue.Equals(refval);
             */

            var vars = new List<ParameterExpression>();
            var exps = new List<Expression>();

            var paramDobj   = Expression.Parameter(typeof(DependencyObject), "dobj");
            var paramDprop  = Expression.Parameter(typeof(DependencyProperty), "dprop");
            var paramRefval = Expression.Parameter(typeof(Object), "refval");

            var varCurrentValue = Expression.Variable(type, "currentValue");
            vars.Add(varCurrentValue);

            var expAssignCurrentValue = Expression.Assign(varCurrentValue, Expression.Call(paramDobj, miGetValue, paramDprop));
            exps.Add(expAssignCurrentValue);

            var expReturnTarget = Expression.Label(typeof(Boolean), "exit");
            var expReturnLabel  = Expression.Label(expReturnTarget, Expression.Constant(false));

            var includeNullChecks = !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
            if (includeNullChecks)
            {
                exps.Add(Expression.IfThen(
                    Expression.And(
                        Expression.Equal(varCurrentValue, Expression.Constant(null)),
                        Expression.Equal(paramRefval, Expression.Constant(null))),
                    Expression.Return(expReturnTarget, Expression.Constant(false))));

                exps.Add(Expression.IfThen(
                    Expression.And(
                        Expression.Equal(varCurrentValue, Expression.Constant(null)),
                        Expression.NotEqual(paramRefval, Expression.Constant(null))),
                    Expression.Return(expReturnTarget, Expression.Constant(true))));

                exps.Add(Expression.IfThen(
                    Expression.And(
                        Expression.NotEqual(varCurrentValue, Expression.Constant(null)),
                        Expression.Equal(paramRefval, Expression.Constant(null))),
                    Expression.Return(expReturnTarget, Expression.Constant(true))));
            }

            var callParamType = miEquals.GetParameters()[0].ParameterType;

            exps.Add(Expression.Return(expReturnTarget,
                Expression.Not(Expression.Call(Lift(varCurrentValue), miEquals, Expression.Convert(Lift(paramRefval), callParamType)))));

            exps.Add(expReturnLabel);

            return Expression.Lambda<TriggerComparison>(Expression.Block(vars, exps), paramDobj, paramDprop, paramRefval).Compile();
        }

        /// <summary>
        /// Creates the trigger comparison using a relative comparison operator such as greater than or less than.
        /// </summary>
        private static TriggerComparison CreateRelativeComparison(Type type, MethodInfo miGetValue, MethodInfo miCompareTo, Func<Expression, Expression> op)
        {
            /*
             * var currentValue = dobj.GetValue<T>(dprop);
             * if (currentValue == null || refval == null) return false;
             * return currentValue.CompareTo(refval) {OPERATOR} 0;
             */

            var vars = new List<ParameterExpression>();
            var exps = new List<Expression>();

            var paramDobj   = Expression.Parameter(typeof(DependencyObject), "dobj");
            var paramDprop  = Expression.Parameter(typeof(DependencyProperty), "dprop");
            var paramRefval = Expression.Parameter(typeof(Object), "refval");

            var varCurrentValue = Expression.Variable(type, "currentValue");
            vars.Add(varCurrentValue);

            var expAssignCurrentValue = Expression.Assign(varCurrentValue, Expression.Call(paramDobj, miGetValue, paramDprop));
            exps.Add(expAssignCurrentValue);

            var expReturnTarget = Expression.Label(typeof(Boolean), "exit");
            var expReturnLabel  = Expression.Label(expReturnTarget, Expression.Constant(false));

            var includeNullChecks = !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
            if (includeNullChecks)
            {
                exps.Add(Expression.IfThen(
                    Expression.And(
                        Expression.Equal(varCurrentValue, Expression.Constant(null)),
                        Expression.Equal(paramRefval, Expression.Constant(null))),
                    Expression.Return(expReturnTarget, Expression.Constant(false))));
            }

            var callParamType = miCompareTo.GetParameters()[0].ParameterType;

            var expCallCompare = Expression.Call(Lift(varCurrentValue), miCompareTo, Expression.Convert(Lift(paramRefval), callParamType));
            var expEval        = op(expCallCompare);
            exps.Add(Expression.Return(expReturnTarget, expEval));

            exps.Add(expReturnLabel);

            return Expression.Lambda<TriggerComparison>(Expression.Block(vars, exps), paramDobj, paramDprop, paramRefval).Compile();
        }

        /// <summary>
        /// Creates the trigger comparison for <see cref="TriggerComparisonOp.GreaterThan"/>.
        /// </summary>
        private static TriggerComparison CreateGreaterThanComparison(Type type, MethodInfo miGetValue, MethodInfo miCompareTo)
        {
            return CreateRelativeComparison(type, miGetValue, miCompareTo,
                (exp) => Expression.GreaterThan(exp, Expression.Constant(0)));
        }

        /// <summary>
        /// Creates the trigger comparison for <see cref="TriggerComparisonOp.GreaterThanOrEqualTo"/>.
        /// </summary>
        private static TriggerComparison CreateGreaterThanEqualsComparison(Type type, MethodInfo miGetValue, MethodInfo miCompareTo)
        {
            return CreateRelativeComparison(type, miGetValue, miCompareTo,
                (exp) => Expression.GreaterThanOrEqual(exp, Expression.Constant(0)));
        }

        /// <summary>
        /// Creates the trigger comparison for <see cref="TriggerComparisonOp.LessThan"/>.
        /// </summary>
        private static TriggerComparison CreateLessThanComparison(Type type, MethodInfo miGetValue, MethodInfo miCompareTo)
        {
            return CreateRelativeComparison(type, miGetValue, miCompareTo,
                (exp) => Expression.LessThan(exp, Expression.Constant(0)));
        }

        /// <summary>
        /// Creates the trigger comparison for <see cref="TriggerComparisonOp.LessThanOrEqualTo"/>.
        /// </summary>
        private static TriggerComparison CreateLessThanEqualsComparison(Type type, MethodInfo miGetValue, MethodInfo miCompareTo)
        {
            return CreateRelativeComparison(type, miGetValue, miCompareTo,
                (exp) => Expression.LessThanOrEqual(exp, Expression.Constant(0)));
        }

        // The cache of delegates for each known type.
        private static readonly Dictionary<Type, Dictionary<Int32, TriggerComparison>> cache = 
            new Dictionary<Type, Dictionary<Int32, TriggerComparison>>();
    }
}

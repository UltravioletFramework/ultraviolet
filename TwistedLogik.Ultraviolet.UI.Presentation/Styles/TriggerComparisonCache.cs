using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Styles
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
        /// <returns>The cached <see cref="TriggerComparison"/> delegate for the specified type and operation.</returns>
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

                return cacheForType[(Int32)op];
            }
        }

        /// <summary>
        /// Creates a full set of comparison delegates for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create comparison delegates.</param>
        private static void CreateComparisonDelegates(Type type)
        {
            var miGetValue  = typeof(DependencyObject).GetMethod("GetValue").MakeGenericMethod(type);
            var miEquals    = type.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance, null, new[] { type }, null);
            var miCompareTo = type.GetMethod("CompareTo", BindingFlags.Public | BindingFlags.Instance, null, new[] { type }, null);

            if (miEquals == null || miCompareTo == null)
                throw new InvalidOperationException(PresentationStrings.TypeDoesNotDefineEqualsOrCompareTo.Format(type.Name));

            var paramDObj     = Expression.Parameter(typeof(DependencyObject), "dobj");
            var paramDProp    = Expression.Parameter(typeof(DependencyProperty), "dprop");
            var paramRefValue = Expression.Parameter(typeof(Object), "refvalue");

            var callGetValue = Expression.Call(paramDObj, miGetValue, paramDProp);

            var expEquals        = Expression.Call(callGetValue, miEquals, Expression.Convert(paramRefValue, type));
            var expNotEquals     = Expression.IsFalse(Expression.Call(callGetValue, miEquals, Expression.Convert(paramRefValue, type)));
            var expGreater       = Expression.GreaterThan(Expression.Call(callGetValue, miCompareTo, Expression.Convert(paramRefValue, type)), Expression.Constant(0));
            var expGreaterEquals = Expression.GreaterThanOrEqual(Expression.Call(callGetValue, miCompareTo, Expression.Convert(paramRefValue, type)), Expression.Constant(0));
            var expLess          = Expression.LessThan(Expression.Call(callGetValue, miCompareTo, Expression.Convert(paramRefValue, type)), Expression.Constant(0));
            var expLessEquals    = Expression.LessThanOrEqual(Expression.Call(callGetValue, miCompareTo, Expression.Convert(paramRefValue, type)), Expression.Constant(0));

            var lambdaEquals        = Expression.Lambda<TriggerComparison>(expEquals, paramDObj, paramDProp, paramRefValue).Compile();
            var lambdaNotEquals     = Expression.Lambda<TriggerComparison>(expNotEquals, paramDObj, paramDProp, paramRefValue).Compile();
            var lambdaGreater       = Expression.Lambda<TriggerComparison>(expGreater, paramDObj, paramDProp, paramRefValue).Compile();
            var lambdaGreaterEquals = Expression.Lambda<TriggerComparison>(expGreaterEquals, paramDObj, paramDProp, paramRefValue).Compile();
            var lambdaLess          = Expression.Lambda<TriggerComparison>(expLess, paramDObj, paramDProp, paramRefValue).Compile();
            var lambdaLessEquals    = Expression.Lambda<TriggerComparison>(expLessEquals, paramDObj, paramDProp, paramRefValue).Compile();

            Dictionary<Int32, TriggerComparison> cacheForType;
            if (!cache.TryGetValue(type, out cacheForType))
                cache[type] = cacheForType = new Dictionary<Int32, TriggerComparison>();

            cacheForType[(Int32)TriggerComparisonOp.Equals]               = lambdaEquals;
            cacheForType[(Int32)TriggerComparisonOp.NotEquals]            = lambdaNotEquals;
            cacheForType[(Int32)TriggerComparisonOp.GreaterThan]          = lambdaGreater;
            cacheForType[(Int32)TriggerComparisonOp.GreaterThanOrEqualTo] = lambdaGreaterEquals;
            cacheForType[(Int32)TriggerComparisonOp.LessThan]             = lambdaLess;
            cacheForType[(Int32)TriggerComparisonOp.LessThanOrEqualTo]    = lambdaLessEquals;
        }

        // The cache of delegates for each known type.
        private static readonly Dictionary<Type, Dictionary<Int32, TriggerComparison>> cache = 
            new Dictionary<Type, Dictionary<Int32, TriggerComparison>>();
    }
}

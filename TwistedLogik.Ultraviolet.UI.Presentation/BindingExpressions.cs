using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a method which compares two data bound values for equality.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="value1">The first value to compare.</param>
    /// <param name="value2">The second value to compare.</param>
    /// <returns><c>true</c> if the specified values are equal; otherwise, <c>false</c>.</returns>
    internal delegate Boolean DataBindingComparer<T>(T value1, T value2);

    /// <summary>
    /// Represents a method which is used to retrieve the value of a data bound dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <returns>The current value of the bound property.</returns>
    internal delegate T DataBindingGetter<T>(Object model);

    /// <summary>
    /// Represents a method which is used to set the value of a data bound dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <param name="value">The value to set on the bound property.</param>
    internal delegate void DataBindingSetter<T>(Object model, T value);

    /// <summary>
    /// Contains methods for creating and manipulating binding expressions.
    /// </summary>
    internal partial class BindingExpressions
    {
        /// <summary>
        /// Initializes the <see cref="BindingExpressions"/> class.
        /// </summary>
        static BindingExpressions()
        {
            miReferenceEquals = typeof(Object).GetMethod("ReferenceEquals", new[] { typeof(Object), typeof(Object) });
            miObjectEquals    = typeof(Object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(Object), typeof(Object) }, null);
            miNullableEquals  = typeof(Nullable).GetMethods().Where(x => x.Name == "Equals" && x.IsGenericMethod).Single();
        }

        /// <summary>
        /// Clears the internal cache of binding getters which is built by
        /// calls to the <see cref="CreateBindingGetter"/> method.
        /// </summary>
        public static void ClearCachedBindingGetters()
        {
            lock(cachedExpressionGetters)
                cachedExpressionGetters.Clear();
        }

        /// <summary>
        /// Clears the internal cache of binding setters which is built by 
        /// calls to the the <see cref="CreateBindingSetter"/> method.
        /// </summary>
        public static void ClearCachedBindingSetters()
        {
            lock (cachedExpressionSetters)
                cachedExpressionSetters.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether the specified string represents a binding expression.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns><c>true</c> if the specified string represents a binding expression; otherwise, <c>false</c>.</returns>
        public static Boolean IsBindingExpression(String expression, Boolean braces = true)
        {
            if (String.IsNullOrEmpty(expression))
                return false;

            if (IsNullBindingExpression(expression))
                return false;

            if (!braces)
                return true;

            if (!expression.StartsWith("{{"))
                return false;

            var closingBracesIx = expression.IndexOf("}}", "{{".Length);
            if (closingBracesIx < 0)
                return false;

            if (closingBracesIx == expression.Length - "}}".Length)
                return true;

            if (expression[closingBracesIx + "}}".Length] == '[' && expression.EndsWith("]"))
                return true;

            return false;
        }
        
        /// <summary>
        /// Gets a value indicating whether the specified binding expression is
        /// the special representation of a null reference (i.e. the {{null}} expression).
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns><c>true</c> if the specified expression represents <c>null</c>; otherwise, <c>false</c>.</returns>
        public static Boolean IsNullBindingExpression(String expression)
        {
            return expression == "{{null}}";
        }

        /// <summary>
        /// Gets a value indicating whether the specified binding expression is a simple reference
        /// to a dependency property; that is, a direct reference with no intermediate steps.
        /// </summary>
        /// <param name="dataSourceType">The type of the data source to evaluate.</param>
        /// <param name="expression">The binding expression to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns><c>true</c> if the specified expression is a simple reference to a dependency property; otherwise, <c>false</c>.</returns>
        public static Boolean IsSimpleDependencyProperty(Type dataSourceType, String expression, Boolean braces = true)
        {
            if (!IsBindingExpression(expression, braces))
                return false;

            return GetSimpleDependencyProperty(dataSourceType, expression, braces) != null;
        }
                
        /// <summary>
        /// Gets the part of a binding expression which represents the member navigation path.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the member navigation path, or <c>null</c> if no such part exists.</returns>
        public static String GetBindingMemberPathPart(String expression, Boolean braces = true)
        {
            Contract.RequireNotEmpty(expression, "expression");

            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(PresentationStrings.InvalidBindingExpression.Format(expression));

            return GetBindingMemberPathPartInternal(expression, braces);
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the format string.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the format string, or <c>null</c> if no such part exists.</returns>
        public static String GetBindingFormatStringPart(String expression, Boolean braces = true)
        {
            Contract.RequireNotEmpty(expression, "expression");

            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(PresentationStrings.InvalidBindingExpression.Format(expression));

            return GetBindingFormatStringPartInternal(expression, braces);
        }

        /// <summary>
        /// Gets the type of the specified binding expression.
        /// </summary>
        /// <param name="dataSourceType">The type of the data source to evaluate.</param>
        /// <param name="expression">The binding expression to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The type of the binding expression.</returns>
        public static Type GetExpressionType(Type dataSourceType, String expression, Boolean braces = true)
        {
            Contract.Require(dataSourceType, "dataSourceType");
            Contract.RequireNotEmpty(expression, "expression");

            var expMemberPath = GetBindingMemberPathPart(expression, braces);

            var members = dataSourceType.GetMember(expMemberPath, BindingFlags.Public | BindingFlags.Instance);
            if (members == null || !members.Any())
                return null;

            var member = members.Where(x =>
                x.MemberType == MemberTypes.Property ||
                x.MemberType == MemberTypes.Field).FirstOrDefault() ?? members.First();

            return GetMemberType(member);
        }

        /// <summary>
        /// Creates an event handler which is bound to a method on a view model.
        /// </summary>
        /// <param name="dataSource">The data source for the binding expression.</param>
        /// <param name="dataSourceType">The type of the data source for the binding expression.</param>
        /// <param name="delegateType">The type of the delegate to create.</param>
        /// <param name="expression">The binding expression from which to create the delegate.</param>
        /// <returns>A <see cref="Delegate"/> which represents the bound event handler.</returns>
        public static Delegate CreateBoundEventDelegate(Object dataSource, Type dataSourceType, Type delegateType, String expression)
        {
            Contract.Require(dataSource, "dataSource");
            Contract.Require(dataSourceType, "dataSourceType");
            Contract.Require(delegateType, "delegateType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new BoundEventBuilder(dataSource, dataSourceType, delegateType, expression);
            return builder.Compile();
        }

        /// <summary>
        /// Creates a getter for the specified binding expression.
        /// </summary>
        /// <param name="boundType">The type of value to which the expression is being bound.</param>
        /// <param name="dataSourceType">The type of the data source to which the expression is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        /// <returns>A <see cref="Delegate"/> that represents the specified model and expression.</returns>
        public static Delegate CreateBindingGetter(Type boundType, Type dataSourceType, String expression)
        {
            Contract.Require(boundType, nameof(boundType));
            Contract.Require(dataSourceType, nameof(dataSourceType));
            Contract.RequireNotEmpty(expression, nameof(expression));
            
            var key = new BindingExpressionAccessorKey(boundType, dataSourceType, expression);
            lock (cachedExpressionGetters)
            {
                var cached = default(Delegate);
                if (cachedExpressionGetters.TryGetValue(key, out cached))
                    return cached;
            }

            var builder = new DataBindingGetterBuilder(boundType, dataSourceType, expression);
            var result = builder.Compile();
            lock (cachedExpressionGetters)
            {
                cachedExpressionGetters[key] = result;
            }
            return result;
        }

        /// <summary>
        /// Creates a setter for the specified binding expression.
        /// </summary>
        /// <param name="boundType">The type of value to which the expression is being bound.</param>
        /// <param name="dataSourceType">The type of the data source to which the expression is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        /// <returns>A <see cref="Delegate"/> that represents the specified model and expression.</returns>
        public static Delegate CreateBindingSetter(Type boundType, Type dataSourceType, String expression)
        {
            Contract.Require(boundType, nameof(boundType));
            Contract.Require(dataSourceType, nameof(dataSourceType));
            Contract.RequireNotEmpty(expression, nameof(expression));
            
            var key = new BindingExpressionAccessorKey(boundType, dataSourceType, expression);
            lock (cachedExpressionSetters)
            {
                var cached = default(Delegate);
                if (cachedExpressionSetters.TryGetValue(key, out cached))
                    return cached;
            }
            
            var builder = new DataBindingSetterBuilder(boundType, dataSourceType, expression);
            var result = builder.Compile();
            lock (cachedExpressionSetters)
            {
                cachedExpressionSetters[key] = result;
            }
            return result;
        }

        /// <summary>
        /// Gets the binding comparison function for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create a binding comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        public static Delegate GetComparisonFunction(Type type)
        {
            lock (comparerRegistry)
            {
                var typeComparer = default(Delegate);

                if (!comparerRegistry.TryGetValue(type, out typeComparer))
                {
                    if (type.IsClass)
                    {
                        typeComparer = GetReferenceComparisonFunction(type);
                    }
                    else if (type.GetInterfaces().Where(x => x == typeof(IEquatable<>).MakeGenericType(type)).Any())
                    {
                        typeComparer = GetIEquatableComparisonFunction(type);
                    }
                    else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        typeComparer = GetNullableComparisonFunction(type);
                    }
                    else if (type.IsEnum)
                    {
                        typeComparer = GetEnumComparisonFunction(type);
                    }
                    else
                    {
                        typeComparer = GetFallbackComparisonFunction(type);
                    }
                    comparerRegistry[type] = typeComparer;
                }

                return typeComparer;
            }
        }

        /// <summary>
        /// Retrieves the dependency property which is referenced by the specified binding expression, if
        /// the expression is a simple dependency property reference.
        /// </summary>
        /// <param name="dataSourceType">The type of the data source to evaluate.</param>
        /// <param name="expression">The binding expression to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The dependency property referenced by the specified expression, or <c>null</c> if the specified
        /// expression is not a simple dependency property reference.</returns>
        public static DependencyProperty GetSimpleDependencyProperty(Type dataSourceType, String expression, Boolean braces = true)
        {
            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(PresentationStrings.InvalidBindingExpression.Format(expression));

            var expMemberPath = GetBindingMemberPathPart(expression, braces);

            var expProperty = dataSourceType.GetProperty(expMemberPath);
            if (expProperty == null)
                return null;

            var expAttribute = (CompiledBindingExpressionAttribute)expProperty.GetCustomAttributes(typeof(CompiledBindingExpressionAttribute), false).SingleOrDefault();
            if (expAttribute == null)
                return null;

            if (expAttribute.SimpleDependencyPropertyOwner == null)
                return null;

            return DependencyProperty.FindByName(expAttribute.SimpleDependencyPropertyName, expAttribute.SimpleDependencyPropertyOwner);
        }

        /// <summary>
        /// Gets the type of the specified member.
        /// </summary>
        /// <param name="member">The member to evaluate.</param>
        /// <returns>The type of the specified member.</returns>
        private static Type GetMemberType(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a comparison function for value types which implement <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetReferenceComparisonFunction(Type type)
        {
            var param1 = Expression.Parameter(type, "o1");
            var param2 = Expression.Parameter(type, "o2");

            var delegateBody = Expression.Call(miReferenceEquals, param1, param2);
            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, delegateBody, param1, param2).Compile();
        }

        /// <summary>
        /// Gets a comparison function for value types which implement <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetIEquatableComparisonFunction(Type type)
        {
            var equalsMethod = type.GetMethod("Equals", new[] { type });

            var arg1 = Expression.Parameter(type, "o1");
            var arg2 = Expression.Parameter(type, "o2");

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Call(arg1, equalsMethod, arg2), arg1, arg2).Compile();
        }

        /// <summary>
        /// Gets a comparison function for nullable value types.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetNullableComparisonFunction(Type type)
        {
            var nullableType = type.GetGenericArguments()[0];
            var nullableEqualsMethod = miNullableEquals.MakeGenericMethod(nullableType);

            var arg1 = Expression.Parameter(type, "o1");
            var arg2 = Expression.Parameter(type, "o2");

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Call(nullableEqualsMethod, arg1, arg2), arg1, arg2).Compile();
        }

        /// <summary>
        /// Gets a comparison function for enumeration types.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetEnumComparisonFunction(Type type)
        {
            var param1 = Expression.Parameter(type, "o1");
            var param2 = Expression.Parameter(type, "o2");

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Equal(param1, param2), param1, param2).Compile();
        }

        /// <summary>
        /// Gets a fallback comparison function for types which don't fit any optimizable category.
        /// </summary>
        /// <param name="type">The type for which to create a comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        private static Delegate GetFallbackComparisonFunction(Type type)
        {
            var param1 = Expression.Parameter(type, "o1");
            var param2 = Expression.Parameter(type, "o2");
            var arg1   = param1;
            var arg2   = Expression.Convert(param2, typeof(Object));

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Call(miObjectEquals, arg1, arg2), param1, param2).Compile();
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the member navigation path.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the member navigation path, or <c>null</c> if no such part exists.</returns>
        private static String GetBindingMemberPathPartInternal(String expression, Boolean braces = true)
        {
            if (!braces)
                return expression;

            var closingBracesIndex = expression.IndexOf("}}", "{{".Length);

            var offset = braces ? "{{".Length : 0;
            return expression.Substring(offset, closingBracesIndex - offset).Trim();
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the format string.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the format string, or <c>null</c> if no such part exists.</returns>
        private static String GetBindingFormatStringPartInternal(String expression, Boolean braces = true)
        {
            if (!braces)
                return null;

            var closingBracesIndex = expression.IndexOf("}}", "{{".Length);
            if (closingBracesIndex == expression.Length - "}}".Length)
                return null;            

            var openFormatIndex = closingBracesIndex + "}}".Length;
            if (expression[openFormatIndex] != '[' || !expression.EndsWith("]"))
                return null;

            var start = openFormatIndex + 1;
            var length = expression.Length - (1 + start);
            return expression.Substring(start, length);
        }

        /// <summary>
        /// Creates an instance of <see cref="DataBindingGetter{T}"/> for cases where the binding expression
        /// is a simple dependency property reference.
        /// </summary>
        private static DataBindingGetter<T> CreateGetterForSimpleDependencyProperty<T>(DependencyProperty dprop)
        {
            return new DataBindingGetter<T>((ds) =>
            {
                var wrapper = ds as IDataSourceWrapper;
                if (wrapper != null)
                {
                    return ((DependencyObject)wrapper.WrappedDataSource).GetValue<T>(dprop);
                }
                else
                {
                    return ((DependencyObject)ds).GetValue<T>(dprop);
                }
            });
        }

        /// <summary>
        /// Creates an instance of <see cref="DataBindingGetter{T}"/> for cases where the binding expression
        /// is a simple dependency property reference.
        /// </summary>
        private static DataBindingSetter<T> CreateSetterForSimpleDependencyProperty<T>(DependencyProperty dprop)
        {
            return new DataBindingSetter<T>((ds, value) =>
            {
                var wrapper = ds as IDataSourceWrapper;
                if (wrapper != null)
                {
                    ((DependencyObject)wrapper.WrappedDataSource).SetValue(dprop, value);
                }
                else
                {
                    ((DependencyObject)ds).SetValue(dprop, value);
                }
            });
        }

        // Reflection information for commonly-used methods.
        private static readonly MethodInfo miReferenceEquals;
        private static readonly MethodInfo miObjectEquals;
        private static readonly MethodInfo miNullableEquals;

        // Comparison functions for various types.
        private static readonly Dictionary<Type, Delegate> comparerRegistry = new Dictionary<Type, Delegate>();

        // Cached expression accessors.
        private static readonly Dictionary<BindingExpressionAccessorKey, Delegate> cachedExpressionGetters =
            new Dictionary<BindingExpressionAccessorKey, Delegate>();
        private static readonly Dictionary<BindingExpressionAccessorKey, Delegate> cachedExpressionSetters =
            new Dictionary<BindingExpressionAccessorKey, Delegate>();
    }
}

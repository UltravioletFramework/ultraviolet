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
            miCreateSimpleGet = typeof(BindingExpressions).GetMethod("CreateGetterForSimpleDependencyProperty", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(DependencyProperty) }, null);
            miCreateSimpleSet = typeof(BindingExpressions).GetMethod("CreateSetterForSimpleDependencyProperty", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(DependencyProperty) }, null);
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

            return !braces || expression.StartsWith("{{") && expression.EndsWith("}}");
        }
        
        /// <summary>
        /// Gets a value indicating whether the specified binding expression is 
        /// globally-scoped using the :: operator.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns><c>true</c> if the specified string represents a globally-scoped binding expression; otherwise, <c>false</c>.</returns>
        public static Boolean IsGloballyScopedBindingExpression(String expression, Boolean braces = true)
        {
            if (String.IsNullOrEmpty(expression))
                return false;

            if (IsNullBindingExpression(expression))
                return false;

            if (IsBindingExpression(expression))
            {
                return braces ? expression.StartsWith("{{::") : expression.StartsWith("::");
            }
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
        /// Parses the specified binding expression into its constituent components.
        /// </summary>
        /// <param name="expression">The binding expression to parse.</param>
        /// <param name="braces">A value indicating whether the expression's containing braces are included.</param>
        /// <returns>The specified binding expression's constituent components.</returns>
        public static IEnumerable<String> ParseBindingExpression(String expression, Boolean braces = true)
        {
            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(PresentationStrings.InvalidBindingExpression.Format(expression));

            var path       = GetBindingMemberPathPartInternal(expression, braces);
            var components = path.Split('.');

            return components;
        }

        /// <summary>
        /// Combines two binding expressions into a single binding expression.
        /// </summary>
        /// <param name="expression1">The first binding expression.</param>
        /// <param name="expression2">The second binding expression.</param>
        /// <param name="braces">A value indicating whether the expressions' containing braces are included.</param>
        /// <returns>A binding expression that represents the combination of the specified binding expressions.</returns>
        public static String Combine(String expression1, String expression2, Boolean braces = true)
        {
            Contract.Ensure<ArgumentException>(String.IsNullOrEmpty(expression1) || IsBindingExpression(expression1, braces), "expression1");
            Contract.Ensure<ArgumentException>(String.IsNullOrEmpty(expression2) || IsBindingExpression(expression2, braces), "expression2");

            if (String.IsNullOrEmpty(expression1))
                return expression2;
            if (String.IsNullOrEmpty(expression2))
                return expression1;

            if (IsGloballyScopedBindingExpression(expression2, braces))
                return expression2;

            var fmt1 = GetBindingFormatStringPartInternal(expression1, braces);
            var fmt2 = GetBindingFormatStringPartInternal(expression2, braces);
            
            var path1 = GetBindingMemberPathPartInternal(expression1, braces);
            var path2 = GetBindingMemberPathPartInternal(expression2, braces);

            var combinedPath = path1 + "." + path2;
            var combinedFmt  = fmt2 ?? fmt1;
            var combinedExp  = String.Format((combinedFmt == null) ? "{0}" : "{0} | {1}", combinedPath, combinedFmt);

            return braces ? "{{" + combinedExp + "}}" : combinedExp;
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
            var components = ParseBindingExpression(expression);
            var currentType = dataSourceType;
            foreach (var component in components)
            {
                var members = currentType.GetMember(component, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (members == null || !members.Any())
                    return null;

                var member = members.Where(x => 
                    x.MemberType == MemberTypes.Property ||
                    x.MemberType == MemberTypes.Field).FirstOrDefault() ?? members.First();

                currentType = GetMemberType(member);
            }
            return currentType;
        }

        /// <summary>
        /// Creates an event handler which is bound to a method on a view model.
        /// </summary>
        /// <param name="uiElement">The interface element to which the event will be bound.</param>
        /// <param name="dataSourceType">The type of the data source to which the expression is being being bound.</param>
        /// <param name="delegateType">The type of the event handler which is being bound.</param>
        /// <param name="expression">The binding expression that represents the method to bind to the event.</param>
        /// <returns>A <see cref="Delegate"/> which represents the bound event handler.</returns>
        public static Delegate CreateViewModelBoundEventDelegate(UIElement uiElement, Type dataSourceType, Type delegateType, String expression)
        {
            Contract.Require(uiElement, "uiElement");
            Contract.Require(dataSourceType, "dataSourceType");
            Contract.Require(delegateType, "delegateType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new BoundEventBuilder(uiElement, dataSourceType, delegateType, expression, false);
            return builder.Compile();
        }

        /// <summary>
        /// Creates an event handler which is bound to a method on an element.
        /// </summary>
        /// <param name="uiElement">The element to which the event will be bound.</param>
        /// <param name="delegateType">The type of the event handler which is being bound.</param>
        /// <param name="expression">The binding expression that represents the method to bind to the event.</param>
        /// <returns>A <see cref="Delegate"/> which represents the bound event handler.</returns>
        public static Delegate CreateElementBoundEventDelegate(UIElement uiElement, Type delegateType, String expression)
        {
            Contract.Require(uiElement, "uiElement");
            Contract.Require(delegateType, "delegateType");
            Contract.RequireNotEmpty(expression, "expression");

            var builder = new BoundEventBuilder(uiElement, uiElement.GetType(), delegateType, expression, true);
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
            Contract.Require(boundType, "boundType");
            Contract.Require(dataSourceType, "dataSourceType");
            Contract.RequireNotEmpty(expression, "expression");

            /* NOTE:
             * The special case for simple dependency properties here is an optimization.
             * Profiling shows that a significant part of our time during loading is spent
             * compiling our expression trees, so we want to bypass that where possible. */

            var dprop = GetSimpleDependencyProperty(dataSourceType, expression);
            if (dprop != null && dprop.PropertyType == boundType)
            {
                return (Delegate)miCreateSimpleGet.MakeGenericMethod(boundType).Invoke(null, new[] { dprop });
            }
            else
            {
                var builder = new DataBindingGetterBuilder(boundType, dataSourceType, expression);
                return builder.Compile();
            }
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
            Contract.Require(boundType, "boundType");
            Contract.Require(dataSourceType, "dataSourceType");
            Contract.RequireNotEmpty(expression, "expression");

            /* NOTE:
             * The special case for simple dependency properties here is an optimization.
             * Profiling shows that a significant part of our time during loading is spent
             * compiling our expression trees, so we want to bypass that where possible. */

            var dprop = GetSimpleDependencyProperty(dataSourceType, expression);
            if (dprop != null && dprop.PropertyType == boundType)
            {
                return (Delegate)miCreateSimpleSet.MakeGenericMethod(boundType).Invoke(null, new[] { dprop });
            }
            else
            {
                var builder = new DataBindingSetterBuilder(boundType, dataSourceType, expression);
                return builder.Compile();
            }
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

            var components = ParseBindingExpression(expression, braces);
            if (components.Count() != 1)
                return null;

            return DependencyProperty.FindByName(components.Single(), dataSourceType);
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
            var ixDelimiter = expression.IndexOf('|');
            if (ixDelimiter >= 0)
            {
                var offset = braces ? 
                    expression.StartsWith("{{::") ? 4 : 2 : 
                    expression.StartsWith("::") ? 2 : 0;

                return expression.Substring(offset, ixDelimiter - offset).Trim();
            }
            else
            {
                if (braces)
                {
                    return (expression.StartsWith("{{::") ? 
                    expression.Substring(4, expression.Length - 6) : 
                    expression.Substring(2, expression.Length - 4)).Trim();
                }
                return (expression.StartsWith("::") ? expression.Substring(2) : expression).Trim();
            }
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the format string.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the format string, or <c>null</c> if no such part exists.</returns>
        private static String GetBindingFormatStringPartInternal(String expression, Boolean braces = true)
        {
            var ixDelimiter = expression.IndexOf('|');
            if (ixDelimiter >= 0)
            {
                var length = (expression.Length - (braces ? 2 : 0)) - (ixDelimiter + 1);
                return expression.Substring(ixDelimiter + 1, length).Trim();
            }
            return null;
        }

        /// <summary>
        /// Creates an instance of <see cref="DataBindingGetter{T}"/> for cases where the binding expression
        /// is a simple dependency property reference.
        /// </summary>
        private static DataBindingGetter<T> CreateGetterForSimpleDependencyProperty<T>(DependencyProperty dprop)
        {
            return new DataBindingGetter<T>((ds) =>
            {
                return ((DependencyObject)ds).GetValue<T>(dprop);
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
                ((DependencyObject)ds).SetValue<T>(dprop, value);
            });
        }

        // Reflection information for commonly-used methods.
        private static readonly MethodInfo miReferenceEquals;
        private static readonly MethodInfo miObjectEquals;
        private static readonly MethodInfo miNullableEquals;
        private static readonly MethodInfo miCreateSimpleGet;
        private static readonly MethodInfo miCreateSimpleSet;

        // Comparison functions for various types.
        private static readonly Dictionary<Type, Delegate> comparerRegistry = new Dictionary<Type, Delegate>();
    }
}

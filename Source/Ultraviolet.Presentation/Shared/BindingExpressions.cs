using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ultraviolet.Audio;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Controls.Primitives;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a method which compares two data bound values for equality.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="value1">The first value to compare.</param>
    /// <param name="value2">The second value to compare.</param>
    /// <returns><see langword="true"/> if the specified values are equal; otherwise, <see langword="false"/>.</returns>
    public delegate Boolean DataBindingComparer<T>(T value1, T value2);

    /// <summary>
    /// Represents a method which is used to retrieve the value of a data bound dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <returns>The current value of the bound property.</returns>
    public delegate T DataBindingGetter<T>(Object model);

    /// <summary>
    /// Represents a method which is used to set the value of a data bound dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <param name="value">The value to set on the bound property.</param>
    public delegate void DataBindingSetter<T>(Object model, T value);

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
            Object.Equals(null, null);
            miReferenceEquals = typeof(Object).GetMethod("ReferenceEquals", new[] { typeof(Object), typeof(Object) });

            Object.ReferenceEquals(null, null);
            miObjectEquals = typeof(Object).GetMethod("Equals", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(Object), typeof(Object) }, null);

            if (UltravioletPlatformInfo.IsRuntimeCodeGenerationSupported())
            {
                Nullable.Equals<Int32>(null, null);
                miNullableEquals = typeof(Nullable).GetMethods().Where(x => x.Name == "Equals" && x.IsGenericMethod).Single();
            }

            RegisterPrecompiledComparisonDelegates();
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
        /// <returns><see langword="true"/> if the specified string represents a binding expression; otherwise, <see langword="false"/>.</returns>
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
        /// <returns><see langword="true"/> if the specified expression represents <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
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
        /// <returns><see langword="true"/> if the specified expression is a simple reference to a dependency property; otherwise, <see langword="false"/>.</returns>
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
        /// <returns>The part of the binding expression which represents the member navigation path, or <see langword="null"/> if no such part exists.</returns>
        public static String GetBindingMemberPathPart(String expression, Boolean braces = true)
        {
            Contract.RequireNotEmpty(expression, nameof(expression));

            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(PresentationStrings.InvalidBindingExpression.Format(expression));

            return GetBindingMemberPathPartInternal(expression, braces);
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the format string.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the format string, or <see langword="null"/> if no such part exists.</returns>
        public static String GetBindingFormatStringPart(String expression, Boolean braces = true)
        {
            Contract.RequireNotEmpty(expression, nameof(expression));

            if (!IsBindingExpression(expression, braces))
                throw new ArgumentException(PresentationStrings.InvalidBindingExpression.Format(expression));

            return GetBindingFormatStringPartInternal(expression, braces);
        }

        /// <summary>
        /// Finds the property or field of a type which has the specified name.
        /// </summary>
        /// <param name="type">The type to search for the member.</param>
        /// <param name="name">The name of the member for which to search.</param>
        public static MemberInfo FindPropertyOrField(Type type, String name)
        {
            Contract.Require(type, nameof(type));
            Contract.RequireNotEmpty(name, nameof(name));

            var members = type.GetMember(name, BindingFlags.Public | BindingFlags.Instance);
            if (members == null || !members.Any())
                return null;

            var member = members.Where(x =>
                x.MemberType == MemberTypes.Property ||
                x.MemberType == MemberTypes.Field).FirstOrDefault() ?? members.First();

            return member;
        }

        /// <summary>
        /// Gets a value indicating whether the specified member is readable.
        /// </summary>
        /// <param name="member">The member to evaluate.</param>
        /// <returns><see langword="true"/> if the specified member is readable; otherwise, <see langword="false"/>.</returns>
        public static Boolean CanReadMember(MemberInfo member)
        {
            Contract.Require(member, nameof(member));

            switch (member)
            {
                case FieldInfo f:
                    return true;
                case PropertyInfo p:
                    return p.CanRead;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified member is writable.
        /// </summary>
        /// <param name="member">The member to evaluate.</param>
        /// <returns><see langword="true"/> if the specified member is writable; otherwise, <see langword="false"/>.</returns>
        public static Boolean CanWriteMember(MemberInfo member)
        {
            Contract.Require(member, nameof(member));

            switch (member)
            {
                case FieldInfo f:
                    return !f.IsInitOnly;
                case PropertyInfo p:
                    return p.CanWrite;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the type of the specified member.
        /// </summary>
        /// <param name="member">The member to evaluate.</param>
        /// <returns>The type of the specified member.</returns>
        public static Type GetMemberType(MemberInfo member)
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
        /// Gets the type of the specified binding expression.
        /// </summary>
        /// <param name="dataSourceType">The type of the data source to evaluate.</param>
        /// <param name="expression">The binding expression to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The type of the binding expression.</returns>
        public static Type GetExpressionType(Type dataSourceType, String expression, Boolean braces = true)
        {
            Contract.Require(dataSourceType, nameof(dataSourceType));
            Contract.RequireNotEmpty(expression, nameof(expression));

            var currentDataSourceType = dataSourceType;

            var expMemberPath = GetBindingMemberPathPart(expression, braces);
            var expMemberPathParts = expMemberPath.Contains(".") ? expMemberPath.Split('.') : null;
            var expMemberFinalPart = (expMemberPathParts == null) ? expMemberPath : expMemberPathParts[expMemberPathParts.Length - 1];

            if (expMemberPathParts != null && expMemberPathParts.Length > 1)
            {
                for (int i = 0; i < expMemberPathParts.Length - 1; i++)
                {
                    var nextMember = FindPropertyOrField(currentDataSourceType, expMemberPathParts[i]);
                    if (nextMember == null)
                        throw new InvalidOperationException(PresentationStrings.CannotResolveBindingExpression.Format(expression));

                    switch (nextMember)
                    {
                        case FieldInfo f:
                            currentDataSourceType = f.FieldType;
                            break;
                        case PropertyInfo p:
                            currentDataSourceType = p.PropertyType;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    currentDataSourceType = PresentationFoundation.Instance.GetDataSourceWrapperType(currentDataSourceType) ?? currentDataSourceType;
                }
            }

            var member = FindPropertyOrField(currentDataSourceType, expMemberFinalPart);

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
            Contract.Require(dataSource, nameof(dataSource));
            Contract.Require(dataSourceType, nameof(dataSourceType));
            Contract.Require(delegateType, nameof(delegateType));
            Contract.RequireNotEmpty(expression, nameof(expression));

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

            var result = default(Delegate);
            var precompiledDelegate = dataSourceType.GetField("__Get" + GetBindingMemberPathPart(expression));
            if (precompiledDelegate != null && precompiledDelegate.FieldType == typeof(DataBindingGetter<>).MakeGenericType(boundType) && precompiledDelegate.IsStatic)
                result = (Delegate)precompiledDelegate.GetValue(null);

            if (result == null)
                result = new DataBindingGetterBuilder(boundType, dataSourceType, expression).Compile();

            lock (cachedExpressionGetters)
                cachedExpressionGetters[key] = result;

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

            var result = default(Delegate);
            var precompiledDelegate = dataSourceType.GetField("__Set" + GetBindingMemberPathPart(expression));
            if (precompiledDelegate != null && precompiledDelegate.FieldType == typeof(DataBindingSetter<>).MakeGenericType(boundType) && precompiledDelegate.IsStatic)
                result = (Delegate)precompiledDelegate.GetValue(null);

            if (result == null)
                result = new DataBindingSetterBuilder(boundType, dataSourceType, expression).Compile();

            lock (cachedExpressionSetters)
                cachedExpressionSetters[key] = result;

            return result;
        }

        /// <summary>
        /// Gets the binding comparison function for the specified type.
        /// </summary>
        /// <param name="type">The type for which to create a binding comparison function.</param>
        /// <returns>The comparison function for the specified type.</returns>
        public static Delegate GetComparisonFunction(Type type)
        {
            if (!type.IsValueType && type != typeof(String))
                type = typeof(Object);

            lock (comparerRegistry)
            {
                var typeComparer = default(Delegate);

                if (!comparerRegistry.TryGetValue(type, out typeComparer))
                {
                    typeComparer = CreateComparisonFunction(type);
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
        /// <returns>The dependency property referenced by the specified expression, or <see langword="null"/> if the specified
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
        /// Creates and registers comparer delegates for common types.
        /// </summary>
        private static void RegisterPrecompiledComparisonDelegates()
        {
            // System
            comparerRegistry[typeof(Object)] = new DataBindingComparer<Object>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(String)] = GetStringComparisonFunction();
            comparerRegistry[typeof(Boolean)] = new DataBindingComparer<Boolean>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Boolean?)] = new DataBindingComparer<Boolean?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Char)] = new DataBindingComparer<Char>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Char?)] = new DataBindingComparer<Char?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Byte)] = new DataBindingComparer<Byte>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Byte?)] = new DataBindingComparer<Byte?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SByte)] = new DataBindingComparer<SByte>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SByte?)] = new DataBindingComparer<SByte?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Int16)] = new DataBindingComparer<Int16>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Int16?)] = new DataBindingComparer<Int16?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UInt16)] = new DataBindingComparer<UInt16>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UInt16?)] = new DataBindingComparer<UInt16?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Int32)] = new DataBindingComparer<Int32>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Int32?)] = new DataBindingComparer<Int32?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UInt32)] = new DataBindingComparer<UInt32>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UInt32?)] = new DataBindingComparer<UInt32?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Int64)] = new DataBindingComparer<Int64>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Int64?)] = new DataBindingComparer<Int64?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UInt64)] = new DataBindingComparer<UInt64>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UInt64?)] = new DataBindingComparer<UInt64?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Single)] = new DataBindingComparer<Single>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Single?)] = new DataBindingComparer<Single?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Double)] = new DataBindingComparer<Double>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Double?)] = new DataBindingComparer<Double?>((v1, v2) => v1 == v2);

            // Ultraviolet
            comparerRegistry[typeof(Circle)] = new DataBindingComparer<Circle>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Circle?)] = new DataBindingComparer<Circle?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CircleF)] = new DataBindingComparer<CircleF>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CircleF?)] = new DataBindingComparer<CircleF?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CircleD)] = new DataBindingComparer<CircleD>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CircleD?)] = new DataBindingComparer<CircleD?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Color)] = new DataBindingComparer<Color>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Color?)] = new DataBindingComparer<Color?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Matrix)] = new DataBindingComparer<Matrix>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Matrix?)] = new DataBindingComparer<Matrix?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Point2)] = new DataBindingComparer<Point2>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Point2?)] = new DataBindingComparer<Point2?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Point2F)] = new DataBindingComparer<Point2F>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Point2F?)] = new DataBindingComparer<Point2F?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Point2D)] = new DataBindingComparer<Point2D>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Point2D?)] = new DataBindingComparer<Point2D?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Radians)] = new DataBindingComparer<Radians>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Radians?)] = new DataBindingComparer<Radians?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Rectangle)] = new DataBindingComparer<Rectangle>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Rectangle?)] = new DataBindingComparer<Rectangle?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RectangleF)] = new DataBindingComparer<RectangleF>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RectangleF?)] = new DataBindingComparer<RectangleF?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RectangleD)] = new DataBindingComparer<RectangleD>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RectangleD?)] = new DataBindingComparer<RectangleD?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size2)] = new DataBindingComparer<Size2>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size2?)] = new DataBindingComparer<Size2?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size2F)] = new DataBindingComparer<Size2F>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size2F?)] = new DataBindingComparer<Size2F?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size2D)] = new DataBindingComparer<Size2D>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size2D?)] = new DataBindingComparer<Size2D?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size3)] = new DataBindingComparer<Size3>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size3?)] = new DataBindingComparer<Size3?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size3F)] = new DataBindingComparer<Size3F>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size3F?)] = new DataBindingComparer<Size3F?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size3D)] = new DataBindingComparer<Size3D>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Size3D?)] = new DataBindingComparer<Size3D?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Vector2)] = new DataBindingComparer<Vector2>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Vector2?)] = new DataBindingComparer<Vector2?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Vector3)] = new DataBindingComparer<Vector3>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Vector3?)] = new DataBindingComparer<Vector3?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Vector4)] = new DataBindingComparer<Vector4>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Vector4?)] = new DataBindingComparer<Vector4?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CurveContinuity)] = new DataBindingComparer<CurveContinuity>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CurveContinuity?)] = new DataBindingComparer<CurveContinuity?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CurveLoopType)] = new DataBindingComparer<CurveLoopType>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CurveLoopType?)] = new DataBindingComparer<CurveLoopType?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UltravioletPlatform)] = new DataBindingComparer<UltravioletPlatform>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UltravioletPlatform?)] = new DataBindingComparer<UltravioletPlatform?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UltravioletSingletonFlags)] = new DataBindingComparer<UltravioletSingletonFlags>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UltravioletSingletonFlags?)] = new DataBindingComparer<UltravioletSingletonFlags?>((v1, v2) => v1 == v2);

            // Ultraviolet.Audio
            comparerRegistry[typeof(PlaybackState)] = new DataBindingComparer<PlaybackState>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(PlaybackState?)] = new DataBindingComparer<PlaybackState?>((v1, v2) => v1 == v2);

            // Ultraviolet.Content
            comparerRegistry[typeof(AssetID)] = new DataBindingComparer<AssetID>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(AssetID?)] = new DataBindingComparer<AssetID?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(AssetFlags)] = new DataBindingComparer<AssetFlags>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(AssetFlags?)] = new DataBindingComparer<AssetFlags?>((v1, v2) => v1 == v2);

            // Ultraviolet.Input
            comparerRegistry[typeof(ButtonState)] = new DataBindingComparer<ButtonState>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ButtonState?)] = new DataBindingComparer<ButtonState?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GamePadAxis)] = new DataBindingComparer<GamePadAxis>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GamePadAxis?)] = new DataBindingComparer<GamePadAxis?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GamePadButton)] = new DataBindingComparer<GamePadButton>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GamePadButton?)] = new DataBindingComparer<GamePadButton?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GamePadJoystick)] = new DataBindingComparer<GamePadJoystick>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GamePadJoystick?)] = new DataBindingComparer<GamePadJoystick?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GamePadJoystickDirection)] = new DataBindingComparer<GamePadJoystickDirection>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GamePadJoystickDirection?)] = new DataBindingComparer<GamePadJoystickDirection?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Key)] = new DataBindingComparer<Key>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Key?)] = new DataBindingComparer<Key?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(KeyboardMode)] = new DataBindingComparer<KeyboardMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(KeyboardMode?)] = new DataBindingComparer<KeyboardMode?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MouseButton)] = new DataBindingComparer<MouseButton>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MouseButton?)] = new DataBindingComparer<MouseButton?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Scancode)] = new DataBindingComparer<Scancode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Scancode?)] = new DataBindingComparer<Scancode?>((v1, v2) => v1 == v2);

            // Ultraviolet.Graphics
            comparerRegistry[typeof(Blend)] = new DataBindingComparer<Blend>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Blend?)] = new DataBindingComparer<Blend?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(BlendFunction)] = new DataBindingComparer<BlendFunction>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(BlendFunction?)] = new DataBindingComparer<BlendFunction?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(BlurDirection)] = new DataBindingComparer<BlurDirection>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(BlurDirection?)] = new DataBindingComparer<BlurDirection?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ColorWriteChannels)] = new DataBindingComparer<ColorWriteChannels>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ColorWriteChannels?)] = new DataBindingComparer<ColorWriteChannels?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CompareFunction)] = new DataBindingComparer<CompareFunction>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CompareFunction?)] = new DataBindingComparer<CompareFunction?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CompositionContext)] = new DataBindingComparer<CompositionContext>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CompositionContext?)] = new DataBindingComparer<CompositionContext?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CullMode)] = new DataBindingComparer<CullMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CullMode?)] = new DataBindingComparer<CullMode?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(FillMode)] = new DataBindingComparer<FillMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(FillMode?)] = new DataBindingComparer<FillMode?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(PrimitiveType)] = new DataBindingComparer<PrimitiveType>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(PrimitiveType?)] = new DataBindingComparer<PrimitiveType?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RenderBufferFormat)] = new DataBindingComparer<RenderBufferFormat>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RenderBufferFormat?)] = new DataBindingComparer<RenderBufferFormat?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RenderBufferOptions)] = new DataBindingComparer<RenderBufferOptions>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RenderBufferOptions?)] = new DataBindingComparer<RenderBufferOptions?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RenderTargetUsage)] = new DataBindingComparer<RenderTargetUsage>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(RenderTargetUsage?)] = new DataBindingComparer<RenderTargetUsage?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SetDataOptions)] = new DataBindingComparer<SetDataOptions>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SetDataOptions?)] = new DataBindingComparer<SetDataOptions?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(StencilOperation)] = new DataBindingComparer<StencilOperation>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(StencilOperation?)] = new DataBindingComparer<StencilOperation?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SurfaceSourceDataFormat)] = new DataBindingComparer<SurfaceSourceDataFormat>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SurfaceSourceDataFormat?)] = new DataBindingComparer<SurfaceSourceDataFormat?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextureAddressMode)] = new DataBindingComparer<TextureAddressMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextureAddressMode?)] = new DataBindingComparer<TextureAddressMode?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextureFilter)] = new DataBindingComparer<TextureFilter>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextureFilter?)] = new DataBindingComparer<TextureFilter?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(VertexElementFormat)] = new DataBindingComparer<VertexElementFormat>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(VertexElementFormat?)] = new DataBindingComparer<VertexElementFormat?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(VertexElementUsage)] = new DataBindingComparer<VertexElementUsage>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(VertexElementUsage?)] = new DataBindingComparer<VertexElementUsage?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Viewport)] = new DataBindingComparer<Viewport>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Viewport?)] = new DataBindingComparer<Viewport?>((v1, v2) => v1 == v2);

            // Ultraviolet.Graphics.Graphics2D
            comparerRegistry[typeof(UltravioletFontStyle)] = new DataBindingComparer<UltravioletFontStyle>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(UltravioletFontStyle?)] = new DataBindingComparer<UltravioletFontStyle?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SpriteAnimationID)] = new DataBindingComparer<SpriteAnimationID>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SpriteAnimationID?)] = new DataBindingComparer<SpriteAnimationID?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SpriteAnimationName)] = new DataBindingComparer<SpriteAnimationName>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SpriteAnimationName?)] = new DataBindingComparer<SpriteAnimationName?>((v1, v2) => v1 == v2);

            // Ultraviolet.Graphics.Graphics2D.Text
            comparerRegistry[typeof(TextFlags)] = new DataBindingComparer<TextFlags>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextFlags?)] = new DataBindingComparer<TextFlags?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextLayoutCommandType)] = new DataBindingComparer<TextLayoutCommandType>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextLayoutCommandType?)] = new DataBindingComparer<TextLayoutCommandType?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextLayoutOptions)] = new DataBindingComparer<TextLayoutOptions>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextLayoutOptions?)] = new DataBindingComparer<TextLayoutOptions?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextParserOptions)] = new DataBindingComparer<TextParserOptions>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextParserOptions?)] = new DataBindingComparer<TextParserOptions?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextParserTokenType)] = new DataBindingComparer<TextParserTokenType>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextParserTokenType?)] = new DataBindingComparer<TextParserTokenType?>((v1, v2) => v1 == v2);

            // Ultraviolet.Platform
            comparerRegistry[typeof(ScreenDensityBucket)] = new DataBindingComparer<ScreenDensityBucket>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ScreenDensityBucket?)] = new DataBindingComparer<ScreenDensityBucket?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ScreenRotation)] = new DataBindingComparer<ScreenRotation>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ScreenRotation?)] = new DataBindingComparer<ScreenRotation?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(WindowFlags)] = new DataBindingComparer<WindowFlags>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(WindowFlags?)] = new DataBindingComparer<WindowFlags?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(WindowMode)] = new DataBindingComparer<WindowMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(WindowMode?)] = new DataBindingComparer<WindowMode?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(WindowState)] = new DataBindingComparer<WindowState>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(WindowState?)] = new DataBindingComparer<WindowState?>((v1, v2) => v1 == v2);

            // Ultraviolet.Presentation
            comparerRegistry[typeof(GridLength)] = new DataBindingComparer<GridLength>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GridLength?)] = new DataBindingComparer<GridLength?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedAssetID)] = new DataBindingComparer<SourcedAssetID>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedAssetID?)] = new DataBindingComparer<SourcedAssetID?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedCursor)] = new DataBindingComparer<SourcedCursor>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedCursor?)] = new DataBindingComparer<SourcedCursor?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedImage)] = new DataBindingComparer<SourcedImage>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedImage?)] = new DataBindingComparer<SourcedImage?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedResource<Graphics.Graphics2D.Sprite>)] = new DataBindingComparer<SourcedResource<Graphics.Graphics2D.Sprite>>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedResource<Graphics.Graphics2D.Sprite>?)] = new DataBindingComparer<SourcedResource<Graphics.Graphics2D.Sprite>?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedResource<UltravioletFont>)] = new DataBindingComparer<SourcedResource<UltravioletFont>>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedResource<UltravioletFont>?)] = new DataBindingComparer<SourcedResource<UltravioletFont>?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedSpriteAnimationID)] = new DataBindingComparer<SourcedSpriteAnimationID>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SourcedSpriteAnimationID?)] = new DataBindingComparer<SourcedSpriteAnimationID?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Thickness)] = new DataBindingComparer<Thickness>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Thickness?)] = new DataBindingComparer<Thickness?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(VersionedStringSource)] = new DataBindingComparer<VersionedStringSource>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(VersionedStringSource?)] = new DataBindingComparer<VersionedStringSource?>((v1, v2) => v1 == v2);

            // Ultraviolet.Presentation.Controls.Primitives
            comparerRegistry[typeof(PlacementMode)] = new DataBindingComparer<PlacementMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(PlacementMode?)] = new DataBindingComparer<PlacementMode?>((v1, v2) => v1 == v2);

            // Ultraviolet.Presentation.Controls
            comparerRegistry[typeof(CharacterCasing)] = new DataBindingComparer<CharacterCasing>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CharacterCasing?)] = new DataBindingComparer<CharacterCasing?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ClickMode)] = new DataBindingComparer<ClickMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ClickMode?)] = new DataBindingComparer<ClickMode?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Dock)] = new DataBindingComparer<Dock>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Dock?)] = new DataBindingComparer<Dock?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Orientation)] = new DataBindingComparer<Orientation>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Orientation?)] = new DataBindingComparer<Orientation?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ScrollBarVisibility)] = new DataBindingComparer<ScrollBarVisibility>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ScrollBarVisibility?)] = new DataBindingComparer<ScrollBarVisibility?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SelectionMode)] = new DataBindingComparer<SelectionMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(SelectionMode?)] = new DataBindingComparer<SelectionMode?>((v1, v2) => v1 == v2);

            // Ultraviolet.Presentation.Input
            comparerRegistry[typeof(CaptureMode)] = new DataBindingComparer<CaptureMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(CaptureMode?)] = new DataBindingComparer<CaptureMode?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(FocusNavigationDirection)] = new DataBindingComparer<FocusNavigationDirection?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(FocusNavigationDirection?)] = new DataBindingComparer<FocusNavigationDirection?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(KeyboardNavigationMode)] = new DataBindingComparer<KeyboardNavigationMode>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(KeyboardNavigationMode?)] = new DataBindingComparer<KeyboardNavigationMode?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ModifierKeys)] = new DataBindingComparer<ModifierKeys>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ModifierKeys?)] = new DataBindingComparer<ModifierKeys?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MouseButtonState)] = new DataBindingComparer<MouseButtonState>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MouseButtonState?)] = new DataBindingComparer<MouseButtonState?>((v1, v2) => v1 == v2);

            // Ultraviolet Presentation Foundation types
            comparerRegistry[typeof(ArrangeOptions)] = new DataBindingComparer<ArrangeOptions>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(ArrangeOptions?)] = new DataBindingComparer<ArrangeOptions?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GridUnitType)] = new DataBindingComparer<GridUnitType>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(GridUnitType?)] = new DataBindingComparer<GridUnitType?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(HorizontalAlignment)] = new DataBindingComparer<HorizontalAlignment>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(HorizontalAlignment?)] = new DataBindingComparer<HorizontalAlignment?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MessageBoxButton)] = new DataBindingComparer<MessageBoxButton>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MessageBoxButton?)] = new DataBindingComparer<MessageBoxButton?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MessageBoxImage)] = new DataBindingComparer<MessageBoxImage>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MessageBoxImage?)] = new DataBindingComparer<MessageBoxImage?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MessageBoxResult)] = new DataBindingComparer<MessageBoxResult>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(MessageBoxResult?)] = new DataBindingComparer<MessageBoxResult?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextAlignment)] = new DataBindingComparer<TextAlignment>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextAlignment?)] = new DataBindingComparer<TextAlignment?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextWrapping)] = new DataBindingComparer<TextWrapping>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(TextWrapping?)] = new DataBindingComparer<TextWrapping?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(VerticalAlignment)] = new DataBindingComparer<VerticalAlignment>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(VerticalAlignment?)] = new DataBindingComparer<VerticalAlignment?>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Visibility)] = new DataBindingComparer<Visibility>((v1, v2) => v1 == v2);
            comparerRegistry[typeof(Visibility?)] = new DataBindingComparer<Visibility?>((v1, v2) => v1 == v2);
        }

        /// <summary>
        /// Gets a comparison function for strings.
        /// </summary>
        /// <returns>The comparison function for strings.</returns>
        public static Delegate GetStringComparisonFunction()
        {
            return new DataBindingComparer<Object>((o1, o2) => String.Equals((String)o1, (String)o2, StringComparison.Ordinal));
        }
        
        /// <summary>
        /// Creates a new comparison function delegate for the specified type.
        /// </summary>
        private static Delegate CreateComparisonFunction(Type type)
        {
            if (type == typeof(String))
                return GetStringComparisonFunction();

            if (type.IsClass)
                return GetReferenceComparisonFunction(type);

            if (UltravioletPlatformInfo.IsRuntimeCodeGenerationSupported())
            {
                if (type.GetInterfaces().Where(x => x == typeof(IEquatable<>).MakeGenericType(type)).Any())
                    return GetIEquatableComparisonFunction(type);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return GetNullableComparisonFunction(type);
            }

            if (type.IsEnum)
                return GetEnumComparisonFunction(type);

            return GetFallbackComparisonFunction(type);
        }

        /// <summary>
        /// Gets a comparison function for reference types.
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
            var arg1 = param1;
            var arg2 = Expression.Convert(param2, typeof(Object));

            var delegateType = typeof(DataBindingComparer<>).MakeGenericType(type);
            return Expression.Lambda(delegateType, Expression.Call(miObjectEquals,
                Expression.Convert(arg1, typeof(Object)), Expression.Convert(arg2, typeof(Object))), param1, param2).Compile();
        }

        /// <summary>
        /// Gets the part of a binding expression which represents the member navigation path.
        /// </summary>
        /// <param name="expression">The expression string to evaluate.</param>
        /// <param name="braces">A value indicating whether the binding expression includes its enclosing braces.</param>
        /// <returns>The part of the binding expression which represents the member navigation path, or <see langword="null"/> if no such part exists.</returns>
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
        /// <returns>The part of the binding expression which represents the format string, or <see langword="null"/> if no such part exists.</returns>
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

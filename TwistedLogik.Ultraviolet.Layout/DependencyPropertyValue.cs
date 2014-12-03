using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TwistedLogik.Nucleus;
using System.Reflection;

namespace TwistedLogik.Ultraviolet.Layout
{
    /// <summary>
    /// Represents a method which is used to retrieve the value of a data bound property.
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <returns>The current value of the bound property.</returns>
    internal delegate T DataBindingGetter<T>(Object model);

    /// <summary>
    /// Represents a method 
    /// </summary>
    /// <typeparam name="T">The type of the property which was bound.</typeparam>
    /// <param name="model">The model object for the current binding context.</param>
    /// <param name="value">The value to set on the bound property.</param>
    internal delegate void DataBindingSetter<T>(Object model, T value);

    /// <summary>
    /// Represents the value contained by a dependency property.
    /// </summary>
    internal class DependencyPropertyValue<T> : IDependencyPropertyValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyValue{T}"/> class.
        /// </summary>
        /// <param name="owner">The dependency object that owns the property value.</param>
        /// <param name="property">The dependency property which has its value represented by this object.</param>
        public DependencyPropertyValue(DependencyObject owner, DependencyProperty property)
        {
            Contract.Require(owner, "owner");
            Contract.Require(property, "property");

            this.owner    = owner;
            this.property = property;
            this.comparer = GetComparisonFunction();
        }

        /// <summary>
        /// Binds the dependency property to the specified model.
        /// </summary>
        /// <param name="model">The model object to which to bind the dependency property.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        public void Bind(Object model, String expression)
        {
            Contract.Require(model, "model");
            Contract.RequireNotEmpty(expression, "expression");

            dataBindingModel  = model;
            dataBindingGetter = CreateBindingGetter(model, expression);
            dataBindingSetter = CreateBindingSetter(model, expression);
        }

        /// <summary>
        /// Removes the dependency property's two-way binding.
        /// </summary>
        public void Unbind()
        {
            dataBindingModel  = null;
            dataBindingGetter = null;
            dataBindingSetter = null;
        }

        /// <inheritdoc/>
        public void Digest()
        {
            var currentValue = GetValue();
            if (!comparer(currentValue, previousValue))
            {
                if (Property.Metadata.ChangedCallback != null)
                {
                    Property.Metadata.ChangedCallback(Owner);
                }
                previousValue = currentValue;
            }
        }

        /// <inheritdoc/>
        public void ClearLocalValue()
        {
            hasLocalValue = false;
        }

        /// <inheritdoc/>
        public void ClearStyledValue()
        {
            hasStyledValue = false;
        }

        /// <summary>
        /// Sets the dependency property's value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public void SetValue(T value)
        {
            if (IsDataBound)
            {
                dataBindingSetter(dataBindingModel, value);
            }
            else
            {
                LocalValue = value;
            }
        }

        /// <summary>
        /// Gets the dependency property's calculated value.
        /// </summary>
        /// <returns>The dependency property's calculated value.</returns>
        public T GetValue()
        {
            if (IsDataBound)
            {
                return dataBindingGetter(dataBindingModel);
            }
            if (hasLocalValue)
            {
                return localValue;
            }
            if (hasStyledValue)
            {
                return styledValue;
            }
            if (Property.Metadata.IsInherited && Owner.DependencyContainer != null)
            {
                return Owner.DependencyContainer.GetValue<T>(Property);
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the dependency property's local value.
        /// </summary>
        public T LocalValue
        {
            get { return localValue; }
            internal set
            {
                localValue = value;
                hasLocalValue = true;
            }
        }

        /// <summary>
        /// Gets the dependency property's styled value.
        /// </summary>
        public T StyledValue
        {
            get { return styledValue; }
            internal set
            {
                styledValue = value;
                hasStyledValue = true;
            }
        }

        /// <summary>
        /// Gets or sets the dependency property's default value.
        /// </summary>
        public T DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// Gets the dependency property's previous value as of the last call to <see cref="Digest()"/>.
        /// </summary>
        public T PreviousValue
        {
            get { return previousValue; }
        }

        /// <inheritdoc/>
        public DependencyObject Owner
        {
            get { return owner; }
        }

        /// <inheritdoc/>
        public DependencyProperty Property
        {
            get { return property; }
        }

        /// <inheritdoc/>
        public Boolean IsDataBound
        {
            get { return dataBindingModel != null; }
        }

        /// <summary>
        /// Creates a getter for the specified binding expression.
        /// </summary>
        /// <param name="model">The model object to which to bind the dependency property.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        /// <returns>A <see cref="DataBindingGetter{T}"/> that represents the specified model and expression.</returns>
        private static DataBindingGetter<T> CreateBindingGetter(Object model, String expression)
        {
            var expressionComponents = ParseBindingExpression(expression);

            var contextParameter  = Expression.Parameter(typeof(Object), "context");
            var contextExpression = Expression.Convert(contextParameter, model.GetType());
            var currentExpression = (Expression)contextExpression;

            foreach (var component in expressionComponents)
            {
                currentExpression = Expression.PropertyOrField(currentExpression, component);
            }

            var lambdaBody = Expression.Convert(currentExpression, typeof(T));
            var lambda     = Expression.Lambda<DataBindingGetter<T>>(lambdaBody, contextParameter).Compile();

            return lambda;
        }

        /// <summary>
        /// Creates a setter for the specified binding expression.
        /// </summary>
        /// <param name="model">The model object to which to bind the dependency property.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        /// <returns>A <see cref="DataBindingSetter{T}"/> that represents the specified model and expression.</returns>
        private static DataBindingSetter<T> CreateBindingSetter(Object model, String expression)
        {
            var expressionComponents = ParseBindingExpression(expression);

            var contextParameter  = Expression.Parameter(typeof(Object), "context");
            var contextExpression = Expression.Convert(contextParameter, model.GetType());
            var currentExpression = (Expression)contextExpression;
            var valueParameter    = Expression.Parameter(typeof(T), "value");

            for (int i = 0; i < expressionComponents.Length; i++)
            {
                if (i + 1 < expressionComponents.Length)
                {
                    currentExpression = Expression.PropertyOrField(currentExpression, expressionComponents[i]);
                }
                else
                {
                    if (currentExpression.Type.IsValueType)
                    {
                        throw new InvalidOperationException(LayoutStrings.BindingAssignmentToValueType.Format(expression));
                    }
                    
                    var propertyOrField = Expression.PropertyOrField(currentExpression, expressionComponents[i]);
                    var propertyOrFieldType = (propertyOrField.Member.MemberType == MemberTypes.Property) ?
                        ((PropertyInfo)propertyOrField.Member).PropertyType : ((FieldInfo)propertyOrField.Member).FieldType;

                    currentExpression = Expression.Assign(propertyOrField, Expression.Convert(valueParameter, propertyOrFieldType));
                }
            }

            var lambda = Expression.Lambda<DataBindingSetter<T>>(currentExpression, contextParameter, valueParameter).Compile();

            return lambda;
        }

        /// <summary>
        /// Parses the specified binding expression into its constituent components.
        /// </summary>
        /// <param name="expression">The binding expression to parse.</param>
        /// <returns>The specified binding expression's constituent components.</returns>
        private static String[] ParseBindingExpression(String expression)
        {
            if (!expression.StartsWith("{{") || !expression.EndsWith("}}"))
                throw new ArgumentException(LayoutStrings.InvalidBindingExpression.Format(expression));

            var code       = expression.Substring("{{".Length, expression.Length - "{{}}".Length);
            var components = code.Split('.');

            return components;
        }

        /// <summary>
        /// Gets the comparison function for the current type.
        /// </summary>
        /// <returns>The comparison function for the current type.</returns>
        private static Func<T, T, Boolean> GetComparisonFunction()
        {
            if (typeof(T).IsClass)
            {
                return referenceComparer;
            }
            else
            {
                lock (comparerRegistry)
                {
                    var typeHandle   = typeof(T).TypeHandle.Value.ToInt64();
                    var typeComparer = default(Func<T, T, Boolean>);

                    if (!comparerRegistry.TryGetValue(typeHandle, out typeComparer))
                    {
                        if (typeof(T).GetInterfaces().Where(x => x == typeof(IEquatable<T>)).Any())
                        {
                            typeComparer = GetIEquatableComparisonFunction();
                        }
                        else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            typeComparer = GetNullableComparisonFunction();
                        }
                        else
                        {
                            typeComparer = GetFallbackComparisonFunction();
                        }
                        comparerRegistry[typeHandle] = typeComparer;
                    }

                    return typeComparer;
                }
            }
        }

        /// <summary>
        /// Gets a fallback comparison function for value types which implement <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <returns>The comparison function for the dependency property value's type.</returns>
        private static Func<T, T, Boolean> GetIEquatableComparisonFunction()
        {
            return (o1, o2) =>
            {
                return ((IEquatable<T>)o1).Equals(o2);
            };
        }

        /// <summary>
        /// Gets a comparison function for nullable value types.
        /// </summary>
        /// <returns>The comparison function for the dependency property value's type.</returns>
        private static Func<T, T, Boolean> GetNullableComparisonFunction()
        {
            var nullableType = typeof(T).GetGenericArguments()[0];
            var nullableEqualsMethod = typeof(Nullable).GetMethods()
                .Where(x => x.Name == "Equals" && x.IsGenericMethod)
                .Single().MakeGenericMethod(nullableType);

            var arg1 = Expression.Parameter(typeof(T), "o1");
            var arg2 = Expression.Parameter(typeof(T), "o2");

            return Expression.Lambda<Func<T, T, Boolean>>(
                Expression.Call(nullableEqualsMethod, arg1, arg2), arg1, arg2).Compile();
        }

        /// <summary>
        /// Gets a fallback comparison function for types which don't fit any optimizable category.
        /// </summary>
        /// <returns>The comparison function for the dependency property value's type.</returns>
        private static Func<T, T, Boolean> GetFallbackComparisonFunction()
        {
            return (o1, o2) =>
            {
                return o1.Equals(o2);
            };
        }

        // Property values.
        private readonly DependencyObject owner;
        private readonly DependencyProperty property;
        private Boolean hasLocalValue;
        private Boolean hasStyledValue;
        private T localValue;
        private T styledValue;
        private T defaultValue;
        private T previousValue;

        // State values.
        private Object dataBindingModel;
        private DataBindingGetter<T> dataBindingGetter;
        private DataBindingSetter<T> dataBindingSetter;

        // Comparison functions for various types.
        private static readonly Dictionary<Int64, Func<T, T, Boolean>> comparerRegistry = new Dictionary<Int64, Func<T, T, Boolean>>();
        private static readonly Func<T, T, Boolean> referenceComparer = (o1, o2) => { return (Object)o1 == (Object)o2; };
        private Func<T, T, Boolean> comparer;
    }
}

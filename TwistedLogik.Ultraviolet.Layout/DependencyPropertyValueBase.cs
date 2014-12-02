using System;
using System.Linq.Expressions;
using TwistedLogik.Nucleus;

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
    /// Represents the base class for dependency property value wrappers.
    /// </summary>
    internal abstract class DependencyPropertyValueBase<T> : IDependencyPropertyValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyValueBase{T}"/> class.
        /// </summary>
        /// <param name="owner">The dependency object that owns the property value.</param>
        /// <param name="property">The dependency property which has its value represented by this object.</param>
        public DependencyPropertyValueBase(DependencyObject owner, DependencyProperty property)
        {
            Contract.Require(owner, "owner");
            Contract.Require(property, "property");

            this.owner    = owner;
            this.property = property;
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
        public abstract void Digest();

        /// <inheritdoc/>
        public abstract void ClearLocalValue();

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
        /// Gets the value of the property which is bound to this dependency property.
        /// </summary>
        /// <returns>The value of the bound property.</returns>
        protected T GetBoundValue()
        {
            return dataBindingGetter(dataBindingModel);
        }

        /// <summary>
        /// Sets the value of the property which is bound to this dependency property.
        /// </summary>
        /// <param name="value">The value to set on the bound property.</param>
        protected void SetBoundValue(T value)
        {
            dataBindingSetter(dataBindingModel, value);
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

            var lambda = Expression.Lambda<DataBindingGetter<T>>(currentExpression, contextParameter).Compile();

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
                    currentExpression = Expression.Assign(Expression.PropertyOrField(currentExpression, expressionComponents[i]), valueParameter);
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

        // Property values.
        private readonly DependencyObject owner;
        private readonly DependencyProperty property;

        // State values.
        private Object dataBindingModel;
        private DataBindingGetter<T> dataBindingGetter;
        private DataBindingSetter<T> dataBindingSetter;
    }
}

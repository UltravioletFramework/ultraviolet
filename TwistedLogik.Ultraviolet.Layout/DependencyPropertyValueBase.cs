using System;
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
        /// Initializes a new instance of the <see cref="DependencyPropertyValueBase"/> class.
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
        /// Gets a value indicating whether the dependency property is bound to a property on a model object.
        /// </summary>
        protected Boolean IsDataBound
        {
            get { return dataBindingModel != null; }
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

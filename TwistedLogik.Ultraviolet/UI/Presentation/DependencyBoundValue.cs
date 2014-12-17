using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a value which is bound to a dependency property.
    /// </summary>
    /// <typeparam name="T">The type of the bound value.</typeparam>
    internal abstract class DependencyBoundValue<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyBoundValue{TDependency}"/> class.
        /// </summary>
        /// <param name="value">The dependency property value which created this object.</param>
        /// <param name="expressionType">The type of the bound expression.</param>
        /// <param name="viewModelType">The type of the view model.</param>
        /// <param name="expression">The binding expression.</param>
        public DependencyBoundValue(IDependencyPropertyValue value, Type expressionType, Type viewModelType, String expression)
        {
            this.dependencyValue = value;
            this.getter          = (DataBindingGetter<T>)BindingExpressions.CreateBindingGetter(expressionType, viewModelType, expression);
            this.setter          = (DataBindingSetter<T>)BindingExpressions.CreateBindingSetter(expressionType, viewModelType, expression);
            this.comparer        = (DataBindingComparer<T>)BindingExpressions.GetComparisonFunction(expressionType);
            this.forceUpdate     = true;
        }

        /// <inheritdoc/>
        public Boolean CheckHasChanged()
        {
            var value = GetUnderlyingValue();
            if (forceUpdate || !comparer(cachedValue, value))
            {
                forceUpdate = false;
                cachedValue = value;
                OnCachedValueChanged(value);
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public Boolean IsReadable
        {
            get { return getter != null; }
        }

        /// <inheritdoc/>
        public Boolean IsWritable
        {
            get { return setter != null; }
        }

        /// <summary>
        /// Occurs when the cached value changes.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected virtual void OnCachedValueChanged(T value)
        {

        }

        /// <summary>
        /// Gets the cached value.
        /// </summary>
        /// <returns>The cached value.</returns>
        protected T GetCachedValue()
        {
            return cachedValue;
        }

        /// <summary>
        /// Gets the underlying bound value.
        /// </summary>
        /// <returns>The underlying bound value.</returns>
        protected T GetUnderlyingValue()
        {
            if (!IsReadable)
            {
                throw new InvalidOperationException(UltravioletStrings.BindingIsReadOnly);
            }
            return getter(dependencyValue.Owner.DependencyDataSource);
        }

        /// <summary>
        /// Sets the underlying bound value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        protected void SetUnderlyingValue(T value)
        {
            if (!IsWritable)
            {
                throw new InvalidOperationException(UltravioletStrings.BindingIsReadOnly);
            }
            setter(dependencyValue.Owner.DependencyDataSource, value);
        }

        /// <summary>
        /// Immediately digests the dependency property.
        /// </summary>
        protected void Digest()
        {
            dependencyValue.Digest(null);
        }

        // State values.
        private readonly IDependencyPropertyValue dependencyValue;
        private readonly DataBindingComparer<T> comparer;
        private readonly DataBindingGetter<T> getter;
        private readonly DataBindingSetter<T> setter;
        private T cachedValue;
        private Boolean forceUpdate;
    }
}

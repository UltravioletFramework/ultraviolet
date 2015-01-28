using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a value which is bound to a dependency property and which does not need
    /// to perform any special type conversion logic.
    /// </summary>
    /// <typeparam name="TDependency">The type of the dependency property.</typeparam>
    internal sealed class DependencyBoundValueNonConverting<TDependency> : DependencyBoundValue<TDependency>, IDependencyBoundValue<TDependency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyBoundValueNonConverting{TDependency}"/> class.
        /// </summary>
        /// <param name="value">The dependency property value which created this object.</param>
        /// <param name="expressionType">The type of the bound expression.</param>
        /// <param name="viewModelType">The type of the view model.</param>
        /// <param name="expression">The binding expression.</param>
        public DependencyBoundValueNonConverting(IDependencyPropertyValue value, Type expressionType, Type viewModelType, String expression)
            : base(value, expressionType, viewModelType, expression)
        {

        }

        /// <inheritdoc/>
        public TDependency Get()
        {
            return GetCachedValue();
        }

        /// <inheritdoc/>
        public TDependency GetFresh()
        {
            return GetUnderlyingValue();
        }

        /// <inheritdoc/>
        public void Set(TDependency value)
        {
            SetUnderlyingValue(value);
        }
    }
}

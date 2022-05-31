using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a delegate which produces instances of the <see cref="DependencyBoundValueNonConverting{TDependency}"/> class.
    /// </summary>
    /// <typeparam name="TDependency">The type of the dependency property.</typeparam>
    /// <param name="value">The dependency property value which created this object.</param>
    /// <param name="expressionType">The type of the bound expression.</param>
    /// <param name="dataSourceType">The type of the data source.</param>
    /// <param name="expression">The binding expression.</param>
    /// <returns>The bound value instance which was created.</returns>
    internal delegate IDependencyBoundValue<TDependency> DependencyBoundValueNonConvertingCtor<TDependency>(
        IDependencyPropertyValue value, Type expressionType, Type dataSourceType, String expression);

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
        /// <param name="dataSourceType">The type of the data source.</param>
        /// <param name="expression">The binding expression.</param>
        public DependencyBoundValueNonConverting(IDependencyPropertyValue value, Type expressionType, Type dataSourceType, String expression)
            : base(value, expressionType, dataSourceType, expression)
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

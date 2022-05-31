using System;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents a key which identifies a particular binding expression getter or setter.
    /// </summary>
    internal partial struct BindingExpressionAccessorKey : IEquatable<BindingExpressionAccessorKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingExpressionAccessorKey"/> structure.
        /// </summary>
        /// <param name="boundType">The type of value to which the expression is being bound.</param>
        /// <param name="dataSourceType">The type of the data source to which the expression is being bound.</param>
        /// <param name="expression">The binding expression with which to bind the dependency property.</param>
        public BindingExpressionAccessorKey(Type boundType, Type dataSourceType, String expression)
        {
            this.BoundType = boundType;
            this.DataSourceType = dataSourceType;
            this.Expression = expression;
        }
        
        /// <summary>
        /// Gets the type of value to which the expression is being bound.
        /// </summary>
        public Type BoundType { get; }

        /// <summary>
        /// Gets the type of the data source to which the expression is being bound.
        /// </summary>
        public Type DataSourceType { get; }

        /// <summary>
        /// Gets the binding expression with which to bind the dependency property.
        /// </summary>
        public String Expression { get; }
    }
}

using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Represents a key which identifies a particular binding expression getter or setter.
    /// </summary>
    internal struct BindingExpressionAccessorKey : IEquatable<BindingExpressionAccessorKey>
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
        /// Compares two keys for equality.
        /// </summary>
        /// <param name="key1">The first <see cref="BindingExpressionAccessorKey"/> to compare.</param>
        /// <param name="key2">The second <see cref="BindingExpressionAccessorKey"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified keys are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(BindingExpressionAccessorKey key1, BindingExpressionAccessorKey key2) =>
            key1.Equals(key2);

        /// <summary>
        /// Compares two keys for inequality.
        /// </summary>
        /// <param name="key1">The first <see cref="BindingExpressionAccessorKey"/> to compare.</param>
        /// <param name="key2">The second <see cref="BindingExpressionAccessorKey"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified keys are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(BindingExpressionAccessorKey key1, BindingExpressionAccessorKey key2) =>
            !key2.Equals(key2);

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is BindingExpressionAccessorKey))
                return false;

            return Equals((BindingExpressionAccessorKey)obj);
        }

        /// <inheritdoc/>
        public Boolean Equals(BindingExpressionAccessorKey other)
        {
            return
                BoundType == other.BoundType &&
                DataSourceType == other.DataSourceType &&
                Expression == other.Expression;
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + BoundType.GetHashCode();
                hash = hash * 23 + DataSourceType.GetHashCode();
                hash = hash * 23 + Expression.GetHashCode();
                return hash;
            }
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

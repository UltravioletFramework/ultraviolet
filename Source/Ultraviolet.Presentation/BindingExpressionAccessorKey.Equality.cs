using System;

namespace Ultraviolet.Presentation
{
    partial struct BindingExpressionAccessorKey
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + BoundType?.GetHashCode() ?? 0;
                hash = hash * 23 + DataSourceType?.GetHashCode() ?? 0;
                hash = hash * 23 + Expression?.GetHashCode() ?? 0;
                return hash;
            }
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(BindingExpressionAccessorKey v1, BindingExpressionAccessorKey v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(BindingExpressionAccessorKey v1, BindingExpressionAccessorKey v2)
        {
            return !v1.Equals(v2);
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is BindingExpressionAccessorKey x) ? Equals(x) : false;
        }

        /// <inheritdoc/>
        public Boolean Equals(BindingExpressionAccessorKey other)
        {
            return
                BoundType == other.BoundType &&
                DataSourceType == other.DataSourceType &&
                Expression == other.Expression;
        }
    }
}
using System;

namespace Ultraviolet.Presentation.Styles
{
    partial struct NavigationExpression
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + propertyName.GetHashCode();
                hash = hash * 23 + propertyType?.GetHashCode() ?? 0;
                hash = hash * 23 + propertyIndex.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compares two <see cref="NavigationExpression"/> values for equality.
        /// </summary>
        /// <param name="exp1">The first <see cref="NavigationExpression"/> to compare.</param>
        /// <param name="exp2">The second <see cref="NavigationExpression"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified expressions are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(NavigationExpression exp1, NavigationExpression exp2)
        {
            return exp1.Equals(exp2);
        }

        /// <summary>
        /// Compares two <see cref="NavigationExpression"/> values for inequality.
        /// </summary>
        /// <param name="exp1">The first <see cref="NavigationExpression"/> to compare.</param>
        /// <param name="exp2">The second <see cref="NavigationExpression"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified expressions are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(NavigationExpression exp1, NavigationExpression exp2)
        {
            return !exp1.Equals(exp2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is NavigationExpression))
            {
                return false;
            }
            return Equals((NavigationExpression)obj);
        }

        /// <inheritdoc/>
        public Boolean Equals(NavigationExpression other)
        {
            return
                propertyName.Equals(other.propertyName) &&
                propertyType == other.propertyType &&
                propertyIndex.HasValue == other.propertyIndex.HasValue &&
                propertyIndex.GetValueOrDefault() == other.propertyIndex.GetValueOrDefault();
        }
    }
}

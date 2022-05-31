using System;

namespace Ultraviolet.Presentation
{
    partial struct Thickness
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + top.GetHashCode();
                hash = hash * 23 + left.GetHashCode();
                hash = hash * 23 + right.GetHashCode();
                hash = hash * 23 + bottom.GetHashCode();
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(Thickness v1, Thickness v2)
        {
            return v1.Equals(v2);
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(Thickness v1, Thickness v2)
        {
            return !v1.Equals(v2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is Thickness x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(Thickness other)
        {
            return
                left == other.left &&
                top == other.top &&
                right == other.right &&
                bottom == other.bottom;
        }
    }
}

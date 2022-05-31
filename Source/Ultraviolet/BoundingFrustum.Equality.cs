using System;

namespace Ultraviolet
{
    partial class BoundingFrustum
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            return matrix.GetHashCode();
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(BoundingFrustum v1, BoundingFrustum v2)
        {
            return v1.Equals(v2);
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(BoundingFrustum v1, BoundingFrustum v2)
        {
            return !v1.Equals(v2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is BoundingFrustum x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(BoundingFrustum other)
        {
            return
                this.matrix == other.matrix;
        }
    }
}

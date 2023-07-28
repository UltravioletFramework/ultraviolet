using System;

namespace Ultraviolet
{
    partial struct BoundingSphere
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Center.GetHashCode();
                hash = hash * 23 + Radius.GetHashCode();
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(BoundingSphere v1, BoundingSphere v2)
        {
            return v1.Equals(v2);
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(BoundingSphere v1, BoundingSphere v2)
        {
            return !v1.Equals(v2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is BoundingSphere x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(BoundingSphere other)
        {
            return
                this.Center == other.Center &&
                this.Radius == other.Radius;
        }
    }
}

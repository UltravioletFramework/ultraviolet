using System;

namespace Ultraviolet
{
    partial struct Vector2
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(Vector2 v1, Vector2 v2)
        {
            return
                v1.X == v2.X &&
                v1.Y == v2.Y;
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(Vector2 v1, Vector2 v2)
        {
            return
                v1.X != v2.X ||
                v1.Y != v2.Y;
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is Vector2 x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(Vector2 other)
        {
            return
                this.X == other.X &&
                this.Y == other.Y;
        }
    }
}

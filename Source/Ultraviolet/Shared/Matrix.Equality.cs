using System;

namespace Ultraviolet
{
    partial struct Matrix
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + M11.GetHashCode();
                hash = hash * 23 + M12.GetHashCode();
                hash = hash * 23 + M13.GetHashCode();
                hash = hash * 23 + M14.GetHashCode();
                hash = hash * 23 + M21.GetHashCode();
                hash = hash * 23 + M22.GetHashCode();
                hash = hash * 23 + M23.GetHashCode();
                hash = hash * 23 + M24.GetHashCode();
                hash = hash * 23 + M31.GetHashCode();
                hash = hash * 23 + M32.GetHashCode();
                hash = hash * 23 + M33.GetHashCode();
                hash = hash * 23 + M34.GetHashCode();
                hash = hash * 23 + M41.GetHashCode();
                hash = hash * 23 + M42.GetHashCode();
                hash = hash * 23 + M43.GetHashCode();
                hash = hash * 23 + M44.GetHashCode();
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(Matrix v1, Matrix v2)
        {
            return
                v1.M11 == v2.M11 &&
                v1.M22 == v2.M22 &&
                v1.M33 == v2.M33 &&
                v1.M44 == v2.M44 &&
                v1.M12 == v2.M12 &&
                v1.M13 == v2.M13 &&
                v1.M14 == v2.M14 &&
                v1.M21 == v2.M21 &&
                v1.M23 == v2.M23 &&
                v1.M24 == v2.M24 &&
                v1.M31 == v2.M31 &&
                v1.M32 == v2.M32 &&
                v1.M34 == v2.M34 &&
                v1.M41 == v2.M41 &&
                v1.M42 == v2.M42 &&
                v1.M43 == v2.M43;
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(Matrix v1, Matrix v2)
        {
            return
                v1.M11 != v2.M11 ||
                v1.M22 != v2.M22 ||
                v1.M33 != v2.M33 ||
                v1.M44 != v2.M44 ||
                v1.M12 != v2.M12 ||
                v1.M13 != v2.M13 ||
                v1.M14 != v2.M14 ||
                v1.M21 != v2.M21 ||
                v1.M23 != v2.M23 ||
                v1.M24 != v2.M24 ||
                v1.M31 != v2.M31 ||
                v1.M32 != v2.M32 ||
                v1.M34 != v2.M34 ||
                v1.M41 != v2.M41 ||
                v1.M42 != v2.M42 ||
                v1.M43 != v2.M43;
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            return (other is Matrix x) ? Equals(x) : false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(Matrix other)
        {
            return
                this.M11 == other.M11 &&
                this.M22 == other.M22 &&
                this.M33 == other.M33 &&
                this.M44 == other.M44 &&
                this.M12 == other.M12 &&
                this.M13 == other.M13 &&
                this.M14 == other.M14 &&
                this.M21 == other.M21 &&
                this.M23 == other.M23 &&
                this.M24 == other.M24 &&
                this.M31 == other.M31 &&
                this.M32 == other.M32 &&
                this.M34 == other.M34 &&
                this.M41 == other.M41 &&
                this.M42 == other.M42 &&
                this.M43 == other.M43;
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        public Boolean EqualsRef(ref Matrix other)
        {
            return
                this.M11 == other.M11 &&
                this.M22 == other.M22 &&
                this.M33 == other.M33 &&
                this.M44 == other.M44 &&
                this.M12 == other.M12 &&
                this.M13 == other.M13 &&
                this.M14 == other.M14 &&
                this.M21 == other.M21 &&
                this.M23 == other.M23 &&
                this.M24 == other.M24 &&
                this.M31 == other.M31 &&
                this.M32 == other.M32 &&
                this.M34 == other.M34 &&
                this.M41 == other.M41 &&
                this.M42 == other.M42 &&
                this.M43 == other.M43;
        }
    }
}

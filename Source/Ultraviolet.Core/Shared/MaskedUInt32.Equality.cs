using System;

namespace Ultraviolet.Core
{
    partial struct MaskedUInt32
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Value.GetHashCode();
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(MaskedUInt32 v1, MaskedUInt32 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(MaskedUInt32 v1, UInt32 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(UInt32 v1, MaskedUInt32 v2)
        {
            return v2.Equals(v1);
        }

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(MaskedUInt32 v1, MaskedUInt32 v2)
        {
            return !v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(UInt32 v1, MaskedUInt32 v2)
        {
            return v2.Equals(v1);
        }

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(MaskedUInt32 v1, UInt32 v2)
        {
            return !v1.Equals(v2);
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            if (other is UInt32 x)
                return Equals(x);

            if (other is MaskedUInt32 y)
                return Equals(y);

            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(UInt32 other)
        {
            return Value == other;
        }

        /// <inheritdoc/>
        public Boolean Equals(MaskedUInt32 other)
        {
            return Value == other.Value;
        }
    }
}

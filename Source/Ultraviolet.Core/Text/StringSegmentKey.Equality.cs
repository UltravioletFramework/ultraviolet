using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    partial struct StringSegmentKey
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode() => cachedHashCode;
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringSegmentKey v1, StringSegmentKey v2) => v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringSegmentKey v1, StringSegmentKey v2) => !v1.Equals(v2);

        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            if (other is StringSegmentKey ssk)
                return Equals(ssk);

            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(StringSegmentKey other) => this.StringSegment.Equals(other.StringSegment);

        /// <summary>
        /// Calculates the hash code for the specified source instance.
        /// </summary>
        private static Int32 CalculateHashCode(Object source, Int32 start, Int32 length)
        {
            if (source == null)
                return 0;

            var hash = 17;
            switch (source)
            {
                case String str:
                    unchecked
                    {
                        for (int count = 0, ix = start; count < length; count++, ix++)
                            hash = hash * 31 + str[ix].GetHashCode();
                    }
                    break;

                case StringBuilder sb:
                    unchecked
                    {
                        for (int count = 0, ix = start; count < length; count++, ix++)
                            hash = hash * 31 + sb[ix].GetHashCode();
                    }
                    break;

                default:
                    return 0;
            }
            return hash;
        }
    }
}

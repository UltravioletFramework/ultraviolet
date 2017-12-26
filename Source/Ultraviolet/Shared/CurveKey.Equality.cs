using System;
using Ultraviolet.Core;

namespace Ultraviolet
{
    partial class CurveKey
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + position.GetHashCode();
                hash = hash * 23 + value.GetHashCode();
                hash = hash * 23 + tangentIn.GetHashCode();
                hash = hash * 23 + tangentOut.GetHashCode();
                hash = hash * 23 + continuity.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Compares two curve keys for equality.
        /// </summary>
        /// <param name="key1">The first <see cref="CurveKey"/> to compare.</param>
        /// <param name="key2">The second <see cref="CurveKey"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified curve keys are equal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator ==(CurveKey key1, CurveKey key2)
        {
            if (key1 == null || key2 == null)
            {
                return key1 == key2;
            }
            return key1.Equals(key2);
        }

        /// <summary>
        /// Compares two curve keys for inequality.
        /// </summary>
        /// <param name="key1">The first <see cref="CurveKey"/> to compare.</param>
        /// <param name="key2">The second <see cref="CurveKey"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified curve keys are unequal; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean operator !=(CurveKey key1, CurveKey key2)
        {
            if (key1 == null || key2 == null)
            {
                return key1 != key2;
            }
            return !key1.Equals(key2);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            return obj is CurveKey && Equals((CurveKey)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns><see langword="true"/> if this instance is equal to the specified object; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Equals(CurveKey obj)
        {
            if (obj == null)
                return false;

            return
                this.position == obj.position &&
                this.value == obj.value &&
                this.tangentIn == obj.tangentIn &&
                this.tangentOut == obj.tangentOut &&
                this.continuity == obj.continuity;
        }
    }
}

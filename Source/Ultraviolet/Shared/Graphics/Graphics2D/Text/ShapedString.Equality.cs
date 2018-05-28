using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial class ShapedString : IEquatable<ShapedString>
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                for (int i = 0; i < buffer.Length; i++)
                    hash = hash * 23 + buffer[i].GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(ShapedString v1, ShapedString v2) =>
            ReferenceEquals(v1, null) ? ReferenceEquals(v2, null) : v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(ShapedString v1, ShapedString v2) =>
            ReferenceEquals(v1, null) ? !ReferenceEquals(v2, null) : !v1.Equals(v2);

        /// <inheritdoc/>
        public override Boolean Equals(Object obj) =>
            (obj is ShapedString str) ? Equals(str) : false;

        /// <inheritdoc/>
        public Boolean Equals(ShapedString other)
        {
            var str = other as ShapedString;
            if (str == null || str.Length != this.Length)
                return false;

            for (int i = 0; i < this.Length; i++)
            {
                if (this[i] != str[i])
                    return false;
            }
            return true;
        }
    }
}

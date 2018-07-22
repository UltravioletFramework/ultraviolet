using System;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    partial struct ShapedStringSegment
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode() => hashCode;
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(ShapedStringSegment v1, ShapedStringSegment v2) => v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(ShapedStringSegment v1, ShapedStringSegment v2) => !v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(ShapedStringSegment v1, ShapedString v2) => v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(ShapedStringSegment v1, ShapedString v2) => !v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(ShapedStringSegment v1, ShapedStringBuilder v2) => v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(ShapedStringSegment v1, ShapedStringBuilder v2) => !v1.Equals(v2);
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            switch (other)
            {
                case ShapedStringSegment ss:
                    return Equals(ss);
                case ShapedString s:
                    return Equals(s);
                case ShapedStringBuilder sb:
                    return Equals(sb);
                default:
                    return false;
            }
        }
        
        /// <inheritdoc/>
        public Boolean Equals(ShapedStringSegment other)
        {
            if (IsEmpty)
                return other.IsEmpty;
            if (other.IsEmpty)
                return IsEmpty;

            if (other.Length != Length)
                return false;

            if (Source is IStringSource<ShapedChar> src)
            {
                for (int ixsb = Start, ixother = 0; ixother < Length; ixsb++, ixother++)
                {
                    if (src[ixsb] != other[ixother])
                        return false;
                }
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(ShapedString other)
        {
            if (other == null || other.Length != Length)
                return false;

            if (IsEmpty && other.Length == 0)
                return true;

            if (Source is IStringSource<ShapedChar> src)
            {
                for (int ixstr = Start, ixother = 0; ixother < Length; ixstr++, ixother++)
                {
                    if (src[ixstr] != other[ixother])
                        return false;
                }
                return true;
            }
            
            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(ShapedStringBuilder other)
        {
            if (other == null || other.Length != Length)
                return false;

            if (IsEmpty && other.Length == 0)
                return true;

            if (Source is IStringSource<ShapedChar> src)
            {
                for (int ixstr = Start, ixother = 0; ixother < Length; ixstr++, ixother++)
                {
                    if (src[ixstr] != other[ixother])
                        return false;
                }
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Calculates the hash code for the specified instance.
        /// </summary>
        private static Int32 CalculateHashCode(Object source, Int32 start, Int32 length)
        {
            if (source == null)
                return 0;

            var hash = 17;
            switch (source)
            {
                case ShapedString str:
                    unchecked
                    {
                        for (int count = 0, ix = start; count < length; count++, ix++)
                            hash = hash * 31 + str[ix].GetHashCode();
                    }
                    break;

                case ShapedStringBuilder sb:
                    unchecked
                    {
                        for (int count = 0, ix = start; count < length; count++, ix++)
                            hash = hash * 32 + sb[ix].GetHashCode();
                    }
                    break;

                default:
                    return 0;
            }
            return hash;
        }
    }
}

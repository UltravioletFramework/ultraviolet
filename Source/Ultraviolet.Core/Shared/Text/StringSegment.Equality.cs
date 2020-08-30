using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    partial struct StringSegment
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode() => Source?.GetHashCode() ?? 0;
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringSegment v1, StringSegment v2) => v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringSegment v1, StringSegment v2) => !v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringSegment v1, String v2) => v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringSegment v1, String v2) => !v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringSegment v1, StringBuilder v2) => v1.Equals(v2);

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringSegment v1, StringBuilder v2) => !v1.Equals(v2);
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            switch (other)
            {
                case StringSegment ss:
                    return Equals(ss);
                case String s:
                    return Equals(s);
                case StringBuilder sb:
                    return Equals(sb);
                default:
                    return false;
            }
        }
        
        /// <inheritdoc/>
        public Boolean Equals(StringSegment other)
        {
            if (IsEmpty)
                return other.IsEmpty;
            if (other.IsEmpty)
                return IsEmpty;

            if (other.Length != Length)
                return false;

            if (Source is String str)
            {
                for (int ixstr = Start, ixother = 0; ixother < Length; ixstr++, ixother++)
                {
                    if (str[ixstr] != other[ixother])
                        return false;
                }
                return true;
            }

            if (Source is StringBuilder sb)
            {
                for (int ixsb = Start, ixother = 0; ixother < Length; ixsb++, ixother++)
                {
                    if (sb[ixsb] != other[ixother])
                        return false;
                }
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(String other)
        {
            if (other == null || other.Length != Length)
                return false;

            if (IsEmpty && other.Length == 0)
                return true;

            if (Source is String str)
            {
                for (int ixstr = Start, ixother = 0; ixother < Length; ixstr++, ixother++)
                {
                    if (str[ixstr] != other[ixother])
                        return false;
                }
                return true;
            }

            if (Source is StringBuilder sb)
            {
                for (int ixsb = Start, ixother = 0; ixother < Length; ixsb++, ixother++)
                {
                    if (sb[ixsb] != other[ixother])
                        return false;
                }
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public Boolean Equals(StringBuilder other)
        {
            if (other == null || other.Length != Length)
                return false;

            if (IsEmpty && other.Length == 0)
                return true;

            if (Source is String str)
            {
                for (int ixstr = Start, ixother = 0; ixother < Length; ixstr++, ixother++)
                {
                    if (str[ixstr] != other[ixother])
                        return false;
                }
                return true;
            }

            if (Source is StringBuilder sb)
            {
                for (int ixsb = Start, ixother = 0; ixother < Length; ixsb++, ixother++)
                {
                    if (sb[ixsb] != other[ixother])
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

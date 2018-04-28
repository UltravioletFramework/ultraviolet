using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    partial struct StringSegment
    {
        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            if ((sourceString == null && sourceBuilder == null) || length == 0)
                return 0;

            unchecked
            {
                var hash = 17;
                for (int i = 0; i < length; i++)
                {
                    hash = hash * 31 + this[i].GetHashCode();
                }
                return hash;
            }
        }
        
        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringSegment v1, StringSegment v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringSegment v1, StringSegment v2)
        {
            return !v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringSegment v1, String v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringSegment v1, String v2)
        {
            return !v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(StringSegment v1, StringBuilder v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Compares two objects to determine whether they are unequal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <returns><see langword="true"/> if the two values are unequal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(StringSegment v1, StringBuilder v2)
        {
            return !v1.Equals(v2);
        }
        
        /// <inheritdoc/>
        public override Boolean Equals(Object other)
        {
            if (other is StringSegment ss) return Equals(ss);
            if (other is String s) return Equals(s);
            if (other is StringBuilder sb) return Equals(sb);
            return false;
        }
        
        /// <inheritdoc/>
        public Boolean Equals(StringSegment other)
        {
            if (IsEmpty)
                return other.IsEmpty;
            if (other.IsEmpty)
                return IsEmpty;

            if (other.length != length)
                return false;

            for (int i = 0; i < length; i++)
            {
                if (this[i] != other[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public Boolean Equals(String other)
        {
            if (IsEmpty && other == String.Empty)
                return true;

            if (other == null || other.Length != length)
                return false;

            for (int i = 0; i < length; i++)
            {
                if (this[i] != other[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public Boolean Equals(StringBuilder other)
        {
            if (IsEmpty && other.Length == 0)
                return true;

            if (other == null || other.Length != length)
                return false;

            for (int i = 0; i < length; i++)
            {
                if (this[i] != other[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

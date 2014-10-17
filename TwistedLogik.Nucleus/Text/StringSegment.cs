using System;
using System.Text;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// Represents a segment of a string.
    /// </summary>
    public struct StringSegment : IEquatable<StringSegment>, IEquatable<String>, IEquatable<StringBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the StringSegment structure.
        /// </summary>
        /// <param name="str">The string that represents the segment.</param>
        public StringSegment(String str)
        {
            Contract.Require(str, "str");

            this.str = str;
            this.start = 0;
            this.length = str.Length;
        }

        /// <summary>
        /// Initializes a new instance of the StringSegment structure.
        /// </summary>
        /// <param name="str">The string that contains this segment.</param>
        /// <param name="start">The index of the string segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the string segment.</param>
        public StringSegment(String str, Int32 start, Int32 length)
        {
            Contract.Require(str, "string");
            Contract.EnsureRange(start >= 0 && start < str.Length, "start");
            Contract.EnsureRange(length >= 0 && start + length <= str.Length, "length");

            this.str = str;
            this.start = start;
            this.length = length;
        }

        /// <summary>
        /// Implicitly converts a string to a string segment.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted string segment.</returns>
        public static implicit operator StringSegment(String s)
        {
            return new StringSegment(s);
        }

        /// <summary>
        /// Compares two string segments for equality.
        /// </summary>
        /// <param name="s1">The first string segment.</param>
        /// <param name="s2">The second string segment.</param>
        /// <returns>true if the two string segments are equal; otherwise, false.</returns>
        public static Boolean operator ==(StringSegment s1, StringSegment s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares two string segments for inequality.
        /// </summary>
        /// <param name="s1">The first string segment.</param>
        /// <param name="s2">The second string segment.</param>
        /// <returns>true if the two string segments are unequal; otherwise, false.</returns>
        public static Boolean operator !=(StringSegment s1, StringSegment s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Compares a string segment and a string for equality.
        /// </summary>
        /// <param name="s1">The string segment.</param>
        /// <param name="s2">The string.</param>
        /// <returns>true if the string segment is equal to the string; otherwise, false.</returns>
        public static Boolean operator ==(StringSegment s1, String s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares a string segment and a string for inequality.
        /// </summary>
        /// <param name="s1">The string segment.</param>
        /// <param name="s2">The string.</param>
        /// <returns>true if the string segment is not equal to the string; otherwise, false.</returns>
        public static Boolean operator !=(StringSegment s1, String s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Compares a string segment and a string builder for equality.
        /// </summary>
        /// <param name="s1">The string segment.</param>
        /// <param name="s2">The string builder.</param>
        /// <returns>true if the string segment is equal to the string builder; otherwise, false.</returns>
        public static Boolean operator ==(StringSegment s1, StringBuilder s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares a string segment and a string builder for inequality.
        /// </summary>
        /// <param name="s1">The string segment.</param>
        /// <param name="s2">The string builder.</param>
        /// <returns>true if the string segment is not equal to the string builder; otherwise, false.</returns>
        public static Boolean operator !=(StringSegment s1, StringBuilder s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Gets a value indicating whether the specified string segments are contiguous.
        /// </summary>
        /// <param name="s1">The first string segment.</param>
        /// <param name="s2">The second string segment.</param>
        /// <returns>true if the string segments are contiguous; otherwise, false.</returns>
        public static Boolean AreSegmentsContiguous(StringSegment s1, StringSegment s2)
        {
            return (s1.String == s2.String) &&
                s1.Start + s1.Length == s2.Start ||
                s2.Start + s2.Length == s1.Start;
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return (String == null) ? "(null)" : String.Substring(Start, Length);
        }

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
        public override Int32 GetHashCode()
        {
            if (str == null || length == 0) 
                return 0;

            unchecked
            {
                var hash = 17;
                for (int i = start; i < start + length; i++)
                {
                    hash = hash * 31 + str[i].GetHashCode();
                }
                return hash;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this string segment is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this segment.</param>
        /// <returns>true if this string segment is equal to the specified object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is StringSegment) return Equals((StringSegment)obj);
            if (obj is String) return Equals((String)obj);
            if (obj is StringBuilder) return Equals((StringBuilder)obj);
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the content of this string segment equals
        /// the content of the specified string segment.
        /// </summary>
        /// <param name="other">The string segment to compare to this segment.</param>
        /// <returns>true if the content of this segment equals the content of the 
        /// specified string segment; otherwise, false.</returns>
        public Boolean Equals(StringSegment other)
        {
            if (IsEmpty)
                return other.IsEmpty;
            if (other.IsEmpty)
                return IsEmpty;

            if (other.length != length)
                return false;

            var i = start;
            var j = other.start;
            for (int n = 0; n < length; n++)
            {
                if (str[i++] != other.str[j++])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the content of this string segment equals
        /// the content of the specified string.
        /// </summary>
        /// <param name="other">The string to compare to this segment.</param>
        /// <returns>true if the content of this segment equals the content of the 
        /// specified string; otherwise, false.</returns>
        public Boolean Equals(String other)
        {
            if (IsEmpty && other == String.Empty)
                return true;

            if (other == null || other.Length != length)
                return false;

            var i = start;
            var j = 0;
            for (int n = 0; n < length; n++)
            {
                if (str[i++] != other[j++])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the content of this string segment equals
        /// the content of the specified string builder.
        /// </summary>
        /// <param name="other">The string builder to compare to this segment.</param>
        /// <returns>true if the content of this segment equals the content of the 
        /// specified string builder; otherwise, false.</returns>
        public Boolean Equals(StringBuilder other)
        {
            if (IsEmpty && other.Length == 0)
                return true;

            if (other == null || other.Length != length)
                return false;

            var i = start;
            var j = 0;
            for (int n = 0; n < length; n++)
            {
                if (str[i++] != other[j++])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a string segment which is a substring of this string segment.
        /// </summary>
        /// <param name="start">The starting character of the substring within this string segment.</param>
        /// <returns>A string segment which is a substring of this string segment.</returns>
        public StringSegment Substring(Int32 start)
        {
            Contract.EnsureRange(start >= 0 && start < this.length, "start");
            Contract.EnsureNot(IsEmpty, NucleusStrings.SegmentIsEmpty);

            return (length == 0) ? new StringSegment() : new StringSegment(str, this.start + start, length);
        }

        /// <summary>
        /// Creates a string segment which is a substring of this string segment.
        /// </summary>
        /// <param name="start">The starting character of the substring within this string segment.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A string segment which is a substring of this string segment.</returns>
        public StringSegment Substring(Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start < this.length, "start");
            Contract.EnsureRange(length > 0 && start + length <= this.length, "length");
            Contract.EnsureNot(IsEmpty, NucleusStrings.SegmentIsEmpty);

            return (length == 0) ? new StringSegment() : new StringSegment(str, this.start + start, length);
        }

        /// <summary>
        /// Gets the character at the specified index within the string segment.
        /// </summary>
        /// <param name="ix">The index of the character to retrieve.</param>
        /// <returns>The character at the specified index.</returns>
        public Char this[Int32 ix]
        {
            get 
            {
                if (ix < 0 || ix >= length)
                    throw new ArgumentOutOfRangeException("ix");
                return str[start + ix];
            }
        }

        /// <summary>
        /// Gets the string that contains this string segment.
        /// </summary>
        public String String
        {
            get { return str; }
        }

        /// <summary>
        /// Gets the index of the string segment's first character within its parent string.
        /// </summary>
        public Int32 Start
        {
            get { return start; }
        }

        /// <summary>
        /// Gets the number of characters in the string segment.
        /// </summary>
        public Int32 Length
        {
            get { return length; }
        }

        /// <summary>
        /// Gets a value indicating whether this is an empty string segment.
        /// </summary>
        public Boolean IsEmpty
        {
            get { return str == null || length == 0; }
        }

        /// <summary>
        /// Represents an empty string segment.
        /// </summary>
        public static readonly StringSegment Empty = new StringSegment();

        // Property values.
        private readonly String str;
        private readonly Int32 start;
        private readonly Int32 length;
    }
}

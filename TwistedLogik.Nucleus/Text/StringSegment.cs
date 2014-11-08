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
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The <see cref="SourceString"/> that represents the segment.</param>
        public StringSegment(String source)
        {
            Contract.Require(source, "source");

            this.sourceString  = source;
            this.sourceBuilder = null;
            this.start         = 0;
            this.length        = source.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The <see cref="SourceStringBuilder"/> that represents the segment.</param>
        public StringSegment(StringBuilder source)
        {
            Contract.Require(source, "source");

            this.sourceString  = null;
            this.sourceBuilder = source;
            this.start         = 0;
            this.length        = source.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The <see cref="SourceString"/> that contains this segment.</param>
        /// <param name="start">The index of the string segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the string segment.</param>
        public StringSegment(String source, Int32 start, Int32 length)
        {
            Contract.Require(source, "string");
            Contract.EnsureRange(start >= 0 && start < source.Length, "start");
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, "length");

            this.sourceString  = source;
            this.sourceBuilder = null;
            this.start         = start;
            this.length        = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The <see cref="SourceStringBuilder"/> that contains this segment.</param>
        /// <param name="start">The index of the string segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the string segment.</param>
        public StringSegment(StringBuilder source, Int32 start, Int32 length)
        {
            Contract.Require(source, "source");
            Contract.EnsureRange(start >= 0 && start < source.Length, "start");
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, "length");

            this.sourceString  = null;
            this.sourceBuilder = source;
            this.start         = start;
            this.length        = length;
        }

        /// <summary>
        /// Implicitly converts a string to a string segment.
        /// </summary>
        /// <param name="s">The <see cref="SourceString"/> to convert.</param>
        /// <returns>The converted <see cref="StringSegment"/>.</returns>
        public static implicit operator StringSegment(String s)
        {
            return new StringSegment(s);
        }

        /// <summary>
        /// Compares two string segments for equality.
        /// </summary>
        /// <param name="s1">The first <see cref="StringSegment"/>.</param>
        /// <param name="s2">The second <see cref="StringSegment"/>.</param>
        /// <returns><c>true</c> if the two string segments are equal; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(StringSegment s1, StringSegment s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares two string segments for inequality.
        /// </summary>
        /// <param name="s1">The first <see cref="StringSegment"/>.</param>
        /// <param name="s2">The second <see cref="StringSegment"/>.</param>
        /// <returns><c>true</c> if the two string segments are unequal; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(StringSegment s1, StringSegment s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Compares a string segment and a string for equality.
        /// </summary>
        /// <param name="s1">The <see cref="StringSegment"/> to compare.</param>
        /// <param name="s2">The <see cref="SourceString"/> to compare.</param>
        /// <returns><c>true</c> if the string segment is equal to the string; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(StringSegment s1, String s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares a string segment and a string for inequality.
        /// </summary>
        /// <param name="s1">The <see cref="StringSegment"/> to compare.</param>
        /// <param name="s2">The <see cref="SourceString"/> to compare.</param>
        /// <returns><c>true</c> if the string segment is not equal to the string; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(StringSegment s1, String s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Compares a string segment and a string builder for equality.
        /// </summary>
        /// <param name="s1">The <see cref="StringSegment"/> to compare.</param>
        /// <param name="s2">The <see cref="SourceStringBuilder"/> to compare.</param>
        /// <returns><c>true</c> if the string segment is equal to the string builder; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(StringSegment s1, StringBuilder s2)
        {
            return s1.Equals(s2);
        }

        /// <summary>
        /// Compares a string segment and a string builder for inequality.
        /// </summary>
        /// <param name="s1">The <see cref="StringSegment"/> to compare.</param>
        /// <param name="s2">The <see cref="SourceStringBuilder"/> to compare.</param>
        /// <returns><c>true</c> if the string segment is not equal to the string builder; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(StringSegment s1, StringBuilder s2)
        {
            return !s1.Equals(s2);
        }

        /// <summary>
        /// Gets a value indicating whether the specified string segments are contiguous.
        /// </summary>
        /// <param name="s1">The first <see cref="StringSegment"/>.</param>
        /// <param name="s2">The second <see cref="StringSegment"/>.</param>
        /// <returns><c>true</c> if the string segments are contiguous; otherwise, <c>false</c>.</returns>
        public static Boolean AreSegmentsContiguous(StringSegment s1, StringSegment s2)
        {
            if (s1.sourceString != null)
            {
                return (s1.sourceString == s2.sourceString) &&
                    s1.Start + s1.Length == s2.Start ||
                    s2.Start + s2.Length == s1.Start;

            }
            return (s1.sourceBuilder == s2.sourceBuilder) &&
                s1.Start + s1.Length == s2.Start ||
                s2.Start + s2.Length == s1.Start;
        }

        /// <summary>
        /// Combines two contiguous string segments.
        /// </summary>
        /// <param name="s1">The first <see cref="StringSegment"/> to combine.</param>
        /// <param name="s2">The second <see cref="StringSegment"/> to combine.</param>
        /// <returns>The combined string segment.</returns>
        public static StringSegment CombineSegments(StringSegment s1, StringSegment s2)
        {
            if (!AreSegmentsContiguous(s1, s2))
            {
                throw new InvalidOperationException(NucleusStrings.SegmentsAreNotContiguous);
            }

            return (s1.sourceString != null) ?
                new StringSegment(s1.sourceString, s1.Start, s1.Length + s2.Length) :                
                new StringSegment(s1.sourceBuilder, s1.Start, s1.Length + s2.Length);
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            if (sourceBuilder != null)
            {
                return sourceBuilder.ToString();
            }
            if (sourceString != null)
            {
                return sourceString.Substring(Start, Length);
            }
            return null;
        }

        /// <summary>
        /// Gets the object's hash code.
        /// </summary>
        /// <returns>The object's hash code.</returns>
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
        /// Gets a value indicating whether this string segment is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this segment.</param>
        /// <returns><c>true</c> ifthis string segment is equal to the specified object; otherwise, <c>false</c>.</returns>
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
        /// <param name="other">The <see cref="StringSegment"/> to compare to this segment.</param>
        /// <returns><c>true</c> if the content of this segment equals the content of the 
        /// specified string segment; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Gets a value indicating whether the content of this string segment equals
        /// the content of the specified string.
        /// </summary>
        /// <param name="other">The <see cref="SourceString"/> to compare to this segment.</param>
        /// <returns><c>true</c> if the content of this segment equals the content of the 
        /// specified string; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Gets a value indicating whether the content of this string segment equals
        /// the content of the specified string builder.
        /// </summary>
        /// <param name="other">The <see cref="SourceStringBuilder"/> to compare to this segment.</param>
        /// <returns><c>true</c> if the content of this segment equals the content of the 
        /// specified string builder; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Creates a string segment which is a substring of this string segment.
        /// </summary>
        /// <param name="start">The starting character of the substring within this string segment.</param>
        /// <returns>A <see cref="StringSegment"/> which is a substring of this string segment.</returns>
        public StringSegment Substring(Int32 start)
        {
            Contract.EnsureRange(start >= 0 && start < this.length, "start");
            Contract.EnsureNot(IsEmpty, NucleusStrings.SegmentIsEmpty);

            var substringLength = (length - start);

            return (sourceString == null) ? 
                new StringSegment(sourceBuilder, this.start + start, substringLength) :
                new StringSegment(sourceString, this.start + start, substringLength);
        }

        /// <summary>
        /// Creates a string segment which is a substring of this string segment.
        /// </summary>
        /// <param name="start">The starting character of the substring within this string segment.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A <see cref="StringSegment"/> which is a substring of this string segment.</returns>
        public StringSegment Substring(Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start < this.length, "start");
            Contract.EnsureRange(length > 0 && start + length <= this.length, "length");
            Contract.EnsureNot(IsEmpty, NucleusStrings.SegmentIsEmpty);

            return (sourceString == null) ? 
                new StringSegment(sourceBuilder, this.start + start, length) :
                new StringSegment(sourceString, this.start + start, length);
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
                Contract.EnsureRange(ix >= 0 && ix < length, "ix");

                return (sourceString == null) ? sourceBuilder[start + ix] : sourceString[start + ix];
            }
        }

        /// <summary>
        /// Gets the <see cref="SourceString"/> that contains this string segment.
        /// </summary>
        public String SourceString
        {
            get { return sourceString; }
        }

        /// <summary>
        /// Gets the <see cref="SourceStringBuilder"/> that contains this string segment.
        /// </summary>
        public StringBuilder SourceStringBuilder
        {
            get { return sourceBuilder; }
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
            get { return length == 0; }
        }

        /// <summary>
        /// Represents an empty string segment.
        /// </summary>
        public static readonly StringSegment Empty = new StringSegment();

        // Property values.
        private readonly String sourceString;
        private readonly StringBuilder sourceBuilder;
        private readonly Int32 start;
        private readonly Int32 length;
    }
}

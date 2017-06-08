using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents a segment of a string.
    /// </summary>
    public partial struct StringSegment : IEquatable<StringSegment>, IEquatable<String>, IEquatable<StringBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The <see cref="SourceString"/> that represents the segment.</param>
        public StringSegment(String source)
        {
            Contract.Require(source, nameof(source));

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
            Contract.Require(source, nameof(source));

            this.sourceString  = null;
            this.sourceBuilder = source;
            this.start         = 0;
            this.length        = source.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="String"/> that contains this segment.</param>
        /// <param name="start">The index of the string segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the string segment.</param>
        public StringSegment(String source, Int32 start, Int32 length)
        {
            Contract.Require(source, nameof(source));
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.sourceString  = source;
            this.sourceBuilder = null;
            this.start         = start;
            this.length        = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="StringBuilder"/> that contains this segment.</param>
        /// <param name="start">The index of the string segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the string segment.</param>
        public StringSegment(StringBuilder source, Int32 start, Int32 length)
        {
            Contract.Require(source, nameof(source));
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.sourceString  = null;
            this.sourceBuilder = source;
            this.start         = start;
            this.length        = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="StringSegment"/> that contains this segment.</param>
        /// <param name="start">The index of the string segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the string segment.</param>
        public StringSegment(StringSegment source, Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.sourceString = source.sourceString;
            this.sourceBuilder = source.sourceBuilder;
            this.start = source.start + start;
            this.length = length;
        }

        /// <summary>
        /// Implicitly converts a <see cref="String"/> to a string segment.
        /// </summary>
        /// <param name="s">The <see cref="String"/> to convert.</param>
        /// <returns>The converted <see cref="StringSegment"/>.</returns>
        public static implicit operator StringSegment(String s)
        {
            return (s == null) ? Empty : new StringSegment(s);
        }
        
        /// <summary>
        /// Explicitly converts a <see cref="StringBuilder"/> to a string segment.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to convert.</param>
        /// <returns>The converted <see cref="StringSegment"/>.</returns>
        public static explicit operator StringSegment(StringBuilder sb)
        {
            return (sb == null) ? Empty : new StringSegment(sb);
        }

        /// <summary>
        /// Gets a value indicating whether the specified string segments are contiguous.
        /// </summary>
        /// <param name="s1">The first <see cref="StringSegment"/>.</param>
        /// <param name="s2">The second <see cref="StringSegment"/>.</param>
        /// <returns><see langword="true"/> if the string segments are contiguous; otherwise, <see langword="false"/>.</returns>
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
                throw new InvalidOperationException(CoreStrings.SegmentsAreNotContiguous);
            }

            return (s1.sourceString != null) ?
                new StringSegment(s1.sourceString, s1.Start, s1.Length + s2.Length) :                
                new StringSegment(s1.sourceBuilder, s1.Start, s1.Length + s2.Length);
        }

        /// <summary>
        /// Creates a new <see cref="StringSegment"/> from the same source as the specified segment.
        /// </summary>
        /// <param name="segment">The segment from which to retrieve a string source.</param>
        /// <param name="start">The index of the string segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the string segment.</param>
        /// <returns>The string segment that was created.</returns>
        public static StringSegment FromSource(StringSegment segment, Int32 start, Int32 length)
        {
            if (segment.sourceString != null)
                return new StringSegment(segment.sourceString, start, length);

            if (segment.sourceBuilder != null)
                return new StringSegment(segment.sourceBuilder, start, length);

            if (start != 0)
                throw new ArgumentOutOfRangeException("start");

            if (length != 0)
                throw new ArgumentOutOfRangeException("length");

            return Empty;
        }

        /// <inheritdoc/>
        public override String ToString() => 
            sourceBuilder?.ToString(Start, Length) ?? sourceString?.Substring(Start, Length);

        /// <summary>
        /// Creates a string segment which is a substring of this string segment.
        /// </summary>
        /// <param name="start">The starting character of the substring within this string segment.</param>
        /// <returns>A <see cref="StringSegment"/> which is a substring of this string segment.</returns>
        public StringSegment Substring(Int32 start)
        {
            Contract.EnsureRange(start >= 0 && start < this.length, nameof(start));
            Contract.EnsureNot(IsEmpty, CoreStrings.SegmentIsEmpty);

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
            Contract.EnsureRange(start >= 0 && start < this.length, nameof(start));
            Contract.EnsureRange(length > 0 && start + length <= this.length, nameof(length));
            Contract.EnsureNot(IsEmpty, CoreStrings.SegmentIsEmpty);

            return (sourceString == null) ? 
                new StringSegment(sourceBuilder, this.start + start, length) :
                new StringSegment(sourceString, this.start + start, length);
        }

        /// <summary>
        /// Gets the index of the first occurrence of the specified character within the string segment.
        /// </summary>
        /// <param name="value">The character for which to search.</param>
        /// <returns>The index of the first occurrence of the specified character, or -1 if the string segment does not contain the character.</returns>
        public Int32 IndexOf(Char value)
        {
            for (int i = 0; i < Length; i++)
            {
                if (this[i] == value)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Gets the index of the first occurrence of the specified string within the string segment.
        /// </summary>
        /// <param name="value">The string for which to search.</param>
        /// <returns>The index of the first occurrence of the specified string, or -1 if the string segment does not contain the string.</returns>
        public Int32 IndexOf(String value)
        {
            Contract.Require(value, nameof(value));

            for (int i = 0; i < Length; i++)
            {
                var matches = 0;

                for (int j = 0; j < value.Length; j++)
                {
                    if (this[i + j] != value[j])
                        break;

                    matches++;
                }

                if (matches == value.Length)
                    return i;
            }
            return -1;
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
                Contract.EnsureRange(ix >= 0 && ix < length, nameof(ix));

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
        /// Gets the number of characters in the segment's source string.
        /// </summary>
        public Int32 SourceLength
        {
            get
            {
                if (sourceString != null)
                {
                    return sourceString.Length;
                }
                if (sourceBuilder != null)
                {
                    return sourceBuilder.Length;
                }
                return 0;
            }
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

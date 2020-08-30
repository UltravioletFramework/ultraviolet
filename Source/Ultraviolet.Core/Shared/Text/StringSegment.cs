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
        /// <param name="source">The source <see cref="String"/> object from which the segment's characters are retrieved.</param>
        public StringSegment(String source)
        {
            Contract.Require(source, nameof(source));

            this.Source = source;
            this.Start = 0;
            this.Length = source.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="StringBuilder"/> object from which the segment's characters are retrieved.</param>
        public StringSegment(StringBuilder source)
        {
            Contract.Require(source, nameof(source));

            this.Source = source;
            this.Start = 0;
            this.Length = source.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="String"/> object from which the segment's characters are retrieved.</param>
        /// <param name="start">The index of the segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the segment.</param>
        public StringSegment(String source, Int32 start, Int32 length)
        {
            Contract.Require(source, nameof(source));
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.Source = source;
            this.Start = start;
            this.Length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="StringBuilder"/> object from which the segment's characters are retrieved.</param>
        /// <param name="start">The index of the segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the segment.</param>
        public StringSegment(StringBuilder source, Int32 start, Int32 length)
        {
            Contract.Require(source, nameof(source));
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.Source = source;
            this.Start = start;
            this.Length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="StringSegment"/> object from which the segment's characters are retrieved.</param>
        /// <param name="start">The index of the segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the segment.</param>
        public StringSegment(StringSegment source, Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.Source = source.Source;
            this.Start = source.Start + start;
            this.Length = length;
        }

        /// <summary>
        /// Implicitly converts a <see cref="String"/> to a string segment.
        /// </summary>
        /// <param name="s">The <see cref="String"/> to convert.</param>
        /// <returns>The converted <see cref="StringSegment"/>.</returns>
        public static implicit operator StringSegment(String s) => 
            (s == null) ? Empty : new StringSegment(s);
        
        /// <summary>
        /// Explicitly converts a <see cref="StringBuilder"/> to a string segment.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to convert.</param>
        /// <returns>The converted <see cref="StringSegment"/>.</returns>
        public static explicit operator StringSegment(StringBuilder sb) =>
            (sb == null) ? Empty : new StringSegment(sb);

        /// <summary>
        /// Gets a value indicating whether the specified string segments are contiguous.
        /// </summary>
        /// <param name="s1">The first <see cref="StringSegment"/>.</param>
        /// <param name="s2">The second <see cref="StringSegment"/>.</param>
        /// <returns><see langword="true"/> if the string segments are contiguous; otherwise, <see langword="false"/>.</returns>
        public static Boolean AreSegmentsContiguous(StringSegment s1, StringSegment s2) =>
            (s1.Source == s2.Source) && (s1.Start + s1.Length == s2.Start || s2.Start + s2.Length == s1.Start);

        /// <summary>
        /// Combines two contiguous string segments.
        /// </summary>
        /// <param name="s1">The first <see cref="StringSegment"/> to combine.</param>
        /// <param name="s2">The second <see cref="StringSegment"/> to combine.</param>
        /// <returns>The combined string segment.</returns>
        public static StringSegment CombineSegments(StringSegment s1, StringSegment s2)
        {
            if (!AreSegmentsContiguous(s1, s2))
                throw new InvalidOperationException(CoreStrings.SegmentsAreNotContiguous);

            if (s1.Source is String str)
                return new StringSegment(str, s1.Start, s1.Length + s2.Length);

            if (s1.Source is StringBuilder sb)
                return new StringSegment(sb, s1.Start, s1.Length + s2.Length);

            return Empty;
        }

        /// <summary>
        /// Creates a new <see cref="StringSegment"/> from the same source as the specified segment.
        /// </summary>
        /// <param name="segment">The segment from which to retrieve a string source.</param>
        /// <param name="start">The index of the segment's first character within its parent string.</param>
        /// <param name="length">The number of characters in the segment.</param>
        /// <returns>The string segment that was created.</returns>
        public static StringSegment FromSource(StringSegment segment, Int32 start, Int32 length)
        {
            if (segment.Source is String str)
                return new StringSegment(str, start, length);

            if (segment.Source is StringBuilder sb)
                return new StringSegment(sb, start, length);

            if (start != 0)
                throw new ArgumentOutOfRangeException("start");

            if (length != 0)
                throw new ArgumentOutOfRangeException("length");

            return Empty;
        }

        /// <inheritdoc/>
        public override String ToString()
        {
            if (Source is String str)
                return str.Substring(Start, Length);

            if (Source is StringBuilder sb)
                return sb.ToString(Start, Length);

            return null;
        }
        
        /// <summary>
        /// Creates a string segment which is a substring of this segment.
        /// </summary>
        /// <param name="start">The starting character of the substring within this string segment.</param>
        /// <returns>A <see cref="StringSegment"/> which is a substring of this segment.</returns>
        public StringSegment Substring(Int32 start)
        {
            Contract.EnsureRange(start >= 0 && start < this.Length, nameof(start));
            Contract.EnsureNot(IsEmpty, CoreStrings.SegmentIsEmpty);

            var substringLength = (this.Length - start);

            if (Source is String str)
                return new StringSegment(str, this.Start + start, substringLength);

            if (Source is StringBuilder sb)
                return new StringSegment(sb, this.Start + start, substringLength);

            return Empty;
        }

        /// <summary>
        /// Creates a string segment which is a substring of this segment.
        /// </summary>
        /// <param name="start">The starting character of the substring within this string segment.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A <see cref="StringSegment"/> which is a substring of this segment.</returns>
        public StringSegment Substring(Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start < this.Length, nameof(start));
            Contract.EnsureRange(length > 0 && start + length <= this.Length, nameof(length));
            Contract.EnsureNot(IsEmpty, CoreStrings.SegmentIsEmpty);

            if (Source is String str)
                return new StringSegment(str, this.Start + start, length);

            if (Source is StringBuilder sb)
                return new StringSegment(sb, this.Start + start, length);

            return Empty;
        }

        /// <summary>
        /// Gets the index of the first occurrence of the specified character within the string segment.
        /// </summary>
        /// <param name="value">The character for which to search.</param>
        /// <returns>The index of the first occurrence of the specified character, or -1 if the segment does not contain the character.</returns>
        public Int32 IndexOf(Char value)
        {
            if (Source is String str)
                return str.IndexOf(value, Start, Length);

            if (Source is StringBuilder sb)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (sb[i] == value)
                        return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of the first occurrence of the specified string within the string segment.
        /// </summary>
        /// <param name="value">The string for which to search.</param>
        /// <returns>The index of the first occurrence of the specified string, or -1 if the segment does not contain the string.</returns>
        public Int32 IndexOf(String value)
        {
            Contract.Require(value, nameof(value));

            if (Source is String str)
                return str.IndexOf(value, Start, Length);

            if (Source is StringBuilder sb)
            {
                for (int i = 0; i < Length; i++)
                {
                    var matches = 0;

                    for (int j = 0; j < value.Length; j++)
                    {
                        if (sb[i + j] != value[j])
                            break;

                        matches++;
                    }

                    if (matches == value.Length)
                        return i;
                }
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
                if (Source is String str)
                    return str[Start + ix];

                if (Source is StringBuilder sb)
                    return sb[Start + ix];

                throw new ArgumentOutOfRangeException(nameof(ix));
            }
        }

        /// <summary>
        /// Gets the source object from which the segment's characters are retrieved.
        /// </summary>
        public Object Source { get; }

        /// <summary>
        /// Gets the source <see cref="String"/> from which the segment's characters are retrieved,
        /// if the segment's source is a <see cref="String"/> object; otherwise, returns <see langword="null"/>.
        /// </summary>
        public String SourceString => Source as String;

        /// <summary>
        /// Gets the source <see cref="StringBuilder"/> from which the segment's characters are retrieved,
        /// if the segment's source is a <see cref="StringBuilder"/> object; otherwise, returns <see langword="null"/>.
        /// </summary>
        public StringBuilder SourceStringBuilder => Source as StringBuilder;

        /// <summary>
        /// Gets the index of the string segment's first character within its parent string.
        /// </summary>
        public Int32 Start { get; }

        /// <summary>
        /// Gets the number of characters in the segment.
        /// </summary>
        public Int32 Length { get; }

        /// <summary>
        /// Gets the number of characters in the segment's source string.
        /// </summary>
        public Int32 SourceLength
        {
            get
            {
                if (Source is String str)
                    return str.Length;

                return ((StringBuilder)Source)?.Length ?? 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is an empty string segment.
        /// </summary>
        public Boolean IsEmpty => Length == 0;

        /// <summary>
        /// Gets a value indicating whether <see cref="Source"/> is a <see cref="String"/> instance.
        /// </summary>
        public Boolean IsBackedByString => Source is String;

        /// <summary>
        /// Gets a value indicating whether <see cref="Source"/> is a <see cref="StringBuilder"/> instance.
        /// </summary>
        public Boolean IsBackedByStringBuilder => Source is StringBuilder;

        /// <summary>
        /// Represents an empty string segment.
        /// </summary>
        public static readonly StringSegment Empty = new StringSegment();
    }
}

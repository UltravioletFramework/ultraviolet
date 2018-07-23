using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a segment of a shaped string.
    /// </summary>
    public partial struct ShapedStringSegment : IEquatable<ShapedStringSegment>, IEquatable<ShapedString>, IEquatable<ShapedStringBuilder>, ISegmentableShapedStringSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedStringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="ShapedString"/> object from which the segment's shaped characters are retrieved.</param>
        public ShapedStringSegment(ShapedString source)
        {
            Contract.Require(source, nameof(source));

            this.Source = source;
            this.Start = 0;
            this.Length = source.Length;

            this.hashCode = CalculateHashCode(source, 0, source.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedStringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="ShapedStringBuilder"/> object from which the segment's shaped characters are retrieved.</param>
        public ShapedStringSegment(ShapedStringBuilder source)
        {
            Contract.Require(source, nameof(source));

            this.Source = source;
            this.Start = 0;
            this.Length = source.Length;

            this.hashCode = CalculateHashCode(source, 0, source.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedStringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="ShapedString"/> object from which the segment's shaped characters are retrieved.</param>
        /// <param name="start">The index of the segment's first shaped character within its parent shaped string.</param>
        /// <param name="length">The number of shaped characters in the segment.</param>
        public ShapedStringSegment(ShapedString source, Int32 start, Int32 length)
        {
            Contract.Require(source, nameof(source));
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.Source = source;
            this.Start = start;
            this.Length = length;

            this.hashCode = CalculateHashCode(source, start, length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedStringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="ShapedStringBuilder"/> object from which the segment's characters are retrieved.</param>
        /// <param name="start">The index of the segment's first shaped character within its parent shaped string.</param>
        /// <param name="length">The number of shaped characters in the string segment.</param>
        public ShapedStringSegment(ShapedStringBuilder source, Int32 start, Int32 length)
        {
            Contract.Require(source, nameof(source));
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.Source = source;
            this.Start = start;
            this.Length = length;

            this.hashCode = CalculateHashCode(source, start, length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapedStringSegment"/> structure.
        /// </summary>
        /// <param name="source">The source <see cref="ShapedStringSegment"/> object from which the segment's characters are retrieved.</param>
        /// <param name="start">The index of the segment's first shaped character within its parent shaped string.</param>
        /// <param name="length">The number of shaped characters in the segment.</param>
        public ShapedStringSegment(ShapedStringSegment source, Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start <= source.Length, nameof(start));
            Contract.EnsureRange(length >= 0 && start + length <= source.Length, nameof(length));

            this.Source = source.Source;
            this.Start = source.Start + start;
            this.Length = length;

            this.hashCode = CalculateHashCode(source, start, length);
        }

        /// <summary>
        /// Implicitly converts a <see cref="ShapedString"/> to a shaped string segment.
        /// </summary>
        /// <param name="s">The <see cref="ShapedString"/> to convert.</param>
        /// <returns>The converted <see cref="ShapedStringSegment"/>.</returns>
        public static implicit operator ShapedStringSegment(ShapedString s) => 
            (s == null) ? Empty : new ShapedStringSegment(s);

        /// <summary>
        /// Explicitly converts a <see cref="ShapedStringBuilder"/> to a shaped string segment.
        /// </summary>
        /// <param name="sb">The <see cref="ShapedStringBuilder"/> to convert.</param>
        /// <returns>The converted <see cref="ShapedStringSegment"/>.</returns>
        public static explicit operator ShapedStringSegment(ShapedStringBuilder sb) =>
            (sb == null) ? Empty : new ShapedStringSegment(sb);

        /// <summary>
        /// Gets a value indicating whether the specified shaped string segments are contiguous.
        /// </summary>
        /// <param name="s1">The first <see cref="ShapedStringSegment"/>.</param>
        /// <param name="s2">The second <see cref="ShapedStringSegment"/>.</param>
        /// <returns><see langword="true"/> if the shaped string segments are contiguous; otherwise, <see langword="false"/>.</returns>
        public static Boolean AreSegmentsContiguous(ShapedStringSegment s1, ShapedStringSegment s2) =>
            (s1.Source == s2.Source) && (s1.Start + s1.Length == s2.Start || s2.Start + s2.Length == s1.Start);

        /// <summary>
        /// Combines two contiguous shaped string segments.
        /// </summary>
        /// <param name="s1">The first <see cref="ShapedStringSegment"/> to combine.</param>
        /// <param name="s2">The second <see cref="ShapedStringSegment"/> to combine.</param>
        /// <returns>The combined shaped string segment.</returns>
        public static ShapedStringSegment CombineSegments(ShapedStringSegment s1, ShapedStringSegment s2)
        {
            if (!AreSegmentsContiguous(s1, s2))
                throw new InvalidOperationException(CoreStrings.SegmentsAreNotContiguous);

            if (s1.Source is ShapedString str)
                return new ShapedStringSegment(str, s1.Start, s1.Length + s2.Length);

            if (s1.Source is ShapedStringBuilder sb)
                return new ShapedStringSegment(sb, s1.Start, s1.Length + s2.Length);

            return Empty;
        }

        /// <summary>
        /// Creates a new <see cref="ShapedStringSegment"/> from the same source as the specified segment.
        /// </summary>
        /// <param name="segment">The segment from which to retrieve a shaped string source.</param>
        /// <param name="start">The index of the segment's first shaped character within its parent shaped string.</param>
        /// <param name="length">The number of characters in the segment.</param>
        /// <returns>The shaped string segment that was created.</returns>
        public static ShapedStringSegment FromSource(ShapedStringSegment segment, Int32 start, Int32 length)
        {
            if (segment.Source is ShapedString str)
                return new ShapedStringSegment(str, start, length);

            if (segment.Source is ShapedStringBuilder sb)
                return new ShapedStringSegment(sb, start, length);

            if (start != 0)
                throw new ArgumentOutOfRangeException("start");

            if (length != 0)
                throw new ArgumentOutOfRangeException("length");

            return Empty;
        }

        /// <summary>
        /// Creates a shaped string segment which is a substring of this segment.
        /// </summary>
        /// <param name="start">The starting shaped character of the substring within this string segment.</param>
        /// <returns>A <see cref="ShapedStringSegment"/> which is a substring of this segment.</returns>
        public ShapedStringSegment Substring(Int32 start)
        {
            Contract.EnsureRange(start >= 0 && start < this.Length, nameof(start));
            Contract.EnsureNot(IsEmpty, CoreStrings.SegmentIsEmpty);

            var substringLength = (this.Length - start);

            if (Source is ShapedString str)
                return new ShapedStringSegment(str, this.Start + start, substringLength);

            if (Source is ShapedStringBuilder sb)
                return new ShapedStringSegment(sb, this.Start + start, substringLength);

            return Empty;
        }

        /// <summary>
        /// Creates a shaped string segment which is a substring of this segment.
        /// </summary>
        /// <param name="start">The starting shaped character of the substring within this string segment.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A <see cref="ShapedStringSegment"/> which is a substring of this segment.</returns>
        public ShapedStringSegment Substring(Int32 start, Int32 length)
        {
            Contract.EnsureRange(start >= 0 && start < this.Length, nameof(start));
            Contract.EnsureRange(length > 0 && start + length <= this.Length, nameof(length));
            Contract.EnsureNot(IsEmpty, CoreStrings.SegmentIsEmpty);

            if (Source is ShapedString str)
                return new ShapedStringSegment(str, this.Start + start, length);

            if (Source is ShapedStringBuilder sb)
                return new ShapedStringSegment(sb, this.Start + start, length);

            return Empty;
        }

        /// <summary>
        /// Gets the index of the first occurrence of the specified character within the shaped string segment.
        /// </summary>
        /// <param name="value">The shaped character for which to search.</param>
        /// <returns>The index of the first occurrence of the specified shaped character, or -1 if the segment does not contain the shaped character.</returns>
        public Int32 IndexOf(ShapedChar value)
        {
            if (Source is IStringSource<ShapedChar> src)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (src[i] == value)
                        return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of the first occurrence of the specified shaped string within the shaped string segment.
        /// </summary>
        /// <param name="value">The shaped string for which to search.</param>
        /// <returns>The index of the first occurrence of the specified shaped string, or -1 if the segment does not contain the shaped string.</returns>
        public Int32 IndexOf(IStringSource<ShapedChar> value)
        {
            Contract.Require(value, nameof(value));

            if (Source is IStringSource<ShapedChar> src)
            {
                for (int i = 0; i < Length; i++)
                {
                    var matches = 0;

                    for (int j = 0; j < value.Length; j++)
                    {
                        if (src[i + j] != value[j])
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
        /// Gets the shaped character at the specified index within the shaped string segment.
        /// </summary>
        /// <param name="ix">The index of the shaped character to retrieve.</param>
        /// <returns>The shaped character at the specified index.</returns>
        public ShapedChar this[Int32 ix]
        {
            get 
            {
                if (Source is ShapedString str)
                    return str[Start + ix];

                if (Source is ShapedStringBuilder sb)
                    return sb[Start + ix];

                throw new ArgumentOutOfRangeException(nameof(ix));
            }
        }

        /// <summary>
        /// Gets the source object from which the segment's shaped characters are retrieved.
        /// </summary>
        public Object Source { get; }

        /// <summary>
        /// Gets the source <see cref="ShapedString"/> from which the segment's shaped characters are retrieved,
        /// if the segment's source is a <see cref="ShapedString"/> object; otherwise, returns <see langword="null"/>.
        /// </summary>
        public ShapedString SourceShapedString => Source as ShapedString;

        /// <summary>
        /// Gets the source <see cref="ShapedStringBuilder"/> from which the segment's shaped characters are retrieved,
        /// if the segment's source is a <see cref="ShapedStringBuilder"/> object; otherwise, returns <see langword="null"/>.
        /// </summary>
        public ShapedStringBuilder SourceShapedStringBuilder => Source as ShapedStringBuilder;

        /// <summary>
        /// Gets the index of the string segment's first shaped character within its parent shaped string.
        /// </summary>
        public Int32 Start { get; }

        /// <summary>
        /// Gets the number of shaped characters in the segment.
        /// </summary>
        public Int32 Length { get; }

        /// <summary>
        /// Gets the number of shaped characters in the segment's source shaped string.
        /// </summary>
        public Int32 SourceLength
        {
            get
            {
                if (Source is ShapedString str)
                    return str.Length;

                return ((ShapedStringBuilder)Source)?.Length ?? 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Source"/> is a <see cref="ShapedString"/> instance.
        /// </summary>
        public Boolean IsBackedByShapedString => Source is ShapedString;

        /// <summary>
        /// Gets a value indicating whether <see cref="Source"/> is a <see cref="ShapedStringBuilder"/> instance.
        /// </summary>
        public Boolean IsBackedByShapedStringBuilder => Source is ShapedStringBuilder;

        /// <summary>
        /// Represents an empty shaped string segment.
        /// </summary>
        public static readonly ShapedStringSegment Empty = new ShapedStringSegment();

        // Hash code is cached to avoid unnecessary recalculations and also
        // to make sure that it doesn't change if our source is a ShapedStringBuilder instance.
        private readonly Int32 hashCode;
    }
}

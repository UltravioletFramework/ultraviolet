using System;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents an <see cref="IStringSource{TChar}"/> which encapsulates a <see cref="StringSegment"/> instance.
    /// </summary>
    public partial struct StringSegmentSource : ISegmentableStringSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSegmentSource"/> structure.
        /// </summary>
        /// <param name="str">The string segment which is encapsulated by this structure.</param>
        public StringSegmentSource(StringSegment str) => source = str;

        /// <summary>
        /// Implicitly converts a <see cref="StringSegment"/> instance to a <see cref="StringSegmentSource"/> instance.
        /// </summary>
        /// <param name="str">The <see cref="StringSegment"/> instance to convert.</param>
        public static implicit operator StringSegmentSource(StringSegment str) => new StringSegmentSource(str);

        /// <summary>
        /// Explicitly converts a <see cref="StringSegmentSource"/> instance to a <see cref="StringSegment"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="StringSegmentSource"/> instance to convert.</param>
        public static explicit operator StringSegment(StringSegmentSource src) => src.source;

        /// <inheritdoc/>
        public override String ToString() =>
            source.ToString();

        /// <inheritdoc/>
        public void GetChar(Int32 index, out Char ch) => 
            ch = source[index];

        /// <inheritdoc/>
        public StringSegment CreateStringSegment() =>
            source;

        /// <inheritdoc/>
        public StringSegment CreateStringSegmentFromSubstring(Int32 start, Int32 length) =>
            source.Substring(start, length);

        /// <inheritdoc/>
        public StringSegment CreateStringSegmentFromSameOrigin(Int32 start, Int32 length)
        {
            if (source.IsBackedByString)
                return new StringSegment(source.SourceString, start, length);
            if (source.IsBackedByStringBuilder)
                return new StringSegment(source.SourceStringBuilder, start, length);

            if (start != 0)
                throw new ArgumentOutOfRangeException(nameof(start));

            if (length != 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            return StringSegment.Empty;
        }

        /// <inheritdoc/>
        public Char this[Int32 index] =>
            source[index];

        /// <inheritdoc/>
        public Int32 Length => 
            source.Length;

        /// <inheritdoc/>
        public Boolean IsNull =>
            source.Source == null;

        /// <inheritdoc/>
        public Boolean IsEmpty =>
            source.Length == 0;

        // The string which is encapsulated by this structure.
        private readonly StringSegment source;
    }
}

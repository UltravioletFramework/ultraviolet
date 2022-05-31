using System;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents an <see cref="IStringSource{TChar}"/> which encapsulates a <see cref="String"/> instance.
    /// </summary>
    public partial struct StringSource : ISegmentableStringSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSource"/> structure.
        /// </summary>
        /// <param name="str">The string which is encapsulated by this structure.</param>
        public StringSource(String str) => source = str;

        /// <summary>
        /// Implicitly converts a <see cref="String"/> instance to a <see cref="StringSource"/> instance.
        /// </summary>
        /// <param name="str">The <see cref="String"/> instance to convert.</param>
        public static implicit operator StringSource(String str) => new StringSource(str);

        /// <summary>
        /// Explicitly converts a <see cref="StringSource"/> instance to a <see cref="String"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="StringSource"/> instance to convert.</param>
        public static explicit operator String(StringSource src) => src.source;

        /// <inheritdoc/>
        public override String ToString() =>
            source;

        /// <inheritdoc/>
        public void GetChar(Int32 index, out Char ch) => 
            ch = source[index];

        /// <inheritdoc/>
        public StringSegment CreateStringSegment() =>
            new StringSegment(source);

        /// <inheritdoc/>
        public StringSegment CreateStringSegmentFromSubstring(Int32 start, Int32 length) =>
            new StringSegment(source, start, length);

        /// <inheritdoc/>
        public StringSegment CreateStringSegmentFromSameOrigin(Int32 start, Int32 length) =>
            new StringSegment(source, start, length);

        /// <inheritdoc/>
        public Char this[Int32 index] =>
            source[index];

        /// <inheritdoc/>
        public Int32 Length => 
            source.Length;

        /// <inheritdoc/>
        public Boolean IsNull => 
            source == null;

        /// <inheritdoc/>
        public Boolean IsEmpty =>
            source != null && source.Length == 0;

        // The string which is encapsulated by this structure.
        private readonly String source;
    }
}

using System;
using System.Text;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// Represents an <see cref="IStringSource{TChar}"/> which encapsulates a <see cref="StringBuilder"/> instance.
    /// </summary>
    public partial struct StringBuilderSource : ISegmentableStringSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilderSource"/> structure.
        /// </summary>
        /// <param name="str">The string builder which is encapsulated by this structure.</param>
        public StringBuilderSource(StringBuilder str) => source = str;

        /// <summary>
        /// Implicitly converts a <see cref="StringBuilder"/> instance to a <see cref="StringBuilderSource"/> instance.
        /// </summary>
        /// <param name="str">The <see cref="StringBuilder"/> instance to convert.</param>
        public static implicit operator StringBuilderSource(StringBuilder str) => new StringBuilderSource(str);

        /// <summary>
        /// Explicitly converts a <see cref="StringBuilderSource"/> instance to a <see cref="StringBuilder"/> instance.
        /// </summary>
        /// <param name="src">The <see cref="StringBuilderSource"/> instance to convert.</param>
        public static explicit operator StringBuilder(StringBuilderSource src) => src.source;

        /// <inheritdoc/>
        public override String ToString() =>
            source?.ToString();

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

        // The string builder which is encapsulated by this structure.
        private readonly StringBuilder source;
    }
}

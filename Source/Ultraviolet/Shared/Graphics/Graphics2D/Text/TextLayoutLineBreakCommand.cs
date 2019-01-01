using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to break a line.
    /// </summary>
    public struct TextLayoutLineBreakCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutLineBreakCommand"/> structure.
        /// </summary>
        /// <param name="glyphOffset">The offset of the line break within the stream of rendered glyphs.</param>
        /// <param name="glyphLength">The length of the line break when represented as rendered glyphs.</param>
        /// <param name="sourceOffset">The offset of the line break within the source string.</param>
        /// <param name="sourceLength">The length of the line break within the source string.</param>
        public TextLayoutLineBreakCommand(Int32 glyphOffset, Int32 glyphLength, Int32 sourceOffset, Int32 sourceLength)
        {
            this.CommandType = TextLayoutCommandType.LineBreak;
            this.GlyphOffset = glyphOffset;
            this.GlyphLength = glyphLength;
            this.SourceOffset = sourceOffset;
            this.SourceLength = sourceLength;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the offset of the line break within the glyph buffer (or the source string if the text is not shaped).
        /// </summary>
        public Int32 GlyphOffset { get; internal set; }

        /// <summary>
        /// Gets the length of the line break within the glyph buffer (or the source string if the text is not shaped).
        /// </summary>
        public Int32 GlyphLength { get; internal set; }

        /// <summary>
        /// Gets the offset of the line break within the source string.
        /// </summary>
        public Int32 SourceOffset { get; private set; }

        /// <summary>
        /// Gets the length of the line break in the source text.
        /// </summary>
        public Int32 SourceLength { get; private set; }
    }
}

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
        /// <param name="glyphLength">The length of the line break in glyphs.</param>
        /// <param name="sourceLength">The length of the line break in the source text.</param>
        public TextLayoutLineBreakCommand(Int32 glyphLength, Int32 sourceLength)
        {
            this.CommandType = TextLayoutCommandType.LineBreak;
            this.GlyphLength = glyphLength;
            this.SourceLength = sourceLength;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the length of the line break in glyphs.
        /// </summary>
        public Int32 GlyphLength { get; private set; }

        /// <summary>
        /// Gets the length of the line break in the source text.
        /// </summary>
        public Int32 SourceLength { get; private set; }
    }
}

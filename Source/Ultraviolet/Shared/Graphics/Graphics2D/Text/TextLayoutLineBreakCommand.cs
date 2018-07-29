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
        /// <param name="length">The number of glyphs in the line break.</param>
        public TextLayoutLineBreakCommand(Int32 length)
        {
            this.CommandType = TextLayoutCommandType.LineBreak;
            this.Length = length;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the number of glyphs in the line break.
        /// </summary>
        public Int32 Length { get; private set; }
    }
}

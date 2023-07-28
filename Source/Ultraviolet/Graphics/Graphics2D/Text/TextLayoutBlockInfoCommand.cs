using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command containing information about the subsequent block of text.
    /// </summary>
    public struct TextLayoutBlockInfoCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutBlockInfoCommand"/> structure.
        /// </summary>
        /// <param name="offset">The vertical offset of the block of text within its layout area.</param>
        /// <param name="lengthInLines">The length of the block of text in lines.</param>
        public TextLayoutBlockInfoCommand(Int32 offset, Int32 lengthInLines)
        {
            this.CommandType = TextLayoutCommandType.BlockInfo;
            this.Offset = offset;
            this.LengthInLines = lengthInLines;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the vertical offset of the block of text within its layout area.
        /// </summary>
        public Int32 Offset { get; internal set; }

        /// <summary>
        /// Gets the length of the block of text in lines.
        /// </summary>
        public Int32 LengthInLines { get; internal set; }
    }
}

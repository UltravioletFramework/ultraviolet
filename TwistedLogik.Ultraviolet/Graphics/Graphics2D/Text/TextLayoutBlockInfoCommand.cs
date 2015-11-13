using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
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
            this.commandType = TextLayoutCommandType.BlockInfo;
            this.offset = offset;
            this.lengthInLines = lengthInLines;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the vertical offset of the block of text within its layout area.
        /// </summary>
        public Int32 Offset
        {
            get { return offset; }
            internal set { offset = value; }
        }

        /// <summary>
        /// Gets the length of the block of text in lines.
        /// </summary>
        public Int32 LengthInLines
        {
            get { return lengthInLines; }
            internal set { lengthInLines = value; }
        }

        // Property values.
        private TextLayoutCommandType commandType;
        private Int32 offset;
        private Int32 lengthInLines;
    }
}

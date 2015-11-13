using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command containing information about the subsequent line of text.
    /// </summary>
    public struct TextLayoutLineInfoCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutLineInfoCommand"/> structure.
        /// </summary>
        /// <param name="offset">The horizontal offset of the line within its layout area.</param>
        /// <param name="lineWidth">The width of the line in pixels.</param>
        /// <param name="lineHeight">The height of the line in pixels.</param>
        /// <param name="lengthInCommands">The length of the line of text in commands.</param>
        public TextLayoutLineInfoCommand(Int32 offset, Int16 lineWidth, Int16 lineHeight, Int32 lengthInCommands)
        {
            this.commandType = TextLayoutCommandType.LineInfo;
            this.offset = offset;
            this.lineWidth = lineWidth;
            this.lineHeight = lineHeight;
            this.lengthInCommands = lengthInCommands;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the horizontal offset of the line of text within its layout area.
        /// </summary>
        public Int32 Offset
        {
            get { return offset; }
            internal set { offset = value; }
        }

        /// <summary>
        /// Gets the width of the line in pixels.
        /// </summary>
        public Int16 LineWidth
        {
            get { return lineWidth; }
            internal set { lineWidth = value; }
        }

        /// <summary>
        /// Gets the height of the line in pixels.
        /// </summary>
        public Int16 LineHeight
        {
            get { return lineHeight; }
            internal set { lineHeight = value; }
        }

        /// <summary>
        /// Gets the length of the line of text in commands.
        /// </summary>
        public Int32 LengthInCommands
        {
            get { return lengthInCommands; }
            internal set { lengthInCommands = value; }
        }

        // Property values.
        private TextLayoutCommandType commandType;
        private Int32 offset;
        private Int16 lineWidth;
        private Int16 lineHeight;
        private Int32 lengthInCommands;
    }
}

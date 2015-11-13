using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to print a string of text.
    /// </summary>
    public struct TextLayoutTextCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutTextCommand"/> structure.
        /// </summary>
        /// <param name="textOffset">The offset from the beginning of the source string to the beginning of the
        /// substring which will be rendered by this command.</param>
        /// <param name="textLength">The number of characters that will be rendered by this command.</param>
        /// <param name="bounds">The bounds of the text drawn by this command relative to the layout area.</param>
        public TextLayoutTextCommand(Int32 textOffset, Int32 textLength, Rectangle bounds)
        {
            this.commandType = TextLayoutCommandType.Text;
            this.textOffset = textOffset;
            this.textLength = textLength;
            this.bounds = bounds;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the offset from the beginning of the source string to the beginning of the substring
        /// which will be rendered by this command.
        /// </summary>
        public Int32 TextOffset
        {
            get { return textOffset; }
        }

        /// <summary>
        /// Gets the number of characters that will be rendered by this command.
        /// </summary>
        public Int32 TextLength
        {
            get { return textLength; }
        }

        /// <summary>
        /// Gets the bounds of the text drawn by this command relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get { return bounds; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int32 textOffset;
        private readonly Int32 textLength;
        private readonly Rectangle bounds;
    }
}

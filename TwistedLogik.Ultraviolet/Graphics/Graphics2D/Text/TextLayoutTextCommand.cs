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
        /// Gets the absolute position of the text when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <returns>A <see cref="Point2"/> that describes the absolute position of the text.</returns>
        public Point2 GetAbsolutePosition(Int32 x, Int32 y, Int32 lineHeight)
        {
            return new Point2(x + bounds.X, y + bounds.Y + ((lineHeight - bounds.Height) / 2));
        }

        /// <summary>
        /// Gets the absolute position of the text when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <returns>A <see cref="Vector2"/> that describes the absolute position of the text.</returns>
        public Vector2 GetAbsolutePositionVector(Single x, Single y, Int32 lineHeight)
        {
            return new Vector2(x + bounds.X, y + bounds.Y + ((lineHeight - bounds.Height) / 2));
        }

        /// <summary>
        /// Gets the absolute bounds of the text when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <returns>A <see cref="Rectangle"/> that describes the absolute bounds of the text.</returns>
        public Rectangle GetAbsoluteBounds(Int32 x, Int32 y, Int32 lineHeight)
        {
            return new Rectangle(x + bounds.X, y + bounds.Y + ((lineHeight - bounds.Height) / 2), bounds.Width, bounds.Height);
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
            internal set { textOffset = value; }
        }

        /// <summary>
        /// Gets the number of characters that will be rendered by this command.
        /// </summary>
        public Int32 TextLength
        {
            get { return textLength; }
            internal set { textLength = value; }
        }

        /// <summary>
        /// Gets the bounds of the text drawn by this command relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get { return bounds; }
            internal set { bounds = value; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private Int32 textOffset;
        private Int32 textLength;
        private Rectangle bounds;
    }
}

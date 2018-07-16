using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
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
        /// <param name="textX">The x-coordinate of the top-left corner of the text relative to its layout area.</param>
        /// <param name="textY">The y-coordinate of the top-left corner of the text relative to its layout area.</param>
        /// <param name="textWidth">The width of the text in pixels.</param>
        /// <param name="textHeight">The height of the text in pixels.</param>
        public TextLayoutTextCommand(Int32 textOffset, Int32 textLength, Int32 textX, Int32 textY, Int16 textWidth, Int16 textHeight)
        {
            this.commandType = TextLayoutCommandType.Text;
            this.textOffset = textOffset;
            this.textLength = textLength;
            this.textX = textX;
            this.textY = textY;
            this.textWidth = textWidth;
            this.textHeight = textHeight;
        }

        /// <summary>
        /// Gets the absolute position of the text when rendered.
        /// </summary>
        /// <param name="font">The font with which the command is being rendered.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineWidth">The width of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <param name="direction">The direction in which the text is oriented.</param>
        /// <returns>A <see cref="Point2"/> that describes the absolute position of the text.</returns>
        public Point2 GetAbsolutePosition(UltravioletFontFace font, Int32 x, Int32 y, Int32 lineWidth, Int32 lineHeight, TextDirection direction)
        {
            var lineHeightSansDescender = lineHeight + font.Descender;
            var textHeightSansDescender = textHeight + font.Descender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (textX + textWidth) : x + textX;
            var absY = (y - font.Descender) + textY + ((lineHeightSansDescender - textHeightSansDescender) / 2);
            return new Point2(absX, absY);
        }

        /// <summary>
        /// Gets the absolute position of the text when rendered.
        /// </summary>
        /// <param name="font">The font with which the command is being rendered.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineWidth">The width of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <param name="direction">The direction in which the text is oriented.</param>
        /// <returns>A <see cref="Vector2"/> that describes the absolute position of the text.</returns>
        public Vector2 GetAbsolutePositionVector(UltravioletFontFace font, Single x, Single y, Int32 lineWidth, Int32 lineHeight, TextDirection direction)
        {
            var lineHeightSansDescender = lineHeight + font.Descender;
            var textHeightSansDescender = textHeight + font.Descender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (textX + textWidth) : x + textX;
            var absY = (y - font.Descender) + textY + ((lineHeightSansDescender - textHeightSansDescender) / 2);
            return new Vector2(absX, absY);
        }

        /// <summary>
        /// Gets the absolute bounds of the text when rendered.
        /// </summary>
        /// <param name="font">The font with which the command is being rendered.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineWidth">The width of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <param name="direction">The direction in which the text is oriented.</param>
        /// <returns>A <see cref="Rectangle"/> that describes the absolute bounds of the text.</returns>
        public Rectangle GetAbsoluteBounds(UltravioletFontFace font, Int32 x, Int32 y, Int32 lineWidth, Int32 lineHeight, TextDirection direction)
        {
            var lineHeightSansDescender = lineHeight + font.Descender;
            var textHeightSansDescender = textHeight + font.Descender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (textX + textWidth) : x + textX;
            var absY = (y - font.Descender) + textY + ((lineHeightSansDescender - textHeightSansDescender) / 2);
            return new Rectangle(absX, absY, textWidth, textHeight);
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
        /// Gets the x-coordinate of the top-left corner of the text relative to its layout area.
        /// </summary>
        public Int32 TextX
        {
            get { return textX; }
            internal set { textX = value; }
        }

        /// <summary>
        /// Gets the y-coordinate of the top-left corner of the text relative to its layout area.
        /// </summary>
        public Int32 TextY
        {
            get { return textY; }
            internal set { textY = value; }
        }

        /// <summary>
        /// Gets the width of the text in pixels.
        /// </summary>
        public Int16 TextWidth
        {
            get { return textWidth; }
            internal set { textWidth = value; }
        }

        /// <summary>
        /// Gets the height of the text in pixels.
        /// </summary>
        public Int16 TextHeight
        {
            get { return textHeight; }
            internal set { textHeight = value; }
        }

        /// <summary>
        /// Gets the bounds of the text drawn by this command relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get { return new Rectangle(textX, textY, textWidth, textHeight); }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private Int32 textOffset;
        private Int32 textLength;
        private Int32 textX;
        private Int32 textY;
        private Int16 textWidth;
        private Int16 textHeight;
    }
}

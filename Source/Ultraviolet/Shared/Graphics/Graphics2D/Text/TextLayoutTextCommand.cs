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
        /// <param name="textLength">The number of glyphs that will be rendered by this command.</param>
        /// <param name="textX">The x-coordinate of the top-left corner of the text relative to its layout area.</param>
        /// <param name="textY">The y-coordinate of the top-left corner of the text relative to its layout area.</param>
        /// <param name="textWidth">The width of the text in pixels.</param>
        /// <param name="textHeight">The height of the text in pixels.</param>
        public TextLayoutTextCommand(Int32 textOffset, Int32 textLength, Int32 textX, Int32 textY, Int16 textWidth, Int16 textHeight)
        {
            this.CommandType = TextLayoutCommandType.Text;
            this.TextOffset = textOffset;
            this.TextLength = textLength;
            this.TextX = textX;
            this.TextY = textY;
            this.TextWidth = textWidth;
            this.TextHeight = textHeight;
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
            var textHeightSansDescender = TextHeight + font.Descender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (TextX + TextWidth) : x + TextX;
            var absY = (y - font.Descender) + TextY + ((lineHeightSansDescender - textHeightSansDescender) / 2);
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
            var textHeightSansDescender = TextHeight + font.Descender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (TextX + TextWidth) : x + TextX;
            var absY = (y - font.Descender) + TextY + ((lineHeightSansDescender - textHeightSansDescender) / 2);
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
            var textHeightSansDescender = TextHeight + font.Descender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (TextX + TextWidth) : x + TextX;
            var absY = (y - font.Descender) + TextY + ((lineHeightSansDescender - textHeightSansDescender) / 2);
            return new Rectangle(absX, absY, TextWidth, TextHeight);
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the offset from the beginning of the source string to the beginning of the substring
        /// which will be rendered by this command.
        /// </summary>
        public Int32 TextOffset { get; internal set; }

        /// <summary>
        /// Gets the number of glyphs that will be rendered by this command.
        /// </summary>
        public Int32 TextLength { get; internal set; }

        /// <summary>
        /// Gets the x-coordinate of the top-left corner of the text relative to its layout area.
        /// </summary>
        public Int32 TextX { get; internal set; }

        /// <summary>
        /// Gets the y-coordinate of the top-left corner of the text relative to its layout area.
        /// </summary>
        public Int32 TextY { get; internal set; }

        /// <summary>
        /// Gets the width of the text in pixels.
        /// </summary>
        public Int16 TextWidth { get; internal set; }

        /// <summary>
        /// Gets the height of the text in pixels.
        /// </summary>
        public Int16 TextHeight { get; internal set; }

        /// <summary>
        /// Gets the bounds of the text drawn by this command relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get { return new Rectangle(TextX, TextY, TextWidth, TextHeight); }
        }
    }
}

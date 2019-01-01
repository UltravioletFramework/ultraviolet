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
        /// <param name="glyphOffset">The offset of the text within the stream of rendered glyphs.</param>
        /// <param name="glyphLength">The length of the text when represented as rendered glyphs.</param>
        /// <param name="sourceOffset">The offset of the text within the source string.</param>
        /// <param name="sourceLength">The length of the text within the source string.</param>
        /// <param name="shapedOffset">The offset of the text within the shaping buffer (or the source string if the text is not shaped).</param>
        /// <param name="shapedLength">The length of the text within the shaping buffer (or the source string if the text is not shaped).</param>
        /// <param name="textX">The x-coordinate of the top-left corner of the text relative to its layout area.</param>
        /// <param name="textY">The y-coordinate of the top-left corner of the text relative to its layout area.</param>
        /// <param name="textWidth">The width of the text in pixels.</param>
        /// <param name="textHeight">The height of the text in pixels.</param>
        public TextLayoutTextCommand(Int32 glyphOffset, Int32 glyphLength, Int32 sourceOffset, Int32 sourceLength, Int32 shapedOffset, Int32 shapedLength,
            Int32 textX, Int32 textY, Int16 textWidth, Int16 textHeight)
        {
            this.CommandType = TextLayoutCommandType.Text;
            this.GlyphOffset = glyphOffset;
            this.GlyphLength = glyphLength;
            this.SourceOffset = sourceOffset;
            this.SourceLength = sourceLength;
            this.ShapedOffset = shapedOffset;
            this.ShapedLength = shapedLength;
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
        /// Gets the offset of the text within the stream of rendered glyphs.
        /// </summary>
        public Int32 GlyphOffset { get; internal set; }

        /// <summary>
        /// Gets the length of the text when represented as rendered glyphs.
        /// </summary>
        public Int32 GlyphLength { get; internal set; }

        /// <summary>
        /// Gets the offset of the text within the source string.
        /// </summary>
        public Int32 SourceOffset { get; internal set; }

        /// <summary>
        /// Gets the length of the text in the source string.
        /// </summary>
        public Int32 SourceLength { get; internal set; }

        /// <summary>
        /// Gets the offset of the text within the shaping buffer (or the source string if the text is not shaped).
        /// </summary>
        public Int32 ShapedOffset { get; internal set; }

        /// <summary>
        /// Gets the length of the text within the shaping buffer (or the source string if the text is not shaped).
        /// </summary>
        public Int32 ShapedLength { get; internal set; }

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

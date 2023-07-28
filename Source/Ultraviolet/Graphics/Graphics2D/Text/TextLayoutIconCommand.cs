using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to print an icon.
    /// </summary>
    public struct TextLayoutIconCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutIconCommand"/> structure.
        /// </summary>
        /// <param name="iconIndex">The index of the icon within the command stream's icon registry.</param>
        /// <param name="iconX">The x-coordinate of the icon's origin relative to its layout area.</param>
        /// <param name="iconY">The y-coordinate of the icon's origin relative to its layout area.</param>
        /// <param name="iconWidth">The icon's width in pixels.</param>
        /// <param name="iconHeight">The icon's height in pixels.</param>
        /// <param name="iconAscender">The icon's ascender value in pixels.</param>
        /// <param name="iconDescender">The icon's descender value in pixels. Negative values are below the baseline.</param>
        /// <param name="glyphOffset">The offset of the text within the stream of rendered glyphs.</param>
        /// <param name="glyphLength">The length of the text when represented as rendered glyphs.</param>
        /// <param name="sourceOffset">The icon's offset within the source string.</param>
        /// <param name="sourceLength">The icon's length within the source string.</param>
        public TextLayoutIconCommand(Int16 iconIndex, Int32 iconX, Int32 iconY, Int16 iconWidth, Int16 iconHeight, Int16 iconAscender, Int16 iconDescender,
            Int32 glyphOffset, Int32 glyphLength, Int32 sourceOffset, Int32 sourceLength)
        {
            this.CommandType = TextLayoutCommandType.Icon;
            this.IconIndex = iconIndex;
            this.IconX = iconX;
            this.IconY = iconY;
            this.IconWidth = iconWidth;
            this.IconHeight = iconHeight;
            this.IconAscender = iconAscender;
            this.IconDescender = iconDescender;
            this.GlyphOffset = glyphOffset;
            this.GlyphLength = glyphLength;
            this.SourceOffset = sourceOffset;
            this.SourceLength = sourceLength;
        }

        /// <summary>
        /// Gets the absolute position of the icon when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineWidth">The width of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <param name="direction">The direction in which the text is oriented.</param>
        /// <returns>A <see cref="Point2"/> that describes the absolute position of the icon.</returns>
        public Point2 GetAbsolutePosition(Int32 x, Int32 y, Int32 lineWidth, Int32 lineHeight, TextDirection direction)
        {
            var lineHeightSansDescender = lineHeight + IconDescender;
            var iconHeightSansDescender = IconHeight + IconDescender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (IconX + IconWidth) : x + IconX;
            var absY = (y - IconDescender) + IconY + ((lineHeightSansDescender - iconHeightSansDescender) / 2);
            return new Point2(absX, absY);
        }

        /// <summary>
        /// Gets the absolute position of the icon when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineWidth">The width of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <param name="direction">The direction in which the text is oriented.</param>
        /// <returns>A <see cref="Vector2"/> that describes the absolute position of the icon.</returns>
        public Vector2 GetAbsolutePositionVector(Single x, Single y, Int32 lineWidth, Int32 lineHeight, TextDirection direction)
        {
            var lineHeightSansDescender = lineHeight + IconDescender;
            var iconHeightSansDescender = IconHeight + IconDescender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (IconX + IconWidth) : x + IconX;
            var absY = (y - IconDescender) + IconY + ((lineHeightSansDescender - iconHeightSansDescender) / 2);
            return new Vector2(absX, absY);
        }

        /// <summary>
        /// Gets the absolute bounds of the icon when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineWidth">The width of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <param name="direction">The direction in which the text is oriented.</param>
        /// <returns>A <see cref="Rectangle"/> that describes the absolute bounds of the icon.</returns>
        public Rectangle GetAbsoluteBounds(Int32 x, Int32 y, Int32 lineWidth, Int32 lineHeight, TextDirection direction)
        {
            var lineHeightSansDescender = lineHeight + IconDescender;
            var iconHeightSansDescender = IconHeight + IconDescender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (IconX + IconWidth) : x + IconX;
            var absY = (y - IconDescender) + IconY + ((lineHeightSansDescender - iconHeightSansDescender) / 2);
            return new Rectangle(absX, absY, IconWidth, IconHeight);
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the index of the icon within the command stream's icon registry.
        /// </summary>
        public Int16 IconIndex { get; private set; }

        /// <summary>
        /// Gets the x-coordinate of the icon's origin relative to its layout area.
        /// </summary>
        public Int32 IconX { get; internal set; }

        /// <summary>
        /// Gets the y-coordinate of the icon's origin relative to its layout area.
        /// </summary>
        public Int32 IconY { get; internal set; }

        /// <summary>
        /// Gets the icon's width in pixels.
        /// </summary>
        public Int16 IconWidth { get; internal set; }

        /// <summary>
        /// Gets the icon's height in pixels.
        /// </summary>
        public Int16 IconHeight { get; internal set; }

        /// <summary>
        /// Gets the icon's ascender value in pixels.
        /// </summary>
        public Int16 IconAscender { get; internal set; }

        /// <summary>
        /// Gets the icon's descender value in pixels. Negative values are below the baseline.
        /// </summary>
        public Int16 IconDescender { get; internal set; }

        /// <summary>
        /// Gets the offset of the text within the glyph buffer (or the source string if the text is not shaped).
        /// </summary>
        public Int32 GlyphOffset { get; internal set; }

        /// <summary>
        /// Gets the length of the text within the glyph buffer (or the source string if the text is not shaped).
        /// </summary>
        public Int32 GlyphLength { get; internal set; }

        /// <summary>
        /// Gets the icon's offset within the source string.
        /// </summary>
        public Int32 SourceOffset { get; internal set; }

        /// <summary>
        /// Gets the icon's length within the source string.
        /// </summary>
        public Int32 SourceLength { get; internal set; }

        /// <summary>
        /// Gets the bounds of the icon drawn by this command relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get { return new Rectangle(IconX, IconY, IconWidth, IconHeight); }
        }
    }
}

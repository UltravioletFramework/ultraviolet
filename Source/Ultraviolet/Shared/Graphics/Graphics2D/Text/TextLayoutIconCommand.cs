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
        public TextLayoutIconCommand(Int16 iconIndex, Int32 iconX, Int32 iconY, Int16 iconWidth, Int16 iconHeight, Int16 iconAscender, Int16 iconDescender)
        {
            this.commandType = TextLayoutCommandType.Icon;
            this.iconIndex = iconIndex;
            this.iconX = iconX;
            this.iconY = iconY;
            this.iconWidth = iconWidth;
            this.iconHeight = iconHeight;
            this.iconAscender = iconAscender;
            this.iconDescender = iconDescender;
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
            var lineHeightSansDescender = lineHeight + iconDescender;
            var iconHeightSansDescender = iconHeight + iconDescender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (iconX + iconWidth) : x + iconX;
            var absY = (y - iconDescender) + iconY + ((lineHeightSansDescender - iconHeightSansDescender) / 2);
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
            var lineHeightSansDescender = lineHeight + iconDescender;
            var iconHeightSansDescender = iconHeight + iconDescender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (iconX + iconWidth) : x + iconX;
            var absY = (y - iconDescender) + iconY + ((lineHeightSansDescender - iconHeightSansDescender) / 2);
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
            var lineHeightSansDescender = lineHeight + iconDescender;
            var iconHeightSansDescender = iconHeight + iconDescender;
            var absX = (direction == TextDirection.RightToLeft) ? (x + lineWidth) - (iconX + iconWidth) : x + iconX;
            var absY = (y - iconDescender) + iconY + ((lineHeightSansDescender - iconHeightSansDescender) / 2);
            return new Rectangle(absX, absY, iconWidth, iconHeight);
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the index of the icon within the command stream's icon registry.
        /// </summary>
        public Int16 IconIndex
        {
            get { return iconIndex; }
        }

        /// <summary>
        /// Gets the x-coordinate of the icon's origin relative to its layout area.
        /// </summary>
        public Int32 IconX
        {
            get { return iconX; }
            internal set { iconX = value; }
        }

        /// <summary>
        /// Gets the y-coordinate of the icon's origin relative to its layout area.
        /// </summary>
        public Int32 IconY
        {
            get { return iconY; }
            internal set { iconY = value; }
        }

        /// <summary>
        /// Gets the icon's width in pixels.
        /// </summary>
        public Int16 IconWidth
        {
            get { return iconWidth; }
            internal set { iconWidth = value; }
        }

        /// <summary>
        /// Gets the icon's height in pixels.
        /// </summary>
        public Int16 IconHeight
        {
            get { return iconHeight; }
            internal set { iconHeight = value; }
        }

        /// <summary>
        /// Gets the icon's ascender value in pixels.
        /// </summary>
        public Int16 IconAscender
        {
            get { return iconAscender; }
            internal set { iconAscender = value; }
        }

        /// <summary>
        /// Gets the icon's descender value in pixels. Negative values are below the baseline.
        /// </summary>
        public Int16 IconDescender
        {
            get { return iconDescender; }
            internal set { iconDescender = value; }
        }

        /// <summary>
        /// Gets the bounds of the icon drawn by this command relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get { return new Rectangle(iconX, iconY, iconWidth, iconHeight); }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int16 iconIndex;
        private Int32 iconX;
        private Int32 iconY;
        private Int16 iconWidth;
        private Int16 iconHeight;
        private Int16 iconAscender;
        private Int16 iconDescender;
    }
}

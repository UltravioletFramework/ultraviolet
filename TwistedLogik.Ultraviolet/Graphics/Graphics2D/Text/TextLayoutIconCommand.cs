using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
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
        /// <param name="iconWidth">The width of the icon, in pixels, or <c>null</c> to use the icon's default width.</param>
        /// <param name="iconHeight">The height of the icon, in pixels, or <c>null</c> to use the icon's default height.</param>
        /// <param name="bounds">The bounds of the icon drawn by this command relative to the layout area.</param>
        public TextLayoutIconCommand(Int32 iconIndex, Int32? iconWidth, Int32? iconHeight, Rectangle bounds)
        {
            this.commandType = TextLayoutCommandType.Icon;
            this.iconIndex = iconIndex;
            this.iconWidth = iconWidth.GetValueOrDefault();
            this.iconHeight = iconHeight.GetValueOrDefault();
            this.bounds = bounds;
            this.hasIconWidth = iconWidth.HasValue;
            this.hasIconHeight = iconHeight.HasValue;
        }

        /// <summary>
        /// Gets the absolute position of the icon when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <returns>A <see cref="Point2"/> that describes the absolute position of the icon.</returns>
        public Point2 GetAbsolutePosition(Int32 x, Int32 y, Int32 lineHeight)
        {
            return new Point2(x + bounds.X, y + bounds.Y + ((lineHeight - bounds.Height) / 2));
        }

        /// <summary>
        /// Gets the absolute position of the icon when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <returns>A <see cref="Vector2"/> that describes the absolute position of the icon.</returns>
        public Vector2 GetAbsolutePositionVector(Single x, Single y, Int32 lineHeight)
        {
            return new Vector2(x + bounds.X, y + bounds.Y + ((lineHeight - bounds.Height) / 2));
        }

        /// <summary>
        /// Gets the absolute bounds of the icon when rendered.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the line of text that is being rendered.</param>
        /// <param name="lineHeight">The height of the line of text that is being rendered.</param>
        /// <returns>A <see cref="Rectangle"/> that describes the absolute bounds of the icon.</returns>
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
        /// Gets the index of the icon within the command stream's icon registry.
        /// </summary>
        public Int32 IconIndex
        {
            get { return iconIndex; }
        }

        /// <summary>
        /// Gets the width of the icon, in pixels, or 0 if the icon's default width is used.
        /// </summary>
        public Int32? IconWidth
        {
            get { return hasIconWidth ? iconWidth : (Int32?)null; }
        }

        /// <summary>
        /// Gets the height of the icon, in pixels, or 0 if the icon's default height is used.
        /// </summary>
        public Int32? IconHeight
        {
            get { return hasIconHeight ? iconHeight : (Int32?)null; }
        }

        /// <summary>
        /// Gets the bounds of the icon drawn by this command relative to the layout area.
        /// </summary>
        public Rectangle Bounds
        {
            get { return bounds; }
            internal set { bounds = value; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int32 iconIndex;
        private readonly Int32 iconWidth;
        private readonly Int32 iconHeight;
        private Rectangle bounds;
        private readonly Boolean hasIconWidth;
        private readonly Boolean hasIconHeight;
    }
}

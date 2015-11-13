using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a lyout command to print an icon.
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
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int32 iconIndex;
        private readonly Int32 iconWidth;
        private readonly Int32 iconHeight;
        private readonly Rectangle bounds;
        private readonly Boolean hasIconWidth;
        private readonly Boolean hasIconHeight;
    }
}

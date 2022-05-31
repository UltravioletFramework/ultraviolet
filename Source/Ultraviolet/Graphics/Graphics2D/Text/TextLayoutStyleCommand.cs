using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a lyout command to push a font onto the style stack.
    /// </summary>
    public struct TextLayoutStyleCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutStyleCommand"/> structure.
        /// </summary>
        /// <param name="styleIndex">The index of the style within the command stream's style registry.</param>
        public TextLayoutStyleCommand(Int16 styleIndex)
        {
            this.CommandType = TextLayoutCommandType.PushStyle;
            this.StyleIndex = styleIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the index of the style within the command stream's style registry.
        /// </summary>
        public Int16 StyleIndex { get; private set; }
    }
}

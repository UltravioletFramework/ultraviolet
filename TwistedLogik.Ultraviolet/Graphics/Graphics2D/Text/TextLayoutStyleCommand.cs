using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
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
            this.commandType = TextLayoutCommandType.PushStyle;
            this.styleIndex = styleIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the index of the style within the command stream's style registry.
        /// </summary>
        public Int16 StyleIndex
        {
            get { return styleIndex; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int16 styleIndex;
    }
}

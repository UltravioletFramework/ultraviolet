using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to change the source string.
    /// </summary>
    public struct TextLayoutSourceStringCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSourceStringCommand"/> structure.
        /// </summary>
        /// <param name="sourceIndex">The index of the source string within the command stream's source registry.</param>
        public TextLayoutSourceStringCommand(Int16 sourceIndex)
        {
            this.CommandType = TextLayoutCommandType.ChangeSourceString;
            this.SourceIndex = sourceIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the index of the source string within the command stream's source registry.
        /// </summary>
        public Int16 SourceIndex { get; private set; }
    }
}

using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to change the source shaped string.
    /// </summary>
    public struct TextLayoutSourceShapedStringCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSourceShapedStringCommand"/> structure.
        /// </summary>
        /// <param name="sourceIndex">The index of the source shaped string within the command stream's source registry.</param>
        public TextLayoutSourceShapedStringCommand(Int16 sourceIndex)
        {
            this.CommandType = TextLayoutCommandType.ChangeSourceShapedString;
            this.SourceIndex = sourceIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; }

        /// <summary>
        /// Gets the index of the source string within the command stream's source registry.
        /// </summary>
        public Int16 SourceIndex { get; }    
    }
}

using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to change the source shaped string builder.
    /// </summary>
    public struct TextLayoutSourceShapedStringBuilderCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSourceShapedStringBuilderCommand"/> structure.
        /// </summary>
        /// <param name="sourceIndex">The index of the source shaped string builder within the command stream's source registry.</param>
        public TextLayoutSourceShapedStringBuilderCommand(Int16 sourceIndex)
        {
            this.CommandType = TextLayoutCommandType.ChangeSourceShapedStringBuilder;
            this.SourceIndex = sourceIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; }

        /// <summary>
        /// Gets the index of the source shaped string builder within the command stream's source registry.
        /// </summary>
        public Int16 SourceIndex { get; }
    }
}

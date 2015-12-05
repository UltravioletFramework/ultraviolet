using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to change the source string builder.
    /// </summary>
    public struct TextLayoutSourceStringBuilderCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutSourceStringBuilderCommand"/> structure.
        /// </summary>
        /// <param name="sourceIndex">The index of the source string builder within the command stream's source registry.</param>
        public TextLayoutSourceStringBuilderCommand(Int16 sourceIndex)
        {
            this.commandType = TextLayoutCommandType.ChangeSourceStringBuilder;
            this.sourceIndex = sourceIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the index of the source string builder within the command stream's source registry.
        /// </summary>
        public Int16 SourceIndex
        {
            get { return sourceIndex; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int16 sourceIndex;
    }
}

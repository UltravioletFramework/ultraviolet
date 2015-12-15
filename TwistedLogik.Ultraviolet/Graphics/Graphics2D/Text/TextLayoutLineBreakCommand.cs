using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to break a line.
    /// </summary>
    public struct TextLayoutLineBreakCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutLineBreakCommand"/> structure.
        /// </summary>
        /// <param name="length">The number of characters in the line break.</param>
        public TextLayoutLineBreakCommand(Int32 length)
        {
            this.commandType = TextLayoutCommandType.LineBreak;
            this.length = length;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the number of characters in the line break.
        /// </summary>
        public Int32 Length
        {
            get { return length; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int32 length;
    }
}

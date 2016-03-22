using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to push a color onto the color stack.
    /// </summary>
    public struct TextLayoutCustomCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutCustomCommand"/> structure.
        /// </summary>
        /// <param name="id">The custom command's identifier.</param>
        /// <param name="value">The custom command's associated value.</param>
        public TextLayoutCustomCommand(Byte id, Int32 value)
        {
            this.commandType = TextLayoutCommandType.Custom;
            this.id = id;
            this.value = value;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the command identifier.
        /// </summary>
        public Byte ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the command value.
        /// </summary>
        public Int32 Value
        {
            get { return value; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Byte id;
        private readonly Int32 value;
    }
}

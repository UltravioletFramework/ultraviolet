using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
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
            this.CommandType = TextLayoutCommandType.Custom;
            this.ID = id;
            this.Value = value;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the command identifier.
        /// </summary>
        public Byte ID { get; private set; }

        /// <summary>
        /// Gets the command value.
        /// </summary>
        public Int32 Value { get; private set; }
    }
}

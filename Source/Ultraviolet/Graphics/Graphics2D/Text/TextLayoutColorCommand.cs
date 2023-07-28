namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to push a color onto the color stack.
    /// </summary>
    public struct TextLayoutColorCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutColorCommand"/> structure.
        /// </summary>
        /// <param name="color">The color to push onto the color stack.</param>
        public TextLayoutColorCommand(Color color)
        {
            this.CommandType = TextLayoutCommandType.PushColor;
            this.Color = color;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType { get; private set; }

        /// <summary>
        /// Gets the color to push onto the color stack.
        /// </summary>
        public Color Color { get; private set; }
    }
}

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a lyout command to push a color onto the color stack.
    /// </summary>
    public struct TextLayoutColorCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutColorCommand"/> structure.
        /// </summary>
        /// <param name="color">The color to push onto the color stack.</param>
        public TextLayoutColorCommand(Color color)
        {
            this.commandType = TextLayoutCommandType.PushColor;
            this.color = color;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the color to push onto the color stack.
        /// </summary>
        public Color Color
        {
            get { return color; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Color color;
    }
}

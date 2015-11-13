using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a lyout command to push a font onto the font stack.
    /// </summary>
    public struct TextLayoutFontCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutFontCommand"/> structure.
        /// </summary>
        /// <param name="fontIndex">The index of the font within the command stream's font registry.</param>
        public TextLayoutFontCommand(Int32 fontIndex)
        {
            this.commandType = TextLayoutCommandType.PushFont;
            this.fontIndex = fontIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the index of the font within the command stream's font registry.
        /// </summary>
        public Int32 FontIndex
        {
            get { return fontIndex; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int32 fontIndex;
    }
}

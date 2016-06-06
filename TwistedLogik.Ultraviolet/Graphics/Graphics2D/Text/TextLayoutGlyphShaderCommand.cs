using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a layout command to push a glyph shader onto the glyph shader stack.
    /// </summary>
    public struct TextLayoutGlyphShaderCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextLayoutGlyphShaderCommand"/> structure.
        /// </summary>
        /// <param name="glyphShaderIndex">The index of the glyph shader within the command stream's glyph shader registry.</param>
        public TextLayoutGlyphShaderCommand(Int16 glyphShaderIndex)
        {
            this.commandType = TextLayoutCommandType.PushGlyphShader;
            this.glyphShaderIndex = glyphShaderIndex;
        }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        public TextLayoutCommandType CommandType
        {
            get { return commandType; }
        }

        /// <summary>
        /// Gets the index of the glyph shader within the command stream's glyph shader registry.
        /// </summary>
        public Int16 GlyphShaderIndex
        {
            get { return glyphShaderIndex; }
        }

        // Property values.
        private readonly TextLayoutCommandType commandType;
        private readonly Int16 glyphShaderIndex;
    }
}

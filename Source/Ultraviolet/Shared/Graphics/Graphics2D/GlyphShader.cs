using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a shader effect which can be applied to glyphs during text rendering.
    /// </summary>
    public abstract class GlyphShader
    {
        /// <summary>
        /// Executes the glyph shader
        /// </summary>
        /// <param name="context">The current glyph shader context.</param>
        /// <param name="data">The data for the glyph being drawn.</param>
        /// <param name="index">The index of the glyph within its source string.</param>
        public abstract void Execute(ref GlyphShaderContext context, ref GlyphData data, Int32 index);
    }
}

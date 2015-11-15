using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
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
        /// <param name="glyph">The glyph that is being rendered.</param>
        /// <param name="x">The x-coordinate at which the glyph is being rendered.</param>
        /// <param name="y">The y-coordinate at which the glyph is being rendered.</param>
        /// <param name="color">The color in which the glyph is being rendered.</param>
        /// <param name="index">The index of the glyph within its source string.</param>
        public abstract void Execute(ref GlyphShaderContext context, Char glyph, ref Single x, ref Single y, ref Color color, Int32 index);
    }
}

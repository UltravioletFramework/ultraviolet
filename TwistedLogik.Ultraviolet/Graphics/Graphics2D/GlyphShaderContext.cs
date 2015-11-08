using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the context in which a glyph shader executes.
    /// </summary>
    public struct GlyphShaderContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShader">The glyph shader that executes within this context.</param>
        public GlyphShaderContext(GlyphShader glyphShader)
            : this(glyphShader, 0, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShader">The glyph shader that executes within this context.</param>
        /// <param name="sourceOffset">The offset of the text being rendered within the overall source text.</param>
        /// <param name="sourceLength">The overall length of the source text.</param>
        public GlyphShaderContext(GlyphShader glyphShader, Int32 sourceOffset, Int32 sourceLength)
        {
            this.glyphShader = glyphShader;
            this.sourceOffset = sourceOffset;
            this.sourceLength = sourceLength;
        }

        /// <summary>
        /// Implicitly converts a <see cref="GlyphShader"/> instance to a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShader">The glyph shader to convert.</param>
        public static implicit operator GlyphShaderContext(GlyphShader glyphShader)
        {
            return new GlyphShaderContext(glyphShader);
        }

        /// <summary>
        /// Represents an invalid shader context.
        /// </summary>
        public static readonly GlyphShaderContext Invalid = new GlyphShaderContext();

        /// <summary>
        /// Gets the glyph shader that executes within this context.
        /// </summary>
        public GlyphShader GlyphShader
        {
            get { return glyphShader; }
        }

        /// <summary>
        /// Gets the offset of the text being rendered within the overall source text.
        /// </summary>
        public Int32 SourceOffset
        {
            get { return sourceOffset; }
        }

        /// <summary>
        /// Gets the overall length of the source text.
        /// </summary>
        public Int32 SourceLength
        {
            get { return sourceLength; }
        }

        // Property values.
        private readonly GlyphShader glyphShader;
        private readonly Int32 sourceOffset;
        private readonly Int32 sourceLength;
    }
}

using System;
using System.Collections.Generic;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Graphics.Graphics2D
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
        public GlyphShaderContext(GlyphShaderProxy glyphShader)
            : this(glyphShader, 0, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShader">The glyph shader that executes within this context.</param>
        /// <param name="sourceOffset">The offset of the text being rendered within the overall source text.</param>
        /// <param name="sourceLength">The overall length of the source text.</param>
        public GlyphShaderContext(GlyphShaderProxy glyphShader, Int32 sourceOffset, Int32 sourceLength)
        {
            this.glyphShader = glyphShader;
            this.sourceOffset = sourceOffset;
            this.sourceLength = sourceLength;
        }
        
        /// <summary>
        /// Implicitly converts a <see cref="GlyphShader"/> instance to a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShader">The glyph shader to convert.</param>
        /// <returns>The <see cref="GlyphShaderContext"/> instance that was created.</returns>
        public static implicit operator GlyphShaderContext(GlyphShader glyphShader)
        {
            return new GlyphShaderContext(glyphShader);
        }

        /// <summary>
        /// Implicitly converts a stack of <see cref="GlyphShader"/> instances to a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShaderStack">The glyph shader stack to convert.</param>
        /// <returns>The <see cref="GlyphShaderContext"/> instance that was created.</returns>
        public static implicit operator GlyphShaderContext(Stack<GlyphShader> glyphShaderStack)
        {
            return new GlyphShaderContext(glyphShaderStack);
        }

        /// <summary>
        /// Implicitly converts a scoped stack of <see cref="GlyphShader"/> instances to a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShaderScopedStack">The glyph shader stack to convert.</param>
        /// <returns>The <see cref="GlyphShaderContext"/> instance that was created.</returns>
        public static implicit operator GlyphShaderContext(Stack<TextStyleScoped<GlyphShader>> glyphShaderScopedStack)
        {
            return new GlyphShaderContext(glyphShaderScopedStack);
        }

        /// <summary>
        /// Executes the glyph shader.
        /// </summary>
        /// <param name="data">The data for the glyph which is being drawn.</param>
        /// <param name="index">The index of the glyph within its source string.</param>
        public void Execute(ref GlyphData data, Int32 index)
        {
            glyphShader.Execute(ref this, ref data, index);
        }

        /// <summary>
        /// Represents an invalid shader context.
        /// </summary>
        public static readonly GlyphShaderContext Invalid = new GlyphShaderContext();
        
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

        /// <summary>
        /// Gets a value indicating whether this is a valid glyph shader context.
        /// </summary>
        public Boolean IsValid
        {
            get { return glyphShader.IsValid; }
        }

        // Property values.
        private readonly GlyphShaderProxy glyphShader;
        private readonly Int32 sourceOffset;
        private readonly Int32 sourceLength;
    }
}

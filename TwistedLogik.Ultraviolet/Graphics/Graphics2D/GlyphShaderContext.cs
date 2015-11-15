using System;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;

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
            this.glyphShaderScopedStack = null;
            this.glyphShaderStack = null;
            this.glyphShader = glyphShader;
            this.sourceOffset = sourceOffset;
            this.sourceLength = sourceLength;
            this.isValid = (glyphShader != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShaderStack">The stack of glyph shaders that execute within this context.</param>
        public GlyphShaderContext(Stack<GlyphShader> glyphShaderStack)
            : this(glyphShaderStack, 0, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShaderStack">The stack of glyph shaders that execute within this context.</param>
        /// <param name="sourceOffset">The offset of the text being rendered within the overall source text.</param>
        /// <param name="sourceLength">The overall length of the source text.</param>
        public GlyphShaderContext(Stack<GlyphShader> glyphShaderStack, Int32 sourceOffset, Int32 sourceLength)
        {
            this.glyphShaderScopedStack = null;
            this.glyphShaderStack = glyphShaderStack;
            this.glyphShader = null;
            this.sourceOffset = sourceOffset;
            this.sourceLength = sourceLength;
            this.isValid = (glyphShaderStack != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="scopedGlyphShaderStack">The stack of glyph shaders that execute within this context.</param>
        internal GlyphShaderContext(Stack<TextStyleScoped<GlyphShader>> scopedGlyphShaderStack)
            : this(scopedGlyphShaderStack, 0, 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="scopedGlyphShaderStack">The stack of glyph shaders that execute within this context.</param>
        /// <param name="sourceOffset">The offset of the text being rendered within the overall source text.</param>
        /// <param name="sourceLength">The overall length of the source text.</param>
        internal GlyphShaderContext(Stack<TextStyleScoped<GlyphShader>> scopedGlyphShaderStack, Int32 sourceOffset, Int32 sourceLength)
        {
            this.glyphShaderScopedStack = scopedGlyphShaderStack;
            this.glyphShaderStack = null;
            this.glyphShader = null;
            this.sourceOffset = sourceOffset;
            this.sourceLength = sourceLength;
            this.isValid = (scopedGlyphShaderStack != null);
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
        /// Implicitly converts a stack of <see cref="GlyphShader"/> instances to a new instance of the <see cref="GlyphShaderContext"/> structure.
        /// </summary>
        /// <param name="glyphShaderStack">The glyph shader stack to convert.</param>
        public static implicit operator GlyphShaderContext(Stack<GlyphShader> glyphShaderStack)
        {
            return new GlyphShaderContext(glyphShaderStack);
        }

        /// <summary>
        /// Executes the glyph shader.
        /// </summary>
        /// <param name="glyph">The glyph that is being rendered.</param>
        /// <param name="x">The x-coordinate at which the glyph is being rendered.</param>
        /// <param name="y">The y-coordinate at which the glyph is being rendered.</param>
        /// <param name="color">The color in which the glyph is being rendered.</param>
        /// <param name="index">The index of the glyph within its source string.</param>
        public void Execute(Char glyph, ref Single x, ref Single y, ref Color color, Int32 index)
        {
            if (glyphShaderScopedStack != null)
            {
                foreach (var glyphShader in glyphShaderScopedStack)
                    glyphShader.Value.Execute(ref this, glyph, ref x, ref y, ref color, index);
            }
            else if (glyphShaderStack != null)
            {
                foreach (var glyphShader in glyphShaderStack)
                    glyphShader.Execute(ref this, glyph, ref x, ref y, ref color, index);
            }
            else
            {
                if (glyphShader != null)
                    glyphShader.Execute(ref this, glyph, ref x, ref y, ref color, index);
            }
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
            get { return isValid; }
        }

        // Property values.
        private readonly Stack<TextStyleScoped<GlyphShader>> glyphShaderScopedStack;
        private readonly Stack<GlyphShader> glyphShaderStack;
        private readonly GlyphShader glyphShader;
        private readonly Int32 sourceOffset;
        private readonly Int32 sourceLength;
        private readonly Boolean isValid;
    }
}

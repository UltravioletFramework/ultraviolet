using System;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a proxy for a <see cref="GlyphShader"/> instance or a stack of <see cref="GlyphShader"/> instances.
    /// </summary>
    public struct GlyphShaderProxy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderProxy"/> structure.
        /// </summary>
        /// <param name="glyphShader">The glyph shader which is represented by this proxy.</param>
        private GlyphShaderProxy(GlyphShader glyphShader)
        {
            this.glyphShaderScopedStack = null;
            this.glyphShaderStack = null;
            this.glyphShader = glyphShader;
            this.isValid = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderProxy"/> structure.
        /// </summary>
        /// <param name="glyphShaderStack">The glyph shader stack which is represented by this proxy.</param>
        private GlyphShaderProxy(Stack<GlyphShader> glyphShaderStack)
        {
            this.glyphShaderScopedStack = null;
            this.glyphShaderStack = glyphShaderStack;
            this.glyphShader = null;
            this.isValid = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlyphShaderProxy"/> structure.
        /// </summary>
        /// <param name="glyphShaderScopedStack">The glyph shader stack which is represented by this proxy.</param>
        private GlyphShaderProxy(Stack<TextStyleScoped<GlyphShader>> glyphShaderScopedStack)
        {
            this.glyphShaderScopedStack = glyphShaderScopedStack;
            this.glyphShaderStack = null;
            this.glyphShader = null;
            this.isValid = true;
        }

        /// <summary>
        /// Implicitly converts a glyph shader to a <see cref="GlyphShaderProxy"/> structure.
        /// </summary>
        /// <param name="glyphShader">The glyph shader to convert.</param>
        public static implicit operator GlyphShaderProxy(GlyphShader glyphShader)
        {
            return glyphShader == null ? Invalid : new GlyphShaderProxy(glyphShader);
        }

        /// <summary>
        /// Implicitly converts a stack of glyph shaders to a <see cref="GlyphShaderProxy"/> structure.
        /// </summary>
        /// <param name="glyphShaderStack">The glyph shader stack to convert.</param>
        public static implicit operator GlyphShaderProxy(Stack<GlyphShader> glyphShaderStack)
        {
            return glyphShaderStack == null ? Invalid : new GlyphShaderProxy(glyphShaderStack);
        }

        /// <summary>
        /// Implicitly converts a scoped stack of glyph shaders to a <see cref="GlyphShaderProxy"/> structure.
        /// </summary>
        /// <param name="glyphShaderScopedStack">The scoped glyph shader stack to convert.</param>
        public static implicit operator GlyphShaderProxy(Stack<TextStyleScoped<GlyphShader>> glyphShaderScopedStack)
        {
            return (glyphShaderScopedStack == null) ? Invalid : new GlyphShaderProxy(glyphShaderScopedStack);
        }

        /// <summary>
        /// Executes the glyph shader.
        /// </summary>
        /// <param name="context">The glyph shader contxt in which to execute the shader.</param>
        /// <param name="data">The data for the glyph which is being drawn.</param>
        /// <param name="index">The index of the glyph within its source string.</param>
        public void Execute(ref GlyphShaderContext context, ref GlyphData data, Int32 index)
        {
            if (glyphShaderScopedStack != null)
            {
                foreach (var glyphShader in glyphShaderScopedStack)
                    glyphShader.Value.Execute(ref context, ref data, index);
            }
            else if (glyphShaderStack != null)
            {
                foreach (var glyphShader in glyphShaderStack)
                    glyphShader.Execute(ref context, ref data, index);
            }
            else if (glyphShader != null)
            {
                glyphShader.Execute(ref context, ref data, index);
            }
        }

        /// <summary>
        /// Performs an action on each distinct instance of <see cref="GlyphShader"/> which is represented by this proxy.
        /// </summary>
        /// <param name="state">A state value which is passed to <paramref name="action"/>.</param>
        /// <param name="action">A delegate to invoke for each <see cref="GlyphShader"/> instance in the proxy.</param>
        public void ForEach(Object state, Action<Object, GlyphShader> action)
        {
            Contract.Require(action, "action");

            if (glyphShader != null)
            {
                action(state, glyphShader);
            }
            else if (glyphShaderStack != null)
            {
                foreach (var glyphShader in glyphShaderStack)
                    action(state, glyphShader);
            }
            else if (glyphShaderScopedStack != null)
            {
                foreach (var glyphShader in glyphShaderScopedStack)
                    action(state, glyphShader.Value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is a valid glyph shader proxy.
        /// </summary>
        public Boolean IsValid
        {
            get { return isValid; }
        }
        
        /// <summary>
        /// Represents an invalid shader proxy.
        /// </summary>
        public static readonly GlyphShaderProxy Invalid = new GlyphShaderProxy();

        // Glyph shader source references.
        private readonly GlyphShader glyphShader;
        private readonly Stack<GlyphShader> glyphShaderStack;
        private readonly Stack<TextStyleScoped<GlyphShader>> glyphShaderScopedStack;
        private readonly Boolean isValid;
    }
}

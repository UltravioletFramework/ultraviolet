using System;
using TwistedLogik.Gluon;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <inheritdoc/>
    public sealed class OpenGLGraphicsCapabilities : GraphicsCapabilities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLGraphicsCapabilities"/> class.
        /// </summary>
        internal OpenGLGraphicsCapabilities()
        {
            this.maximumTextureSize = gl.GetInteger(gl.GL_MAX_TEXTURE_SIZE);
            this.maximumViewportSize = gl.GetInteger(gl.GL_MAX_VIEWPORT_DIMS);
        }

        /// <inheritdoc/>
        public override Int32 MaximumTextureSize
        {
            get { return maximumTextureSize; }
        }

        /// <inheritdoc/>
        public override Int32 MaximumViewportSize
        {
            get { return maximumViewportSize; }
        }

        // Property values.
        private readonly Int32 maximumTextureSize;
        private readonly Int32 maximumViewportSize;
    }
}

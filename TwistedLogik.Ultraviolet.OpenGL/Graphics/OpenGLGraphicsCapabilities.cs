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
        internal unsafe OpenGLGraphicsCapabilities()
        {
            this.maximumTextureSize = gl.GetInteger(gl.GL_MAX_TEXTURE_SIZE);
            gl.ThrowIfError();

            var viewportDims = stackalloc int[2];
            gl.GetIntegerv(gl.GL_MAX_VIEWPORT_DIMS, viewportDims);
            gl.ThrowIfError();

            this.maximumViewportWidth  = viewportDims[0];
            this.maximumViewportHeight = viewportDims[1];
        }

        /// <inheritdoc/>
        public override Boolean SupportsDepthStencilTextures
        {
            get { return !gl.IsGLES2; }
        }

        /// <inheritdoc/>
        public override Int32 MaximumTextureSize
        {
            get { return maximumTextureSize; }
        }

        /// <inheritdoc/>
        public override Int32 MaximumViewportWidth
        {
            get { return maximumViewportWidth; }
        }

        /// <inheritdoc/>
        public override Int32 MaximumViewportHeight
        {
            get { return maximumViewportHeight; }
        }

        // Property values.
        private readonly Int32 maximumTextureSize;
        private readonly Int32 maximumViewportWidth;
        private readonly Int32 maximumViewportHeight;
    }
}

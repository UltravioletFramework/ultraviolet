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

            this.maximumViewportWidth = viewportDims[0];
            this.maximumViewportHeight = viewportDims[1];

            this.SupportsNonZeroBaseInstance = SupportsInstancedRendering && !gl.IsGLES &&
                (gl.IsVersionAtLeast(4, 2) || gl.IsExtensionSupported("GL_ARB_base_instance"));

            this.SupportsIndependentSamplerState = (gl.IsGLES ? gl.IsVersionAtLeast(3, 0) : gl.IsVersionAtLeast(3, 3)) ||
                gl.IsExtensionSupported("GL_ARB_sampler_objects");
        }

        /// <inheritdoc/>
        public override Boolean SupportsDepthStencilTextures
        {
            get { return !gl.IsGLES2; }
        }

        /// <inheritdoc/>
        public override Boolean SupportsInstancedRendering
        {
            get { return !gl.IsGLES2; }
        }

        /// <inheritdoc/>
        public override Boolean SupportsNonZeroBaseInstance
        {
            get;
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreservingRenderTargetContentInHardware
        {
            get { return true; }
        }

        /// <inheritdoc/>
        public override Boolean SupportsIndependentSamplerState { get; }

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

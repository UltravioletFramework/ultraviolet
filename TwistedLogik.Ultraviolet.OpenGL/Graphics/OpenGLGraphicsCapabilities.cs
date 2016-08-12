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

            this.supportsDepthStencilTextures = !gl.IsGLES2 || gl.IsExtensionSupported("GL_OES_packed_depth_stencil");

            if (gl.IsGLES2 && !this.supportsDepthStencilTextures)
            {
                // HACK: Guess what? The Visual Studio Emulator for Android flat-out lies about this.
                // So it seems the only reliable way to determine support for depth/stencil is to
                // actually try to create one and see if it fails. I hate Android emulators.
                var rb = gl.GenRenderbuffer();
                using (var state = OpenGLState.ScopedBindRenderbuffer(rb, true))
                {
                    gl.RenderbufferStorage(gl.GL_RENDERBUFFER, gl.GL_DEPTH24_STENCIL8, 32, 32);
                    this.supportsDepthStencilTextures = (gl.GetError() == gl.GL_NO_ERROR);
                }
                gl.DeleteRenderBuffers(rb);
            }

            this.SupportsNonZeroBaseInstance = SupportsInstancedRendering && !gl.IsGLES &&
                (gl.IsVersionAtLeast(4, 2) || gl.IsExtensionSupported("GL_ARB_base_instance"));

            this.SupportsIndependentSamplerState = (gl.IsGLES ? gl.IsVersionAtLeast(3, 0) : gl.IsVersionAtLeast(3, 3)) ||
                gl.IsExtensionSupported("GL_ARB_sampler_objects");

            this.SupportsMapBufferRange = true;
            if (gl.IsGLES2)
            {
                this.SupportsMapBufferRange =
                    gl.IsExtensionSupported("GL_ARB_map_buffer_range") ||
                    gl.IsExtensionSupported("GL_EXT_map_buffer_range");
            };

            this.MinMapBufferAlignment = gl.IsExtensionSupported("GL_ARB_map_buffer_alignment") ?
                gl.GetInteger(gl.GL_MIN_MAP_BUFFER_ALIGNMENT) : 0;
        }

        /// <inheritdoc/>
        public override Boolean SupportsDepthStencilTextures
        {
            get { return supportsDepthStencilTextures; }
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

        /// <summary>
        /// Gets a value indicating whether the OpenGL context supports glMapBufferRange().
        /// </summary>
        public Boolean SupportsMapBufferRange { get; }

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

        /// <summary>
        /// Gets the value of GL_MIN_MAP_BUFFER_ALIGNMENT.
        /// </summary>
        public Int32 MinMapBufferAlignment { get; private set; }

        // Property values.
        private readonly Boolean supportsDepthStencilTextures;
        private readonly Int32 maximumTextureSize;
        private readonly Int32 maximumViewportWidth;
        private readonly Int32 maximumViewportHeight;
    }
}

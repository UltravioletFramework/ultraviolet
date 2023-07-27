using System;
using System.Text.RegularExpressions;
using Ultraviolet.Graphics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <inheritdoc/>
    public sealed class OpenGLGraphicsCapabilities : GraphicsCapabilities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLGraphicsCapabilities"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings for the Ultraviolet context.</param>
        internal unsafe OpenGLGraphicsCapabilities(UltravioletConfiguration configuration)
        {
            var glGraphicsConfiguration = configuration.GraphicsConfiguration as OpenGLGraphicsConfiguration;
            if (glGraphicsConfiguration == null)
                throw new InvalidOperationException(OpenGLStrings.InvalidGraphicsConfiguration);

            this.MaximumTextureSize = GL.GetInteger(GL.GL_MAX_TEXTURE_SIZE);
            GL.ThrowIfError();

            var viewportDims = stackalloc int[2];
            GL.GetIntegerv(GL.GL_MAX_VIEWPORT_DIMS, viewportDims);
            GL.ThrowIfError();

            this.MaximumViewportWidth = viewportDims[0];
            this.MaximumViewportHeight = viewportDims[1];

            this.SupportsInstancedRendering = GL.IsInstancedRenderingAvailable;
            this.Supports3DTextures = GL.IsTexture3DAvailable;
            this.SupportsDepthStencilTextures = GL.IsCombinedDepthStencilAvailable;

            if (GL.IsGLES2 && !this.SupportsDepthStencilTextures)
            {
                // HACK: Guess what? The Visual Studio Emulator for Android flat-out lies about this.
                // So it seems the only reliable way to determine support for depth/stencil is to
                // actually try to create one and see if it fails. I hate Android emulators.
                var rb = GL.GenRenderbuffer();
                using (var state = OpenGLState.ScopedBindRenderbuffer(rb, true))
                {
                    GL.RenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_DEPTH24_STENCIL8, 32, 32);
                    this.SupportsDepthStencilTextures = (GL.GetError() == GL.GL_NO_ERROR);
                }
                GL.DeleteRenderBuffers(rb);
            }

            this.SupportsNonZeroBaseInstance = GL.IsNonZeroBaseInstanceAvailable;
            this.SupportsIndependentSamplerState = GL.IsSamplerObjectAvailable;
            this.SupportsIntegerVertexAttributes = GL.IsIntegerVertexAttribAvailable;
            this.SupportsDoublePrecisionVertexAttributes = GL.IsDoublePrecisionVertexAttribAvailable;                         

            this.MinMapBufferAlignment = GL.IsExtensionSupported("GL_ARB_map_buffer_alignment") ?
                GL.GetInteger(GL.GL_MIN_MAP_BUFFER_ALIGNMENT) : 0;

            // SRGB is always supported unless we're on GLES2, in which case we need an extension.
            // If it wasn't explicitly enabled in the configuration, we treat it like it's not supported.
            this.SrgbEncodingEnabled = glGraphicsConfiguration.SrgbBuffersEnabled && GL.IsHardwareSrgbSupportAvailable;                

            // There seems to be a bug in the version of Mesa which is distributed
            // with Ubuntu 16.04 that causes long stalls when using glMapBufferRange.
            // Testing indicates that this is fixed in 11.2.2.
            if (glGraphicsConfiguration.UseBufferMapping)
            {
                var version = GL.GetString(GL.GL_VERSION);
                var versionMatchMesa = Regex.Match(version, "Mesa (?<major>\\d+).(?<minor>\\d+).(?<build>\\d+)");
                if (versionMatchMesa != null && versionMatchMesa.Success)
                {
                    var mesaMajor = Int32.Parse(versionMatchMesa.Groups["major"].Value);
                    var mesaMinor = Int32.Parse(versionMatchMesa.Groups["minor"].Value);
                    var mesaBuild = Int32.Parse(versionMatchMesa.Groups["build"].Value);
                    var mesaVersion = new Version(mesaMajor, mesaMinor, mesaBuild);
                    if (mesaVersion < new Version(11, 2, 2))
                    {
                        glGraphicsConfiguration.UseBufferMapping = false;
                    }
                }
            }

            // If we've been explicitly told to disable buffer mapping, override the caps from the driver.
            if (!GL.IsMapBufferRangeAvailable || !glGraphicsConfiguration.UseBufferMapping)
                this.MinMapBufferAlignment = Int32.MinValue;
        }

        /// <inheritdoc/>
        public override Boolean FlippedTextures { get; } = true;

        /// <inheritdoc/>
        public override Boolean Supports3DTextures { get; }

        /// <inheritdoc/>
        public override Boolean SupportsDepthStencilTextures { get; }

        /// <inheritdoc/>
        public override Boolean SupportsInstancedRendering { get; }

        /// <inheritdoc/>
        public override Boolean SupportsNonZeroBaseInstance { get; }

        /// <inheritdoc/>
        public override Boolean SupportsPreservingRenderTargetContentInHardware { get; } = true;

        /// <inheritdoc/>
        public override Boolean SupportsIndependentSamplerState { get; }

        /// <inheritdoc/>
        public override Boolean SupportsIntegerVertexAttributes { get; }

        /// <inheritdoc/>
        public override Boolean SupportsDoublePrecisionVertexAttributes { get; }

        /// <inheritdoc/>
        public override Boolean SrgbEncodingEnabled { get; }

        /// <inheritdoc/>
        public override Int32 MaximumTextureSize { get; }

        /// <inheritdoc/>
        public override Int32 MaximumViewportWidth { get; }

        /// <inheritdoc/>
        public override Int32 MaximumViewportHeight { get; }

        /// <summary>
        /// Gets the value of GL_MIN_MAP_BUFFER_ALIGNMENT.
        /// </summary>
        public Int32 MinMapBufferAlignment { get; private set; }
    }
}

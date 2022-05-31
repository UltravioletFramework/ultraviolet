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

            this.MaximumTextureSize = gl.GetInteger(gl.GL_MAX_TEXTURE_SIZE);
            gl.ThrowIfError();

            var viewportDims = stackalloc int[2];
            gl.GetIntegerv(gl.GL_MAX_VIEWPORT_DIMS, viewportDims);
            gl.ThrowIfError();

            this.MaximumViewportWidth = viewportDims[0];
            this.MaximumViewportHeight = viewportDims[1];

            this.SupportsInstancedRendering = gl.IsInstancedRenderingAvailable;
            this.Supports3DTextures = gl.IsTexture3DAvailable;
            this.SupportsDepthStencilTextures = gl.IsCombinedDepthStencilAvailable;

            if (gl.IsGLES2 && !this.SupportsDepthStencilTextures)
            {
                // HACK: Guess what? The Visual Studio Emulator for Android flat-out lies about this.
                // So it seems the only reliable way to determine support for depth/stencil is to
                // actually try to create one and see if it fails. I hate Android emulators.
                var rb = gl.GenRenderbuffer();
                using (var state = OpenGLState.ScopedBindRenderbuffer(rb, true))
                {
                    gl.RenderbufferStorage(gl.GL_RENDERBUFFER, gl.GL_DEPTH24_STENCIL8, 32, 32);
                    this.SupportsDepthStencilTextures = (gl.GetError() == gl.GL_NO_ERROR);
                }
                gl.DeleteRenderBuffers(rb);
            }

            this.SupportsNonZeroBaseInstance = gl.IsNonZeroBaseInstanceAvailable;
            this.SupportsIndependentSamplerState = gl.IsSamplerObjectAvailable;
            this.SupportsIntegerVertexAttributes = gl.IsIntegerVertexAttribAvailable;
            this.SupportsDoublePrecisionVertexAttributes = gl.IsDoublePrecisionVertexAttribAvailable;                         

            this.MinMapBufferAlignment = gl.IsExtensionSupported("GL_ARB_map_buffer_alignment") ?
                gl.GetInteger(gl.GL_MIN_MAP_BUFFER_ALIGNMENT) : 0;

            // SRGB is always supported unless we're on GLES2, in which case we need an extension.
            // If it wasn't explicitly enabled in the configuration, we treat it like it's not supported.
            this.SrgbEncodingEnabled = glGraphicsConfiguration.SrgbBuffersEnabled && gl.IsHardwareSrgbSupportAvailable;                

            // There seems to be a bug in the version of Mesa which is distributed
            // with Ubuntu 16.04 that causes long stalls when using glMapBufferRange.
            // Testing indicates that this is fixed in 11.2.2.
            if (glGraphicsConfiguration.UseBufferMapping)
            {
                var version = gl.GetString(gl.GL_VERSION);
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
            if (!gl.IsMapBufferRangeAvailable || !glGraphicsConfiguration.UseBufferMapping)
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

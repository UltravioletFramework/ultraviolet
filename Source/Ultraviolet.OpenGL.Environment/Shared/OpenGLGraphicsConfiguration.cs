using System;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the configuration settings for the OpenGL graphics subsystem implementation.
    /// </summary>
    public class OpenGLGraphicsConfiguration : UltravioletGraphicsConfiguration
    {
        /// <inheritdoc/>
        public override String GraphicsApiName { get; } = "OpenGL";

        /// <summary>
        /// Gets or sets the minimum OpenGL version that is required by the application.
        /// This cannot be lower than the minimum version required by Ultraviolet itself.
        /// </summary>
        public Version MinimumOpenGLVersion { get; set; }

        /// <summary>
        /// Gets or sets the minimum OpenGL ES version that is required by the application.
        /// This cannot be lower than the minimum version required by Ultraviolet itself.
        /// </summary>
        public Version MinimumOpenGLESVersion { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of bits in the depth buffer.
        /// </summary>
        public Int32 BackBufferDepthSize
        {
            get { return backBufferDepthSize; }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                backBufferDepthSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of bits in the stencil buffer.
        /// </summary>
        public Int32 BackBufferStencilSize
        {
            get { return backBufferStencilSize; }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                backBufferStencilSize = value;
            }
        }

        /// <summary>
        /// Gets the number of buffers used for multisample anti-aliasing.
        /// </summary>
        public Int32 MultiSampleBuffers
        {
            get { return multiSampleBuffers; }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                multiSampleBuffers = value;
            }
        }

        /// <summary>
        /// Gets the number of samples around the current pixel used for multisample anti-aliasing.
        /// </summary>
        public Int32 MultiSampleSamples
        {
            get { return multiSampleSamples; }
            set
            {
                Contract.EnsureRange(value >= 0, nameof(value));

                multiSampleSamples = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the context will disable hardware input by default.
        /// </summary>
        public Boolean IsHardwareInputDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the default framebuffer should be 32-bit (if <see langword="true"/>)
        /// or 16-bit (if <see langword="false"/>). 
        /// </summary>
        public Boolean Use32BitFramebuffer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Ultraviolet should attempt to use buffer mapping when
        /// rendering geometry, if it is available on the current driver.
        /// </summary>
        public Boolean UseBufferMapping { get; set; }

        /// <summary>
        /// The default configuration for the OpenGL implementation.
        /// </summary>
        public static OpenGLGraphicsConfiguration Default => new OpenGLGraphicsConfiguration();

        // Property values.
        private Int32 backBufferDepthSize = 16;
        private Int32 backBufferStencilSize = 1;
        private Int32 multiSampleBuffers = 0;
        private Int32 multiSampleSamples = 0;
    }
}

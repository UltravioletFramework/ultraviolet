using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the Ultraviolet Framework's configuration settings under the OpenGL/SDL2 implementation.
    /// </summary>
    [CLSCompliant(true)]
    public sealed class OpenGLUltravioletConfiguration : UltravioletConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletConfiguration class.
        /// </summary>
        public OpenGLUltravioletConfiguration()
        {
#if PRODUCTION
            AudioSubsystemAssembly = "TwistedLogik.Ultraviolet.BASS, Version=1.2.0.0, Culture=neutral, PublicKeyToken=78da2f4877323311, processorArchitecture=MSIL";
#else
            AudioSubsystemAssembly = "TwistedLogik.Ultraviolet.BASS, Version=1.2.0.0, Culture=neutral, processorArchitecture=MSIL";
#endif
        }

        /// <summary>
        /// Gets or sets the name of the assembly that implements the audio subsystem.
        /// </summary>
        public String AudioSubsystemAssembly
        {
            get { return audioSubsystemAssembly; }
            set { audioSubsystemAssembly = value; }
        }

        /// <summary>
        /// Gets or sets the minimum OpenGL version that is required by the application.
        /// This cannot be lower than the minimum version required by Ultraviolet itself.
        /// </summary>
        public Version MinimumOpenGLVersion
        {
            get { return minimumOpenGLVersion; }
            set { minimumOpenGLVersion = value; }
        }

        /// <summary>
        /// Gets or sets the minimum number of bits in the depth buffer.
        /// </summary>
        public Int32 BackBufferDepthSize
        {
            get { return backBufferDepthSize; }
            set
            {
                Contract.EnsureRange(value >= 0, "value");

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
                Contract.EnsureRange(value >= 0, "value");

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
                Contract.EnsureRange(value >= 0, "value");

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
                Contract.EnsureRange(value >= 0, "value");

                multiSampleSamples = value;
            }
        }

        /// <summary>
        /// The default configuration for the OpenGL/SDL2 implementation.
        /// </summary>
        public static readonly OpenGLUltravioletConfiguration Default = new OpenGLUltravioletConfiguration();

        // Property values.
        private String audioSubsystemAssembly;
        private Version minimumOpenGLVersion;
        private Int32 backBufferDepthSize = 24;
        private Int32 backBufferStencilSize = 8;
        private Int32 multiSampleBuffers = 1;
        private Int32 multiSampleSamples = 4;
    }
}

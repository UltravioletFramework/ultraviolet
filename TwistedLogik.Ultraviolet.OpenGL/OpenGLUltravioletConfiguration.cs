using System;

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
            AudioSubsystemAssembly = "TwistedLogik.Ultraviolet.BASS, Version=1.1.0.0, Culture=neutral, PublicKeyToken=78da2f4877323311, processorArchitecture=MSIL";
        }

        /// <summary>
        /// Gets or sets the name of the assembly that implements the audio subsystem.
        /// </summary>
        public String AudioSubsystemAssembly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum OpenGL version that is required by the application.
        /// This cannot be lower than the minimum version required by Ultraviolet itself.
        /// </summary>
        public Version MinimumOpenGLVersion
        {
            get;
            set;
        }

        /// <summary>
        /// The default configuration for the OpenGL/SDL2 implementation.
        /// </summary>
        public static readonly OpenGLUltravioletConfiguration Default = new OpenGLUltravioletConfiguration();
    }
}

using System;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents the configuration information for SDL2 windows.
    /// </summary>
    public sealed class SDL2PlatformConfiguration
    {
        /// <summary>
        /// Gets or sets the rendering API which will be used by the application.
        /// </summary>
        public SDL2PlatformRenderingAPI RenderingAPI { get; set; }

        /// <summary>
        /// Gets the number of buffers used for multisample anti-aliasing.
        /// </summary>
        public Int32 MultiSampleBuffers { get; set; }

        /// <summary>
        /// Gets the number of samples around the current pixel used for multisample anti-aliasing.
        /// </summary>
        public Int32 MultiSampleSamples { get; set; }
    }
}

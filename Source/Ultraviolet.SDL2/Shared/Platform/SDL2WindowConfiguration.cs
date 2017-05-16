using System;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the configuration information for SDL2 windows.
    /// </summary>
    public sealed class SDL2WindowConfiguration
    {
        /// <summary>
        /// Gets or sets the type of window to create.
        /// </summary>
        public SDL2WindowType WindowType { get; set; }

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

using System;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Encapsulates the capabilities of a graphics device.
    /// </summary>
    public abstract class GraphicsCapabilities
    {
        /// <summary>
        /// Gets the maximum texture size supported by the device.
        /// </summary>
        public abstract Int32 MaximumTextureSize { get; }

        /// <summary>
        /// Gets the maximum supported viewport width.
        /// </summary>
        public abstract Int32 MaximumViewportWidth { get; }

        /// <summary>
        /// Gets the maximum supported viewport height.
        /// </summary>
        public abstract Int32 MaximumViewportHeight { get; }
    }
}

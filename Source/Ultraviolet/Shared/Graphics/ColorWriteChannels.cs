using System;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the color channels which are used during rendering. Color writes can be enabled or
    /// disabled per-channel using the appropriate <see cref="BlendState"/>.
    /// </summary>
    [Flags]
    public enum ColorWriteChannels
    {
        /// <summary>
        /// No color channels.
        /// </summary>
        None = 0,

        /// <summary>
        /// The red color channel.
        /// </summary>
        Red = 1,

        /// <summary>
        /// The green color channel.
        /// </summary>
        Green = 2,

        /// <summary>
        /// The blue color channel.
        /// </summary>
        Blue = 4,

        /// <summary>
        /// The alpha channel.
        /// </summary>
        Alpha = 8,

        /// <summary>
        /// All color channels, plus alpha.
        /// </summary>
        All = Alpha | Red | Green | Blue
    }
}

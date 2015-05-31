using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the effects that can be applied to a sprite.
    /// </summary>
    [Flags]
    public enum SpriteEffects
    {
        /// <summary>
        /// No effects.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Flips the sprite horizontally across the y-axis.
        /// </summary>
        FlipHorizontally = 0x0001,

        /// <summary>
        /// Flips the sprite vertically across the x-axis.
        /// </summary>
        FlipVertically = 0x0002,

        /// <summary>
        /// Indicates that the sprite's point of origin should be considered relative to
        /// its destination rectangle, rather than its source rectangle.
        /// </summary>
        OriginRelativeToDestination = 0x0004,
    }
}

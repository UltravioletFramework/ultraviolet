using System;

namespace Ultraviolet.Graphics.Graphics2D
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

        /// <summary>
        /// Indicates that text should be drawn left-to-right (the default behavior).
        /// </summary>
        LtrText = 0x0000,

        /// <summary>
        /// Indicates that text should be drawn right-to-left.
        /// </summary>
        RtlText = 0x0020,
    }
}

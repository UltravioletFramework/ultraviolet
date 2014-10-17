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
        None,

        /// <summary>
        /// Flips the sprite horizontally across the y-axis.
        /// </summary>
        FlipHorizontally,

        /// <summary>
        /// Flips the sprite vertically across the x-axis.
        /// </summary>
        FlipVertically,
    }
}

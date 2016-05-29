using System.Collections.Generic;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a <see cref="Sprite"/> object during deserialization.
    /// </summary>
    internal sealed class SpriteDescription
    {
        /// <summary>
        /// Retrieves an array containing the sprite's list of animations.
        /// </summary>
        public IList<SpriteAnimationDescription> Animations { get; set; }
    }
}

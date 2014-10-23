
namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a <see cref="Sprite"/> object during deserialization.
    /// </summary>
    public sealed class SpriteDescription
    {
        /// <summary>
        /// Retrieves an array containing the sprite's list of animations.
        /// </summary>
        public SpriteAnimationDescription[] Animations
        {
            get;
            set;
        }
    }
}

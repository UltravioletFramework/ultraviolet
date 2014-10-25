using System;
using System.Collections.Generic;
using System.Linq;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents an animated two-dimensional image.
    /// </summary>
    public sealed class Sprite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        /// <param name="animations">A collection containing the sprite's animations, or <c>null</c> if the sprite has no animations.</param>
        public Sprite(IEnumerable<SpriteAnimation> animations = null)
        {
            this.animations = new List<SpriteAnimation>(animations ?? Enumerable.Empty<SpriteAnimation>());
            this.RefreshNameCache();
        }

        /// <summary>
        /// Updates the sprite's default animation controllers.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            foreach (var animation in animations)
            {
                animation.Controller.Update(time);
            }
        }

        /// <summary>
        /// Retrieves the animation with the specified index.
        /// </summary>
        /// <param name="i">The index of the animation to retrieve.</param>
        /// <returns>The <see cref="SpriteAnimation"/> with the specified index.</returns>
        public SpriteAnimation this[int i]
        {
            get
            {
                return animations[i];
            }
        }

        /// <summary>
        /// Retrieves the animation with the specified name.
        /// </summary>
        /// <param name="name">The name of the animation to retrieve.</param>
        /// <returns>The <see cref="SpriteAnimation"/> with the specified name, or <c>null</c> if no such animation exists.</returns>
        public SpriteAnimation this[string name]
        {
            get
            {
                SpriteAnimation animation;
                animationCacheByName.TryGetValue(name, out animation);
                return animation;
            }
        }

        /// <summary>
        /// Gets the number of animations in the sprite.
        /// </summary>
        public Int32 AnimationCount
        {
            get { return animations.Count; }
        }

        /// <summary>
        /// Refreshes the sprite's animation name cache.
        /// </summary>
        private void RefreshNameCache()
        {
            animationCacheByName.Clear();
            foreach (var animation in animations)
            {
                if (animation.Name != null)
                {
                    animationCacheByName[animation.Name] = animation;
                }
            }
        }

        // Sprite animations.
        private readonly List<SpriteAnimation> animations;
        private readonly Dictionary<String, SpriteAnimation> animationCacheByName =
            new Dictionary<String, SpriteAnimation>();
    }
}

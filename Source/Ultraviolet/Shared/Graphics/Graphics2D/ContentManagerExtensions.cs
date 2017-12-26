using System;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Contains extension methods for the <see cref="ContentManager"/> class.
    /// </summary>
    public static class ContentManagerExtensions
    {
        /// <summary>
        /// Loads the specified sprite animation.
        /// </summary>
        /// <remarks>Content managers maintain a cache of references to all loaded assets, so calling <see cref="ContentManager.Load"/> multiple
        /// times on a content manager with the same parameter will return the same object rather than reloading the source file.</remarks>
        /// <param name="contentManager">The <see cref="ContentManager"/> with which to load the animation's associated sprite asset.</param>
        /// <param name="id">The identifier that represents the sprite animation to load.</param>
        /// <param name="cache">A value indicating whether to add the sprite asset to the manager's cache.</param>
        /// <returns>The sprite animation that was loaded.</returns>
        public static SpriteAnimation Load(this ContentManager contentManager, SpriteAnimationID id, Boolean cache = true)
        {
            Contract.Require(contentManager, nameof(contentManager));
            Contract.Ensure<ArgumentException>(id.IsValid, nameof(id));
            Contract.EnsureNotDisposed(contentManager, contentManager.Disposed);

            var primaryDisplay = contentManager.Ultraviolet.GetPlatform().Displays.PrimaryDisplay;
            var primaryDisplayDensity = primaryDisplay?.DensityBucket ?? ScreenDensityBucket.Desktop;

            return LoadInternal(contentManager, id, primaryDisplayDensity, cache);
        }

        /// <summary>
        /// Loads the specified sprite animation.
        /// </summary>
        /// <remarks>Content managers maintain a cache of references to all loaded assets, so calling <see cref="ContentManager.Load"/> multiple
        /// times on a content manager with the same parameter will return the same object rather than reloading the source file.</remarks>
        /// <param name="contentManager">The <see cref="ContentManager"/> with which to load the animation's associated sprite asset.</param>
        /// <param name="id">The identifier that represents the sprite animation to load.</param>
        /// <param name="density">The screen density for which to load the sprite.</param>
        /// <param name="cache">A value indicating whether to add the sprite asset to the manager's cache.</param>
        /// <returns>The sprite animation that was loaded.</returns>
        public static SpriteAnimation Load(this ContentManager contentManager, SpriteAnimationID id, ScreenDensityBucket density, Boolean cache = true)
        {
            Contract.Require(contentManager, nameof(contentManager));
            Contract.Ensure<ArgumentException>(id.IsValid, nameof(id));
            Contract.EnsureNotDisposed(contentManager, contentManager.Disposed);

            return LoadInternal(contentManager, id, density, cache);
        }

        /// <summary>
        /// Loads the specified sprite animation.
        /// </summary>
        private static SpriteAnimation LoadInternal(this ContentManager contentManager, SpriteAnimationID id, ScreenDensityBucket density, Boolean cache)
        {
            var sprite = contentManager.Load<Sprite>(SpriteAnimationID.GetSpriteAssetIDRef(ref id), density, cache);

            var name = SpriteAnimationID.GetAnimationNameRef(ref id);
            if (!String.IsNullOrEmpty(name))
            {
                var value = sprite[name];
                if (value == null)
                    throw new ArgumentException(UltravioletStrings.InvalidSpriteAnimationReference.Format(id));

                return value;
            }

            var index = SpriteAnimationID.GetAnimationIndexRef(ref id);
            if (index < 0 || index >= sprite.AnimationCount)
                throw new ArgumentException(UltravioletStrings.InvalidSpriteAnimationReference.Format(id));

            return sprite[index];
        }
    }
}

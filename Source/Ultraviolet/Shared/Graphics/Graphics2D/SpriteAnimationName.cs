using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the name or index of a sprite animation.
    /// </summary>
    public partial struct SpriteAnimationName : IEquatable<SpriteAnimationName>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimationName"/> structure.
        /// </summary>
        /// <param name="name">The animation name represented by this structure.</param>
        public SpriteAnimationName(String name)
        {
            this.animationIndex = 0;
            this.animationName = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimationName"/> structure.
        /// </summary>
        /// <param name="index">The animation index represented by this structure.</param>
        public SpriteAnimationName(Int32 index)
        {
            this.animationIndex = index;
            this.animationName = null;
        }

        /// <summary>
        /// Explicitly converts an instance of the <see cref="SpriteAnimationName"/> structure
        /// to its underlying animation index.
        /// </summary>
        /// <param name="name">The <see cref="SpriteAnimationName"/> structure to convert.</param>
        public static explicit operator Int32(SpriteAnimationName name)
        {
            if (!name.IsIndex)
                throw new InvalidCastException();

            return name.animationIndex;
        }

        /// <summary>
        /// Explicitly converts an instance of the <see cref="SpriteAnimationName"/> structure
        /// to its underlying animation name.
        /// </summary>
        /// <param name="name">The <see cref="SpriteAnimationName"/> structure to convert.</param>
        public static explicit operator String(SpriteAnimationName name)
        {
            if (!name.IsName)
                throw new InvalidCastException();

            return name.animationName;
        }

        /// <inheritdoc/>
        public override String ToString() => animationName ?? animationIndex.ToString();

        /// <summary>
        /// Gets a value indicating whether this structure represents an animation index.
        /// </summary>
        public Boolean IsIndex => animationName == null;

        /// <summary>
        /// Gets a value indicating whether this structure represents an animation name.
        /// </summary>
        public Boolean IsName => animationName != null;

        // State values.
        private String animationName;
        private Int32 animationIndex;
    }
}

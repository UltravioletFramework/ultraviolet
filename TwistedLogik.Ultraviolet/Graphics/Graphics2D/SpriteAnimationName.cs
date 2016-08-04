using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the name or index of a sprite animation.
    /// </summary>
    public struct SpriteAnimationName : IEquatable<SpriteAnimationName>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimationName"/> structure.
        /// </summary>
        /// <param name="name">The animation name represented by this structure.</param>
        [Preserve]
        public SpriteAnimationName(String name)
        {
            this.animationIndex = 0;
            this.animationName = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimationName"/> structure.
        /// </summary>
        /// <param name="index">The animation index represented by this structure.</param>
        [Preserve]
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

        /// <summary>
        /// Returns <see langword="true"/> if the specified animation names are equal.
        /// </summary>
        /// <param name="name1">The first <see cref="SpriteAnimationName"/> to compare.</param>
        /// <param name="name2">The second <see cref="SpriteAnimationName"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified animation names are equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator ==(SpriteAnimationName name1, SpriteAnimationName name2)
        {
            return name1.Equals(name2);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the specified animation names are not equal.
        /// </summary>
        /// <param name="name1">The first <see cref="SpriteAnimationName"/> to compare.</param>
        /// <param name="name2">The second <see cref="SpriteAnimationName"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified animation names are not equal; otherwise, <see langword="false"/>.</returns>
        public static Boolean operator !=(SpriteAnimationName name1, SpriteAnimationName name2)
        {
            return !name1.Equals(name2);
        }

        /// <summary>
        /// Converts the string representation of a sprite animation name to an instance of
        /// the <see cref="SpriteAnimationName"/> structure.
        /// </summary>
        /// <param name="s">A string containing the sprite animation name to convert.</param>
        /// <returns>An instance of the <see cref="SpriteAnimationName"/> structure that is equivalent to the specified string.</returns>
        [Preserve]
        public static SpriteAnimationName Parse(String s)
        {
            Contract.Require(s, nameof(s));

            SpriteAnimationName value;
            if (!TryParseInternal(s, out value))
            {
                throw new FormatException();
            }
            return value;
        }

        /// <summary>
        /// Converts the string representation of a sprite animation name to an instance of
        /// the <see cref="SpriteAnimationName"/> structure.
        /// </summary>
        /// <param name="s">A string containing the sprite animation name to convert.</param>
        /// <param name="value">An instance of the <see cref="SpriteAnimationName"/> structure that is equivalent to the specified string.</param>
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public static Boolean TryParse(String s, out SpriteAnimationName value)
        {
            Contract.Require(s, nameof(s));

            return TryParseInternal(s, out value);
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + animationName?.GetHashCode() ?? 0;
                hash = hash * 23 + animationIndex;
                return hash;
            }
        }
        
        /// <inheritdoc/>
        [Preserve]
        public override Boolean Equals(Object obj)
        {
            return obj is SpriteAnimationName && Equals((SpriteAnimationName)obj);
        }

        /// <inheritdoc/>
        [Preserve]
        public Boolean Equals(SpriteAnimationName other)
        {
            return
                this.animationIndex == other.animationIndex &&
                this.animationName == other.animationName;
        }

        /// <summary>
        /// Gets a value indicating whether this structure represents an animation index.
        /// </summary>
        public Boolean IsIndex => animationName == null;

        /// <summary>
        /// Gets a value indicating whether this structure represents an animation name.
        /// </summary>
        public Boolean IsName => animationName != null;

        /// <summary>
        /// Converts the string representation of a sprite animation name to an instance of
        /// the <see cref="SpriteAnimationName"/> structure.
        /// </summary>
        private static Boolean TryParseInternal(String s, out SpriteAnimationName name)
        {
            Int32 index;
            if (Int32.TryParse(s, out index))
            {
                name = new SpriteAnimationName(index);
                return true;
            }

            name = new SpriteAnimationName(s);
            return true;
        }
        
        // State values.
        private String animationName;
        private Int32 animationIndex;
    }
}

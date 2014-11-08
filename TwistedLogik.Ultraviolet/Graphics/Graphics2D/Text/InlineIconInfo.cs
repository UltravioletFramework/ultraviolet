using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the metadata for an inline icon used by a text layout engine.
    /// </summary>
    public struct InlineIconInfo : IEquatable<InlineIconInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InlineIconInfo"/> structure.
        /// </summary>
        /// <param name="icon">The <see cref="SpriteAnimation"/> that represents the inline icon.</param>
        /// <param name="width">The width of the icon when rendered, in pixels.</param>
        /// <param name="height">The height of the icon when rendered, in pixels.</param>
        internal InlineIconInfo(SpriteAnimation icon, Int32? width, Int32? height)
            : this()
        {
            Icon   = icon;
            Width  = width;
            Height = height;
        }

        /// <inheritdoc/>
        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + (Icon == null ? 0 : Icon.GetHashCode());
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is InlineIconInfo))
            {
                return false;
            }
            return Equals((InlineIconInfo)obj);
        }

        /// <summary>
        /// Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="other">The object to compare to this instance.</param>
        /// <returns><c>true</c> if this instance is equal to the specified object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(InlineIconInfo other)
        {
            return
                this.Icon == other.Icon &&
                this.Width == other.Width &&
                this.Height == other.Height;
        }

        /// <summary>
        /// Gets the <see cref="SpriteAnimation"/> that represents the inline icon.
        /// </summary>
        public SpriteAnimation Icon { get; private set; }

        /// <summary>
        /// Gets the width of the icon when rendered, in pixels.
        /// </summary>
        public Int32? Width { get; private set; }

        /// <summary>
        /// Gets the height of the icon when rendered, in pixels.
        /// </summary>
        public Int32? Height { get; private set; }
    }
}

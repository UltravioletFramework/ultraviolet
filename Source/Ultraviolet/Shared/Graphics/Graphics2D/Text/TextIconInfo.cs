using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the metadata for an icon used by the text layout engine.
    /// </summary>
    public partial struct TextIconInfo : IEquatable<TextIconInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextIconInfo"/> structure.
        /// </summary>
        /// <param name="icon">The <see cref="SpriteAnimation"/> that represents the inline icon.</param>
        /// <param name="width">The width of the icon when rendered, in pixels.</param>
        /// <param name="height">The height of the icon when rendered, in pixels.</param>
        internal TextIconInfo(SpriteAnimation icon, Int32? width, Int32? height)
            : this()
        {
            Icon = icon;
            Width = width;
            Height = height;
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

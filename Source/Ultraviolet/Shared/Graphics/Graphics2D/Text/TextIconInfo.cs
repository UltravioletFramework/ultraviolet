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
        /// <param name="ascender">The ascender value, in pixels, for this icon.</param>
        /// <param name="descender">The descender value, in pixels, for this icon. Values below the baseline are negative.</param>
        internal TextIconInfo(SpriteAnimation icon, Int32? width, Int32? height, Int32? ascender, Int32? descender)
            : this()
        {
            Icon = icon;
            Width = width;
            Height = height;
            Ascender = ascender ?? 0;
            Descender = descender ?? 0;
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

        /// <summary>
        /// Gets the ascender value, in pixels, for this icon.
        /// </summary>
        public Int32 Ascender { get; private set; }

        /// <summary>
        /// Gets the descender value, in pixels, for this icon. Values below the baseline are negative.
        /// </summary>
        public Int32 Descender { get; private set; }
    }
}

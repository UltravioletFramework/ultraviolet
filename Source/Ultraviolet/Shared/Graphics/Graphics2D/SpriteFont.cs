namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a bitmap font used for rendering text.
    /// </summary>
    public class SpriteFont : UltravioletFont<SpriteFontFace>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFont"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="face">The <see cref="SpriteFontFace"/> that constitutes the font.</param>
        public SpriteFont(UltravioletContext uv, SpriteFontFace face)
            : this(uv, face, face, face, face)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFont"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="regular">The <see cref="SpriteFontFace"/> that represents the font's regular style.</param>
        /// <param name="bold">The <see cref="SpriteFontFace"/> that represents the font's bold style.</param>
        /// <param name="italic">The <see cref="SpriteFontFace"/> that represents the font's italic style.</param>
        /// <param name="boldItalic">The <see cref="SpriteFontFace"/> that represents the font's bold/italic style.</param>
        public SpriteFont(UltravioletContext uv, SpriteFontFace regular, SpriteFontFace bold, SpriteFontFace italic, SpriteFontFace boldItalic)
            : base(uv, regular, bold, italic, boldItalic)
        { }      
    }
}

using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the strongly-typed base class for all font classes.
    /// </summary>
    /// <typeparam name="TFontFace">The type of font face exposed by this font.</typeparam>
    public abstract class UltravioletFont<TFontFace> : UltravioletFont
        where TFontFace : UltravioletFontFace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletFont{TFontFace}"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="regular">The font's regular font face.</param>
        /// <param name="bold">The font's bold font face.</param>
        /// <param name="italic">The font's italic font face.</param>
        /// <param name="boldItalic">The font's bold italic font face.</param>
        public UltravioletFont(UltravioletContext uv, TFontFace regular, TFontFace bold, TFontFace italic, TFontFace boldItalic)
            : base(uv, regular, bold, italic, boldItalic)
        { }
        
        /// <inheritdoc/>
        public new TFontFace GetFace(UltravioletFontStyle style) => (TFontFace)base.GetFace(style);

        /// <inheritdoc/>
        public new TFontFace GetFace(Boolean bold, Boolean italic) => (TFontFace)base.GetFace(bold, italic);

        /// <inheritdoc/>
        public new TFontFace Regular => (TFontFace)base.Regular;

        /// <inheritdoc/>
        public new TFontFace Bold => (TFontFace)base.Bold;

        /// <inheritdoc/>
        public new TFontFace Italic => (TFontFace)base.Italic;

        /// <inheritdoc/>
        public new TFontFace BoldItalic => (TFontFace)base.BoldItalic;
    }
}

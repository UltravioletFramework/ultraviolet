using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the base class for all font classes.
    /// </summary>
    public abstract class UltravioletFont : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletFont{TFontFace}"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="regular">The font's regular font face.</param>
        /// <param name="bold">The font's bold font face.</param>
        /// <param name="italic">The font's italic font face.</param>
        /// <param name="boldItalic">The font's bold italic font face.</param>
        protected UltravioletFont(UltravioletContext uv, UltravioletFontFace regular, UltravioletFontFace bold, UltravioletFontFace italic, UltravioletFontFace boldItalic)
            : base(uv)
        {
            uv.ValidateResource(regular);
            uv.ValidateResource(bold);
            uv.ValidateResource(italic);
            uv.ValidateResource(boldItalic);

            if (regular == null && bold == null && italic == null && boldItalic == null)
                throw new ArgumentException(UltravioletStrings.InvalidFontFaces);

            this.Regular = regular;
            this.Bold = bold;
            this.Italic = italic;
            this.BoldItalic = boldItalic;
        }

        /// <summary>
        /// Implicitly converts an <see cref="UltravioletFont"/> into a font face by returning the font's regular face.
        /// </summary>
        /// <param name="font">The <see cref="UltravioletFont"/> to convert.</param>
        /// <returns>The converted font face.</returns>
        public static implicit operator UltravioletFontFace(UltravioletFont font) => font?.GetFace(UltravioletFontStyle.Regular);

        /// <summary>
        /// Gets the font face that corresponds to the specified style.
        /// </summary>
        /// <remarks>If the requested font face does not exist, the closest matching font face will be returned instead.</remarks>
        /// <param name="style">The style for which to retrieve a font face.</param>
        /// <returns>The <see cref="UltravioletFontFace"/> that corresponds to the specified style.</returns>
        public UltravioletFontFace GetFace(UltravioletFontStyle style)
        {
            switch (style)
            {
                case UltravioletFontStyle.Regular:
                    return Regular ?? Bold ?? Italic ?? BoldItalic;
                case UltravioletFontStyle.Bold:
                    return Bold ?? BoldItalic ?? Regular ?? Italic;
                case UltravioletFontStyle.Italic:
                    return Italic ?? BoldItalic ?? Regular ?? Bold;
                case UltravioletFontStyle.BoldItalic:
                    return BoldItalic ?? Italic ?? Bold ?? Regular;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the font face that corresponds to the specified style.
        /// </summary>
        /// <remarks>If the requested font face does not exist, the closest matching font face will be returned instead.</remarks>
        /// <param name="bold">A value indicating whether to retrieve a bold font face.</param>
        /// <param name="italic">A value indicating whether to retrieve an italic font face.</param>
        /// <returns>The <see cref="UltravioletFontFace"/> that corresponds to the specified style.</returns>
        public UltravioletFontFace GetFace(Boolean bold, Boolean italic)
        {
            if (bold && italic)
                return GetFace(UltravioletFontStyle.BoldItalic);

            if (bold)
                return GetFace(UltravioletFontStyle.Bold);

            if (italic)
                return GetFace(UltravioletFontStyle.Italic);

            return GetFace(UltravioletFontStyle.Regular);
        }

        /// <summary>
        /// Gets the font's regular face.
        /// </summary>
        public UltravioletFontFace Regular { get; }

        /// <summary>
        /// Gets the font's bold face.
        /// </summary>
        public UltravioletFontFace Bold { get; }

        /// <summary>
        /// Gets the font's italic face.
        /// </summary>
        public UltravioletFontFace Italic { get; }

        /// <summary>
        /// Gets the font's bold/italic face.
        /// </summary>
        public UltravioletFontFace BoldItalic { get; }
        
    }
}

using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a bitmap font used for rendering text.
    /// </summary>
    public class SpriteFont : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFont"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="face">The <see cref="SpriteFontFace"/> that constitutes the font.</param>
        public SpriteFont(UltravioletContext uv, SpriteFontFace face)
            : this(uv, face, face, face, face)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFont"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="faceRegular">The <see cref="SpriteFontFace"/> that represents the font's regular style.</param>
        /// <param name="faceBold">The <see cref="SpriteFontFace"/> that represents the font's bold style.</param>
        /// <param name="faceItalic">The <see cref="SpriteFontFace"/> that represents the font's italic style.</param>
        /// <param name="faceBoldItalic">The <see cref="SpriteFontFace"/> that represents the font's bold/italic style.</param>
        public SpriteFont(UltravioletContext uv, SpriteFontFace faceRegular, SpriteFontFace faceBold, SpriteFontFace faceItalic, SpriteFontFace faceBoldItalic)
            : base(uv)
        {
            uv.ValidateResource(faceRegular);
            uv.ValidateResource(faceBold);
            uv.ValidateResource(faceItalic);
            uv.ValidateResource(faceBoldItalic);

            if (faceRegular == null && faceBold == null && faceItalic == null && faceBoldItalic == null)
                throw new ArgumentException(UltravioletStrings.InvalidFontFaces);

            this.faceRegular = faceRegular;
            this.faceBold = faceBold;
            this.faceItalic = faceItalic;
            this.faceBoldItalic = faceBoldItalic;
        }

        /// <summary>
        /// Implicitly converts a <see cref="SpriteFont"/> into a <see cref="SpriteFontFace"/> by returning the font's regular face.
        /// </summary>
        /// <param name="font">The <see cref="SpriteFont"/> to convert.</param>
        /// <returns>The converted <see cref="SpriteFontFace"/>.</returns>
        public static implicit operator SpriteFontFace(SpriteFont font)
        {
            return (font == null) ? null : font.GetFace(SpriteFontStyle.Regular);
        }

        /// <summary>
        /// Gets the font face that corresponds to the specified style.
        /// </summary>
        /// <remarks>If the requested font face does not exist, the closest matching font face will be returned instead.</remarks>
        /// <param name="style">The style for which to retrieve a font face.</param>
        /// <returns>The <see cref="SpriteFontFace"/> that corresponds to the specified style.</returns>
        public SpriteFontFace GetFace(SpriteFontStyle style)
        {
            switch (style)
            {
                case SpriteFontStyle.Regular:
                    return faceRegular ?? faceBold ?? faceItalic ?? faceBoldItalic;
                case SpriteFontStyle.Bold:
                    return faceBold ?? faceBoldItalic ?? faceRegular ?? faceItalic;
                case SpriteFontStyle.Italic:
                    return faceItalic ?? faceBoldItalic ?? faceRegular ?? faceBold;
                case SpriteFontStyle.BoldItalic:
                    return faceBoldItalic ?? faceItalic ?? faceBold ?? faceRegular;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the font face that corresponds to the specified style.
        /// </summary>
        /// <remarks>If the requested font face does not exist, the closest matching font face will be returned instead.</remarks>
        /// <param name="bold">A value indicating whether to retrieve a bold font face.</param>
        /// <param name="italic">A value indicating whether to retrieve an italic font face.</param>
        /// <returns>The <see cref="SpriteFontFace"/> that corresponds to the specified style.</returns>
        public SpriteFontFace GetFace(Boolean bold, Boolean italic)
        {
            if (bold && italic)
            {
                return GetFace(SpriteFontStyle.BoldItalic);
            }
            if (bold)
            {
                return GetFace(SpriteFontStyle.Bold);
            }
            if (italic)
            {
                return GetFace(SpriteFontStyle.Italic);
            }
            return GetFace(SpriteFontStyle.Regular);
        }

        /// <summary>
        /// Gets the font's regular face.
        /// </summary>
        public SpriteFontFace Regular
        {
            get { return faceRegular; }
        }

        /// <summary>
        /// Gets the font's bold face.
        /// </summary>
        public SpriteFontFace Bold
        {
            get { return faceBold; }
        }

        /// <summary>
        /// Gets the font's italic face.
        /// </summary>
        public SpriteFontFace Italic
        {
            get { return faceItalic; }
        }

        /// <summary>
        /// Gets the font's bold/italic face.
        /// </summary>
        public SpriteFontFace BoldItalic
        {
            get { return faceBoldItalic; }
        }

        // Property values.
        private readonly SpriteFontFace faceRegular;
        private readonly SpriteFontFace faceBold;
        private readonly SpriteFontFace faceItalic;
        private readonly SpriteFontFace faceBoldItalic;
    }
}

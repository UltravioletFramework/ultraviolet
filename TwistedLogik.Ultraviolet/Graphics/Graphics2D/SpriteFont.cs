using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents a sprite font.
    /// </summary>
    public class SpriteFont : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the SpriteFont class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="face">The font's face.</param>
        public SpriteFont(UltravioletContext uv, SpriteFontFace face)
            : this(uv, face, face, face, face)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the SpriteFont class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="faceRegular">The font's regular face.</param>
        /// <param name="faceBold">The font's bold face.</param>
        /// <param name="faceItalic">The font's italic face.</param>
        /// <param name="faceBoldItalic">The font's bold/italic face.</param>
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
        /// Implicitly converts a sprite font into a font face by returning the sprite font's regular face.
        /// </summary>
        /// <param name="font">The font to convert.</param>
        /// <returns>The converted font face.</returns>
        public static implicit operator SpriteFontFace(SpriteFont font)
        {
            return (font == null) ? null : font.GetFace(SpriteFontStyle.Regular);
        }

        /// <summary>
        /// Gets the font face that corresponds to the specified style.
        /// </summary>
        /// <remarks>If the requested font face does not exist, the closest matching font face will be returned instead.</remarks>
        /// <param name="style">The style for which to retrieve a font face.</param>
        /// <returns>The font face that corresponds to the specified style.</returns>
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
        /// <returns>The font face that corresponds to the specified style.</returns>
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

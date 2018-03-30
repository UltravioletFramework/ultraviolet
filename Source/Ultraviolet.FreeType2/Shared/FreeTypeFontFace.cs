using System;
using System.Text;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents one of a FreeType font's font faces.
    /// </summary>
    public class FreeTypeFontFace : UltravioletFontFace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public FreeTypeFontFace(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public override void GetGlyphRenderInfo(Char c, out Texture2D texture, out Rectangle region)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(String text)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(String text, Int32 start, Int32 count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringBuilder text)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringBuilder text, Int32 start, Int32 count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringSegment text)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringSegment text, Int32 start, Int32 count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(ref StringSource source, Int32 start, Int32 count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(String text, Int32 ix)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(StringBuilder text, Int32 ix)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(StringSegment text, Int32 ix)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(ref StringSource source, Int32 ix)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(Char c1, Char? c2 = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Int32 GetKerningInfo(Char c1, Char c2)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Int32 GetKerningInfo(SpriteFontKerningPair pair)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Int32 Characters => throw new NotImplementedException();

        /// <inheritdoc/>
        public override Int32 SpaceWidth => throw new NotImplementedException();

        /// <inheritdoc/>
        public override Int32 TabWidth => throw new NotImplementedException();

        /// <inheritdoc/>
        public override Int32 LineSpacing => throw new NotImplementedException();

        /// <inheritdoc/>
        public override Char SubstitutionCharacter => throw new NotImplementedException();        
    }
}

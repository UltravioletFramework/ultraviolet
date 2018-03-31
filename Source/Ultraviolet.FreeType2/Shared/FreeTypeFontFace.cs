using System;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.FreeType2.Native;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Err;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents one of a FreeType font's font faces.
    /// </summary>
    public unsafe class FreeTypeFontFace : UltravioletFontFace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="face">The FreeType2 face which this instance represents.</param>
        internal FreeTypeFontFace(UltravioletContext uv, FT_FaceRec_* face)
            : base(uv)
        {
            Contract.Require((IntPtr)face, nameof(face));

            this.face = face;
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

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                var err = FT_Done_Face(face);
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                face = null;
            }

            base.Dispose(disposing);
        }

        // The FreeType2 face which this instance represents.
        private FT_FaceRec_* face;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents one of a sprite font's font faces.
    /// </summary>
    public class SpriteFontFace : UltravioletFontFace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="texture">The texture that contains the glyph images.</param>
        /// <param name="regions">A collection containing the font face's character regions.</param>
        /// <param name="glyphs">A collection containing the positions of the font's glyphs.</param>
        /// <param name="kerning">The font's kerning information.</param>
        /// <param name="ownsTexture">A value indicating whether this font face is responsible for disposing of its texture.</param>
        public SpriteFontFace(UltravioletContext uv, Texture2D texture, IEnumerable<CharacterRegion> regions, IEnumerable<Rectangle> glyphs,
            SpriteFontKerning kerning, Boolean ownsTexture = false)
            : this(uv, texture, regions, glyphs, kerning, 0, 0, '?', ownsTexture)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="texture">The texture that contains the font face's glyphs.</param>
        /// <param name="regions">A collection containing the font face's character regions.</param>
        /// <param name="glyphs">A collection containing the positions of the font face's glyphs on its texture.</param>
        /// <param name="kerning">The font's kerning information.</param>
        /// <param name="substitutionCharacter">The character that corresponds to the font face's substitution glyph.</param>
        /// <param name="ownsTexture">A value indicating whether this font face is responsible for disposing of its texture.</param>
        public SpriteFontFace(UltravioletContext uv, Texture2D texture, IEnumerable<CharacterRegion> regions, IEnumerable<Rectangle> glyphs, 
            SpriteFontKerning kerning, Char substitutionCharacter, Boolean ownsTexture = false)
            : this(uv, texture, regions, glyphs, kerning, 0, 0, substitutionCharacter, ownsTexture)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="texture">The texture that contains the font face's glyphs.</param>
        /// <param name="regions">A collection containing the font face's character regions.</param>
        /// <param name="glyphs">A collection containing the positions of the font face's glyphs on its texture.</param>
        /// <param name="kerning">The font's kerning information.</param>
        /// <param name="ascender">The height of the font's ascender in pixels.</param>
        /// <param name="descender">The height of the font's descender in pixels.</param>
        /// <param name="substitutionCharacter">The character that corresponds to the font face's substitution glyph.</param>
        /// <param name="ownsTexture">A value indicating whether this font face is responsible for disposing of its texture.</param>
        public SpriteFontFace(UltravioletContext uv, Texture2D texture, IEnumerable<CharacterRegion> regions, IEnumerable<Rectangle> glyphs, 
            SpriteFontKerning kerning, Int32 ascender, Int32 descender, Char substitutionCharacter, Boolean ownsTexture = false)
            : base(uv)
        {
            Contract.Require(texture, nameof(texture));
            Contract.Require(glyphs, nameof(glyphs));

            this.texture = texture;
            this.ownsTexture = ownsTexture;

            this.glyphs = new SpriteFontGlyphIndex(regions ?? new[] { CharacterRegion.Default }, glyphs, substitutionCharacter);
            this.kerning = kerning ?? new SpriteFontKerning();

            this.Ascender = ascender;
            this.Descender = descender;
        }

        /// <inheritdoc/>
        public override void GetGlyphRenderInfo(Char c, out GlyphRenderInfo info)
        {
            var texture = this.texture;
            var textureRegion = glyphs[c];
            var advance = textureRegion.Width;

            info = new GlyphRenderInfo
            {
                Texture = texture,
                TextureRegion = textureRegion,
                Advance = advance,
            };
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(String text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(String text, Int32 start, Int32 count)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringBuilder text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringBuilder text, Int32 start, Int32 count)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringSegment text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringSegment text, Int32 start, Int32 count)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(ref StringSource source, Int32 start, Int32 count)
        {
            if (count == 0)
                return Size2.Zero;

            Contract.EnsureRange(start >= 0 && start < source.Length, nameof(start));
            Contract.EnsureRange(count >= 0 && start + count <= source.Length, nameof(count));

            var cx = 0;
            var cy = 0;
            for (int i = 0; i < count; i++)
            {
                var character = source[start + i];
                switch (character)
                {
                    case '\r':
                        continue;

                    case '\n':
                        cx = 0;
                        cy = cy + LineSpacing;
                        continue;

                    case '\t':
                        cx = cx + TabWidth;
                        continue;
                }
                cx += MeasureGlyph(ref source, start + i).Width;
            }

            return new Size2(cx, cy + LineSpacing);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(String text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(StringBuilder text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(StringSegment text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(ref StringSource source, Int32 ix)
        {
            var c1 = source[ix];
            switch (c1)
            {
                case '\n':
                    return new Size2(0, LineSpacing);

                case '\t':
                    return new Size2(TabWidth, LineSpacing);

                default:
                    var c2 = (ix + 1 < source.Length) ? source[ix + 1] : (Char?)null;
                    var glyph = glyphs[c1];
                    var offset = c2.HasValue ? kerning.Get(c1, c2.GetValueOrDefault()) : 0;
                    return new Size2(glyph.Width + offset, glyph.Height);
            }
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(Char c1, Char? c2 = null)
        {
            var glyph = glyphs[c1];
            var offset = c2.HasValue ? kerning.Get(c1, c2.GetValueOrDefault()) : 0;
            return new Size2(glyph.Width + offset, glyph.Height);
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo(Char c1, Char c2)
        {
            return new Size2(kerning.Get(c1, c2), 0);
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo(SpriteFontKerningPair pair)
        {
            return new Size2(kerning.Get(pair), 0);
        }

        /// <inheritdoc/>
        public override Int32 Characters => glyphs.Count;

        /// <inheritdoc/>
        public override Int32 SpaceWidth => glyphs[' '].Width;

        /// <inheritdoc/>
        public override Int32 TabWidth => SpaceWidth * 4;

        /// <inheritdoc/>
        public override Int32 Ascender { get; }

        /// <inheritdoc/>
        public override Int32 Descender { get; }

        /// <inheritdoc/>
        public override Int32 LineSpacing => glyphs.LineSpacing;

        /// <inheritdoc/>
        public override Char SubstitutionCharacter => glyphs.SubstitutionCharacter;

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (ownsTexture)
            {
                SafeDispose.Dispose(texture);
            }

            base.Dispose(disposing);
        }

        // Property values.
        private readonly Texture2D texture;
        private readonly Boolean ownsTexture;

        // State values.
        private readonly SpriteFontGlyphIndex glyphs;
        private readonly SpriteFontKerning kerning;        
    }
}

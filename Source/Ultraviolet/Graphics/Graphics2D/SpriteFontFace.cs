using System;
using System.Collections.Generic;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics.Graphics2D.Text;

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
        public override Int32 GetGlyphIndex(Int32 codePoint) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override void GetCodePointRenderInfo(Int32 c, out GlyphRenderInfo info)
        {
            if (c < 0 || c > Char.MaxValue)
                c = SubstitutionCharacter;

            var texture = this.texture;
            var textureRegion = glyphs[(Char)c];
            var advance = textureRegion.Width;

            info = new GlyphRenderInfo
            {
                Texture = texture,
                TextureRegion = textureRegion,
                Advance = advance,
            };
        }

        /// <inheritdoc/>
        public override void GetGlyphIndexRenderInfo(Int32 glyphIndex, out GlyphRenderInfo info) => 
            throw new NotSupportedException();

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
            var source = new StringBuilderSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(StringBuilder text, Int32 start, Int32 count)
        {
            var source = new StringBuilderSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(ref StringSegment text)
        {
            var source = new StringSegmentSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString(ref StringSegment text, Int32 start, Int32 count)
        {
            var source = new StringSegmentSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString<TSource>(ref TSource source)
        {
            return MeasureString(ref source, 0, source.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString<TSource>(ref TSource source, Int32 start, Int32 count)
        {
            if (count == 0)
                return Size2.Zero;

            Contract.EnsureRange(start >= 0 && start < source.Length, nameof(start));
            Contract.EnsureRange(count >= 0 && start + count <= source.Length, nameof(count));

            var cx = 0;
            var cy = 0;
            for (int i = 0; i < count; i++)
            {
                var ix = start + i;
                var ixNext = ix + 1;

                var character = source[ix];
                if (ixNext < count && Char.IsSurrogatePair(source[ix], source[ixNext]))
                    i++;

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

                cx += MeasureGlyph(ref source, ix).Width;
            }

            return new Size2(cx, cy + LineSpacing);
        }

        /// <inheritdoc/>
        public override Size2 MeasureShapedString(ShapedString text, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureShapedString(ShapedString text, Int32 start, Int32 count, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureShapedString(ShapedStringBuilder text, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureShapedString(ShapedStringBuilder text, Int32 start, Int32 count, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureShapedString<TSource>(ref TSource text, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureShapedString<TSource>(ref TSource text, Int32 start, Int32 count, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(Int32 c1, Int32? c2 = null)
        {
            if (c1 < 0 || c1 > Char.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(c1));
            if (c2.HasValue)
            {
                var c2Value = c2.GetValueOrDefault();
                if (c2Value < 0 || c2Value > Char.MaxValue)
                    throw new ArgumentOutOfRangeException(nameof(c2));
            }

            var char1 = (Char)c1;
            var char2 = (Char)c2.GetValueOrDefault();

            var glyph = glyphs[char1];
            var offset = c2.HasValue ? kerning.Get(char1, char2) : 0;
            return new Size2(glyph.Width + offset, glyph.Height);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyphByGlyphIndex(Int32 glyphIndex1, Int32? glyphIndex2 = null) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(String text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(StringBuilder text, Int32 ix)
        {
            var source = new StringBuilderSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(ref StringSegment text, Int32 ix)
        {
            var source = new StringSegmentSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph<TSource>(ref TSource text, Int32 ix)
        {
            var c1 = text[ix];

            var ixNext = ix + 1;
            if (ixNext < text.Length && Char.IsSurrogatePair(c1, text[ixNext]))
            {
                c1 = SubstitutionCharacter;
                ixNext++;
            }
            else if (Char.IsSurrogate(c1))
            {
                c1 = SubstitutionCharacter;
            }

            switch (c1)
            {
                case '\n':
                    return new Size2(0, LineSpacing);

                case '\t':
                    return new Size2(TabWidth, LineSpacing);

                default:
                    var c2 = (ixNext < text.Length) ? text[ixNext++] : (Char?)null;
                    if (c2.HasValue)
                    {
                        var c2Value = c2.GetValueOrDefault();
                        if (Char.IsSurrogate(c2Value))
                            c2 = SubstitutionCharacter;
                    }
                    var glyph = glyphs[c1];
                    var offset = c2.HasValue ? kerning.Get(c1, c2.GetValueOrDefault()) : 0;
                    return new Size2(glyph.Width + offset, glyph.Height);
            }
        }

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyph(ShapedString text, Int32 ix, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyph(ShapedStringBuilder text, Int32 ix, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyph<TSource>(ref TSource source, Int32 ix, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureGlyphWithHypotheticalKerning(ref StringSegment text, Int32 ix, Int32 c2)
        {
            var source = new StringSegmentSource(text);
            return MeasureGlyphWithHypotheticalKerning(ref source, ix, c2);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyphWithHypotheticalKerning<TSource>(ref TSource text, Int32 ix, Int32 c2)
        {
            if (c2 < 0 || c2 > Char.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(c2));

            var c1 = text[ix];
            if (Char.IsSurrogate(c1))
                c1 = SubstitutionCharacter;

            switch (c1)
            {
                case '\n':
                    return new Size2(0, LineSpacing);

                case '\t':
                    return new Size2(TabWidth, LineSpacing);

                default:
                    var glyph = glyphs[c1];
                    var offset = kerning.Get(c1, (Char)c2);
                    return new Size2(glyph.Width + offset, glyph.Height);
            }
        }

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyphWithHypotheticalKerning<TSource>(ref TSource text, Int32 ix, Int32 glyphIndex2, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 MeasureGlyphWithoutKerning(ref StringSegment text, Int32 ix)
        {
            var source = new StringSegmentSource(text);
            return MeasureGlyphWithoutKerning(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyphWithoutKerning<TSource>(ref TSource text, Int32 ix)
        {
            var c1 = text[ix];
            if (Char.IsSurrogate(c1))
                c1 = SubstitutionCharacter;

            switch (c1)
            {
                case '\n':
                    return new Size2(0, LineSpacing);

                case '\t':
                    return new Size2(TabWidth, LineSpacing);

                default:
                    return glyphs[c1].Size;
            }
        }

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyphWithoutKerning<TSource>(ref TSource text, Int32 ix, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 GetKerningInfoByGlyphIndex(Int32 glyphIndex1, Int32 glyphIndex2) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 GetKerningInfo(Int32 c1, Int32 c2)
        {
            if (c1 < 0 || c1 > Char.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(c1));
            if (c2 < 0 || c2 > Char.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(c2));

            var char1 = (Char)c1;
            var char2 = (Char)c2;

            return new Size2(kerning.Get(char1, char2), 0);
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo(ref StringSegment text, Int32 ix)
        {
            var source = new StringSegmentSource(text);
            return GetKerningInfo(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo(ref StringSegment text1, Int32 ix1, ref StringSegment text2, Int32 ix2)
        {
            var source1 = new StringSegmentSource(text1);
            var source2 = new StringSegmentSource(text2);
            return GetKerningInfo(ref source1, ix1, ref source2, ix2);
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo<TSource>(ref TSource text, Int32 ix)
        {
            var c1 = text[ix];

            var ixNext = ix + 1;
            if (ixNext < text.Length && Char.IsSurrogatePair(c1, text[ixNext]))
            {
                c1 = SubstitutionCharacter;
                ixNext++;
            }
            else if (Char.IsSurrogate(c1))
            {
                c1 = SubstitutionCharacter;
            }

            if (ixNext >= text.Length)
                return Size2.Zero;

            var c2 = text[ixNext];
            if (Char.IsSurrogate(c2))
                c2 = SubstitutionCharacter;

            return GetKerningInfo(c1, c2);
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo<TSource1, TSource2>(ref TSource1 text1, Int32 ix1, ref TSource2 text2, Int32 ix2)
        {
            var c1 = text1[ix1];
            if (Char.IsSurrogate(c1))
                c1 = SubstitutionCharacter;

            var c2 = text2[ix2];
            if (Char.IsSurrogate(c2))
                c2 = SubstitutionCharacter;

            return GetKerningInfo(c1, c2);
        }

        /// <inheritdoc/>
        public override Size2 GetShapedKerningInfo<TSource>(ref TSource text, Int32 ix) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 GetShapedKerningInfo<TSource1, TSource2>(ref TSource1 text1, Int32 ix1, ref TSource2 text2, Int32 ix2, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Size2 GetHypotheticalKerningInfo(ref StringSegment text, Int32 ix, Int32 c2)
        {
            var source = new StringSegmentSource(text);
            return GetHypotheticalKerningInfo(ref source, ix, c2);
        }

        /// <inheritdoc/>
        public override Size2 GetHypotheticalKerningInfo<TSource>(ref TSource text, Int32 ix, Int32 c2)
        {
            if (c2 < 0 || c2 > Char.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(c2));

            var c1 = text[ix];
            if (Char.IsSurrogate(c1))
                c1 = SubstitutionCharacter;

            return GetKerningInfo(c1, c2);
        }

        /// <inheritdoc/>
        public override Size2 GetHypotheticalShapedKerningInfo<TSource>(ref TSource text, Int32 ix, Int32 glyphIndex2, Boolean rtl = false) =>
            throw new NotSupportedException();

        /// <inheritdoc/>
        public override Boolean ContainsGlyph(Int32 c) => glyphs.ContainsGlyph(c);

        /// <inheritdoc/>
        public override Boolean SupportsGlyphIndices => false;

        /// <inheritdoc/>
        public override Boolean SupportsShapedText => false;

        /// <inheritdoc/>
        public override Int32 Glyphs => glyphs.Count;

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

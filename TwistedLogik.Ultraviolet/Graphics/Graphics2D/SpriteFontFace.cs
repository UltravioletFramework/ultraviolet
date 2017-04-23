using System;
using System.Collections.Generic;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents one of a sprite font's font faces.
    /// </summary>
    public class SpriteFontFace : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="texture">The texture that contains the glyph images.</param>
        /// <param name="regions">A collection containing the font face's character regions.</param>
        /// <param name="glyphs">A collection containing the positions of the font's glyphs.</param>
        /// <param name="ownsTexture">A value indicating whether this font face is responsible for disposing of its texture.</param>
        public SpriteFontFace(UltravioletContext uv, Texture2D texture, IEnumerable<CharacterRegion> regions, IEnumerable<Rectangle> glyphs, Boolean ownsTexture = false)
            : this(uv, texture, regions, glyphs, '?', ownsTexture)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="texture">The texture that contains the font face's glyphs.</param>
        /// <param name="regions">A collection containing the font face's character regions.</param>
        /// <param name="glyphs">A collection containing the positions of the font face's glyphs on its texture.</param>
        /// <param name="substitutionCharacter">The character that corresponds to the font face's substitution glyph.</param>
        /// <param name="ownsTexture">A value indicating whether this font face is responsible for disposing of its texture.</param>
        public SpriteFontFace(UltravioletContext uv, Texture2D texture, IEnumerable<CharacterRegion> regions, IEnumerable<Rectangle> glyphs, Char substitutionCharacter, Boolean ownsTexture = false)
            : base(uv)
        {
            Contract.Require(texture, nameof(texture));
            Contract.Require(glyphs, nameof(glyphs));

            this.texture = texture;
            this.ownsTexture = ownsTexture;

            this.glyphs = new SpriteFontGlyphIndex(regions ?? new[] { CharacterRegion.Default }, glyphs, substitutionCharacter);
            this.kerning = new SpriteFontKerning();           
        }

        /// <summary>
        /// Measures the size of the specified string of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>The size of the specified string of text when rendered using this font.</returns>
        public Size2 MeasureString(String text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public Size2 MeasureString(String text, Int32 start, Int32 count)
        {
            Contract.EnsureRange(start >= 0 && start < text.Length, nameof(start));
            Contract.EnsureRange(count >= 0 && start + count <= text.Length, nameof(count));

            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <summary>
        /// Measures the size of the specified string of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>The size of the specified string of text when rendered using this font.</returns>
        public Size2 MeasureString(StringBuilder text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public Size2 MeasureString(StringBuilder text, Int32 start, Int32 count)
        {
            Contract.EnsureRange(start >= 0 && start < text.Length, nameof(start));
            Contract.EnsureRange(count >= 0 && start + count <= text.Length, nameof(count));

            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <summary>
        /// Measures the size of the specified string of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>The size of the specified string of text when rendered using this font.</returns>
        public Size2 MeasureString(StringSegment text)
        {
            var source = new StringSource(text);
            return MeasureString(ref source, 0, text.Length);
        }

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public Size2 MeasureString(StringSegment text, Int32 start, Int32 count)
        {
            Contract.EnsureRange(start >= 0 && start < text.Length, nameof(start));
            Contract.EnsureRange(count >= 0 && start + count <= text.Length, nameof(count));

            var source = new StringSource(text);
            return MeasureString(ref source, start, count);
        }

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public Size2 MeasureGlyph(String text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public Size2 MeasureGlyph(StringBuilder text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public Size2 MeasureGlyph(StringSegment text, Int32 ix)
        {
            var source = new StringSource(text);
            return MeasureGlyph(ref source, ix);
        }

        /// <summary>
        /// Measures the specified glyph, taking kerning into account.
        /// </summary>
        /// <param name="c1">The glyph to measure.</param>
        /// <param name="c2">The glyph that comes immediately after the glyph being measured.</param>
        /// <returns>The size of the specified glyph.</returns>
        public Size2 MeasureGlyph(Char c1, Char? c2 = null)
        {
            var glyph  = this[c1];
            var offset = c2.HasValue ? kerning.Get(c1, c2.GetValueOrDefault()) : 0;
            return new Size2(glyph.Width + offset, glyph.Height);
        }

        /// <summary>
        /// Gets the font face's kerning information.
        /// </summary>
        public SpriteFontKerning Kerning
        {
            get { return kerning; }
        }

        /// <summary>
        /// Gets the texture that contains the font face's glyphs.
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// Gets the character that corresponds to the font face's substitution glyph.
        /// </summary>
        /// <remarks>The substitution glyph is used as a replacement for characters which do not exist in the collection.</remarks>
        public Char SubstitutionCharacter
        {
            get { return glyphs.SubstitutionCharacter; }
        }

        /// <summary>
        /// Gets the number of characters in the font face.
        /// </summary>
        public Int32 Characters
        {
            get { return glyphs.Count; }
        }

        /// <summary>
        /// Gets the width of a space in this font face.
        /// </summary>
        public Int32 SpaceWidth
        {
            get { return this[' '].Width; }
        }

        /// <summary>
        /// Gets the width of a tab in this font face.
        /// </summary>
        public Int32 TabWidth
        {
            get { return SpaceWidth * 4; }
        }

        /// <summary>
        /// Gets the height of a line written with this font face.
        /// </summary>
        public Int32 LineSpacing
        {
            get { return glyphs.LineSpacing; }
        }

        /// <summary>
        /// Gets the position of the specified glyph on the font face's texture.
        /// </summary>
        /// <param name="character">The character for which to retrieve glyph position information.</param>
        /// <returns>The position of the specified glyph on the font face's texture.</returns>
        public Rectangle this[Char character]
        {
            get { return glyphs[character]; }
        }
        
        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="source">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        internal Size2 MeasureString(ref StringSource source, Int32 start, Int32 count)
        {
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

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <param name="source">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        internal Size2 MeasureGlyph(ref StringSource source, Int32 ix)
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

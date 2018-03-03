using System;
using System.Text;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the base class for font faces.
    /// </summary>
    public abstract class UltravioletFontFace : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletFontFace"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UltravioletFontFace(UltravioletContext uv)
            : base(uv)
        { }

        /// <summary>
        /// Gets the information required to draw the specified glyph.
        /// </summary>
        /// <param name="c">The character to draw.</param>
        /// <param name="texture">The texture that contains the character's glyph.</param>
        /// <param name="region">The region of the texture which contains the character's glyph.</param>
        public abstract void GetGlyphRenderInfo(Char c, out Texture2D texture, out Rectangle region);

        /// <summary>
        /// Measures the size of the specified string of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>The size of the specified string of text when rendered using this font.</returns>
        public abstract Size2 MeasureString(String text);

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public abstract Size2 MeasureString(String text, Int32 start, Int32 count);

        /// <summary>
        /// Measures the size of the specified string of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>The size of the specified string of text when rendered using this font.</returns>
        public abstract Size2 MeasureString(StringBuilder text);

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public abstract Size2 MeasureString(StringBuilder text, Int32 start, Int32 count);

        /// <summary>
        /// Measures the size of the specified string of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>The size of the specified string of text when rendered using this font.</returns>
        public abstract Size2 MeasureString(StringSegment text);

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public abstract Size2 MeasureString(StringSegment text, Int32 start, Int32 count);

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="source">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public abstract Size2 MeasureString(ref StringSource source, Int32 start, Int32 count);

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyph(String text, Int32 ix);

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyph(StringBuilder text, Int32 ix);

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyph(StringSegment text, Int32 ix);

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <param name="source">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyph(ref StringSource source, Int32 ix);

        /// <summary>
        /// Measures the specified glyph, taking kerning into account.
        /// </summary>
        /// <param name="c1">The glyph to measure.</param>
        /// <param name="c2">The glyph that comes immediately after the glyph being measured.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyph(Char c1, Char? c2 = null);

        /// <summary>
        /// Gets this font's kerning offset for the specified character pair.
        /// </summary>
        /// <param name="c1">The first character in the pair to evaluate.</param>
        /// <param name="c2">The second character in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified character pair.</returns>
        public abstract Int32 GetKerningInfo(Char c1, Char c2);

        /// <summary>
        /// Gets this font's kerning offset for the specified character pair.
        /// </summary>
        /// <param name="pair">The character pair to evaluate.</param>
        /// <returns>The kerning offset for the specified character pair.</returns>
        public abstract Int32 GetKerningInfo(SpriteFontKerningPair pair);

        /// <summary>
        /// Gets the number of characters in the font face.
        /// </summary>
        public abstract Int32 Characters { get; }

        /// <summary>
        /// Gets the width of a space in this font face.
        /// </summary>
        public abstract Int32 SpaceWidth { get; }

        /// <summary>
        /// Gets the width of a tab in this font face.
        /// </summary>
        public abstract Int32 TabWidth { get; }

        /// <summary>
        /// Gets the height of a line written with this font face.
        /// </summary>
        public abstract Int32 LineSpacing { get; }

        /// <summary>
        /// Gets the character that corresponds to the font face's substitution glyph.
        /// </summary>
        /// <remarks>The substitution glyph is used as a replacement for characters which do not exist in the collection.</remarks>
        public abstract Char SubstitutionCharacter { get; }
    }
}

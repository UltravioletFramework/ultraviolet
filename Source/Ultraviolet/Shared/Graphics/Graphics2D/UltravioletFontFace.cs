using System;
using System.Text;
using Ultraviolet.Core.Text;
using Ultraviolet.Graphics.Graphics2D.Text;

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
        /// Gets the glyph index that corresponds to the specified code point.
        /// </summary>
        /// <param name="codePoint">The 32-bit Unicode value of the glyph for which to retrieve an index.</param>
        /// <returns>The glyph index which corresponds to the specified code point, or 0 if the font cannot represent the code point.</returns>
        public abstract Int32 GetGlyphIndex(Int32 codePoint);

        /// <summary>
        /// Gets the information required to draw the specified Unicode code point.
        /// </summary>
        /// <param name="codePoint">The 32-bit Unicode value of the glyph to draw.</param>
        /// <param name="info">The rendering information for the specified glyph.</param>
        public abstract void GetCodePointRenderInfo(Int32 codePoint, out GlyphRenderInfo info);

        /// <summary>
        /// Gets the information required to draw the specified glyph.
        /// </summary>
        /// <param name="glyphIndex">The glyph index of the glyph to draw.</param>
        /// <param name="info">The rendering information for the specified glyph.</param>
        public abstract void GetGlyphIndexRenderInfo(Int32 glyphIndex, out GlyphRenderInfo info);

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
        public abstract Size2 MeasureString(ref StringSegment text);

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public abstract Size2 MeasureString(ref StringSegment text, Int32 start, Int32 count);

        /// <summary>
        /// Measures the size of the specified string of text when rendered using this font.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the text to measure.</typeparam>
        /// <param name="text">The text to measure.</param>
        /// <returns>The size of the specified string of text when rendered using this font.</returns>
        public abstract Size2 MeasureString<TSource>(ref TSource text)
            where TSource : IStringSource<Char>;

        /// <summary>
        /// Measures the size of the specified substring of text when rendered using this font.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the text to measure.</typeparam>
        /// <param name="text">The text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <returns>The size of the specified substring of text when rendered using this font.</returns>
        public abstract Size2 MeasureString<TSource>(ref TSource text, Int32 start, Int32 count)
            where TSource : IStringSource<Char>;

        /// <summary>
        /// Measures the size of the specified string of shaped text when rendered using this font.
        /// </summary>
        /// <param name="text">The shaped text to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified string of shaped text when rendered using this font.</returns>
        public abstract Size2 MeasureShapedString(ShapedString text, Boolean rtl = false);

        /// <summary>
        /// Measures the size of the specified substring of shaped text when rendered using this font.
        /// </summary>
        /// <param name="text">The shaped text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified substring of shaped text when rendered using this font.</returns>
        public abstract Size2 MeasureShapedString(ShapedString text, Int32 start, Int32 count, Boolean rtl = false);

        /// <summary>
        /// Measures the size of the specified string of shaped text when rendered using this font.
        /// </summary>
        /// <param name="text">The shaped text to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified string of shaped text when rendered using this font.</returns>
        public abstract Size2 MeasureShapedString(ShapedStringBuilder text, Boolean rtl = false);

        /// <summary>
        /// Measures the size of the specified substring of shaped text when rendered using this font.
        /// </summary>
        /// <param name="text">The shaped text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified substring of shaped text when rendered using this font.</returns>
        public abstract Size2 MeasureShapedString(ShapedStringBuilder text, Int32 start, Int32 count, Boolean rtl = false);

        /// <summary>
        /// Measures the size of the specified string of shaped text when rendered using this font.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the shaped text to measure.</typeparam>
        /// <param name="text">The shaped text to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified string of shaped text when rendered using this font.</returns>
        public abstract Size2 MeasureShapedString<TSource>(ref TSource text, Boolean rtl = false)
            where TSource : IStringSource<ShapedChar>;

        /// <summary>
        /// Measures the size of the specified substring of shaped text when rendered using this font.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the shaped text to measure.</typeparam>
        /// <param name="text">The shaped text to measure.</param>
        /// <param name="start">The index of the first character of the substring to measure.</param>
        /// <param name="count">The number of characters in the substring to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified substring of shaped text when rendered using this font.</returns>
        public abstract Size2 MeasureShapedString<TSource>(ref TSource text, Int32 start, Int32 count, Boolean rtl = false)
            where TSource : IStringSource<ShapedChar>;

        /// <summary>
        /// Measures the glyph that corresponds to the specified Unicode code point, taking kerning into account.
        /// </summary>
        /// <param name="c1">The code point of the first glyph to measure.</param>
        /// <param name="c2">The code point of the glyph that comes immediately after the glyph being measured.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyph(Int32 c1, Int32? c2 = null);

        /// <summary>
        /// Measures the glyph that corresponds to the specified glyph index, taking kerning into account.
        /// </summary>
        /// <param name="glyphIndex1">The glyph index of the first glyph to measure.</param>
        /// <param name="glyphIndex2">The glyph index of the glyph that comes immediatley after the glyph being measures.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyphByGlyphIndex(Int32 glyphIndex1, Int32? glyphIndex2 = null);

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
        public abstract Size2 MeasureGlyph(ref StringSegment text, Int32 ix);

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning into account.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the text to measure.</typeparam>
        /// <param name="source">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyph<TSource>(ref TSource source, Int32 ix)
            where TSource : IStringSource<Char>;

        /// <summary>
        /// Measures the specified glyph in a shaped string, taking kerning into account.
        /// </summary>
        /// <param name="text">The shaped text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureShapedGlyph(ShapedString text, Int32 ix, Boolean rtl = false);

        /// <summary>
        /// Measures the specified glyph in a shaped string, taking kerning into account.
        /// </summary>
        /// <param name="text">The shaped text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureShapedGlyph(ShapedStringBuilder text, Int32 ix, Boolean rtl = false);

        /// <summary>
        /// Measures the specified glyph in a shaped string, taking kerning into account.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the shaped text to measure.</typeparam>
        /// <param name="source">The shaped text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureShapedGlyph<TSource>(ref TSource source, Int32 ix, Boolean rtl = false)
            where TSource : IStringSource<ShapedChar>;

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning with a hypothetical second character into account.
        /// </summary>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <param name="c2">The glyph that comes immediately after the glyph being measured.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyphWithHypotheticalKerning(ref StringSegment text, Int32 ix, Int32 c2);

        /// <summary>
        /// Measures the specified glyph in a string, taking kerning with a hypothetical second character into account.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the text to measure.</typeparam>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <param name="c2">The Unicode code point of the glyph that comes immediately after the glyph being measured.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyphWithHypotheticalKerning<TSource>(ref TSource text, Int32 ix, Int32 c2)
            where TSource : IStringSource<Char>;

        /// <summary>
        /// Measures the specified glyph in a shaped string, taking kerning with a hypothetical second character into account.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the text to measure.</typeparam>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <param name="glyphIndex2">The glyph index of the glyph that comes immediately after the glyph being measured.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureShapedGlyphWithHypotheticalKerning<TSource>(ref TSource text, Int32 ix, Int32 glyphIndex2, Boolean rtl = false)
            where TSource : IStringSource<ShapedChar>;

        /// <summary>
        /// Measures the specified glyph in a string, ignoring kerning.
        /// </summary>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyphWithoutKerning(ref StringSegment text, Int32 ix);

        /// <summary>
        /// Measures the specified glyph in a string, ignoring kerning.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the text to measure.</typeparam>
        /// <param name="text">The text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureGlyphWithoutKerning<TSource>(ref TSource text, Int32 ix)
            where TSource : IStringSource<Char>;

        /// <summary>
        /// Measures the specified glyph in a shaped string, ignoring kerning.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which represents the shaped text to measure.</typeparam>
        /// <param name="text">The shaped text that contains the glyph to measure.</param>
        /// <param name="ix">The index of the glyph to measure.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The size of the specified glyph.</returns>
        public abstract Size2 MeasureShapedGlyphWithoutKerning<TSource>(ref TSource text, Int32 ix, Boolean rtl = false)
            where TSource : IStringSource<ShapedChar>;

        /// <summary>
        /// Gets this font's kerning offset for the specified glyph pair.
        /// </summary>
        /// <param name="glyphIndex1">The glyph index that represents the first glyph in the pair to evaluate.</param>
        /// <param name="glyphIndex2">The glyph index that represents the second glyph in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified glyph pair.</returns>
        public abstract Size2 GetKerningInfoByGlyphIndex(Int32 glyphIndex1, Int32 glyphIndex2);

        /// <summary>
        /// Gets this font's kerning offset for the specified glyph pair.
        /// </summary>
        /// <param name="c1">The Unicode code point that represents the first glyph in the pair to evaluate.</param>
        /// <param name="c2">The Unicode code point that represents the second glyph in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified glyph pair.</returns>
        public abstract Size2 GetKerningInfo(Int32 c1, Int32 c2);

        /// <summary>
        /// Gets this font's kerning offset for the specified character pair.
        /// </summary>
        /// <param name="text">The string segment that contains the character pair.</param>
        /// <param name="ix">The index of the first character in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified character pair.</returns>
        public abstract Size2 GetKerningInfo(ref StringSegment text, Int32 ix);

        /// <summary>
        /// Gets this font's kerning offset for the specified character pair.
        /// </summary>
        /// <param name="text1">The string segment that contains the first character in the pair.</param>
        /// <param name="ix1">The index of the first character in the pair to evaluate.</param>
        /// <param name="text2">The string segment that contains the second character in the pair.</param>
        /// <param name="ix2">The index of the second character in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified character pair.</returns>
        public abstract Size2 GetKerningInfo(ref StringSegment text1, Int32 ix1, ref StringSegment text2, Int32 ix2);

        /// <summary>
        /// Gets this font's kerning offset for the specified character pair.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which contains the character pair.</typeparam>
        /// <param name="text">The string source that contains the character pair.</param>
        /// <param name="ix">The index of the first character in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified character pair.</returns>
        public abstract Size2 GetKerningInfo<TSource>(ref TSource text, Int32 ix)
            where TSource : IStringSource<Char>;

        /// <summary>
        /// Gets this font's kerning offset for the specified character pair.
        /// </summary>
        /// <typeparam name="TSource1">The type of string source contains the first character in the pair.</typeparam>
        /// <typeparam name="TSource2">The type of string source contains the second character in the pair.</typeparam>
        /// <param name="text1">The string source that contains the first character in the pair.</param>
        /// <param name="ix1">The index of the first character in the pair to evaluate.</param>
        /// <param name="text2">The string source that contains the second character in the pair.</param>
        /// <param name="ix2">The index of the second character in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified character pair.</returns>
        public abstract Size2 GetKerningInfo<TSource1, TSource2>(ref TSource1 text1, Int32 ix1, ref TSource2 text2, Int32 ix2)
            where TSource1 : IStringSource<Char>
            where TSource2 : IStringSource<Char>;

        /// <summary>
        /// Gets this font's kerning offset for the specified glyph pair.
        /// </summary>
        /// <typeparam name="TSource">The type of shaped string source which contains the glyph pair.</typeparam>
        /// <param name="text">The shaped string source that contains the glyph pair.</param>
        /// <param name="ix">The index of the first glyph in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified glyph pair.</returns>
        public abstract Size2 GetShapedKerningInfo<TSource>(ref TSource text, Int32 ix)
            where TSource : IStringSource<ShapedChar>;

        /// <summary>
        /// Gets this font's kerning offset for the specified shaped glyph pair.
        /// </summary>
        /// <typeparam name="TSource1">The type of shaped string source contains the first character in the pair.</typeparam>
        /// <typeparam name="TSource2">The type of shaped string source contains the second character in the pair.</typeparam>
        /// <param name="text1">The shaped string source that contains the first glyph in the pair.</param>
        /// <param name="ix1">The index of the first glyph in the pair to evaluate.</param>
        /// <param name="text2">The shaped string source that contains the second glyph in the pair.</param>
        /// <param name="ix2">The index of the second glyph in the pair to evaluate.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The kerning offset for the specified glyph pair.</returns>
        public abstract Size2 GetShapedKerningInfo<TSource1, TSource2>(ref TSource1 text1, Int32 ix1, ref TSource2 text2, Int32 ix2, Boolean rtl = false)
            where TSource1 : IStringSource<ShapedChar>
            where TSource2 : IStringSource<ShapedChar>;

        /// <summary>
        /// Gets this font's kerning offset for the specified character pair.
        /// </summary>
        /// <param name="text">The string segment that contains the character pair.</param>
        /// <param name="ix">The index of the first character in the pair to evaluate.</param>
        /// <param name="c2">The second character in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified character pair.</returns>
        public abstract Size2 GetHypotheticalKerningInfo(ref StringSegment text, Int32 ix, Int32 c2);

        /// <summary>
        /// Gets this font's kerning offset for the specified character pair.
        /// </summary>
        /// <typeparam name="TSource">The type of string source which contains the character pair.</typeparam>
        /// <param name="text">The string source that contains the character pair.</param>
        /// <param name="ix">The index of the first character in the pair to evaluate.</param>
        /// <param name="c2">The Unicode code point of the second character in the pair to evaluate.</param>
        /// <returns>The kerning offset for the specified character pair.</returns>
        public abstract Size2 GetHypotheticalKerningInfo<TSource>(ref TSource text, Int32 ix, Int32 c2)
            where TSource : IStringSource<Char>;

        /// <summary>
        /// Gets this font's kerning offset for the specified glyph pair.
        /// </summary>
        /// <typeparam name="TSource">The type of shaped string source which contains the characglyphter pair.</typeparam>
        /// <param name="text">The shaped string source that contains the glyph pair.</param>
        /// <param name="ix">The index of the first glyph in the pair to evaluate.</param>
        /// <param name="glyphIndex2">The glyph index of the second glyph in the pair to evaluate.</param>
        /// <param name="rtl">A value indicating whether to reverse the order in which glyphs are accessed.</param>
        /// <returns>The kerning offset for the specified glyph pair.</returns>
        public abstract Size2 GetHypotheticalShapedKerningInfo<TSource>(ref TSource text, Int32 ix, Int32 glyphIndex2, Boolean rtl = false)
            where TSource : IStringSource<ShapedChar>;

        /// <summary>
        /// Gets a value indicating whether this font face contains the glyph 
        /// that corresponds to the specified Unicode code point.
        /// </summary>
        /// <param name="c">The UTF-32 Unicode code point to evaluate.</param>
        /// <returns><see langword="true"/> if the font face contains the specified glyph; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean ContainsGlyph(Int32 c);

        /// <summary>
        /// Gets a value indicating whether this font face supports methods which take
        /// glyph indices as parameters.
        /// </summary>
        public abstract Boolean SupportsGlyphIndices { get; }

        /// <summary>
        /// Gets a value indicating whether this font face supports methods
        /// which operate on shaped text.
        /// </summary>
        public abstract Boolean SupportsShapedText { get; }

        /// <summary>
        /// Gets the number of glyphs in the font face.
        /// </summary>
        public abstract Int32 Glyphs { get; }

        /// <summary>
        /// Gets the width of a space in this font face.
        /// </summary>
        public abstract Int32 SpaceWidth { get; }

        /// <summary>
        /// Gets the width of a tab in this font face.
        /// </summary>
        public abstract Int32 TabWidth { get; }

        /// <summary>
        /// Gets the size of the font face's ascender metric in pixels.
        /// </summary>
        public abstract Int32 Ascender { get; }

        /// <summary>
        /// Gets the size of the font face's descender metric in pixels. Values below the baseline are negative.
        /// </summary>
        public abstract Int32 Descender { get; }

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

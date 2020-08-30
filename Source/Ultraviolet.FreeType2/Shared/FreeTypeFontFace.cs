using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.FreeType2.Native;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Error;
using static Ultraviolet.FreeType2.Native.FT_Kerning_Mode;
using static Ultraviolet.FreeType2.Native.FT_Pixel_Mode;
using static Ultraviolet.FreeType2.Native.FT_Render_Mode;
using static Ultraviolet.FreeType2.Native.FT_Stroker_LineCap;
using static Ultraviolet.FreeType2.Native.FT_Stroker_LineJoin;

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
        /// <param name="metadata">The processor metadata with which this font face was loaded.</param>
        internal FreeTypeFontFace(UltravioletContext uv, IntPtr face, FreeTypeFontProcessorMetadata metadata)
            : base(uv)
        {
            Contract.Require(face, nameof(face));

            this.face = face;
            this.facade = new FT_FaceRecFacade(face);
            this.stroker = CreateStroker(face, metadata);

            if (metadata.AtlasWidth < 1 || metadata.AtlasHeight < 1 || metadata.AtlasSpacing < 0)
                throw new InvalidOperationException(FreeTypeStrings.InvalidAtlasParameters);

            this.AtlasWidth = metadata.AtlasWidth;
            this.AtlasHeight = metadata.AtlasHeight;
            this.AtlasSpacing = metadata.AtlasSpacing;

            this.scale = metadata.Scale;
            this.offsetXAdjustment = metadata.AdjustOffsetX;
            this.offsetYAdjustment = metadata.AdjustOffsetY;
            this.hAdvanceAdjustment = metadata.AdjustHorizontalAdvance;
            this.vAdvanceAdjustment = metadata.AdjustVerticalAdvance;
            this.glyphWidthAdjustment = (metadata.StrokeRadius * 2);
            this.glyphHeightAdjustment = (metadata.StrokeRadius * 2);

            this.HasColorStrikes = facade.HasColorFlag;
            this.HasKerningInfo = facade.HasKerningFlag;
            this.FamilyName = facade.MarshalFamilyName();
            this.StyleName = facade.MarshalStyleName();
            if (scale != 1f)
            {
                this.Ascender = (Int32)Math.Floor(scale * facade.Ascender) + metadata.AdjustAscender;
                this.Descender = (Int32)Math.Floor(scale * facade.Descender) + metadata.AdjustDescender;
                this.LineSpacing = (Int32)Math.Floor(scale * facade.LineSpacing) + metadata.AdjustLineSpacing;
            }
            else
            {
                this.Ascender = facade.Ascender + metadata.AdjustAscender;
                this.Descender = facade.Descender + metadata.AdjustDescender;
                this.LineSpacing = facade.LineSpacing + metadata.AdjustLineSpacing;
            }

            if (GetGlyphInfo(' ', false, out var spaceGlyphInfo))
                this.SpaceWidth = spaceGlyphInfo.Width;

            this.SizeInPoints = metadata.SizeInPoints;
            this.SizeInPixels = SizeInPixels;
            this.TabWidth = SpaceWidth * 4;

            this.totalDesignHeight = Ascender - Descender;

            this.SubstitutionCharacter = metadata.Substitution ??
                facade.GetCharIfDefined('�') ?? facade.GetCharIfDefined('□') ?? '?';

            PopulateGlyph(SubstitutionCharacter);
        }

        /// <inheritdoc/>
        public override String ToString() => String.Format("{0} {1} {2}pt", FamilyName, StyleName, SizeInPoints);

        /// <inheritdoc/>
        public override Int32 GetGlyphIndex(Int32 codePoint) => 
            (Int32)facade.GetCharIndex((UInt32)codePoint);
        
        /// <inheritdoc/>
        public override void GetCodePointRenderInfo(Int32 codePoint, out GlyphRenderInfo info) =>
            info = GetGlyphInfo((UInt32)codePoint, false, out var ginfo) ? ginfo.ToGlyphRenderInfo() : default(GlyphRenderInfo);

        /// <inheritdoc/>
        public override void GetGlyphIndexRenderInfo(Int32 glyphIndex, out GlyphRenderInfo info) =>
            info = GetGlyphInfo((UInt32)glyphIndex, true, out var ginfo) ? ginfo.ToGlyphRenderInfo() : default(GlyphRenderInfo);

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
        public override Size2 MeasureString<TSource>(ref TSource text)
        {
            return MeasureString(ref text, 0, text.Length);
        }

        /// <inheritdoc/>
        public override Size2 MeasureString<TSource>(ref TSource text, Int32 start, Int32 count)
        {
            if (count == 0)
                return Size2.Zero;

            Contract.EnsureRange(start >= 0 && start < text.Length, nameof(start));
            Contract.EnsureRange(count >= 0 && start + count <= text.Length, nameof(count));

            var cx = 0;
            var cy = 0;
            var maxLineWidth = 0;

            for (var i = 0; i < count; i++)
            {
                var ix = start + i;
                var ixNext = ix + 1;

                var character = text[ix];
                if (ixNext < count && Char.IsSurrogatePair(text[ix], text[ixNext]))
                    i++;

                switch (character)
                {
                    case '\r':
                        continue;

                    case '\n':
                        maxLineWidth = Math.Max(maxLineWidth, cx);
                        cx = 0;
                        cy = cy + LineSpacing;
                        continue;

                    case '\t':
                        cx = cx + TabWidth;
                        continue;
                }

                cx += MeasureGlyph(ref text, ix).Width;
            }
            maxLineWidth = Math.Max(maxLineWidth, cx);

            return new Size2(maxLineWidth, cy + totalDesignHeight);
        }

        /// <inheritdoc/>
        public override Size2 MeasureShapedString(ShapedString text, Boolean rtl = false) =>
            MeasureShapedString(ref text, 0, text.Length, rtl);

        /// <inheritdoc/>
        public override Size2 MeasureShapedString(ShapedString text, Int32 start, Int32 count, Boolean rtl = false) =>
            MeasureShapedString(ref text, start, count, rtl);

        /// <inheritdoc/>
        public override Size2 MeasureShapedString(ShapedStringBuilder text, Boolean rtl = false) =>
            MeasureShapedString(ref text, 0, text.Length, rtl);

        /// <inheritdoc/>
        public override Size2 MeasureShapedString(ShapedStringBuilder text, Int32 start, Int32 count, Boolean rtl = false) =>
            MeasureShapedString(ref text, start, count, rtl);

        /// <inheritdoc/>
        public override Size2 MeasureShapedString<TSource>(ref TSource text, Boolean rtl = false) =>
            MeasureShapedString(ref text, 0, text.Length, rtl);

        /// <inheritdoc/>
        public override Size2 MeasureShapedString<TSource>(ref TSource text, Int32 start, Int32 count, Boolean rtl = false)
        {
            if (count == 0)
                return Size2.Zero;

            Contract.EnsureRange(start >= 0 && start < text.Length, nameof(start));
            Contract.EnsureRange(count >= 0 && start + count <= text.Length, nameof(count));

            var cx = 0;
            var cy = 0;
            var maxLineWidth = 0;

            for (var i = 0; i < count; i++)
            {
                var sc = text[rtl ? (text.Length - 1) - i : i];
                switch (sc.GetSpecialCharacter())
                {
                    case '\n':
                        maxLineWidth = Math.Max(maxLineWidth, cx);
                        cx = 0;
                        cy = cy + LineSpacing;
                        break;

                    case '\t':
                        cx = cx + TabWidth;
                        break;

                    default:
                        cx = cx + sc.Advance;
                        break;
                }
            }
            maxLineWidth = Math.Max(maxLineWidth, cx);

            return new Size2(maxLineWidth, totalDesignHeight);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyph(Int32 c1, Int32? c2 = null)
        {
            if (!GetGlyphInfo((UInt32)c1, false, out var c1Info))
                return Size2.Zero;

            return new Size2(c1Info.Advance, totalDesignHeight);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyphByGlyphIndex(Int32 glyphIndex1, Int32? glyphIndex2 = null)
        {
            if (!GetGlyphInfo((UInt32)glyphIndex1, true, out var info))
                return Size2.Zero;

            return new Size2(info.Advance, totalDesignHeight);
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
            GetUtf32CodePointFromString(ref text, ix, out var c1, out var c1length);
            if (!GetGlyphInfo(c1, false, out var cinfo))
                return Size2.Zero;

            var c2 = (ix + c1length < text.Length) ? text[ix + c1length] : (Char?)null;
            var offset = c2.HasValue ? GetKerningInfo((Int32)c1, c2.GetValueOrDefault()) : Size2.Zero;
            return new Size2(cinfo.Advance, totalDesignHeight) + offset;
        }

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyph(ShapedString text, Int32 ix, Boolean rtl = false) =>
            MeasureShapedGlyph(ref text, ix, rtl);

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyph(ShapedStringBuilder text, Int32 ix, Boolean rtl = false) =>
            MeasureShapedGlyph(ref text, ix, rtl);

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyph<TSource>(ref TSource source, Int32 ix, Boolean rtl = false)
        {
            var sc = source[rtl ? (source.Length - 1) - ix : ix];
            switch (sc.GetSpecialCharacter())
            {
                case '\n':
                    return Size2.Zero;

                case '\t':
                    return new Size2(TabWidth, totalDesignHeight);

                default:
                    return new Size2(sc.Advance, totalDesignHeight);
            }
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyphWithHypotheticalKerning(ref StringSegment text, Int32 ix, Int32 c2)
        {
            var source = new StringSegmentSource(text);
            return MeasureGlyphWithHypotheticalKerning(ref source, ix, c2);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyphWithHypotheticalKerning<TSource>(ref TSource text, Int32 ix, Int32 c2)
        {
            GetUtf32CodePointFromString(ref text, ix, out var c1, out _);
            if (!GetGlyphInfo(c1, false, out var cinfo))
                return Size2.Zero;

            var offset = GetKerningInfo((Int32)c1, c2);
            return new Size2(cinfo.Advance, totalDesignHeight) + offset;
        }

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyphWithHypotheticalKerning<TSource>(ref TSource text, Int32 ix, Int32 glyphIndex2, Boolean rtl = false) =>
            new Size2(text[rtl ? (text.Length - 1) - ix : ix].Advance, totalDesignHeight);

        /// <inheritdoc/>
        public override Size2 MeasureGlyphWithoutKerning(ref StringSegment text, Int32 ix)
        {
            var source = new StringSegmentSource(text);
            return MeasureGlyphWithoutKerning(ref source, ix);
        }

        /// <inheritdoc/>
        public override Size2 MeasureGlyphWithoutKerning<TSource>(ref TSource text, Int32 ix)
        {
            GetUtf32CodePointFromString(ref text, ix, out var c1, out _);
            if (!GetGlyphInfo(c1, false, out var cinfo))
                return Size2.Zero;

            return new Size2(cinfo.Advance, totalDesignHeight);
        }

        /// <inheritdoc/>
        public override Size2 MeasureShapedGlyphWithoutKerning<TSource>(ref TSource text, Int32 ix, Boolean rtl = false) =>
            new Size2(text[rtl ? (text.Length - 1) - ix : ix].Advance, totalDesignHeight);

        /// <inheritdoc/>
        public override Size2 GetKerningInfoByGlyphIndex(Int32 glyphIndex1, Int32 glyphIndex2)
        {
            if (face == IntPtr.Zero || !HasKerningInfo)
                return Size2.Zero;

            if (Use64BitInterface)
            {
                var kerning = default(FT_Vector64);
                var err = FT_Get_Kerning(face, (UInt32)glyphIndex1, (UInt32)glyphIndex2, (UInt32)FT_KERNING_DEFAULT, (IntPtr)(&kerning));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                var x = FreeTypeCalc.F26Dot6ToInt32(kerning.x);
                var y = FreeTypeCalc.F26Dot6ToInt32(kerning.y);
                return new Size2(x, y);
            }
            else
            {
                var kerning = default(FT_Vector32);
                var err = FT_Get_Kerning(face, (UInt32)glyphIndex1, (UInt32)glyphIndex2, (UInt32)FT_KERNING_DEFAULT, (IntPtr)(&kerning));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                var x = FreeTypeCalc.F26Dot6ToInt32(kerning.x);
                var y = FreeTypeCalc.F26Dot6ToInt32(kerning.y);
                return new Size2(x, y);
            }
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo(Int32 c1, Int32 c2)
        {
            if (face == IntPtr.Zero || !HasKerningInfo)
                return Size2.Zero;

            var c1Index = facade.GetCharIndex((UInt32)c1);
            if (c1Index == 0)
                return Size2.Zero;

            var c2Index = facade.GetCharIndex((UInt32)c2);
            if (c2Index == 0)
                return Size2.Zero;

            if (Use64BitInterface)
            {
                var kerning = default(FT_Vector64);
                var err = FT_Get_Kerning(face, c1Index, c2Index, (UInt32)FT_KERNING_DEFAULT, (IntPtr)(&kerning));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                var x = FreeTypeCalc.F26Dot6ToInt32(kerning.x);
                var y = FreeTypeCalc.F26Dot6ToInt32(kerning.y);
                return new Size2(x, y);
            }
            else
            {
                var kerning = default(FT_Vector32);
                var err = FT_Get_Kerning(face, c1Index, c2Index, (UInt32)FT_KERNING_DEFAULT, (IntPtr)(&kerning));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                var x = FreeTypeCalc.F26Dot6ToInt32(kerning.x);
                var y = FreeTypeCalc.F26Dot6ToInt32(kerning.y);
                return new Size2(x, y);
            }
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
            if (ix + 1 >= text.Length)
                return Size2.Zero;

            var pos = ix;
            GetUtf32CodePointFromString(ref text, pos, out var c1, out var c1length);
            pos += c1length;

            if (pos >= text.Length)
                return Size2.Zero;

            GetUtf32CodePointFromString(ref text, pos, out var c2, out _);
            return GetKerningInfo((Int32)c1, (Int32)c2);
        }

        /// <inheritdoc/>
        public override Size2 GetKerningInfo<TSource1, TSource2>(ref TSource1 text1, Int32 ix1, ref TSource2 text2, Int32 ix2)
        {
            var c1 = text1[ix1];
            var c2 = text2[ix2];
            return GetKerningInfo(c1, c2);
        }

        /// <inheritdoc/>
        public override Size2 GetShapedKerningInfo<TSource>(ref TSource text, Int32 ix) => Size2.Zero;

        /// <inheritdoc/>
        public override Size2 GetShapedKerningInfo<TSource1, TSource2>(ref TSource1 text1, Int32 ix1, ref TSource2 text2, Int32 ix2, Boolean rtl = false) => Size2.Zero;

        /// <inheritdoc/>
        public override Size2 GetHypotheticalKerningInfo(ref StringSegment text, Int32 ix, Int32 c2)
        {
            var source = new StringSegmentSource(text);
            return GetHypotheticalKerningInfo(ref source, ix, c2);
        }

        /// <inheritdoc/>
        public override Size2 GetHypotheticalKerningInfo<TSource>(ref TSource text, Int32 ix, Int32 c2)
        {
            var c1 = text[ix];
            return GetKerningInfo(c1, c2);
        }

        /// <inheritdoc/>
        public override Size2 GetHypotheticalShapedKerningInfo<TSource>(ref TSource text, Int32 ix, Int32 glyphIndex2, Boolean rtl = false) => Size2.Zero;

        /// <inheritdoc/>
        public override Boolean ContainsGlyph(Int32 c) =>
            facade.GetCharIndex((UInt32)c) > 0;

        /// <summary>
        /// Ensures that the specified character's associated glyph has been added to the font's texture atlases.
        /// </summary>
        /// <param name="c">The character for which to ensure that a glyph has been populated.</param>
        public void PopulateGlyph(Char c) =>
            GetGlyphInfo(c, false, out var info);

        /// <summary>
        /// Gets the font's family name.
        /// </summary>
        public String FamilyName { get; }

        /// <summary>
        /// Gets the font's style name.
        /// </summary>
        public String StyleName { get; }

        /// <summary>
        /// Gets the width of the texture atlases used by FreeType2 font faces.
        /// </summary>
        public Int32 AtlasWidth { get; } = 1024;

        /// <summary>
        /// Gets the height of the texture atlases used by FreeType2 font faces.
        /// </summary>
        public Int32 AtlasHeight { get; } = 1024;

        /// <summary>
        /// Gets the spacing between cells on the atlases used by FreeType2 font faces.
        /// </summary>
        public Int32 AtlasSpacing { get; } = 4;

        /// <summary>
        /// Gets the font's size in points.
        /// </summary>
        public Int32 SizeInPoints { get; }

        /// <summary>
        /// Gets the font's size in pixels.
        /// </summary>
        public Int32 SizeInPixels { get; }

        /// <summary>
        /// Gets a value indicating whether this font has colored strikes.
        /// </summary>
        public Boolean HasColorStrikes { get; }

        /// <summary>
        /// Gets a value indicating whether this font has kerning information.
        /// </summary>
        public Boolean HasKerningInfo { get; }

        /// <summary>
        /// Gets a value indicating whether this font face is stroked.
        /// </summary>
        public Boolean IsStroked => stroker != IntPtr.Zero;

        /// <inheritdoc/>
        public override Boolean SupportsGlyphIndices => true;

        /// <inheritdoc/>
        public override Boolean SupportsShapedText => true;

        /// <inheritdoc/>
        public override Int32 Glyphs => glyphInfoCache.Count;

        /// <inheritdoc/>
        public override Int32 SpaceWidth { get; }

        /// <inheritdoc/>
        public override Int32 TabWidth { get; }

        /// <inheritdoc/>
        public override Int32 Ascender { get; }

        /// <inheritdoc/>
        public override Int32 Descender { get; }

        /// <inheritdoc/>
        public override Int32 LineSpacing { get; }

        /// <inheritdoc/>
        public override Char SubstitutionCharacter { get; }

        /// <summary>
        /// Gets a pointer to the native font object.
        /// </summary>
        internal IntPtr NativePointer => face;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                SafeDispose.DisposeRef(ref resamplingSurface1);
                SafeDispose.DisposeRef(ref resamplingSurface2);

                if (stroker != IntPtr.Zero)
                {
                    FT_Stroker_Done(stroker);
                    stroker = IntPtr.Zero;
                }

                var err = FT_Done_Face(face);
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                face = IntPtr.Zero;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates a stroker if this font face requires one.
        /// </summary>
        private static IntPtr CreateStroker(IntPtr face, FreeTypeFontProcessorMetadata metadata)
        {
            var err = default(FT_Error);
            var pstroker = IntPtr.Zero;

            if (metadata.StrokeRadius <= 0)
                return pstroker;

            err = FT_Stroker_New(FreeTypeFontPlugin.Library, (IntPtr)(&pstroker));
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            var lineCapMode = FT_STROKER_LINECAP_BUTT;
            switch (metadata.StrokeLineCap)
            {
                case FreeTypeLineCapMode.Round:
                    lineCapMode = FT_STROKER_LINECAP_ROUND;
                    break;

                case FreeTypeLineCapMode.Square:
                    lineCapMode = FT_STROKER_LINECAP_SQUARE;
                    break;
            }

            var lineJoinMode = FT_STROKER_LINEJOIN_ROUND;
            switch (metadata.StrokeLineJoin)
            {
                case FreeTypeLineJoinMode.Bevel:
                    lineJoinMode = FT_STROKER_LINEJOIN_BEVEL;
                    break;

                case FreeTypeLineJoinMode.Miter:
                    lineJoinMode = FT_STROKER_LINEJOIN_MITER;
                    break;

                case FreeTypeLineJoinMode.MiterFixed:
                    lineJoinMode = FT_STROKER_LINEJOIN_MITER_FIXED;
                    break;
            }

            var fixedRadius = FreeTypeCalc.Int32ToF26Dot6(metadata.StrokeRadius);
            var fixedMiterLimit = FreeTypeCalc.Int32ToF26Dot6(metadata.StrokeMiterLimit);

            if (Use64BitInterface)
                FT_Stroker_Set64(pstroker, fixedRadius, lineCapMode, lineJoinMode, fixedMiterLimit);
            else
                FT_Stroker_Set32(pstroker, fixedRadius, lineCapMode, lineJoinMode, fixedMiterLimit);

            return pstroker;
        }

        /// <summary>
        /// Performs alpha blending between two colors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Color AlphaBlend(Color src, Color dst, Boolean srgb)
        {
            if (srgb)
            {
                src = Color.ConvertSrgbColorToLinear(src);
                dst = Color.ConvertSrgbColorToLinear(dst);
            }

            var srcA = src.A / 255f;
            var srcR = src.R / 255f;
            var srcG = src.G / 255f;
            var srcB = src.B / 255f;

            var dstA = dst.A / 255f;
            var dstR = dst.R / 255f;
            var dstG = dst.G / 255f;
            var dstB = dst.B / 255f;

            var invSrcA = (1f - srcA);
            var outA = srcA + (dstA * invSrcA);
            var outR = srcR + (dstR * invSrcA);
            var outG = srcG + (dstG * invSrcA);
            var outB = srcB + (dstB * invSrcA);
            var outColor = new Color(outR, outG, outB, outA);

            return srgb ? Color.ConvertLinearColorToSrgb(outColor) : outColor;
        }

        /// <summary>
        /// Ensures that the specified scratch surface exists and has at least the specified size.
        /// </summary>
        private static void CreateResamplingSurface(UltravioletContext uv, ref Surface2D srf, Int32 w, Int32 h)
        {
            if (srf == null)
            {
                var srgb = uv.GetGraphics().Capabilities.SrgbEncodingEnabled;
                var opts = srgb ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;

                srf?.Dispose();
                srf = Surface2D.Create(w, h, opts);
            }
            srf.Clear(Color.Transparent);
        }

        /// <summary>
        /// Calculates the weights used during a resampling operation.
        /// </summary>
        private static void CreateResamplingWeights(ref Single[] weights, Int32 srcSize, Int32 dstSize, Single ratio, Single radius)
        {
            Single TriangleKernel(Single x)
            {
                if (x < 0f) x = -x;
                if (x < 1f) return 1f - x;
                return 0f;
            };

            var scale = ratio;
            if (scale < 1.0f)
                scale = 1.0f;

            var weightCountPerPixel = (Int32)Math.Floor(radius) - (Int32)Math.Ceiling(-radius);
            var weightOffset = 0;

            var resultSize = dstSize * weightCountPerPixel;
            if (weights == null || weights.Length < resultSize)
                weights = new Single[resultSize];

            for (int i = 0; i < dstSize; i++)
            {
                var pos = ((i + 0.5f) * ratio) - 0.5f;
                var min = (Int32)Math.Ceiling(pos - radius);
                var max = min + weightCountPerPixel;
                var sum = 0.0f;

                for (var j = min; j < max; j++)
                {
                    var weight = TriangleKernel((j - pos) / scale);
                    sum += weight;
                    weights[weightOffset + (j - min)] = weight;
                }

                if (sum > 0)
                {
                    for (var j = 0; j < weightCountPerPixel; j++)
                        weights[weightOffset + j] /= sum;
                }

                weightOffset += weightCountPerPixel;
            }
        }

        /// <summary>
        /// Blits the specified bitmap onto a texture atlas.
        /// </summary>
        private void BlitBitmap(ref FT_Bitmap bmp, Int32 adjustX, Int32 adjustY, 
            Color color, FreeTypeBlendMode blendMode, ref DynamicTextureAtlas.Reservation reservation)
        {
            var width = (Int32)bmp.width;
            if (width == 0)
                return;

            var height = (Int32)bmp.rows;
            if (height == 0)
                return;

            var resampleRatio = 0f;
            var resampleRadius = 0f;
            var scaledWidth = width;
            var scaledHeight = height;

            // Prepare for scaling the glyph if required.
            var scaled = (scale != 1f);
            if (scaled)
            {
                // Calculate resampling parameters.
                scaledWidth = (Int32)Math.Floor(width * scale);
                scaledHeight = (Int32)Math.Floor(height * scale);

                resampleRatio = 1f / scale;
                resampleRadius = resampleRatio;
                if (resampleRadius < 1f)
                    resampleRadius = 1f;

                // Ensure that our scratch surfaces exist with enough space.
                CreateResamplingSurface(Ultraviolet, ref resamplingSurface1, width, height);
                CreateResamplingSurface(Ultraviolet, ref resamplingSurface2, scaledWidth, height);

                // Precalculate pixel weights.
                CreateResamplingWeights(ref resamplingWeightsX, width, reservation.Width, resampleRatio, resampleRadius);
                CreateResamplingWeights(ref resamplingWeightsY, height, reservation.Height, resampleRatio, resampleRadius);
            }

            // Blit the glyph to the specified reservation.
            switch ((FT_Pixel_Mode)bmp.pixel_mode)
            {
                case FT_PIXEL_MODE_MONO:
                    if (scaled)
                    {
                        BlitGlyphBitmapMono(resamplingSurface1, 0, 0, ref bmp, width, height, bmp.pitch, color, blendMode, reservation.Atlas.IsFlipped);
                        BlitResampledX(resamplingSurface1, new Rectangle(0, 0, width, height), resamplingSurface2, new Rectangle(0, 0, scaledWidth, height), resamplingWeightsX);
                        BlitResampledY(resamplingSurface2, new Rectangle(0, 0, scaledWidth, height), reservation.Atlas.Surface,
                            new Rectangle(reservation.X + adjustX, reservation.Y + adjustY, scaledWidth, scaledHeight), resamplingWeightsY);
                    }
                    else
                    {
                        BlitGlyphBitmapMono(reservation.Atlas.Surface, reservation.X + adjustX, reservation.Y + adjustY,
                            ref bmp, width, height, bmp.pitch, color, blendMode, reservation.Atlas.IsFlipped);
                    }
                    break;

                case FT_PIXEL_MODE_GRAY:
                    if (scaled)
                    {
                        BlitGlyphBitmapGray(resamplingSurface1, 0, 0, ref bmp, width, height, bmp.pitch, color, blendMode, reservation.Atlas.IsFlipped);
                        BlitResampledX(resamplingSurface1, new Rectangle(0, 0, width, height), resamplingSurface2, new Rectangle(0, 0, scaledWidth, height), resamplingWeightsX);
                        BlitResampledY(resamplingSurface2, new Rectangle(0, 0, scaledWidth, height), reservation.Atlas.Surface,
                            new Rectangle(reservation.X + adjustX, reservation.Y + adjustY, scaledWidth, scaledHeight), resamplingWeightsY);
                    }
                    else
                    {
                        BlitGlyphBitmapGray(reservation.Atlas.Surface, reservation.X + adjustX, reservation.Y + adjustY,
                            ref bmp, width, height, bmp.pitch, color, blendMode, reservation.Atlas.IsFlipped);
                    }
                    break;

                case FT_PIXEL_MODE_BGRA:
                    if (scaled)
                    {
                        BlitGlyphBitmapBgra(resamplingSurface1, 0, 0, ref bmp, width, height, bmp.pitch, blendMode, reservation.Atlas.IsFlipped);
                        BlitResampledX(resamplingSurface1, new Rectangle(0, 0, width, height), resamplingSurface2, new Rectangle(0, 0, scaledWidth, height), resamplingWeightsX);
                        BlitResampledY(resamplingSurface2, new Rectangle(0, 0, scaledWidth, height), reservation.Atlas.Surface,
                            new Rectangle(reservation.X + adjustX, reservation.Y + adjustY, scaledWidth, scaledHeight), resamplingWeightsY);
                    }
                    else
                    {
                        BlitGlyphBitmapBgra(reservation.Atlas.Surface, reservation.X + adjustX, reservation.Y + adjustY,
                            ref bmp, width, height, bmp.pitch, blendMode, reservation.Atlas.IsFlipped);
                    }
                    break;

                default:
                    throw new NotSupportedException(FreeTypeStrings.PixelFormatNotSupported);
            }

            reservation.Atlas.Invalidate();
        }

        /// <summary>
        /// Converts the specified index of a string to a UTF-32 codepoint.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean GetUtf32CodePointFromString(ref StringSegment text, Int32 ix, out UInt32 utf32, out Int32 length)
        {
            var ixNext = ix + 1;
            if (ixNext < text.Length)
            {
                var c1 = text[ix];
                var c2 = text[ixNext];
                if (Char.IsSurrogatePair(c1, c2))
                {
                    utf32 = (UInt32)Char.ConvertToUtf32(c1, c2);
                    length = 2;
                    return true;
                }
            }

            length = 1;

            if (Char.IsSurrogate(text[ix]))
            {
                utf32 = SubstitutionCharacter;
                return false;
            }

            utf32 = text[ix];
            return false;
        }

        /// <summary>
        /// Converts the specified index of a string to a UTF-32 codepoint.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean GetUtf32CodePointFromString<TSource>(ref TSource text, Int32 ix, out UInt32 utf32, out Int32 length)
            where TSource : IStringSource<Char>
        {
            var ixNext = ix + 1;
            if (ixNext < text.Length)
            {
                var c1 = text[ix];
                var c2 = text[ixNext];
                if (Char.IsSurrogatePair(c1, c2))
                {
                    utf32 = (UInt32)Char.ConvertToUtf32(c1, c2);
                    length = 2;
                    return true;
                }
            }

            length = 1;

            if (Char.IsSurrogate(text[ix]))
            {
                utf32 = SubstitutionCharacter;
                return false;
            }

            utf32 = text[ix];
            return false;
        }

        /// <summary>
        /// Blits a mono glyph bitmap to the specified atlas' surface.
        /// </summary>
        private static void BlitGlyphBitmapMono(Surface2D dstSurface, Int32 dstX, Int32 dstY,
            ref FT_Bitmap bmp, Int32 bmpWidth, Int32 bmpHeight, Int32 bmpPitch, Color color, FreeTypeBlendMode blendMode, Boolean flip)
        {
            var srgb = dstSurface.SrgbEncoded;

            for (int y = 0; y < bmpHeight; y++)
            {
                var pSrcY = flip ? (bmpHeight - 1) - y : y;
                var pSrc = (Byte*)bmp.buffer + (pSrcY * bmpPitch);
                var pDst = (Color*)dstSurface.Pixels + ((dstY + y) * dstSurface.Width) + dstX;
                if (blendMode == FreeTypeBlendMode.Opaque)
                {
                    for (int x = 0; x < bmpWidth; x += 8)
                    {
                        var bits = *pSrc++;
                        for (int b = 0; b < 8; b++)
                        {
                            var srcColor = ((bits >> (7 - b)) & 1) == 0 ? Color.Transparent : color;

                            if (srgb)
                                srcColor = Color.ConvertLinearColorToSrgb(srcColor);

                            *pDst++ = srcColor;
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < bmpWidth; x += 8)
                    {
                        var bits = *pSrc++;
                        for (int b = 0; b < 8; b++)
                        {
                            var srcColor = ((bits >> (7 - b)) & 1) == 0 ? Color.Transparent : color;
                            var dstColor = *pDst;

                            if (srgb)
                                srcColor = Color.ConvertLinearColorToSrgb(srcColor);

                            *pDst++ = AlphaBlend(srcColor, dstColor, srgb);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Blits a grayscale glyph bitmap to the specified atlas' surface.
        /// </summary>
        private static void BlitGlyphBitmapGray(Surface2D dstSurface, Int32 dstX, Int32 dstY,
            ref FT_Bitmap bmp, Int32 bmpWidth, Int32 bmpHeight, Int32 bmpPitch, Color color, FreeTypeBlendMode blendMode, Boolean flip)
        {
            var srgb = dstSurface.SrgbEncoded;

            for (int y = 0; y < bmpHeight; y++)
            {
                var pSrcY = flip ? (bmpHeight - 1) - y : y;
                var pSrc = (Byte*)bmp.buffer + (pSrcY * bmpPitch);
                var pDst = (Color*)dstSurface.Pixels + ((dstY + y) * dstSurface.Width) + dstX;
                if (blendMode == FreeTypeBlendMode.Opaque)
                {
                    for (int x = 0; x < bmpWidth; x++)
                    {
                        var srcColor = color * (*pSrc++ / 255f);

                        if (srgb)
                            srcColor = Color.ConvertLinearColorToSrgb(srcColor);

                        *pDst++ = srcColor;
                    }
                }
                else
                {
                    for (int x = 0; x < bmpWidth; x++)
                    {
                        var srcColor = color * (*pSrc++ / 255f);
                        var dstColor = *pDst;

                        if (srgb)
                            srcColor = Color.ConvertLinearColorToSrgb(srcColor);

                        *pDst++ = AlphaBlend(srcColor, dstColor, srgb);
                    }
                }
            }
        }

        /// <summary>
        /// Blits a BGRA color glyph bitmap to the specified atlas' surface.
        /// </summary>
        private static void BlitGlyphBitmapBgra(Surface2D dstSurface, Int32 dstX, Int32 dstY, 
            ref FT_Bitmap bmp, Int32 bmpWidth, Int32 bmpHeight, Int32 bmpPitch, FreeTypeBlendMode blendMode, Boolean flip)
        {
            var srgb = dstSurface.SrgbEncoded;

            for (int y = 0; y < bmpHeight; y++)
            {
                var pSrcY = flip ? (bmpHeight - 1) - y : y;
                var pSrc = (Color*)((Byte*)bmp.buffer + (pSrcY * bmpPitch));
                var pDst = (Color*)dstSurface.Pixels + ((dstY + y) * dstSurface.Width) + dstX;
                if (blendMode == FreeTypeBlendMode.Opaque)
                {
                    for (int x = 0; x < bmpWidth; x++)
                    {
                        var srcColor = *pSrc++;
                        var dstColor = new Color(srcColor.B, srcColor.G, srcColor.R, srcColor.A);
                        *pDst++ = dstColor;
                    }
                }
                else
                {
                    for (int x = 0; x < bmpWidth; x++)
                    {
                        var srcColor = *pSrc++;
                        var dstColor = *pDst;

                        *pDst++ = AlphaBlend(srcColor, dstColor, srgb);
                    }
                }
            }
        }

        /// <summary>
        /// Blits from one surface to another surface, performing bilinear resampling along the x-axis.
        /// </summary>
        private static void BlitResampledX(Surface2D srcSurface, Rectangle srcRect, Surface2D dstSurface, Rectangle dstRect, Single[] weights)
        {
            var scale = dstRect.Width / (Single)srcRect.Width;
            var ratio = 1f / scale;
            var radius = ratio;
            if (radius < 1f)
                radius = 1f;
            
            var weightCount = (Int32)Math.Floor(radius) - (Int32)Math.Ceiling(-radius);
            var weightOffset = 0;

            for (var y = 0; y < srcRect.Height; y++)
            {
                weightOffset = 0;

                var pSrc = (Color*)((Byte*)srcSurface.Pixels + ((srcRect.Y + y) * srcSurface.Pitch)) + srcRect.X;
                var pDst = (Color*)((Byte*)dstSurface.Pixels + ((dstRect.Y + y) * dstSurface.Pitch)) + dstRect.X;

                for (var x = 0; x < dstRect.Width; x++)
                {
                    var pos = ((x + 0.5f) * ratio) - 0.5f;
                    var min = (Int32)Math.Ceiling(pos - radius);

                    var totalR = 0f;
                    var totalG = 0f;
                    var totalB = 0f;
                    var totalA = 0f;

                    for (int i = 0; i < weightCount; i++)
                    {
                        var pixOffset = min + i;
                        if (pixOffset < 0)
                            pixOffset = 0;
                        else if (pixOffset >= srcRect.Width)
                            pixOffset = srcRect.Width - 1;

                        var pixWeight = weights[weightOffset + i];
                        var pixColor = pSrc[srcRect.X + pixOffset];

                        var weight = weights[weightOffset + i];
                        totalR += weight * pixColor.R;
                        totalG += weight * pixColor.G;
                        totalB += weight * pixColor.B;
                        totalA += weight * pixColor.A;
                    }

                    *pDst++ = new Color((Byte)totalR, (Byte)totalG, (Byte)totalB, (Byte)totalA);

                    weightOffset += weightCount;
                }
            }
        }

        /// <summary>
        /// Blits from one surface to another surface, performing bilinear resampling along the y-axis.
        /// </summary>
        private static void BlitResampledY(Surface2D srcSurface, Rectangle srcRect, Surface2D dstSurface, Rectangle dstRect, Single[] weights)
        {
            var scale = dstRect.Height / (Single)srcRect.Height;
            var ratio = 1f / scale;
            var radius = ratio;
            if (radius < 1f)
                radius = 1f;

            var weightCount = (Int32)Math.Floor(radius) - (Int32)Math.Ceiling(-radius);
            var weightOffset = 0;

            var bytesPerPixel = sizeof(Color);
            for (var x = 0; x < dstRect.Width; x++)
            {
                var pSrc = (Byte*)srcSurface.Pixels + (srcRect.Y * srcSurface.Pitch) + ((srcRect.X + x) * bytesPerPixel);
                var pDst = (Byte*)dstSurface.Pixels + (dstRect.Y * dstSurface.Pitch) + ((dstRect.X + x) * bytesPerPixel);

                weightOffset = 0;

                for (var y = 0; y < dstRect.Height; y++)
                {
                    var pos = ((y + 0.5f) * ratio) - 0.5f;
                    var min = (Int32)Math.Ceiling(pos - radius);

                    var totalR = 0f;
                    var totalG = 0f;
                    var totalB = 0f;
                    var totalA = 0f;

                    var pPix = pSrc + (min * srcSurface.Pitch);

                    for (int i = 0; i < weightCount; i++)
                    {
                        var pixOffset = min + i;
                        if (pixOffset < 0)
                            pixOffset = 0;
                        else if (pixOffset >= srcRect.Height)
                            pixOffset = srcRect.Height - 1;

                        var pixWeight = weights[weightOffset + i];
                        var pixColor = *(Color*)(pSrc + (pixOffset * srcSurface.Pitch));

                        var weight = weights[weightOffset + i];
                        totalR += weight * pixColor.R;
                        totalG += weight * pixColor.G;
                        totalB += weight * pixColor.B;
                        totalA += weight * pixColor.A;
                    }

                    *(Color*)pDst = new Color((Byte)totalR, (Byte)totalG, (Byte)totalB, (Byte)totalA);                    
                    pDst += dstSurface.Pitch;

                    weightOffset += weightCount;
                }
            }
        }

        /// <summary>
        /// Gets the font face's metadata for the specified code point or glyph index.
        /// </summary>
        private Boolean GetGlyphInfo(UInt32 glyphIndexOrCodePoint, Boolean isGlyphIndex, out FreeTypeGlyphInfo info)
        {
            var glyphIndex = isGlyphIndex ? glyphIndexOrCodePoint : facade.GetCharIndex(glyphIndexOrCodePoint);
            if (glyphIndex == 0)
            {
                if (!isGlyphIndex)
                {
                    var cu16 = (glyphIndexOrCodePoint <= Char.MaxValue) ? (Char?)glyphIndexOrCodePoint : null;
                    if (cu16 != SubstitutionCharacter)
                        return GetGlyphInfo(SubstitutionCharacter, false, out info);
                }
                info = default(FreeTypeGlyphInfo);
                return false;
            }
            else
            {
                if (glyphInfoCache.TryGetValue(glyphIndex, out var cached))
                {
                    info = cached;
                }
                else
                {
                    LoadGlyphTexture(glyphIndex, out info);
                    glyphInfoCache[glyphIndex] = info;
                }
            }
            return true;
        }

        /// <summary>
        /// Loads the texture data for the specified glyph.
        /// </summary>
        private void LoadGlyphTexture(UInt32 glyphIndex, out FreeTypeGlyphInfo info)
        {
            var err = default(FT_Error);

            // Load the glyph into the face's glyph slot.
            var flags = IsStroked ? FT_LOAD_NO_BITMAP : FT_LOAD_COLOR;
            err = FT_Load_Glyph(face, glyphIndex, flags);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            var glyphOffsetX = 0;
            var glyphOffsetY = 0;
            var glyphAscent = 0;
            var glyphAdvance = facade.GlyphMetricHorizontalAdvance + hAdvanceAdjustment;


            var reservation = default(DynamicTextureAtlas.Reservation);
            var reservationFound = false;

            // Stroke the glyph.   
            StrokeGlyph(out var strokeGlyph, out var strokeBmp,
                out var strokeOffsetX, out var strokeOffsetY);

            // Render the glyph.
            err = FT_Render_Glyph(facade.GlyphSlot, FT_RENDER_MODE_NORMAL);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            var glyphBmp = facade.GlyphBitmap;
            if (glyphBmp.rows > 0 && glyphBmp.width > 0)
            {
                var glyphBmpHeightScaled = (Int32)glyphBmp.rows;
                var glyphAdjustX = 0;
                var glyphAdjustY = 0;

                var reservationWidth = 0;
                var reservationHeight = 0;
                if (strokeGlyph == IntPtr.Zero)
                {
                    reservationWidth = (Int32)glyphBmp.width;
                    reservationHeight = (Int32)glyphBmp.rows;

                    glyphAscent = facade.GlyphBitmapTop;
                }
                else
                {
                    reservationWidth = Math.Max((Int32)strokeBmp.width, (Int32)glyphBmp.width);
                    reservationHeight = Math.Max((Int32)strokeBmp.rows, (Int32)glyphBmp.rows);

                    glyphAscent = Math.Max(facade.GlyphBitmapTop, strokeOffsetY);
                    glyphAdjustX = -(strokeOffsetX - facade.GlyphBitmapLeft);
                    glyphAdjustY = +(strokeOffsetY - facade.GlyphBitmapTop);
                }

                // Apply scaling to metrics.
                if (scale != 1f)
                {
                    glyphBmpHeightScaled = (Int32)Math.Floor(glyphBmpHeightScaled * scale);
                    glyphAdvance = (Int32)Math.Floor(glyphAdvance * scale);
                    glyphAscent = (Int32)Math.Floor(glyphAscent * scale);
                    reservationWidth = (Int32)Math.Floor(reservationWidth * scale);
                    reservationHeight = (Int32)Math.Floor(reservationHeight * scale);
                }

                // Attempt to reserve space on one of the font's existing atlases.
                if (reservationWidth > 0 && reservationHeight > 0)
                {
                    foreach (var atlas in atlases)
                    {
                        if (atlas.TryReserveCell(reservationWidth, reservationHeight, out reservation))
                        {
                            reservationFound = true;
                            break;
                        }
                    }

                    // Attempt to create a new atlas if we weren't able to make a reservation.
                    if (!reservationFound)
                    {
                        var srgb = Ultraviolet.GetGraphics().Capabilities.SrgbEncodingEnabled;
                        var opts = TextureOptions.ImmutableStorage | (srgb ? TextureOptions.SrgbColor : TextureOptions.LinearColor);
                        var atlas = DynamicTextureAtlas.Create(AtlasWidth, AtlasHeight, AtlasSpacing, opts);
                        atlas.Surface.Clear(Color.Transparent);
                        atlases.Add(atlas);

                        if (!atlas.TryReserveCell(reservationWidth, reservationHeight, out reservation))
                            throw new InvalidOperationException(FreeTypeStrings.GlyphTooBigForAtlas.Format());
                    }

                    // If we're using flipped textures, we need to modify our y-adjust...
                    if (reservation.Atlas.IsFlipped)
                        glyphAdjustY = (reservationHeight - glyphBmpHeightScaled) - glyphAdjustY;

                    // Update the atlas surface.
                    var blendMode = FreeTypeBlendMode.Opaque;
                    if (strokeGlyph != IntPtr.Zero)
                    {
                        BlitBitmap(ref strokeBmp, 0, 0, Color.Black, blendMode, ref reservation);
                        blendMode = FreeTypeBlendMode.Blend;

                        FT_Done_Glyph(strokeGlyph);
                    }
                    BlitBitmap(ref glyphBmp, glyphAdjustX, glyphAdjustY, Color.White, blendMode, ref reservation);
                }
            }

            // Calculate the glyph's metrics and apply scaling.
            var glyphWidth = facade.GlyphMetricWidth + glyphWidthAdjustment;
            var glyphHeight = facade.GlyphMetricHeight + glyphHeightAdjustment;
            if (scale != 1f)
            {
                glyphWidth = (Int32)Math.Floor(glyphWidth * scale);
                glyphHeight = (Int32)Math.Floor(glyphHeight * scale);
                glyphOffsetX = (Int32)Math.Floor(facade.GlyphBitmapLeft * scale) + offsetXAdjustment;
                glyphOffsetY = (Ascender - glyphAscent) + offsetYAdjustment;
            }
            else
            {
                glyphOffsetX = facade.GlyphBitmapLeft + offsetXAdjustment;
                glyphOffsetY = (Ascender - glyphAscent) + offsetYAdjustment;
            }

            // Create the glyph info structure for the glyph cache.
            info = new FreeTypeGlyphInfo
            {
                Advance = glyphAdvance,
                Width = glyphWidth,
                Height = glyphHeight,
                OffsetX = glyphOffsetX,
                OffsetY = glyphOffsetY,
                Texture = reservation.Atlas,
                TextureRegion = reservation.Atlas == null ? Rectangle.Empty : reservation.Atlas.IsFlipped ?
                    new Rectangle(reservation.X, reservation.Atlas.Height - (reservation.Y + reservation.Height), reservation.Width, reservation.Height) :
                    new Rectangle(reservation.X, reservation.Y, reservation.Width, reservation.Height),
            };
        }

        /// <summary>
        /// Strokes the currently loaded glyph, if a stroker exists.
        /// </summary>
        private void StrokeGlyph(out IntPtr glyph, out FT_Bitmap strokeBmp, out Int32 strokeOffsetX, out Int32 strokeOffsetY)
        {
            var err = default(FT_Error);
            var strokeGlyph = IntPtr.Zero;

            if (stroker == IntPtr.Zero)
            {
                glyph = IntPtr.Zero;
                strokeBmp = default(FT_Bitmap);
                strokeOffsetX = 0;
                strokeOffsetY = 0;
                return;
            }

            err = FT_Get_Glyph(facade.GlyphSlot, (IntPtr)(&strokeGlyph));
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            err = FT_Glyph_StrokeBorder((IntPtr)(&strokeGlyph), stroker, false, true);
            if (err != FT_Err_Ok)
            {
                FT_Done_Glyph(strokeGlyph);
                throw new FreeTypeException(err);
            }

            err = FT_Glyph_To_Bitmap((IntPtr)(&strokeGlyph), FT_RENDER_MODE_NORMAL, IntPtr.Zero, true);
            if (err != FT_Err_Ok)
            {
                FT_Done_Glyph(strokeGlyph);
                throw new FreeTypeException(err);
            }

            if (Use64BitInterface)
            {
                var bmp64 = (FT_BitmapGlyphRec64*)strokeGlyph;
                strokeBmp = bmp64->bitmap;
                strokeOffsetX = bmp64->left;
                strokeOffsetY = bmp64->top;
            }
            else
            {
                var bmp32 = (FT_BitmapGlyphRec32*)strokeGlyph;
                strokeBmp = bmp32->bitmap;
                strokeOffsetX = bmp32->left;
                strokeOffsetY = bmp32->top;
            }

            glyph = strokeGlyph;
        }

        // The FreeType2 face which this instance represents.
        private readonly FT_FaceRecFacade facade;
        private IntPtr face;
        private IntPtr stroker;

        // Metric adjustments.
        private readonly Single scale;
        private readonly Int32 totalDesignHeight;
        private readonly Int32 offsetXAdjustment;
        private readonly Int32 offsetYAdjustment;
        private readonly Int32 hAdvanceAdjustment;
        private readonly Int32 vAdvanceAdjustment;
        private readonly Int32 glyphWidthAdjustment;
        private readonly Int32 glyphHeightAdjustment;

        // Surfaces and buffers used for resizing glyphs.
        private Single[] resamplingWeightsX;
        private Single[] resamplingWeightsY;
        private Surface2D resamplingSurface1;
        private Surface2D resamplingSurface2;

        // Cache of atlases used to store glyph images.
        private readonly List<DynamicTextureAtlas> atlases = 
            new List<DynamicTextureAtlas>();

        // Cache of metadata for loaded glyphs.
        private readonly Dictionary<UInt32, FreeTypeGlyphInfo> glyphInfoCache =
            new Dictionary<UInt32, FreeTypeGlyphInfo>();
    }
}

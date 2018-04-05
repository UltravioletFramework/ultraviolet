using System;
using System.Collections.Generic;
using System.Globalization;
using Ultraviolet.Content;
using Ultraviolet.FreeType2.Native;
using Ultraviolet.Platform;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Error;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Loads FreeType fonts.
    /// </summary>
    [ContentProcessor]
    public unsafe partial class FreeTypeFontProcessor : ContentProcessor<FreeTypeFontInfo, FreeTypeFont>
    {
        /// <inheritdoc/>
        public override FreeTypeFont Process(ContentManager manager, IContentProcessorMetadata metadata, FreeTypeFontInfo input)
        {
            var mdata = metadata.As<FreeTypeFontProcessorMetadata>();
            var prepopGlyphRanges = new List<PrepopulatedGlyphRange>();

            GetBestScreenDensityMatch(manager.Ultraviolet, metadata.AssetDensity, out var dpiX, out var dpiY);
            GetPrepopulatedGlyphList(mdata.PrepopulatedGlyphs, prepopGlyphRanges);

            var sizeInPoints = (Int32)mdata.SizeInPoints;

            var ftFaceRegular = LoadFontFace(sizeInPoints, (UInt32)dpiX, (UInt32)dpiY, input.FaceDataRegular, input.FaceDataRegularLength);
            var ftFaceBold = LoadFontFace(sizeInPoints, (UInt32)dpiX, (UInt32)dpiY, input.FaceDataBold, input.FaceDataBoldLength);
            var ftFaceItalic = LoadFontFace(sizeInPoints, (UInt32)dpiX, (UInt32)dpiY, input.FaceDataItalic, input.FaceDataItalicLength);
            var ftFaceBoldItalic = LoadFontFace(sizeInPoints, (UInt32)dpiX, (UInt32)dpiY, input.FaceDataBoldItalic, input.FaceDataBoldItalicLength);

            var uvFaceRegular = (ftFaceRegular == IntPtr.Zero) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceRegular, sizeInPoints, mdata.Substitution);
            var uvFaceBold = (ftFaceBold == IntPtr.Zero) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceBold, sizeInPoints, mdata.Substitution);
            var uvFaceItalic = (ftFaceItalic == IntPtr.Zero) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceItalic, sizeInPoints, mdata.Substitution);
            var uvFaceBoldItalic = (ftFaceBoldItalic == IntPtr.Zero) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceBoldItalic, sizeInPoints, mdata.Substitution);

            if (uvFaceRegular != null)
                PrepopulateGlyphs(uvFaceRegular, prepopGlyphRanges);

            if (uvFaceBold != null)
                PrepopulateGlyphs(uvFaceBold, prepopGlyphRanges);

            if (uvFaceItalic != null)
                PrepopulateGlyphs(uvFaceItalic, prepopGlyphRanges);

            if (uvFaceBoldItalic != null)
                PrepopulateGlyphs(uvFaceBoldItalic, prepopGlyphRanges);

            return new FreeTypeFont(manager.Ultraviolet, uvFaceRegular, uvFaceBold, uvFaceItalic, uvFaceBoldItalic);
        }

        /// <summary>
        /// Loads the specified FreeType2 font face.
        /// </summary>
        private static IntPtr LoadFontFace(Int32 sizeInPoints, UInt32 dpiX, UInt32 dpiY, IntPtr faceData, Int32 faceDataLength)
        {
            if (faceData == IntPtr.Zero)
                return IntPtr.Zero;

            var face = default(IntPtr);
            var err = default(FT_Error);

            if (Use64BitInterface)
            {
                err = FT_New_Memory_Face64(FreeTypeFontPlugin.Library, faceData, faceDataLength, 0, (IntPtr)(&face));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                err = FT_Set_Char_Size64(face, 0, FreeTypeCalc.Int32ToF26Dot6(sizeInPoints), dpiX, dpiY);
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);
            }
            else
            {
                err = FT_New_Memory_Face32(FreeTypeFontPlugin.Library, faceData, faceDataLength, 0, (IntPtr)(&face));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                err = FT_Set_Char_Size32(face, 0, FreeTypeCalc.Int32ToF26Dot6(sizeInPoints), dpiX, dpiY);
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);
            }

            return face;
        }

        /// <summary>
        /// Parses a value which starts or ends a prepopulated glyph range.
        /// </summary>
        private static Char ParsePrepopulatedGlyphRangeValue(String value)
        {
            if (value.Length == 1)
                return value[0];

            if (value.StartsWith("\\u", StringComparison.OrdinalIgnoreCase))
            {
                if (value.Length == 6)
                {
                    value = value.Substring(2);
                    return (Char)UInt32.Parse(value, NumberStyles.HexNumber);
                }
                else throw new FormatException();
            }
            else throw new FormatException();
        }

        /// <summary>
        /// Finds the best set of DPI values to use when loading the font for the specified density bucket.
        /// </summary>
        private static void GetBestScreenDensityMatch(UltravioletContext uv, ScreenDensityBucket bucket, out Single dpiX, out Single dpiY)
        {
            dpiX = 96f;
            dpiY = 96f;

            var bestMatchFound = false;
            var bestMatchScale = Single.MaxValue;

            foreach (var display in uv.GetPlatform().Displays)
            {
                if (display.DensityBucket == bucket)
                {
                    if (display.DensityScale < bestMatchScale)
                    {
                        bestMatchFound = true;
                        bestMatchScale = display.DensityScale;
                        dpiX = display.DpiX;
                        dpiY = display.DpiY;
                    }
                }
            }

            if (bestMatchFound)
                return;

            dpiX = dpiY = ScreenDensityService.GuessDensityFromBucket(bucket);
        }

        /// <summary>
        /// Populates the specified collection with the list of glyph ranges which should be prepopulated.
        /// </summary>
        private static void GetPrepopulatedGlyphList(String input, IList<PrepopulatedGlyphRange> ranges)
        {
            ranges.Clear();

            if (String.IsNullOrWhiteSpace(input))
                return;
            
            var splitRanges = input.Split(',');
            foreach (var splitRange in splitRanges)
            {
                var splitRangeParts = splitRange.Split('-');
                if (splitRangeParts.Length == 1)
                {
                    if (String.Equals("ASCII", splitRangeParts[0], StringComparison.OrdinalIgnoreCase))
                    {
                        ranges.Add(new PrepopulatedGlyphRange { Start = (Char)0, End = (Char)127 });
                    }
                    else
                    {
                        var value = ParsePrepopulatedGlyphRangeValue(splitRangeParts[0]);
                        ranges.Add(new PrepopulatedGlyphRange { Start = value, End = value });
                    }
                }
                else
                {
                    if (splitRangeParts.Length == 2)
                    {
                        var start = ParsePrepopulatedGlyphRangeValue(splitRangeParts[0]);
                        var end = ParsePrepopulatedGlyphRangeValue(splitRangeParts[1]);
                        if (end < start)
                        {
                            var tmp = end;
                            end = start;
                            start = tmp;
                        }
                        ranges.Add(new PrepopulatedGlyphRange { Start = start, End = end });
                    }
                    else throw new FormatException();
                }
            }
        }

        /// <summary>
        /// Prepopulates the specified face's atlases with the specified list of glyphs.
        /// </summary>
        private static void PrepopulateGlyphs(FreeTypeFontFace face, IList<PrepopulatedGlyphRange> ranges)
        {
            foreach (var range in ranges)
            {
                for (var g = range.Start; g <= range.End; g++)
                {
                    face.PopulateGlyph(g);
                }
            }
        }
    }
}

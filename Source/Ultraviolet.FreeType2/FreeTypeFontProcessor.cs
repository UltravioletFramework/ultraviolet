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

            var sizeInPoints = mdata.SizeInPoints;
            var sizeInPixels = mdata.SizeInPixels;
            if (sizeInPixels != 0 && sizeInPoints != 0)
                throw new InvalidOperationException(FreeTypeStrings.CannotSpecifyPointAndPixelSize);

            var iDpiX = (UInt32)dpiX;
            var iDpiY = (UInt32)dpiY;

            var pxSizeRegular = mdata.SizeInPixels;
            var ftFaceRegular = LoadFontFace(input.FaceDataRegular, input.FaceDataRegularLength, sizeInPoints, ref pxSizeRegular, iDpiX, iDpiY, !mdata.UseClosestPixelSize);

            var pxSizeBold = mdata.SizeInPixels;
            var ftFaceBold = LoadFontFace(input.FaceDataBold, input.FaceDataBoldLength, sizeInPoints, ref pxSizeBold, iDpiX, iDpiY, !mdata.UseClosestPixelSize);

            var pxSizeItalic = mdata.SizeInPixels;
            var ftFaceItalic = LoadFontFace(input.FaceDataItalic, input.FaceDataItalicLength, sizeInPoints, ref pxSizeItalic, iDpiX, iDpiY, !mdata.UseClosestPixelSize);

            var pxSizeBoldItalic = mdata.SizeInPixels;
            var ftFaceBoldItalic = LoadFontFace(input.FaceDataBoldItalic, input.FaceDataBoldItalicLength, sizeInPoints, ref pxSizeBoldItalic, iDpiX, iDpiY, !mdata.UseClosestPixelSize);

            var uvFaceRegular = (ftFaceRegular == IntPtr.Zero) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceRegular, mdata);
            var uvFaceBold = (ftFaceBold == IntPtr.Zero) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceBold, mdata);
            var uvFaceItalic = (ftFaceItalic == IntPtr.Zero) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceItalic, mdata);
            var uvFaceBoldItalic = (ftFaceBoldItalic == IntPtr.Zero) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceBoldItalic, mdata);

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
        private static IntPtr LoadFontFace(IntPtr faceData, Int32 faceDataLength, Int32 sizeInPoints, ref Int32 sizeInPixels, UInt32 dpiX, UInt32 dpiY, Boolean exactPixelSizeMatching)
        {
            if (faceData == IntPtr.Zero)
                return IntPtr.Zero;

            var face = default(IntPtr);
            var facade = default(FT_FaceRecFacade);

            if (Use64BitInterface)
            {
                var err = FT_New_Memory_Face64(FreeTypeFontPlugin.Library, faceData, faceDataLength, 0, (IntPtr)(&face));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                facade = new FT_FaceRecFacade(face);
            }
            else
            {
                var err = FT_New_Memory_Face32(FreeTypeFontPlugin.Library, faceData, faceDataLength, 0, (IntPtr)(&face));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                facade = new FT_FaceRecFacade(face);
            }

            if (sizeInPixels == 0)
            {
                // If no size was given, default to 16 points for scalable fonts.
                if (facade.HasScalableFlag && sizeInPoints == 0)
                    sizeInPoints = 16;

                // If we're loading a bitmap font but haven't specified any sizes, use the smallest one available.
                // Otherwise, load scalable fonts according to the specified point size.
                if (sizeInPoints == 0)
                {
                    var nearestMatchIx = facade.FindNearestMatchingPixelSize(0, false);
                    facade.SelectFixedSize(nearestMatchIx);
                }
                else
                { 
                    if (!facade.HasScalableFlag)
                        throw new InvalidOperationException(FreeTypeStrings.NonScalableFontFaceRequiresPixelSize);

                    facade.SelectCharSize(sizeInPoints, dpiX, dpiY);
                }
            }
            else
            {
                var nearestMatchIx = facade.FindNearestMatchingPixelSize(sizeInPixels, exactPixelSizeMatching);
                facade.SelectFixedSize(nearestMatchIx);
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

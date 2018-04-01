using System;
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
    public unsafe class FreeTypeFontProcessor : ContentProcessor<FreeTypeFontInfo, FreeTypeFont>
    {
        /// <inheritdoc/>
        public override FreeTypeFont Process(ContentManager manager, IContentProcessorMetadata metadata, FreeTypeFontInfo input)
        {
            GetBestScreenDensityMatch(manager.Ultraviolet, metadata.AssetDensity, out var dpiX, out var dpiY);

            var sizeInPoints = (Int32)input.SizeInPoints;

            var ftFaceRegular = LoadFontFace(sizeInPoints, (UInt32)dpiX, (UInt32)dpiY, input.FaceDataRegular, input.FaceDataRegularLength);
            var ftFaceBold = LoadFontFace(sizeInPoints, (UInt32)dpiX, (UInt32)dpiY, input.FaceDataBold, input.FaceDataBoldLength);
            var ftFaceItalic = LoadFontFace(sizeInPoints, (UInt32)dpiX, (UInt32)dpiY, input.FaceDataItalic, input.FaceDataItalicLength);
            var ftFaceBoldItalic = LoadFontFace(sizeInPoints, (UInt32)dpiX, (UInt32)dpiY, input.FaceDataBoldItalic, input.FaceDataBoldItalicLength);

            var uvFaceRegular = (ftFaceRegular == null) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceRegular, input.SizeInPoints);
            var uvFaceBold = (ftFaceBold == null) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceBold, input.SizeInPoints);
            var uvFaceItalic = (ftFaceItalic == null) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceItalic, input.SizeInPoints);
            var uvFaceBoldItalic = (ftFaceBoldItalic == null) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceBoldItalic, input.SizeInPoints);

            return new FreeTypeFont(manager.Ultraviolet, uvFaceRegular, uvFaceBold, uvFaceItalic, uvFaceBoldItalic);
        }

        /// <summary>
        /// Loads the specified FreeType2 font face.
        /// </summary>
        private static FT_FaceRec* LoadFontFace(Int32 sizeInPoints, UInt32 dpiX, UInt32 dpiY, IntPtr faceData, Int32 faceDataLength)
        {
            if(faceData == IntPtr.Zero)
                return null;

            var face = default(FT_FaceRec*);
            var err = default(FT_Error);

            err = FT_New_Memory_Face(FreeTypeFontPlugin.Library, faceData, faceDataLength, 0, &face);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            err = FT_Set_Char_Size(face, 0, FreeTypeCalc.Int32ToF26Dot6(sizeInPoints), dpiX, dpiY);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);

            return face;
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
    }
}

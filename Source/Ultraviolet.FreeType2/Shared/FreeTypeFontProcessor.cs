using System;
using Ultraviolet.Platform;
using Ultraviolet.Content;
using Ultraviolet.FreeType2.Native;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Err;

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

            var ftFaceRegular = LoadFontFace(input.SizeInPoints, dpiX, dpiY, input.FaceDataRegular);
            var ftFaceBold = LoadFontFace(input.SizeInPoints, dpiX, dpiY, input.FaceDataBold);
            var ftFaceItalic = LoadFontFace(input.SizeInPoints, dpiX, dpiY, input.FaceDataItalic);
            var ftFaceBoldItalic = LoadFontFace(input.SizeInPoints, dpiX, dpiY, input.FaceDataBoldItalic);

            var uvFaceRegular = (ftFaceRegular == null) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceRegular);
            var uvFaceBold = (ftFaceBold == null) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceBold);
            var uvFaceItalic = (ftFaceItalic == null) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceItalic);
            var uvFaceBoldItalic = (ftFaceBoldItalic == null) ? null : new FreeTypeFontFace(manager.Ultraviolet, ftFaceBoldItalic);

            return new FreeTypeFont(manager.Ultraviolet, uvFaceRegular, uvFaceBold, uvFaceItalic, uvFaceBoldItalic);
        }

        /// <summary>
        /// Loads the specified FreeType2 font face.
        /// </summary>
        private static FT_FaceRec_* LoadFontFace(Single sizeInPoints, Single dpiX, Single dpiY, Byte[] faceData)
        {
            if (faceData == null)
                return null;

            var face = default(FT_FaceRec_*);
            var err = default(FT_Err);

            fixed (Byte* file_base = faceData)
            {
                err = FT_New_Memory_Face(FreeTypeFontPlugin.Library, (IntPtr)file_base, faceData.LongLength, 0, &face);
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);
            }

            err = FT_Set_Char_Size(face, 0, (Int64)(sizeInPoints * 64), (UInt32)dpiX, (UInt32)dpiY);
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

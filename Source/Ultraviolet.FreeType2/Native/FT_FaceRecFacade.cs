using System;
using System.Runtime.InteropServices;
using static Ultraviolet.FreeType2.Native.FreeTypeNative;
using static Ultraviolet.FreeType2.Native.FT_Error;
using static Ultraviolet.FreeType2.Native.FT_Kerning_Mode;

namespace Ultraviolet.FreeType2.Native
{
    /// <summary>
    /// Represents an interface-agnostic facade over a FreeType2 face pointer.
    /// </summary>
    internal unsafe struct FT_FaceRecFacade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FT_FaceRecFacade"/> structure.
        /// </summary>
        /// <param name="face">A pointer to the wrapped FreeType2 face object.</param>
        public FT_FaceRecFacade(IntPtr face)
        {
            this.face = face;
        }

        /// <summary>
        /// Selects the specified character size for the font face.
        /// </summary>
        /// <param name="sizeInPoints">The size in points to select.</param>
        /// <param name="dpiX">The horizontal pixel density.</param>
        /// <param name="dpiY">The vertical pixel density.</param>
        public void SelectCharSize(Int32 sizeInPoints, UInt32 dpiX, UInt32 dpiY)
        {
            var err = Use64BitInterface ?
                FT_Set_Char_Size64(face, 0, FreeTypeCalc.Int32ToF26Dot6(sizeInPoints), dpiX, dpiY) :
                FT_Set_Char_Size32(face, 0, FreeTypeCalc.Int32ToF26Dot6(sizeInPoints), dpiX, dpiY);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);
        }

        /// <summary>
        /// Selects the specified fixed size for the font face.
        /// </summary>
        /// <param name="ix">The index of the fixed size to select.</param>
        public void SelectFixedSize(Int32 ix)
        {
            var err = FT_Select_Size(face, ix);
            if (err != FT_Err_Ok)
                throw new FreeTypeException(err);
        }

        /// <summary>
        /// Gets the glyph index of the specified character, if it is defined by this face.
        /// </summary>
        /// <param name="charcode">The character code for which to retrieve a glyph index.</param>
        /// <returns>The glyph index of the specified character, or 0 if the character is not defined by this face.</returns>
        public UInt32 GetCharIndex(UInt32 charcode) => Use64BitInterface ? FT_Get_Char_Index64(face, charcode) : FT_Get_Char_Index32(face, charcode);

        /// <summary>
        /// Marshals the face's family name to a C# string.
        /// </summary>
        /// <returns>The marshalled string.</returns>
        public String MarshalFamilyName() => Marshal.PtrToStringAnsi(Use64BitInterface ? ((FT_FaceRec64*)face)->family_name : ((FT_FaceRec32*)face)->family_name);

        /// <summary>
        /// Marshals the face's style name to a C# string.
        /// </summary>
        /// <returns>The marshalled string.</returns>
        public String MarshalStyleName() => Marshal.PtrToStringAnsi(Use64BitInterface ? ((FT_FaceRec64*)face)->style_name : ((FT_FaceRec32*)face)->style_name);

        /// <summary>
        /// Returns the specified character if it is defined by this face; otherwise, returns <see langword="null"/>.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns>The specified character, if it is defined by this face; otherwise, <see langword="null"/>.</returns>
        public Char? GetCharIfDefined(Char c) => (Use64BitInterface ? FT_Get_Char_Index64(face, c) : FT_Get_Char_Index32(face, c)) > 0 ? c : (Char?)null;

        /// <summary>
        /// Gets the kerning information for the specified glyph pair.
        /// </summary>
        /// <param name="ixLeft">The index of the left glyph.</param>
        /// <param name="ixRight">The index of the right glyph.</param>
        /// <returns>The kerning information for the specified glyph pair.</returns>
        public Size2 GetKerning(UInt32 ixLeft, UInt32 ixRight)
        {
            var kx = 0;
            var ky = 0;

            if (Use64BitInterface)
            {
                var kerning = default(FT_Vector64);
                var err = FT_Get_Kerning(face, ixLeft, ixRight, (UInt32)FT_KERNING_DEFAULT, (IntPtr)(&kerning));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                kx = (Int32)kerning.x;
                ky = (Int32)kerning.y;
            }
            else
            {
                var kerning = default(FT_Vector32);
                var err = FT_Get_Kerning(face, ixLeft, ixRight, (UInt32)FT_KERNING_DEFAULT, (IntPtr)(&kerning));
                if (err != FT_Err_Ok)
                    throw new FreeTypeException(err);

                kx = kerning.x;
                ky = kerning.y;
            }

            var x = FreeTypeCalc.F26Dot6ToInt32(kx);
            var y = FreeTypeCalc.F26Dot6ToInt32(ky);
            return new Size2(x, y);
        }

        /// <summary>
        /// Gets the size of the current glyph.
        /// </summary>
        /// <returns>The size of the current glyph.</returns>
        public Size2 GetGlyphSize()
        {
            return new Size2(GlyphMetricWidth, GlyphMetricHeight);
        }
        
        /// <summary>
        /// Returns the index of the fixed size which is the closest match to the specified pixel size.
        /// </summary>
        /// <param name="sizeInPixels">The desired size in pixels.</param>
        /// <param name="requireExactMatch">A value indicating whether to require an exact match.</param>
        /// <returns>The index of the closest matching fixed size.</returns>
        public Int32 FindNearestMatchingPixelSize(Int32 sizeInPixels, Boolean requireExactMatch = false)
        {
            var numFixedSizes = Use64BitInterface ? ((FT_FaceRec64*)face)->num_fixed_sizes : ((FT_FaceRec32*)face)->num_fixed_sizes;
            if (numFixedSizes == 0)
                throw new InvalidOperationException(FreeTypeStrings.FontDoesNotHaveBitmapStrikes);

            Int32 GetFixedSizeInPixels(IntPtr face, Int32 ix) =>
                Use64BitInterface ? ((FT_FaceRec64*)face)->available_sizes[ix].height : ((FT_FaceRec32*)face)->available_sizes[ix].height;

            var bestMatchIx = 0;
            var bestMatchDiff = Math.Abs(GetFixedSizeInPixels(face, 0) - sizeInPixels);

            for (int i = 0; i < numFixedSizes; i++)
            {
                var size = GetFixedSizeInPixels(face, i);
                var diff = Math.Abs(size - sizeInPixels);
                if (diff < bestMatchDiff)
                {
                    bestMatchDiff = diff;
                    bestMatchIx = i;
                }
            }

            if (bestMatchDiff != 0 && requireExactMatch)
                throw new InvalidOperationException(FreeTypeStrings.NoMatchingPixelSize.Format(sizeInPixels));

            return bestMatchIx;
        }

        /// <summary>
        /// Gets a value indicating whether the face has the specified flag defined.
        /// </summary>
        /// <param name="flag">The flag to evaluate.</param>
        /// <returns><see langword="true"/> if the face has the specified flag defined; otherwise, <see langword="false"/>.</returns>
        public Boolean HasFaceFlag(Int32 flag) => ((Use64BitInterface ? (Int32)((FT_FaceRec64*)face)->face_flags : ((FT_FaceRec32*)face)->face_flags) & flag) != 0;

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_SCALABLE flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_SCALABLE flag defined; otherwise, <see langword="false"/>.</returns>
        public Boolean HasScalableFlag => HasFaceFlag(FT_FACE_FLAG_SCALABLE);

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_FIXED_SIZES flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_FIXED_SIZES flag defined; otherwise, <see langword="false"/>.</returns>
        public Boolean HasFixedSizes => HasFaceFlag(FT_FACE_FLAG_FIXED_SIZES);

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_COLOR flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_COLOR flag defined; otherwise, <see langword="false"/>.</returns>
        public Boolean HasColorFlag => HasFaceFlag(FT_FACE_FLAG_COLOR);

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_KERNING flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_KERNING flag defined; otherwise, <see langword="false"/>.</returns>
        public Boolean HasKerningFlag => HasFaceFlag(FT_FACE_FLAG_KERNING);

        /// <summary>
        /// Gets a value indicating whether the face has any bitmap strikes with fixed sizes.
        /// </summary>
        public Boolean HasBitmapStrikes => (Use64BitInterface ? ((FT_FaceRec64*)face)->num_fixed_sizes : ((FT_FaceRec32*)face)->num_fixed_sizes) > 0;

        /// <summary>
        /// Gets the current glyph bitmap.
        /// </summary>
        public FT_Bitmap GlyphBitmap => Use64BitInterface ? ((FT_FaceRec64*)face)->glyph->bitmap : ((FT_FaceRec32*)face)->glyph->bitmap;

        /// <summary>
        /// Gets the left offset of the current glyph bitmap.
        /// </summary>
        public Int32 GlyphBitmapLeft => Use64BitInterface ? ((FT_FaceRec64*)face)->glyph->bitmap_left : ((FT_FaceRec32*)face)->glyph->bitmap_left;

        /// <summary>
        /// Gets the right offset of the current glyph bitmap.
        /// </summary>
        public Int32 GlyphBitmapTop => Use64BitInterface ? ((FT_FaceRec64*)face)->glyph->bitmap_top : ((FT_FaceRec32*)face)->glyph->bitmap_top;

        /// <summary>
        /// Gets the width in pixels of the current glyph.
        /// </summary>
        public Int32 GlyphMetricWidth => FreeTypeCalc.F26Dot6ToInt32(Use64BitInterface ? ((FT_FaceRec64*)face)->glyph->metrics.width : ((FT_FaceRec32*)face)->glyph->metrics.width);

        /// <summary>
        /// Gets the height in pixels of the current glyph.
        /// </summary>
        public Int32 GlyphMetricHeight => FreeTypeCalc.F26Dot6ToInt32(Use64BitInterface ? ((FT_FaceRec64*)face)->glyph->metrics.height : ((FT_FaceRec32*)face)->glyph->metrics.height);

        /// <summary>
        /// Gets the horizontal advance of the current glyph.
        /// </summary>
        public Int32 GlyphMetricHorizontalAdvance => FreeTypeCalc.F26Dot6ToInt32(Use64BitInterface ? ((FT_FaceRec64*)face)->glyph->metrics.horiAdvance : ((FT_FaceRec32*)face)->glyph->metrics.horiAdvance);

        /// <summary>
        /// Gets the vertical advance of the current glyph.
        /// </summary>
        public Int32 GlyphMetricVerticalAdvance => FreeTypeCalc.F26Dot6ToInt32(Use64BitInterface ? ((FT_FaceRec64*)face)->glyph->metrics.vertAdvance : ((FT_FaceRec32*)face)->glyph->metrics.vertAdvance);

        /// <summary>
        /// Gets the face's ascender size in pixels.
        /// </summary>
        public Int32 Ascender => FreeTypeCalc.F26Dot6ToInt32(Use64BitInterface ? ((FT_FaceRec64*)face)->size->metrics.ascender : ((FT_FaceRec32*)face)->size->metrics.ascender);

        /// <summary>
        /// Gets the face's descender size in pixels.
        /// </summary>
        public Int32 Descender => FreeTypeCalc.F26Dot6ToInt32(Use64BitInterface ? ((FT_FaceRec64*)face)->size->metrics.descender : ((FT_FaceRec32*)face)->size->metrics.descender);

        /// <summary>
        /// Gets the face's line spacing in pixels.
        /// </summary>
        public Int32 LineSpacing => FreeTypeCalc.F26Dot6ToInt32(Use64BitInterface ? ((FT_FaceRec64*)face)->size->metrics.height : ((FT_FaceRec32*)face)->size->metrics.height);

        /// <summary>
        /// Gets a pointer to the face's glyph slot.
        /// </summary>
        public IntPtr GlyphSlot => Use64BitInterface ? (IntPtr)((FT_FaceRec64*)face)->glyph : (IntPtr)((FT_FaceRec32*)face)->glyph;

        // A pointer to the wrapped FreeType2 face object.
        private readonly IntPtr face;
    }
}

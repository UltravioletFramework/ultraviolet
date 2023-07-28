using System;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the information required to load a FreeType font.
    /// </summary>
    public class FreeTypeFontInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeFontInfo"/> class.
        /// </summary>
        /// <param name="faceDataRegular">A pointer to the native memory buffer which contains the regular face data.</param>
        /// <param name="faceDataRegularLength">The length, in bytes, of the memory buffer pointed to by <paramref name="faceDataRegular"/>.</param>
        /// <param name="faceDataBold">A pointer to the native memory buffer which contains the bold face data.</param>
        /// <param name="faceDataBoldLength">The length, in bytes, of the memory buffer pointed to by <paramref name="faceDataBold"/>.</param>
        /// <param name="faceDataItalic">A pointer to the native memory buffer which contains the italic face data.</param>
        /// <param name="faceDataItalicLength">The length, in bytes, of the memory buffer pointed to by <paramref name="faceDataItalic"/>.</param>
        /// <param name="faceDataBoldItalic">A pointer to the native memory buffer which contains the bold italic face data.</param>
        /// <param name="faceDataBoldItalicLength">The length, in bytes, of the memory buffer pointed to by <paramref name="faceDataBoldItalic"/>.</param>
        public FreeTypeFontInfo(
            IntPtr faceDataRegular, Int32 faceDataRegularLength, 
            IntPtr faceDataBold, Int32 faceDataBoldLength,
            IntPtr faceDataItalic, Int32 faceDataItalicLength,
            IntPtr faceDataBoldItalic, Int32 faceDataBoldItalicLength)
        {
            this.FaceDataRegular = faceDataRegular;
            this.FaceDataRegularLength = faceDataRegularLength;
            this.FaceDataBold = faceDataBold;
            this.FaceDataBoldLength = faceDataBoldLength;
            this.FaceDataItalic = faceDataItalic;
            this.FaceDataItalicLength = faceDataItalicLength;
            this.FaceDataBoldItalic = faceDataBoldItalic;
            this.FaceDataBoldItalicLength = faceDataBoldItalicLength;
        }
        
        /// <summary>
        /// Gets a pointer to the the raw data for the font's regular face.
        /// </summary>
        public IntPtr FaceDataRegular { get; }

        /// <summary>
        /// Gets the length, in bytes, of the raw data for the font's regular face.
        /// </summary>
        public Int32 FaceDataRegularLength { get; }

        /// <summary>
        /// Gets a pointer to the raw data for the font's bold face.
        /// </summary>
        public IntPtr FaceDataBold { get; }

        /// <summary>
        /// Gets the length, in bytes, of the raw data for the font's bold face.
        /// </summary>
        public Int32 FaceDataBoldLength { get; }

        /// <summary>
        /// Gets a pointer to the raw data for the font's italic face.
        /// </summary>
        public IntPtr FaceDataItalic { get; }

        /// <summary>
        /// Gets the length, in bytes, of the raw data for the font's italic face.
        /// </summary>
        public Int32 FaceDataItalicLength { get; }

        /// <summary>
        /// Gets a pointer to the raw data for the font's bold italic face.
        /// </summary>
        public IntPtr FaceDataBoldItalic { get; }

        /// <summary>
        /// Gets the length, in bytes, of the raw data for the font's bold italic face.
        /// </summary>
        public Int32 FaceDataBoldItalicLength { get; }
    }
}

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
        /// <param name="fontPath">The path to the font to load.</param>
        /// <param name="fontType">The type of font represented by the specified path.</param>
        /// <param name="fontData">The font data loaded from the file.</param>
        public FreeTypeFontInfo(Single sizeInPoints, Byte[] faceDataRegular, Byte[] faceDataBold, Byte[] faceDataItalic, Byte[] faceDataBoldItalic)
        {
            this.SizeInPoints = sizeInPoints;
            this.FaceDataRegular = faceDataRegular;
            this.FaceDataBold = faceDataBold;
            this.FaceDataItalic = faceDataItalic;
            this.FaceDataBoldItalic = faceDataBoldItalic;
        }

        /// <summary>
        /// Gets the size of the font in points.
        /// </summary>
        public Single SizeInPoints { get; }

        /// <summary>
        /// Gets the raw data for the font's regular face.
        /// </summary>
        public Byte[] FaceDataRegular { get; }

        /// <summary>
        /// Gets the raw data for the font's bold face.
        /// </summary>
        public Byte[] FaceDataBold { get; }

        /// <summary>
        /// Gets the raw data for the font's italic face.
        /// </summary>
        public Byte[] FaceDataItalic { get; }

        /// <summary>
        /// Gets the raw data for the font's bold italic face.
        /// </summary>
        public Byte[] FaceDataBoldItalic { get; }
    }
}

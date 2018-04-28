using System;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the asset metadata for the <see cref="FreeTypeFontImporter"/> class.
    /// </summary>
    public sealed class FreeTypeFontImporterMetadata
    {
        /// <summary>
        /// Gets the name of the file that contains the font's bold face.
        /// </summary>
        public String BoldFace { get; private set; }

        /// <summary>
        /// Gets the name of the file that contains the font's italic face.
        /// </summary>
        public String ItalicFace { get; private set; }

        /// <summary>
        /// Gets the name of the file that contains the font's bold italic face.
        /// </summary>
        public String BoldItalicFace { get; private set; }
    }
}

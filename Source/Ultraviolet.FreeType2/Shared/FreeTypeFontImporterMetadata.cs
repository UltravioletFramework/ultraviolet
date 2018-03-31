using System;
using Newtonsoft.Json;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the asset metadata for the <see cref="FreeTypeFontImporter"/> class.
    /// </summary>
    public sealed class FreeTypeFontImporterMetadata
    {
        /// <summary>
        /// Gets the size of the font in points.
        /// </summary>
        [JsonProperty(PropertyName = "sizeInPoints")]
        public Single SizeInPoints { get; private set; } = 16f;

        /// <summary>
        /// Gets the name of the file that contains the font's bold face.
        /// </summary>
        [JsonProperty(PropertyName = "boldFace")]
        public String BoldFace { get; private set; }

        /// <summary>
        /// Gets the name of the file that contains the font's italic face.
        /// </summary>
        [JsonProperty(PropertyName = "italicFace")]
        public String ItalicFace { get; private set; }

        /// <summary>
        /// Gets the name of the file that contains the font's bold italic face.
        /// </summary>
        [JsonProperty(PropertyName = "boldItalicFace")]
        public String BoldItalicFace { get; private set; }
    }
}

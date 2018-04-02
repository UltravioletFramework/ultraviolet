using System;
using Newtonsoft.Json;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents the asset metadata for the <see cref="FreeTypeFontProcessor"/> class.
    /// </summary>
    public sealed class FreeTypeFontProcessorMetadata
    {
        /// <summary>
        /// Gets a string representing the list of glyphs which should be prepopulated on the font's texture atlas.
        /// </summary>
        [JsonProperty(PropertyName = "prepopulatedGlyphs")]
        public String PrepopulatedGlyphs { get; private set; } = "ASCII";
    }
}

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
        /// Gets the size of the font in points.
        /// </summary>
        [JsonProperty(PropertyName = "sizeInPoints")]
        public Single SizeInPoints { get; private set; } = 16f;

        /// <summary>
        /// Gets the font face's substitution character.
        /// </summary>
        [JsonProperty(PropertyName = "substitution")]
        public Char? Substitution { get; private set; } = null;

        /// <summary>
        /// Gets a string representing the list of glyphs which should be prepopulated on the font's texture atlas.
        /// </summary>
        [JsonProperty(PropertyName = "prepopulatedGlyphs")]
        public String PrepopulatedGlyphs { get; private set; } = "ASCII";
    }
}

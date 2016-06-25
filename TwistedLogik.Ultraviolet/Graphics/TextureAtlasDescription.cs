using System.Collections.Generic;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// An intermediate representation of a <see cref="TextureAtlas"/> used during content processing.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal class TextureAtlasDescription
    {
        /// <summary>
        /// Gets or sets the atlas' metadata.
        /// </summary>
        [JsonProperty(PropertyName = "metadata", Required = Required.Default)]
        public TextureAtlasMetadataDescription Metadata { get; set; }

        /// <summary>
        /// Gets or sets the atlas' image collection.
        /// </summary>
        [JsonProperty(PropertyName = "images", Required = Required.Default)]
        public IEnumerable<TextureAtlasImageDescription> Images { get; set; }
    }
}

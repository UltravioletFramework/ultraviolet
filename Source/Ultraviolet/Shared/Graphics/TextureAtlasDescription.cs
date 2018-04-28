using System.Collections.Generic;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// An intermediate representation of a <see cref="TextureAtlas"/> used during content processing.
    /// </summary>
    internal class TextureAtlasDescription
    {
        /// <summary>
        /// Gets or sets the atlas' metadata.
        /// </summary>
        public TextureAtlasMetadataDescription Metadata { get; set; }

        /// <summary>
        /// Gets or sets the atlas' image collection.
        /// </summary>
        public IEnumerable<TextureAtlasImageDescription> Images { get; set; }
    }
}

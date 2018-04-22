using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
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
        public TextureAtlasMetadataDescription Metadata { get; set; }

        /// <summary>
        /// Gets or sets the atlas' image collection.
        /// </summary>
        public IEnumerable<TextureAtlasImageDescription> Images { get; set; }
    }
}

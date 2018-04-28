using System;
using Newtonsoft.Json;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// An intermediate representation of an image in a <see cref="TextureAtlas"/> which is used during content processing.
    /// </summary>
    internal class TextureAtlasImageDescription
    {
        /// <summary>
        /// Gets or sets the image's name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the image's path.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Path { get; set; }
    }
}

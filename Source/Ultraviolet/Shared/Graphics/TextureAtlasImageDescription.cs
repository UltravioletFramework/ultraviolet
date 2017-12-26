using System;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// An intermediate representation of an image in a <see cref="TextureAtlas"/> which is used during content processing.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal class TextureAtlasImageDescription
    {
        /// <summary>
        /// Gets or sets the image's name.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Default)]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the image's path.
        /// </summary>
        [JsonProperty(PropertyName = "path", Required = Required.Always)]
        public String Path { get; set; }
    }
}

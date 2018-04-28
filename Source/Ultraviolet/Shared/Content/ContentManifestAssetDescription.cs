using System;
using Newtonsoft.Json;

namespace Ultraviolet.Content
{
    /// <summary>
    /// An intermediate representation of a <see cref="ContentManifestAsset"/> used during loading.
    /// </summary>
    internal class ContentManifestAssetDescription
    {
        /// <summary>
        /// Gets or sets the name of the manifest asset.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the path of the manifest asset.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Path { get; set; }
    }
}

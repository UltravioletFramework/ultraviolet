using System;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// An intermediate representation of a <see cref="ContentManifestAsset"/> used during loading.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal class ContentManifestAssetDescription
    {
        /// <summary>
        /// Gets or sets the name of the manifest asset.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the path of the manifest asset.
        /// </summary>
        [JsonProperty(PropertyName = "path", Required = Required.Always)]
        public String Path { get; set; }
    }
}

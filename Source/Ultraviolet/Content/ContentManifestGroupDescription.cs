using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Content
{
    /// <summary>
    /// An intermediate representation of a <see cref="ContentManifestGroup"/> used during loading.
    /// </summary>
    internal class ContentManifestGroupDescription
    {
        /// <summary>
        /// Gets or sets the name of the manifest group.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the full name of the type of asset which is contained by the manifest group.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Type { get; set; }

        /// <summary>
        /// Gets or sets the directory which contains the manifest group's assets.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Directory { get; set; }

        /// <summary>
        /// Gets or sets the manifest group's assets.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public IEnumerable<ContentManifestAssetDescription> Assets { get; set; }
    }
}

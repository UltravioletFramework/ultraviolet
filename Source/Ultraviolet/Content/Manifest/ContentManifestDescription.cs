using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Content
{
    /// <summary>
    /// An intermediate representation of a <see cref="ContentManifest"/> used during loading.
    /// </summary>
    internal class ContentManifestDescription
    {
        /// <summary>
        /// Gets or sets the name of the content manifest.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the content manifest's groups.
        /// </summary>
        [JsonProperty(Required = Required.Default)]
        public IEnumerable<ContentManifestGroupDescription> Groups { get; set; }
    }
}

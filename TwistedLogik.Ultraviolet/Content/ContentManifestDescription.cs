using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// An intermediate representation of a <see cref="ContentManifest"/> used during loading.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal class ContentManifestDescription
    {
        /// <summary>
        /// Gets or sets the name of the content manifest.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the content manifest's groups.
        /// </summary>
        [JsonProperty(PropertyName = "groups", Required = Required.Default)]
        public IEnumerable<ContentManifestGroupDescription> Groups { get; set; }
    }
}

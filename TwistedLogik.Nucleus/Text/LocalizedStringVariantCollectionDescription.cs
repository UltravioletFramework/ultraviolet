using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="LocalizedStringVariantCollection"/> used during serialization.
    /// </summary>
    [Preserve(AllMembers = true)]
    [JsonConverter(typeof(LocalizedStringVariantCollectionJsonConverter))]
    internal sealed class LocalizedStringVariantCollectionDescription
    {
        /// <summary>
        /// Gets or sets the collection's comma-delimited list of properties.
        /// </summary>
        [JsonProperty(PropertyName = "properties")]
        public String Properties { get; set; }

        /// <summary>
        /// Gets or sets the collection's list of variants.
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public IEnumerable<LocalizedStringVariantDescription> Items { get; set; }
    }
}

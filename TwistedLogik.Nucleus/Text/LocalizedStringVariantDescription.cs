using System;
using Newtonsoft.Json;

namespace TwistedLogik.Nucleus.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="LocalizedStringVariant"/> used during serialization.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class LocalizedStringVariantDescription
    {
        /// <summary>
        /// Gets or sets the name of the variant's group.
        /// </summary>
        [JsonProperty(PropertyName = "group")]
        public String Group { get; set; }

        /// <summary>
        /// Gets or sets the variant's comma-delimited list of properties.
        /// </summary>
        [JsonProperty(PropertyName = "properties")]
        public String Properties { get; set; }

        /// <summary>
        /// Gets or sets the variant's text.
        /// </summary>
        [JsonProperty(PropertyName = "text", Required = Required.Always)]
        public String Text { get; set; }
    }
}

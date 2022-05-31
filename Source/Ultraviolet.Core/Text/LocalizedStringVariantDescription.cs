using System;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="LocalizedStringVariant"/> used during serialization.
    /// </summary>
    internal sealed class LocalizedStringVariantDescription
    {
        /// <summary>
        /// Gets or sets the name of the variant's group.
        /// </summary>
        public String Group { get; set; }

        /// <summary>
        /// Gets or sets the variant's comma-delimited list of properties.
        /// </summary>
        public String Properties { get; set; }

        /// <summary>
        /// Gets or sets the variant's text.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Text { get; set; }
    }
}

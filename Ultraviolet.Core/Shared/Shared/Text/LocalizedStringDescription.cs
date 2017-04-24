using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="LocalizedStringDescription"/> used during serialization.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class LocalizedStringDescription
    {
        /// <summary>
        /// Gets or sets the localized string's identifying key.
        /// </summary>
        [JsonProperty(PropertyName = "key", Required = Required.Always)]
        public String Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the string contains HTML.
        /// </summary>
        [JsonProperty(PropertyName = "html", Required = Required.DisallowNull)]
        public Boolean Html { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the string should be pseudolocalized.
        /// </summary>
        [JsonProperty(PropertyName = "pseudo", Required = Required.DisallowNull)]
        public Boolean Pseudo { get; set; }

        /// <summary>
        /// Gets or sets the string's collection of variants.
        /// </summary>
        [JsonProperty(PropertyName = "variants", Required = Required.Always)]
        public IDictionary<String, LocalizedStringVariantCollectionDescription> Variants { get; set; }
    }
}

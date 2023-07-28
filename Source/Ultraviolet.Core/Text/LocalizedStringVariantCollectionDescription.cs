using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="LocalizedStringVariantCollection"/> used during serialization.
    /// </summary>
    [JsonConverter(typeof(LocalizedStringVariantCollectionJsonConverter))]
    internal sealed class LocalizedStringVariantCollectionDescription
    {
        /// <summary>
        /// Gets or sets the collection's comma-delimited list of properties.
        /// </summary>
        public String Properties { get; set; }

        /// <summary>
        /// Gets or sets the collection's list of variants.
        /// </summary>
        public IEnumerable<LocalizedStringVariantDescription> Items { get; set; }
    }
}

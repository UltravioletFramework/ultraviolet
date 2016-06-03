using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// An intermediate representation of a <see cref="DataObjectRegistry{T}"/> used during serialization.
    /// </summary>
    internal sealed class DataObjectRegistryKeysDescription
    {
        /// <summary>
        /// Gets or sets the registry's collection of items.
        /// </summary>
        [JsonProperty(PropertyName = "items", Required = Required.Always)]
        public IEnumerable<DataObjectKeysDescription> Items { get; set; }
    }
}

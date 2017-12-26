using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Data
{
    /// <summary>
    /// An intermediate representation of a <see cref="DataObjectRegistry{T}"/> used during serialization.
    /// </summary>
    internal sealed class DataObjectRegistryDescription<T> where T : DataObject
    {
        /// <summary>
        /// Gets or sets the registry's collection of items.
        /// </summary>
        [JsonProperty(PropertyName = "items", Required = Required.Always)]
        public IEnumerable<T> Items { get; set; }
    }
}

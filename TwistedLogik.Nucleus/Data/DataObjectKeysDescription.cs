using System;
using Newtonsoft.Json;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// An intermediate representation of a <see cref="DataObject"/> used during serialization.
    /// </summary>
    internal sealed class DataObjectKeysDescription
    {
        /// <summary>
        /// Gets or sets the object's identifying key.
        /// </summary>
        [JsonProperty(PropertyName = "key", Required = Required.Always)]
        public String Key { get; set; }

        /// <summary>
        /// Gets or sets the object's globally-unique identifier.
        /// </summary>
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid ID { get; set; }
    }
}

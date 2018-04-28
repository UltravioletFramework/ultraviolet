using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="LocalizationDatabase"/> used during serialization.
    /// </summary>
    internal sealed class LocalizationDatabaseDescription
    {
        /// <summary>
        /// Gets or sets the database's collection of localizable strings.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<LocalizedStringDescription> Strings { get; set; }
    }
}

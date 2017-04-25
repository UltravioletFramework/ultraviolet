using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Core.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="LocalizationDatabase"/> used during serialization.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class LocalizationDatabaseDescription
    {
        /// <summary>
        /// Gets or sets the database's collection of localizable strings.
        /// </summary>
        [JsonProperty(PropertyName = "strings", Required = Required.Always)]
        public IEnumerable<LocalizedStringDescription> Strings { get; set; }
    }
}

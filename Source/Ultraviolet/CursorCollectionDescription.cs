using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// An intermediate representation of a <see cref="CursorCollection"/> used during content processing.
    /// </summary>
    internal class CursorCollectionDescription
    {
        /// <summary>
        /// Gets or sets the asset path of the cursor's texture.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Texture { get; set; }

        /// <summary>
        /// Gets or sets the collection's cursors.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<CursorDescription> Cursors { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="TextTableLayout"/> used during content loading.
    /// </summary>
    internal class TextTableLayoutDescription
    {
        /// <summary>
        /// Gets or sets the table's width in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "width", Required = Required.Default)]
        public Int32? Width { get; set; }

        /// <summary>
        /// Gets or sets the table's height in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "height", Required = Required.Default)]
        public Int32? Height { get; set; }

        /// <summary>
        /// Gets or sets the table's collection of rows.
        /// </summary>
        [JsonProperty(PropertyName = "rows", Required = Required.Default)]
        public IEnumerable<TextTableLayoutRowDescription> Rows { get; set; }
    }
}

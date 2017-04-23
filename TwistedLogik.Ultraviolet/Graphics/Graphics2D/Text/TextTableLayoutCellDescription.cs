using System;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// An intermediate representation of a cell in a <see cref="TextTableLayout"/> used during content loading.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal class TextTableLayoutCellDescription
    {
        /// <summary>
        /// Gets or sets the set of <see cref="TextFlags"/> values used to draw the cell's text.
        /// </summary>
        [JsonProperty(PropertyName = "textFlags", Required = Required.DisallowNull)]
        [JsonConverter(typeof(CoreEnumJsonConverter))]
        public TextFlags TextFlags { get; set; }

        /// <summary>
        /// Gets or sets the cell's text.
        /// </summary>
        [JsonProperty(PropertyName = "text", Required = Required.Default)]
        public String Text { get; set; }

        /// <summary>
        /// Gets or sets the cell's format string.
        /// </summary>
        [JsonProperty(PropertyName = "format", Required = Required.Default)]
        public String Format { get; set; }

        /// <summary>
        /// Gets or sets the cell's view model binding string.
        /// </summary>
        [JsonProperty(PropertyName = "binding", Required = Required.Default)]
        public String Binding { get; set; }

        /// <summary>
        /// Gets or sets the cell's width in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "width", Required = Required.Default)]
        public Int32? Width { get; set; }

        /// <summary>
        /// Gets or sets the cell's height in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "height", Required = Required.Default)]
        public Int32? Height { get; set; }
    }
}

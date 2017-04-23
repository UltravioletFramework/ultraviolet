using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// An intermediate representation of a row in a <see cref="TextTableLayout"/> used during content loading.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal class TextTableLayoutRowDescription
    {
        /// <summary>
        /// Gets or sets the row's collection of cells.
        /// </summary>
        [JsonProperty(PropertyName = "cells", Required = Required.Default)]
        public IEnumerable<TextTableLayoutCellDescription> Cells { get; set; }
    }
}

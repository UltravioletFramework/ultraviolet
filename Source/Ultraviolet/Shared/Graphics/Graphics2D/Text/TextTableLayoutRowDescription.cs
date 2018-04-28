using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// An intermediate representation of a row in a <see cref="TextTableLayout"/> used during content loading.
    /// </summary>
    internal class TextTableLayoutRowDescription
    {
        /// <summary>
        /// Gets or sets the row's collection of cells.
        /// </summary>
        public IEnumerable<TextTableLayoutCellDescription> Cells { get; set; }
    }
}

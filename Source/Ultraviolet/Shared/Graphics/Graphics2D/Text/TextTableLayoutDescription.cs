using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// An intermediate representation of a <see cref="TextTableLayout"/> used during content loading.
    /// </summary>
    internal class TextTableLayoutDescription
    {
        /// <summary>
        /// Gets or sets the table's width in pixels.
        /// </summary>
        public Int32? Width { get; set; }

        /// <summary>
        /// Gets or sets the table's height in pixels.
        /// </summary>
        public Int32? Height { get; set; }

        /// <summary>
        /// Gets or sets the table's collection of rows.
        /// </summary>
        public IEnumerable<TextTableLayoutRowDescription> Rows { get; set; }
    }
}

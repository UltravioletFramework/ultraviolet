using System;
using Newtonsoft.Json;

namespace Ultraviolet
{
    /// <summary>
    /// An intermediate representation of <see cref="Cursor"/> used during content processing.
    /// </summary>
    internal class CursorDescription
    {
        /// <summary>
        /// Gets or sets the cursor's name.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the cursor's position and size on the texture.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Rectangle Area { get; set; }

        /// <summary>
        /// Gets or sets the cursor's hotspot point.
        /// </summary>
        [JsonProperty(Required = Required.DisallowNull)]
        public Point2 Hotspot { get; set; }
    }
}

using System;
using Newtonsoft.Json;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a <see cref="CharacterRegion"/> used during content processing.
    /// </summary>
    internal sealed class CharacterRegionDescription
    {
        /// <summary>
        /// Gets or sets the first character in the region.
        /// </summary>
        [JsonProperty(PropertyName = "start")]
        public Char Start { get; set; }

        /// <summary>
        /// Gets or sets the last character in the region.
        /// </summary>
        [JsonProperty(PropertyName = "end")]
        public Char End { get; set; }
    }
}

using System;
using Newtonsoft.Json;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An intermediate representation of the default values used for sprite frames.
    /// </summary>
    internal sealed class SpriteFrameDefaultsDescription
    {
        /// <summary>
        /// Gets or sets the asset path of the frame's atlas.
        /// </summary>
        [JsonProperty(PropertyName = "atlas")]
        public String Atlas { get; set; }

        /// <summary>
        /// Gets or sets the name of the frame's atlas cell.
        /// </summary>
        [JsonProperty(PropertyName = "atlasCell")]
        public String AtlasCell { get; set; }

        /// <summary>
        /// Gets or sets the asset path of the frame's texture.
        /// </summary>
        [JsonProperty(PropertyName = "textures")]
        public String Texture { get; set; }
        
        /// <summary>
        /// Gets or sets the frame's area on its texture.
        /// </summary>
        [JsonProperty(PropertyName = "area")]
        public Rectangle? Area { get; set; }

        /// <summary>
        /// Gets or sets the frame's point of origin.
        /// </summary>
        [JsonProperty(PropertyName = "origin")]
        public Point2? Origin { get; set; }

        /// <summary>
        /// Gets or sets the frame's duration in milliseconds.
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public Int32? Duration { get; set; }
    }
}

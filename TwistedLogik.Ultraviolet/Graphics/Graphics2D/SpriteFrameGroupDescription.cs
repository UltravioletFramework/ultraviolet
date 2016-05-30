using System;
using Newtonsoft.Json;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a collectively-defined group of <see cref="SpriteFrame"/> objects during deserialization.
    /// </summary>
    internal sealed class SpriteFrameGroupDescription
    {
        /// <summary>
        /// Gets or sets the name of the texture on which the group's frames are defined.
        /// </summary>
        [JsonProperty(PropertyName = "texture", Required = Required.DisallowNull)]
        public String Texture { get; set; }

        /// <summary>
        /// Gets or sets the frame group's area on its texture.
        /// </summary>
        [JsonProperty(PropertyName = "area", Required = Required.DisallowNull)]
        public Rectangle? Area { get; set; }

        /// <summary>
        /// Gets or sets the point of origin for the frame group's frames.
        /// </summary>
        [JsonProperty(PropertyName = "origin", Required = Required.DisallowNull)]
        public Point2? Origin { get; set; }

        /// <summary>
        /// Gets or sets the number of frames in this group.
        /// </summary>
        [JsonProperty(PropertyName = "frameCount", Required = Required.DisallowNull)]
        public Int32? FrameCount { get; set; }

        /// <summary>
        /// Gets or sets the width of frames in this group.
        /// </summary>
        [JsonProperty(PropertyName = "frameWidth", Required = Required.DisallowNull)]
        public Int32? FrameWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of frames in this group.
        /// </summary>
        [JsonProperty(PropertyName = "frameHeight", Required = Required.DisallowNull)]
        public Int32? FrameHeight { get; set; }

        /// <summary>
        /// Gets or sets duration of frames in this group.
        /// </summary>
        [JsonProperty(PropertyName = "duration", Required = Required.DisallowNull)]
        public Int32? Duration { get; set; }
    }
}

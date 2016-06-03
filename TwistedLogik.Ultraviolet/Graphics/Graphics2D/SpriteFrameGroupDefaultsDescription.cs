using System;
using Newtonsoft.Json;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An intermediate representation of the default values used for sprite frame groups.
    /// </summary>
    internal sealed class SpriteFrameGroupDefaultsDescription
    {
        /// <summary>
        /// Gets or sets the asset path of the frame group's texture.
        /// </summary>
        [JsonProperty(PropertyName = "texture")]
        public String Texture { get; set; }
        
        /// <summary>
        /// Gets or sets the duration of the frames in the group in milliseconds.
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public Int32? Duration { get; set; }

        /// <summary>
        /// Gets or sets the distance between the left edge of the frame's texture
        /// and the left edge of the frame group, in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "x")]
        public Int32? X { get; set; }

        /// <summary>
        /// Gets or sets the distance between the top edge of the frame's texture
        /// and the top edge of the frame group, in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "y")]
        public Int32? Y { get; set; }

        /// <summary>
        /// Gets or sets the frame group's width in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "width")]
        public Int32? Width { get; set; }

        /// <summary>
        /// Gets or sets the frame group's height in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "height")]
        public Int32? Height { get; set; }

        /// <summary>
        /// Gets or sets the number of frames in the group.
        /// </summary>
        [JsonProperty(PropertyName = "frameCount")]
        public Int32? FrameCount { get; set; }

        /// <summary>
        /// Gets or sets the width of the frames in the group in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "frameWidth")]
        public Int32? FrameWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of the frame in the group in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "frameHeight")]
        public Int32? FrameHeight { get; set; }

        /// <summary>
        /// Gets or sets the point of origin for the frame group's frames.
        /// </summary>
        [JsonProperty(PropertyName = "origin")]
        public Point2? Origin { get; set; }
    }
}

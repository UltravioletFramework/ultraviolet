using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An intermediate representation of a related batch of frames within a sprite animation.
    /// </summary>
    internal sealed class SpriteFrameBatchDescription
    {
        /// <summary>
        /// Gets the frame batch's default values for frames.
        /// </summary>
        public SpriteFrameDefaultsDescription FrameDefaults { get; set; }
        
        /// <summary>
        /// Gets the frame batch's items.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<SpriteFrameDescription> Items { get; set; }
    }
}

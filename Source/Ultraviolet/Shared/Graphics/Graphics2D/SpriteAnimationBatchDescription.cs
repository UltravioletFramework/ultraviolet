using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An intermediate representation of a related batch of animations within a sprite.
    /// </summary>
    internal sealed class SpriteAnimationBatchDescription
    {
        /// <summary>
        /// Gets the animation batch's default values for frames.
        /// </summary>
        public SpriteFrameDefaultsDescription FrameDefaults { get; set; }

        /// <summary>
        /// Gets the animation batch's default values for frame groups.
        /// </summary>
        public SpriteFrameGroupDefaultsDescription FrameGroupDefaults { get; set; }

        /// <summary>
        /// Gets the animation batch's items.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<SpriteAnimationDescription> Items { get; set; }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An intermediate representation of a related batch of animations within a sprite.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class SpriteAnimationBatchDescription
    {
        /// <summary>
        /// Gets the animation batch's default values for frames.
        /// </summary>
        [JsonProperty("frameDefaults")]
        public SpriteFrameDefaultsDescription FrameDefaults { get; set; }

        /// <summary>
        /// Gets the animation batch's default values for frame groups.
        /// </summary>
        [JsonProperty("frameGroupDefaults")]
        public SpriteFrameGroupDefaultsDescription FrameGroupDefaults { get; set; }

        /// <summary>
        /// Gets the animation batch's items.
        /// </summary>
        [JsonProperty(PropertyName = "items", Required = Required.Always)]
        public IEnumerable<SpriteAnimationDescription> Items { get; set; }
    }
}

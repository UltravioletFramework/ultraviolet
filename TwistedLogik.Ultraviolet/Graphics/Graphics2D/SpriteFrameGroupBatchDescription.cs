using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An intermediate representation of a related batch of frame groups within a sprite animation.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class SpriteFrameGroupBatchDescription
    {
        /// <summary>
        /// Gets the frame group batch's default values for frame groups.
        /// </summary>
        [JsonProperty(PropertyName = "frameGroupDefaults")]
        public SpriteFrameGroupDefaultsDescription FrameGroupDefaults { get; set; }
        
        /// <summary>
        /// Gets the frame group batch's items.
        /// </summary>
        [JsonProperty(PropertyName = "items", Required = Required.Always)]
        public IEnumerable<SpriteFrameGroupDescription> Items { get; set; }
    }
}

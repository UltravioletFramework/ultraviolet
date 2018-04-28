using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a <see cref="Sprite"/> object during deserialization.
    /// </summary>
    internal sealed class SpriteDescription
    {
        /// <summary>
        /// Gets or sets the sprite's default values for frames.
        /// </summary>
        public SpriteFrameDefaultsDescription FrameDefaults { get; set; }

        /// <summary>
        /// Gets or sets the sprite's default values for frame groups.
        /// </summary>
        public SpriteFrameGroupDefaultsDescription FrameGroupDefaults { get; set; }

        /// <summary>
        /// Retrieves an array containing the sprite's list of animations.
        /// </summary>
        [JsonConverter(typeof(CoreEnumerableJsonConverter<SpriteAnimationBatchDescription>))]
        public IList<SpriteAnimationBatchDescription> Animations { get; set; }
    }
}

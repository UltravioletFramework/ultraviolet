using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Describes a <see cref="SpriteAnimation"/> object during deserialization.
    /// </summary>
    internal sealed class SpriteAnimationDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteAnimationDescription"/> class.
        /// </summary>
        public SpriteAnimationDescription()
        {
            Repeat = "loop";
        }

        /// <summary>
        /// Retrieves the animation's name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Retrieves the animation's repeat value.
        /// </summary>
        public String Repeat { get; set; }

        /// <summary>
        /// Gets an array containing the animation's frames.
        /// </summary>
        [JsonConverter(typeof(CoreEnumerableJsonConverter<SpriteFrameBatchDescription>))]
        public IList<SpriteFrameBatchDescription> Frames { get; set; }

        /// <summary>
        /// Gets an array of frame groups describing the animation's frames.
        /// </summary>
        [JsonConverter(typeof(CoreEnumerableJsonConverter<SpriteFrameGroupBatchDescription>))]
        public IList<SpriteFrameGroupBatchDescription> FrameGroups { get; set; }
    }
}

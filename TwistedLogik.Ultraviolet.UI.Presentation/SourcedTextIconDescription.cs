using System;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// An internal representation of a formatted text icon used during deserialization.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class SourcedTextIconDescription
    {
        /// <summary>
        /// Gets or sets the sprite animation identifier of the icon image.
        /// </summary>
        [JsonProperty(PropertyName = "icon", Required = Required.Always)]
        public SourcedSpriteAnimationID Icon { get; set; }

        /// <summary>
        /// Gets or sets the icon's width in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "width", Required = Required.Default)]
        public Int32? Width { get; set; }

        /// <summary>
        /// Gets or sets the icon's height in pixels.
        /// </summary>
        [JsonProperty(PropertyName = "height", Required = Required.Default)]
        public Int32? Height { get; set; }
    }
}

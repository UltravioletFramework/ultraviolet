using System;
using Newtonsoft.Json;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// An internal representation of a formatted text icon used during deserialization.
    /// </summary>
    internal sealed class SourcedTextIconDescription
    {
        /// <summary>
        /// Gets or sets the sprite animation identifier of the icon image.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public SourcedSpriteAnimationID Icon { get; set; }

        /// <summary>
        /// Gets or sets the icon's width in pixels.
        /// </summary>
        public Int32? Width { get; set; }

        /// <summary>
        /// Gets or sets the icon's height in pixels.
        /// </summary>
        public Int32? Height { get; set; }
    }
}

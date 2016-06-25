using System.Collections.Generic;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a <see cref="SpriteFont"/> used during content processing.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class SpriteFontDescription
    {
        /// <summary>
        /// Gets or sets the font's set of faces.
        /// </summary>
        [JsonProperty(PropertyName = "faces")]
        public SpriteFontFacesDescription Faces { get; set; }

        /// <summary>
        /// Gets or sets the font's collection of character regions.
        /// </summary>
        [JsonProperty(PropertyName = "characterRegions")]
        public IEnumerable<CharacterRegionDescription> CharacterRegions { get; set; }
    }
}

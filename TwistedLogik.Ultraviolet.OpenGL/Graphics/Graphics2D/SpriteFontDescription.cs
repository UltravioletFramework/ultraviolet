using System.Collections.Generic;
using Newtonsoft.Json;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a <see cref="SpriteFont"/> used during content processing.
    /// </summary>
    internal sealed class SpriteFontDescription
    {
        /// <summary>
        /// Gets or sets a description of the font's regular face.
        /// </summary>
        [JsonProperty(PropertyName = "regular")]
        public SpriteFontFaceDescription Regular { get; set; }

        /// <summary>
        /// Gets or sets a description of the font's bold face.
        /// </summary>
        [JsonProperty(PropertyName = "bold")]
        public SpriteFontFaceDescription Bold { get; set; }

        /// <summary>
        /// Gets or sets a description of the font's italic face.
        /// </summary>
        [JsonProperty(PropertyName = "italic")]
        public SpriteFontFaceDescription Italic { get; set; }

        /// <summary>
        /// Gets or sets a description of the font's bold-italic face.
        /// </summary>
        [JsonProperty(PropertyName = "boldItalic")]
        public SpriteFontFaceDescription BoldItalic { get; set; }

        /// <summary>
        /// Gets or sets the font's collection of character regions.
        /// </summary>
        [JsonProperty(PropertyName = "characterRegions")]
        public IEnumerable<CharacterRegionDescription> CharacterRegions { get; set; }
    }
}

using Newtonsoft.Json;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a sprite font's collection of faces used during content processing.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class SpriteFontFacesDescription
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
    }
}

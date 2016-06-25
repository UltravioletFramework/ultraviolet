using System;
using Newtonsoft.Json;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D.Text;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// An internal representation of a <see cref="TextStyle"/> used during deserialization.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class SourcedTextStyleDescription
    {
        /// <summary>
        /// Gets or sets the asset identifier of the style's font.
        /// </summary>
        [JsonProperty(PropertyName = "font", Required = Required.Default)]
        public SourcedAssetID? Font { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the style requires (or disallows) a bold font face.
        /// </summary>
        [JsonProperty(PropertyName = "bold", Required = Required.Default)]
        public Boolean? Bold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the style requires (or disallows) an italic font face.
        /// </summary>
        [JsonProperty(PropertyName = "italic", Required = Required.Default)]
        public Boolean? Italic { get; set; }

        /// <summary>
        /// Gets or sets the style's font color.
        /// </summary>
        [JsonProperty(PropertyName = "color", Required = Required.Default)]
        public Color? Color { get; set; }

        /// <summary>
        /// Gets or sets a collection containing the style's glyph shaders
        /// </summary>
        [JsonProperty(PropertyName = "shaders", Required = Required.Default)]
        public GlyphShader[] GlyphShaders { get; set; }
    }
}

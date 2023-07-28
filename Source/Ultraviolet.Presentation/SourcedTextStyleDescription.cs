using System;
using Newtonsoft.Json;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// An internal representation of a <see cref="TextStyle"/> used during deserialization.
    /// </summary>
    internal sealed class SourcedTextStyleDescription
    {
        /// <summary>
        /// Gets or sets the asset identifier of the style's font.
        /// </summary>
        public SourcedAssetID? Font { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the style requires (or disallows) a bold font face.
        /// </summary>
        public Boolean? Bold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the style requires (or disallows) an italic font face.
        /// </summary>
        public Boolean? Italic { get; set; }

        /// <summary>
        /// Gets or sets the style's font color.
        /// </summary>
        public Color? Color { get; set; }

        /// <summary>
        /// Gets or sets a collection containing the style's glyph shaders
        /// </summary>
        [JsonProperty(PropertyName = "shaders")]
        public GlyphShader[] GlyphShaders { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Core;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a <see cref="SpriteFontFace"/> used during content processing.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class SpriteFontFaceDescription
    {
        /// <summary>
        /// Gets or sets the asset path of the font face's texture.
        /// </summary>
        [JsonProperty(PropertyName = "texture", Required = Required.Always)]
        public String Texture { get; set; }

        /// <summary>
        /// Gets or sets the region on the texture that contains the font face's glyph.
        /// </summary>
        [JsonProperty(PropertyName = "textureRegion", Required = Required.Always)]
        public Rectangle? TextureRegion { get; set; }

        /// <summary>
        /// Gets or sets the face's glyph metadata.
        /// </summary>
        [JsonProperty(PropertyName = "glyphs")]
        public SpriteFontFaceGlyphDescription Glyphs { get; set; }

        /// <summary>
        /// Gets or sets the face's collection of kerning values.
        /// </summary>
        [JsonProperty(PropertyName = "kernings")]
        public Dictionary<String, Int32> Kernings { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a <see cref="SpriteFontFace"/> used during content processing.
    /// </summary>
    internal sealed class SpriteFontFaceDescription
    {
        /// <summary>
        /// Gets or sets the asset path of the font face's texture.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public String Texture { get; set; }

        /// <summary>
        /// Gets or sets the region on the texture that contains the font face's glyph.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Rectangle? TextureRegion { get; set; }

        /// <summary>
        /// Gets or sets the face's glyph metadata.
        /// </summary>
        public SpriteFontFaceGlyphDescription Glyphs { get; set; }

        /// <summary>
        /// Gets or sets the face's collection of kerning values.
        /// </summary>
        public Dictionary<String, Int32> Kernings { get; set; }

        /// <summary>
        /// Gets the height of the font's ascender in pixels.
        /// </summary>
        public Int32 Ascender { get; set; }

        /// <summary>
        /// Gets the height of the font's descender in pixels.
        /// </summary>
        public Int32 Descender { get; set; }
    }
}

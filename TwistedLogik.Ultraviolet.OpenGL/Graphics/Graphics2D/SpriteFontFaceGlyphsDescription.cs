using System;
using Newtonsoft.Json;
using Ultraviolet.Core;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of the glyph metadata for a <see cref="SpriteFontFace"/> used during content processing.
    /// </summary>
    [Preserve(AllMembers = true)]
    internal sealed class SpriteFontFaceGlyphDescription
    {
        /// <summary>
        /// Gets or sets the face's substitution glyph.
        /// </summary>
        [JsonProperty(PropertyName = "substitution")]
        public Char? Substitution { get; set; }
    }
}

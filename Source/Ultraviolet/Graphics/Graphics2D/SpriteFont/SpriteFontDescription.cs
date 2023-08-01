using System.Collections.Generic;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// An internal representation of a <see cref="SpriteFont"/> used during content processing.
    /// </summary>
    internal sealed class SpriteFontDescription
    {
        /// <summary>
        /// Gets or sets the font's set of faces.
        /// </summary>
        public SpriteFontFacesDescription Faces { get; set; }

        /// <summary>
        /// Gets or sets the font's collection of character regions.
        /// </summary>
        public IEnumerable<CharacterRegionDescription> CharacterRegions { get; set; }
    }
}

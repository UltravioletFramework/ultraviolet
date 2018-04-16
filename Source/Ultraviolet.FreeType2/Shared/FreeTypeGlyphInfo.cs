using System;
using Ultraviolet.Graphics;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents metadata for a particular glyph in a FreeType2 font face.
    /// </summary>
    internal struct FreeTypeGlyphInfo
    {
        /// <summary>
        /// Gets or sets the Unicode character which the glyph represents.
        /// </summary>
        public UInt32 UnicodeCharacter { get; set; }

        /// <summary>
        /// Gets or sets the glyph's advance in pixels.
        /// </summary>
        public Int32 Advance { get; set; }

        /// <summary>
        /// Gets or sets the glyph's width in pixels.
        /// </summary>
        public Int32 Width { get; set; }

        /// <summary>
        /// Gets or sets the glyph's height in pixels.
        /// </summary>
        public Int32 Height { get; set; }

        /// <summary>
        /// Gets or sets the glyph's x-offset in pixels.
        /// </summary>
        public Int32 OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the glyph's y-offset in pixels.
        /// </summary>
        public Int32 OffsetY { get; set; }
        
        /// <summary>
        /// Gets or sets a reference to the texture that contains the glyph image.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets the region of the glyph's texture which contains its image.
        /// </summary>
        public Rectangle TextureRegion { get; set; }
    }
}

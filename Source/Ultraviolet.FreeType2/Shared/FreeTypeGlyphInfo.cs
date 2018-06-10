using System;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents metadata for a particular glyph in a FreeType2 font face.
    /// </summary>
    internal struct FreeTypeGlyphInfo
    {
        /// <summary>
        /// Creates a <see cref="GlyphRenderInfo"/> structure from this instance.
        /// </summary>
        /// <returns>The <see cref="GlyphRenderInfo"/> structure which was created.</returns>
        public GlyphRenderInfo ToGlyphRenderInfo()
        {
            return new GlyphRenderInfo
            {
                Texture = Texture,
                TextureRegion = TextureRegion,
                OffsetX = OffsetX,
                OffsetY = OffsetY,
                Advance = Advance,
            };
        }

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

using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Represents the information necessary to render a glyph.
    /// </summary>
    public struct GlyphRenderInfo
    {
        /// <summary>
        /// Gets or sets the texture which contains the glyph's image, or <see langword="null"/> if
        /// the glyph does not have an associated image.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets rectangle which represents the region of the glyph's texture which contains its image.
        /// </summary>
        public Rectangle TextureRegion { get; set; }

        /// <summary>
        /// Gets or sets the offset from the current x-position at which the glyph should be drawn.
        /// </summary>
        public Int32 OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the offset from the current y-position at which the glyph should be drawn.
        /// </summary>
        public Int32 OffsetY { get; set; }

        /// <summary>
        /// Gets or sets the glyph's advance.
        /// </summary>
        public Int32 Advance { get; set; }
    }
}

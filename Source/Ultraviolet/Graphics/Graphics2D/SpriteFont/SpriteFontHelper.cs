using System;
using System.Collections.Generic;
using Ultraviolet.Graphics;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Contains methods for processing sprite fonts.
    /// </summary>
    public static class SpriteFontHelper
    {
        /// <summary>
        /// Identifies the positions of the glyphs on the specified surface.
        /// </summary>
        /// <param name="surface">The surface on which to identify glyphs.</param>
        /// <param name="source">The region on the surface in which to look for glyphs, or null to examine the entire surface.</param>
        /// <returns>A collection of rectangles describing the positions of the glyphs on the specified surface.</returns>
        public static IEnumerable<Rectangle> IdentifyGlyphs(PlatformNativeSurface surface, Rectangle? source = null)
        {
            if (source.HasValue && (source.Value.Width > surface.Width | source.Value.Height > surface.Height))
                throw new ArgumentOutOfRangeException("source");

            var regionValue = source ?? new Rectangle(0, 0, surface.Width, surface.Height);
            var data = new Color[regionValue.Width * regionValue.Height];
            surface.GetData(data, regionValue);

            // Find the glyphs on the texture.
            var positions = new List<Rectangle>();
            var nextLine = 0;
            var nextGlyph = 0;
            for (int y = 0; y < regionValue.Height; y = nextLine)
            {
                nextLine = y + 1;
                for (int x = 0; x <regionValue.Width; x = nextGlyph)
                {
                    nextGlyph = x + 1;

                    // Skip buffer pixels.
                    var pixel = data[y * regionValue.Width + x];
                    if (pixel.Equals(Color.Magenta))
                        continue;

                    // Find the width of the glyph.
                    var x2 = x + 1;
                    while (x2 < regionValue.Width)
                    {
                        if (data[y * regionValue.Width + x2++].Equals(Color.Magenta))
                            break;
                    }
                    var glyphWidth = (x2 - x) - 1;

                    // Find the height of the glyph.
                    var y2 = y + 1;
                    while (y2 < regionValue.Height)
                    {
                        if (data[y2++ * regionValue.Width + x].Equals(Color.Magenta))
                            break;
                    }
                    var glyphHeight = (y2 - y) - 1;

                    // Calculate the position of the next glyph and the next line of glyphs.
                    nextGlyph = x + glyphWidth + 1;
                    if (y + glyphHeight > nextLine)
                    {
                        nextLine = y + glyphHeight + 1;
                    }

                    // Store the glyph's position.
                    positions.Add(new Rectangle(regionValue.X + x, regionValue.Y + y, glyphWidth, glyphHeight));
                }
            }
            return positions;
        }
    }
}

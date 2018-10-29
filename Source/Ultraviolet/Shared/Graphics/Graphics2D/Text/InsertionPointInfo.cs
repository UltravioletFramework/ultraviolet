using System;

namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a point in a block of text where additional characters can be inserted or removed.
    /// </summary>
    public struct InsertionPointInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertionPointInfo"/> structure.
        /// </summary>
        /// <param name="glyphIndex">The index of the glyph which comes immediately after the insertion point.</param>
        public InsertionPointInfo(Int32 glyphIndex)
            : this(glyphIndex, glyphIndex)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertionPointInfo"/> structure.
        /// </summary>
        /// <param name="glyphIndex">The index of the glyph which comes immediately after the insertion point.</param>
        /// <param name="glyphSourceIndex">The index of the source character which comes immediately after the insertion point.</param>
        public InsertionPointInfo(Int32 glyphIndex, Int32 glyphSourceIndex)
        {
            this.GlyphIndex = glyphIndex;
            this.GlyphSourceIndex = glyphSourceIndex;
        }

        /// <summary>
        /// Gets the index of the glyph which comes immediately after the insertion point.
        /// </summary>
        public Int32 GlyphIndex { get; }

        /// <summary>
        /// Gets the index of the source character which comes immediately after the insertion point.
        /// If the formatted text does not use text shaping, this value should be identical to the <see cref="GlyphIndex"/> property.
        /// </summary>
        public Int32 GlyphSourceIndex { get; }
    }
}

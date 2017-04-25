namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the supported options for searching within a <see cref="TextLayoutCommandStream"/> for a particular glyph or insertion point.
    /// </summary>
    internal enum GlyphSearchMode
    {
        /// <summary>
        /// Search for a glyph index.
        /// </summary>
        SearchGlyphs,

        /// <summary>
        /// Search for a glyph index, snapping the search point to the nearest line of text.
        /// </summary>
        SearchGlyphsSnapToLine,

        /// <summary>
        /// Search for an insertion point.
        /// </summary>
        SearchInsertionPoints,
    }
}

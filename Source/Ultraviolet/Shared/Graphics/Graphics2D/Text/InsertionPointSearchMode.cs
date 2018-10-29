namespace Ultraviolet.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents the supported options for searching within a <see cref="TextLayoutCommandStream"/> for a particular insertion point.
    /// </summary>
    internal enum InsertionPointSearchMode
    {
        /// <summary>
        /// The insertion point which is returned is always placed before the glyph which
        /// contains the search point.
        /// </summary>
        BeforeGlyph,

        /// <summary>
        /// The insertion point which is returned can be placed either before or after the glyph
        /// which contains the search point, whichever is closer.
        /// </summary>
        BeforeOrAfterGlyph,

        /// <summary>
        /// Instructs the search algorithm to always return a result, even if the search point
        /// is before the beginning of a line or after the end of a line. In either case, the returned 
        /// value will be snapped to the nearest valid insertion point on that line.
        /// </summary>
        SnapToLine,
    }
}

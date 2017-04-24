
namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the state of a text box's caret.
    /// </summary>
    internal enum CaretMode
    {
        /// <summary>
        /// Text is inserted at the current caret position, moving any existing characters forward.
        /// </summary>
        Insert,

        /// <summary>
        /// Text is inserted at the current caret position, overwriting any existing characters.
        /// </summary>
        Overwrite,
    }
}

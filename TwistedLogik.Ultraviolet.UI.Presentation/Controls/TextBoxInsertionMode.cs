
namespace TwistedLogik.Ultraviolet.UI.Presentation.Controls
{
    /// <summary>
    /// Represents a set of flags which control how text is inserted into a text box.
    /// </summary>
    public enum TextBoxInsertionMode
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

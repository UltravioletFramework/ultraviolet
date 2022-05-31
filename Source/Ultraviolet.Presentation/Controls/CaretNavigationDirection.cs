namespace Ultraviolet.Presentation.Controls
{
    /// <summary>
    /// Represents the directions in which the user can navigate through the text editor.
    /// </summary>
    internal enum CaretNavigationDirection
    {
        /// <summary>
        /// No direction.
        /// </summary>
        None,

        /// <summary>
        /// The caret is being moved left.
        /// </summary>
        Left,

        /// <summary>
        /// The caret is being moved right.
        /// </summary>
        Right,

        /// <summary>
        /// The caret is being moved up.
        /// </summary>
        Up,

        /// <summary>
        /// The caret is being moved down.
        /// </summary>
        Down,

        /// <summary>
        /// The caret is being moved to the beginning of the text.
        /// </summary>
        Home,

        /// <summary>
        /// The caret is being moved to the end of the text.
        /// </summary>
        End,

        /// <summary>
        /// The caret is being moved up one page.
        /// </summary>
        PageUp,

        /// <summary>
        /// The caret is being moved down one page.
        /// </summary>
        PageDown,

        /// <summary>
        /// The caret is being moved to the end of the next word.
        /// </summary>
        NextWord,

        /// <summary>
        /// The caret is being moved to the beginning of the previous word.
        /// </summary>
        PreviousWord,
    }
}

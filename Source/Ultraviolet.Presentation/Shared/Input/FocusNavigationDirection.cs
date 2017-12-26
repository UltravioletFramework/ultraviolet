
namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the directions in which focus can be moved.
    /// </summary>
    public enum FocusNavigationDirection
    {
        /// <summary>
        /// Moves focus to the next control in tab order.
        /// </summary>
        Next,

        /// <summary>
        /// Moves focus to the previous control in tab order.
        /// </summary>
        Previous,

        /// <summary>
        /// Moves focus to the first control in tab order.
        /// </summary>
        First,

        /// <summary>
        /// Moves focus to the last control in tab order.
        /// </summary>
        Last,

        /// <summary>
        /// Moves focus left.
        /// </summary>
        Left,

        /// <summary>
        /// Moves focus right.
        /// </summary>
        Right,

        /// <summary>
        /// Moves focus up.
        /// </summary>
        Up,

        /// <summary>
        /// Moves focus down.
        /// </summary>
        Down,
    }
}

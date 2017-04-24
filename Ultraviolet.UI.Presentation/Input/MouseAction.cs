namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents one of the actions that can be performed by the mouse.
    /// </summary>
    public enum MouseAction
    {
        /// <summary>
        /// No action.
        /// </summary>
        None,

        /// <summary>
        /// The left mouse button is clicked.
        /// </summary>
        LeftClick,

        /// <summary>
        /// The right mouse button is clicked.
        /// </summary>
        RightClick,

        /// <summary>
        /// The middle mouse button is clicked.
        /// </summary>
        MiddleClick,
        
        /// <summary>
        /// The mouse wheel is rotated.
        /// </summary>
        WheelClick,

        /// <summary>
        /// The left mouse button is double clicked.
        /// </summary>
        LeftDoubleClick,

        /// <summary>
        /// The right mouse button is double clicked.
        /// </summary>
        RightDoubleClick,

        /// <summary>
        /// The middle mouse button is double clicked.
        /// </summary>
        MiddleDoubleClick,
    }
}

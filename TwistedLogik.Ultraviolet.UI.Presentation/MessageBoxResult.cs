namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the message box button which was clicked by the user.
    /// </summary>
    public enum MessageBoxResult
    {
        /// <summary>
        /// The message box did not return a value.
        /// </summary>
        None = 0,

        /// <summary>
        /// The user clicked the "OK" button.
        /// </summary>
        OK = 1,

        /// <summary>
        /// The user clicked the "Cancel" button.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The user clicked the "Yes" button.
        /// </summary>
        Yes = 6,

        /// <summary>
        /// The user clicked the "No" button.
        /// </summary>
        No = 7,
    }
}

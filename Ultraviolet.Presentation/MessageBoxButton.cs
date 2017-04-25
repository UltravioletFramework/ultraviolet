namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the sets of buttons that can be displayed on a message box.
    /// </summary>
    public enum MessageBoxButton
    {
        /// <summary>
        /// Specifies that the message box contains an "OK" button.
        /// </summary>
        OK,

        /// <summary>
        /// Specifies that the message box contains an "OK" button and a "Cancel" button.
        /// </summary>
        OKCancel,

        /// <summary>
        /// Specifies that the message box contains a "Yes" button, a "No" button, and a "Cancel" button.
        /// </summary>
        YesNoCancel,

        /// <summary>
        /// Specifies that the message box contains a "Yes" button and a "No" button.
        /// </summary>
        YesNo,
    }
}

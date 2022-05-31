namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the set of images that can be displayed in a message box.
    /// </summary>
    public enum MessageBoxImage
    {
        /// <summary>
        /// The message box contains no images.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// The message box contains a hand symbol.
        /// </summary>
        Hand = 0x0010,

        /// <summary>
        /// The message box contains a question mark symbol.
        /// </summary>
        Question = 0x0020,

        /// <summary>
        /// The message box contains an exclamation mark symbol.
        /// </summary>
        Exclamation = 0x030,

        /// <summary>
        /// The message box contains an asterisk symbol.
        /// </summary>
        Asterisk = 0x040,

        /// <summary>
        /// The message box contains a hand symbol.
        /// </summary>
        Stop = Hand,

        /// <summary>
        /// The message box contains a hand symbol.
        /// </summary>
        Error = Hand,

        /// <summary>
        /// The message box contains an exclamation mark symbol.
        /// </summary>
        Warning = Exclamation,

        /// <summary>
        /// The message box contains an asterisk symbol.
        /// </summary>
        Information = Asterisk,
    }
}


namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents the loading state of a UI layout.
    /// </summary>
    public enum UILayoutState
    {
        /// <summary>
        /// The layout is in the process of being initialized.
        /// </summary>
        Initializing,

        /// <summary>
        /// The layout has been initialized but has not yet been loaded.
        /// </summary>
        Initialized,

        /// <summary>
        /// The layout is in the process of being loaded.
        /// </summary>
        Loading,

        /// <summary>
        /// The layout is loaded and ready for interaction.
        /// </summary>
        Ready,
    }
}

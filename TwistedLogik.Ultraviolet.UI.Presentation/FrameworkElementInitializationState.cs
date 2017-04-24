namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the initialization state of a <see cref="FrameworkElement"/> instance.
    /// </summary>
    internal enum FrameworkElementInitializationState : byte
    {
        /// <summary>
        /// The element has not been initialized.
        /// </summary>
        Uninitialized,

        /// <summary>
        /// The element is in the process of initializing.
        /// </summary>
        Initializing,

        /// <summary>
        /// The element is in the process of initializing, and is raising any pending change events.
        /// </summary>
        InitializingRaisingEvents,

        /// <summary>
        /// Thhe element is fully initialized.
        /// </summary>
        Initialized,
    }
}

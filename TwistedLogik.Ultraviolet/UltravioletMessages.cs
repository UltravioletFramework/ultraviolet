
namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the standard set of Ultraviolet Framework events.
    /// </summary>
    public static class UltravioletMessages
    {
        /// <summary>
        /// An event indicating that the application should exit.
        /// </summary>
        public static readonly UltravioletMessageID Quit = UltravioletMessageID.Acquire("Quit");

        /// <summary>
        /// An event indicating that the screen orientation has changed.
        /// </summary>
        public static readonly UltravioletMessageID OrientationChanged = UltravioletMessageID.Acquire("OrientationChanged");

        /// <summary>
        /// An event indicating that the application is being terminated by the operating system.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationTerminating = UltravioletMessageID.Acquire("ApplicationTerminating");

        /// <summary>
        /// An event indicating that the application is about to be suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationSuspending = UltravioletMessageID.Acquire("ApplicationSuspending");

        /// <summary>
        /// An event indicating that the application was suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationSuspended = UltravioletMessageID.Acquire("ApplicationSuspended");

        /// <summary>
        /// An event indicating that the application is about to resume after being suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationResuming = UltravioletMessageID.Acquire("ApplicationResuming");

        /// <summary>
        /// An event indicating that the application was resumed after being suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationResumed = UltravioletMessageID.Acquire("ApplicationResumed");

        /// <summary>
        /// An event indicating that the operation system is low on memory.
        /// </summary>
        public static readonly UltravioletMessageID LowMemory = UltravioletMessageID.Acquire("LowMemory");

        /// <summary>
        /// An event indicating that the software keyboard was shown.
        /// </summary>
        public static readonly UltravioletMessageID SoftwareKeyboardShown = UltravioletMessageID.Acquire("SoftwareKeyboardShown");

        /// <summary>
        /// An event indicating that the software keyboard was hidden.
        /// </summary>
        public static readonly UltravioletMessageID SoftwareKeyboardHidden = UltravioletMessageID.Acquire("SoftwareKeyboardHidden");
    }
}

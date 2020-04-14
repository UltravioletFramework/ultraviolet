
namespace Ultraviolet
{
    /// <summary>
    /// Represents the standard set of Ultraviolet Framework events.
    /// </summary>
    public static partial class UltravioletMessages
    {
        /// <summary>
        /// An event indicating that the application should exit.
        /// </summary>
        public static readonly UltravioletMessageID Quit = UltravioletMessageID.Acquire(nameof(Quit));

        /// <summary>
        /// An event indicating that the screen orientation has changed.
        /// </summary>
        public static readonly UltravioletMessageID OrientationChanged = UltravioletMessageID.Acquire(nameof(OrientationChanged));

        /// <summary>
        /// An event indicating that the application has been created by the operating system.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationCreated = UltravioletMessageID.Acquire(nameof(ApplicationCreated));

        /// <summary>
        /// An event indicating that the application is being terminated by the operating system.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationTerminating = UltravioletMessageID.Acquire(nameof(ApplicationTerminating));

        /// <summary>
        /// An event indicating that the application is about to be suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationSuspending = UltravioletMessageID.Acquire(nameof(ApplicationSuspending));

        /// <summary>
        /// An event indicating that the application was suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationSuspended = UltravioletMessageID.Acquire(nameof(ApplicationSuspended));

        /// <summary>
        /// An event indicating that the application is about to resume after being suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationResuming = UltravioletMessageID.Acquire(nameof(ApplicationResuming));

        /// <summary>
        /// An event indicating that the application was resumed after being suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationResumed = UltravioletMessageID.Acquire(nameof(ApplicationResumed));

        /// <summary>
        /// An event indicating that the operation system is low on memory.
        /// </summary>
        public static readonly UltravioletMessageID LowMemory = UltravioletMessageID.Acquire(nameof(LowMemory));

        /// <summary>
        /// An event indicating that the software keyboard was shown.
        /// </summary>
        public static readonly UltravioletMessageID SoftwareKeyboardShown = UltravioletMessageID.Acquire(nameof(SoftwareKeyboardShown));

        /// <summary>
        /// An event indicating that the software keyboard was hidden.
        /// </summary>
        public static readonly UltravioletMessageID SoftwareKeyboardHidden = UltravioletMessageID.Acquire(nameof(SoftwareKeyboardHidden));

        /// <summary>
        /// An event indicating that the text input region has been changed.
        /// </summary>
        public static readonly UltravioletMessageID TextInputRegionChanged = UltravioletMessageID.Acquire(nameof(TextInputRegionChanged));

        /// <summary>
        /// An event indicating that the density settings for a particular display were changed.
        /// </summary>
        public static readonly UltravioletMessageID DisplayDensityChanged = UltravioletMessageID.Acquire(nameof(DisplayDensityChanged));

        /// <summary>
        /// An event indicating that a window was moved to a display with a different density.
        /// </summary>
        public static readonly UltravioletMessageID WindowDensityChanged = UltravioletMessageID.Acquire(nameof(WindowDensityChanged));

        /// <summary>
        /// An event indicating that the file source for content assets has changed.
        /// </summary>
        public static readonly UltravioletMessageID FileSourceChanged = UltravioletMessageID.Acquire(nameof(FileSourceChanged));
    }
}


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
        /// An event indicating that the application was suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationSuspended = UltravioletMessageID.Acquire("ApplicationSuspended");

        /// <summary>
        /// An event indicating that the application was resumed after being suspended.
        /// </summary>
        public static readonly UltravioletMessageID ApplicationResumed = UltravioletMessageID.Acquire("ApplicationResumed");
    }
}


namespace TwistedLogik.Ultraviolet.WindowsForms
{
    /// <summary>
    /// Represents Ultraviolet's supported methods for ticking a Windows Forms-based application.
    /// </summary>
    public enum UltravioletTickMode
    {
        /// <summary>
        /// The application is ticked regularly by a Windows Forms timer.
        /// </summary>
        Timer,

        /// <summary>
        /// The application runs in an infinite loop whenever the application is idle,
        /// but yields to Windows message processing.
        /// </summary>
        Idle,
    }
}

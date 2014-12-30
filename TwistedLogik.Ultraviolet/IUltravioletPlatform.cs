using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's platform interop subsystem.
    /// </summary>
    public interface IUltravioletPlatform : IUltravioletSubsystem
    {
        /// <summary>
        /// Gets or sets the current cursor.
        /// </summary>
        /// <remarks>Setting this property to <c>null</c> will restore the default cursor.</remarks>
        Cursor Cursor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the system clipboard manager.
        /// </summary>
        IUltravioletClipboardInfo Clipboard
        {
            get;
        }

        /// <summary>
        /// Gets the window information manager.
        /// </summary>
        IUltravioletWindowInfo Windows
        {
            get;
        }

        /// <summary>
        /// Gets the display information manager.
        /// </summary>
        IUltravioletDisplayInfo Displays
        {
            get;
        }
    }
}

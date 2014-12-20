using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.UI.Presentation;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's UI subsystem.
    /// </summary>
    public interface IUltravioletUI : IUltravioletSubsystem
    {
        /// <summary>
        /// Gets the screen stack associated with the primary window.
        /// </summary>
        /// <returns>The <see cref="UIScreenStack"/> associated with the primary window.</returns>
        UIScreenStack GetScreens();

        /// <summary>
        /// Gets the screen stack associated with the specified window.
        /// </summary>
        /// <param name="window">The window for which to retrieve a screen stack, 
        /// or <c>null</c> to retrieve the screen stack for the primary window.</param>
        /// <returns>The <see cref="UIScreenStack"/> associated with the specified window.</returns>
        UIScreenStack GetScreens(IUltravioletWindow window);

        /// <summary>
        /// Gets the management object for the Ultraviolet Presentation Framework.
        /// </summary>
        PresentationFrameworkManager PresentationFramework
        {
            get;
        }
    }
}

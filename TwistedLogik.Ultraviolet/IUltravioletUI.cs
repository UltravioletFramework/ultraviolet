using System;
using System.Reflection;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI;

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
        /// <returns>The screen stack associated with the primary window.</returns>
        UIScreenStack GetScreens();

        /// <summary>
        /// Gets the screen stack associated with the specified window.
        /// </summary>
        /// <param name="window">The window for which to retrieve a screen stack, 
        /// or <c>null</c> to retrieve the screen stack for the primary window.</param>
        /// <returns>The screen stack associated with the specified window.</returns>
        UIScreenStack GetScreens(IUltravioletWindow window);

        /// <summary>
        /// Sets the assembly which contains the layout provider.
        /// </summary>
        /// <param name="assembly">The name of the assembly that contains the layout provider.</param>
        void SetLayoutProvider(String assembly);

        /// <summary>
        /// Sets the assembly which contains the layout provider.
        /// </summary>
        /// <param name="assembly">The assembly that contains the layout provider.</param>
        void SetLayoutProvider(Assembly assembly);

        /// <summary>
        /// Gets the current layout provider service.
        /// </summary>
        /// <returns>The current layout provider service.</returns>
        IUILayoutProvider GetLayoutProvider();
    }
}

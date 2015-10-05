using System;
using System.Threading.Tasks;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for displaying modal dialogs.
    /// </summary>
    public static class Modal
    {
        /// <summary>
        /// Opens the specified screen as a modal dialog box in the primary window.
        /// </summary>
        /// <param name="screen">The screen to open as a modal dialog box.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> which will be continued once the modal dialog has been closed.</returns>
        public static Task ShowDialog(UIScreen screen, TimeSpan? duration = null)
        {
            return ShowDialog(null, screen, duration);
        }

        /// <summary>
        /// Opens the specified screen as a modal dialog box.
        /// </summary>
        /// <param name="window">The window in which to open the dialog box, or null to use the primary window.</param>
        /// <param name="screen">The screen to open as a modal dialog box.</param>
        /// <param name="duration">The amount of time over which to transition the screen's state, or <c>null</c> to use the default transition time.</param>
        /// <returns>A <see cref="Task"/> which will be continued once the modal dialog has been closed.</returns>
        public static Task ShowDialog(IUltravioletWindow window, UIScreen screen, TimeSpan? duration = null)
        {
            Contract.Require(screen, "screen");
            
            var uv = screen.Ultraviolet;
            var screens = uv.GetUI().GetScreens(window);
            
            if (screen.State != UIPanelState.Closed)
            {
                screens.Close(screen, TimeSpan.Zero);
            }

            var source = new TaskCompletionSource<UIPanel>();
            var handler = default(UIPanelEventHandler);
            handler = new UIPanelEventHandler(panel => 
            {
                screen.Closed -= handler;
                source.SetResult(panel);
            });
            screen.Closed += handler;

            screens.Open(screen, duration);

            return source.Task;
        }        
    }
}

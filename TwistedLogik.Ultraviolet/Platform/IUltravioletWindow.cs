using System;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents the method that is called when an Ultraviolet window raises an event.
    /// </summary>
    /// <param name="window">The window that raised the event.</param>
    public delegate void UltravioletWindowEventHandler(IUltravioletWindow window);

    /// <summary>
    /// Represents the method that is called when an Ultraviolet window is drawn.
    /// </summary>
    /// <param name="window">The window that raised the event.</param>
    /// <param name="time">Time elapsed since the last call to Draw.</param>
    public delegate void UltravioletWindowDrawingEventHandler(IUltravioletWindow window, UltravioletTime time);

    /// <summary>
    /// Represents a window that is attached to an Ultraviolet context.
    /// </summary>
    public interface IUltravioletWindow
    {
        /// <summary>
        /// Sets the window's fullscreen display mode.
        /// </summary>
        /// <param name="displayMode">The fullscreen display mode to set, or <c>null</c> to use the desktop display mode.</param>
        void SetFullscreenDisplayMode(DisplayMode displayMode);

        /// <summary>
        /// Sets the window's fullscreen display mode.
        /// </summary>
        /// <param name="width">The display mode's width.</param>
        /// <param name="height">The display mode's height.</param>
        /// <param name="bpp">The display mode's bit depth.</param>
        /// <param name="refresh">The display mode's refresh rate in hertz.</param>
        void SetFullscreenDisplayMode(Int32 width, Int32 height, Int32 bpp, Int32 refresh);

        /// <summary>
        /// Gets the window's fullscreen display mode.
        /// </summary>
        /// <returns>The window's fullscreen display mode, or <c>null</c> if the window is using the desktop display mode.</returns>
        DisplayMode GetFullscreenDisplayMode();

        /// <summary>
        /// Sets the window's fullscreen/windowed mode.
        /// </summary>
        /// <param name="mode">The window mode to set.</param>
        void SetWindowMode(WindowMode mode);

        /// <summary>
        /// Gets the fullscreen/windowed mode for this window.
        /// </summary>
        /// <returns>The window's current fullscreen/windowed mode.</returns>
        WindowMode GetWindowMode();

        /// <summary>
        /// Sets the window's maximization/minimization state.
        /// </summary>
        /// <param name="state">The window state to set.</param>
        void SetWindowState(WindowState state);

        /// <summary>
        /// Gets the window's maximization/minimization state.
        /// </summary>
        /// <returns>The window's current maximization/minimization state.</returns>
        WindowState GetWindowState();

        /// <summary>
        /// Gets the window's identifier within its windowing system.
        /// </summary>
        Int32 ID
        {
            get;
        }

        /// <summary>
        /// Gets or sets the window's caption.
        /// </summary>
        String Caption
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the window's position.
        /// </summary>
        Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position to which the window will be restored upon entering non-maximized windowed mode.
        /// </summary>
        Vector2 WindowedPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the window's client size.
        /// </summary>
        Size2 ClientSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client size to which the window will be restored upon entering non-maximized windowed mode.
        /// </summary>
        Size2 WindowedClientSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the window's maximum client size.
        /// </summary>
        Size2 MaximumClientSize
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this window's rendering is synchronized
        /// with the vertical retrace (i.e, whether vsync is enabled).
        /// </summary>
        Boolean SynchronizeWithVerticalRetrace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the window is currently active.
        /// </summary>
        Boolean Active
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this window is visible.
        /// </summary>
        Boolean Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this window is resizable.
        /// </summary>
        Boolean Resizable
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this window is borderless.
        /// </summary>
        Boolean Borderless
        {
            get;
        }

        /// <summary>
        /// Gets or sets the window's icon.
        /// </summary>
        Surface2D Icon
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when the window is shown.
        /// </summary>
        event UltravioletWindowEventHandler Shown;

        /// <summary>
        /// Occurs when the window is hidden.
        /// </summary>
        event UltravioletWindowEventHandler Hidden;

        /// <summary>
        /// Occurs when the window is minimized.
        /// </summary>
        event UltravioletWindowEventHandler Minimized;

        /// <summary>
        /// Occurs when the window is maximized.
        /// </summary>
        event UltravioletWindowEventHandler Maximized;

        /// <summary>
        /// Occurs when the window is restored.
        /// </summary>
        event UltravioletWindowEventHandler Restored;

        /// <summary>
        /// Occurs when the window is drawn.
        /// </summary>
        event UltravioletWindowDrawingEventHandler Drawing;

        /// <summary>
        /// Occurs when the window is drawing its UI layer.
        /// </summary>
        event UltravioletWindowDrawingEventHandler DrawingUI;
    }
}

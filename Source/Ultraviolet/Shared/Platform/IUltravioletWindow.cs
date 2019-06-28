using System;
using Ultraviolet.Graphics;

namespace Ultraviolet.Platform
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
    /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
    public delegate void UltravioletWindowDrawingEventHandler(IUltravioletWindow window, UltravioletTime time);

    /// <summary>
    /// Represents a window that is attached to an Ultraviolet context.
    /// </summary>
    public interface IUltravioletWindow
    {
        /// <summary>
        /// Warps the cursor to the specified position within this window.
        /// </summary>
        /// <param name="x">The x-coordinate within the window to which the mouse will be warped.</param>
        /// <param name="y">The y-coordinate within the window to which the mouse will be warped.</param>
        void WarpMouseWithinWindow(Int32 x, Int32 y);

        /// <summary>
        /// Sets the window's fullscreen display mode.
        /// </summary>
        /// <param name="displayMode">The <see cref="DisplayMode"/> to use when the window is in fullscreen mode,
        /// or <see langword="null"/> to use the desktop display mode.</param>
        void SetFullscreenDisplayMode(DisplayMode displayMode);

        /// <summary>
        /// Sets the window's fullscreen display mode.
        /// </summary>
        /// <param name="width">The width of the display in pixels when it is in fullscreen mode.</param>
        /// <param name="height">The height of the display in pixels when it is in fullscreen mode.</param>
        /// <param name="bpp">The bit depth of the display when it is in fullscreen mode.</param>
        /// <param name="refresh">The refresh rate of the display in hertz when it is in fullscreen mode.</param>
        /// <param name="displayIndex">The index of the display in which to place the window when it enters fullscreen mode,
        /// or <see langword="null"/> to keep the window in its current display.</param>
        void SetFullscreenDisplayMode(Int32 width, Int32 height, Int32 bpp, Int32 refresh, Int32? displayIndex = null);

        /// <summary>
        /// Gets the window's fullscreen display mode.
        /// </summary>
        /// <returns>The <see cref="DisplayMode"/> used when the window is in fullscreen mode, 
        /// or <see langword="null"/> if the window is using the desktop display mode.</returns>
        DisplayMode GetFullscreenDisplayMode();

        /// <summary>
        /// Sets the window's bounds.
        /// </summary>
        /// <param name="bounds">The window's bounding rectangle.</param>
        /// <param name="scale">The window's scaling factor.</param>
        void SetWindowBounds(Rectangle bounds, Single scale = 1f);

        /// <summary>
        /// Sets the client size to which the window will be restored upon entering non-maximized windowed mode,
        /// preserving its current position on the screen.
        /// </summary>
        /// <param name="size">The window's client size.</param>
        /// <param name="scale">The window's scaling factor.</param>
        void SetWindowedClientSize(Size2 size, Single scale = 1f);

        /// <summary>
        /// Sets the client size to which the window will be restored upon entering non-maximized windowed mode,
        /// and centers the window on the screen.
        /// </summary>
        /// <param name="size">The window's client size.</param>
        /// <param name="scale">The window's scaling factor.</param>
        void SetWindowedClientSizeCentered(Size2 size, Single scale = 1f);

        /// <summary>
        /// Sets the window's window mode.
        /// </summary>
        /// <param name="mode">The <see cref="WindowMode"/> value that represents the window mode to set.</param>
        void SetWindowMode(WindowMode mode);

        /// <summary>
        /// Gets the window's window mode.
        /// </summary>
        /// <returns>The <see cref="WindowMode"/> value that represents the window's current window mode.</returns>
        WindowMode GetWindowMode();

        /// <summary>
        /// Sets the window's maximization/minimization state.
        /// </summary>
        /// <param name="state">The <see cref="WindowState"/> value that represents the maximization/minimization state to set.</param>
        void SetWindowState(WindowState state);

        /// <summary>
        /// Gets the window's maximization/minimization state.
        /// </summary>
        /// <returns>The <see cref="WindowState"/> value that represents the window's maximization/minimization state.</returns>
        WindowState GetWindowState();

        /// <summary>
        /// Changes the window's compositor.
        /// </summary>
        /// <param name="compositor">The compositor to set at the window's current compositor.</param>
        /// <remarks>The previous compositor instance will be disposed.</remarks>
        void ChangeCompositor(Compositor compositor);

        /// <summary>
        /// Moves the window to the center of the specified display.
        /// </summary>
        /// <param name="display">The display to which the window will be moved.</param>
        void ChangeDisplay(IUltravioletDisplay display);

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
        /// Gets or sets the scaling factor which is applied to the window.
        /// </summary>
        Single WindowScale
        {
            get;
        }

        /// <summary>
        /// Gets or sets the window's position.
        /// </summary>
        Point2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position to which the window will be restored upon entering non-maximized windowed mode.
        /// </summary>
        Point2 WindowedPosition
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the window's drawable size. On high-density displays, this value may be larger than the value
        /// of <see cref="ClientSize"/> due to the distinction between logical and virtual pixels.
        /// </summary>
        Size2 DrawableSize
        {
            get;
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
        /// Gets or sets the window's minimum client size.
        /// </summary>
        Size2 MinimumClientSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the window's maximum client size.
        /// </summary>
        Size2 MaximumClientSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the window's underlying native resources have been released.
        /// </summary>
        Boolean Disposed
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
        /// Gets or sets a value indicating whether this window grabs
        /// the mouse when it enters windowed mode.
        /// </summary>
        Boolean GrabsMouseWhenWindowed
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether this window grabs
        /// the mouse when it enters fullscreen windowed mode.
        /// </summary>
        Boolean GrabsMouseWhenFullscreenWindowed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this window grabs 
        /// the mouse when it enters fullscreen mode.
        /// </summary>
        Boolean GrabsMouseWhenFullscreen
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value (from 0.0f to 1.0f) representing the window's opacity.
        /// </summary>
        Single Opacity
        {
            get;
            set;
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
        /// Gets the window's current compositor.
        /// </summary>
        Compositor Compositor
        {
            get;
        }

        /// <summary>
        /// Gets the display on which this window currently resides.
        /// </summary>
        IUltravioletDisplay Display
        {
            get;
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

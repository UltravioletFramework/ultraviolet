using System;
using System.Runtime.InteropServices;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.OpenGL.Graphics;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.SDL2;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Platform
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the IUltravioletWindow interface.
    /// </summary>    
    public sealed unsafe partial class OpenGLUltravioletWindow : 
        IMessageSubscriber<UltravioletMessageID>,
        IUltravioletWindow, 
        IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletWindow class.
        /// </summary>
        /// <param name="ptr">The SDL2 pointer that represents the window.</param>
        /// <param name="native">A value indicating whether the window was created from a native pointer.</param>
        internal OpenGLUltravioletWindow(IntPtr ptr, Boolean native = false)
        {
            this.ptr = ptr;
            this.id = SDL.GetWindowID(ptr);
            this.native = native;

            SetIcon(DefaultWindowIcon.Value);

            FixPlatformSpecificIssues();

            UpdateWindowedPosition(Position);
            UpdateWindowedClientSize(ClientSize);
        }

        /// <summary>
        /// Explicitly converts an Ultraviolet window to its underlying SDL2 pointer.
        /// </summary>
        /// <param name="window">The Ultraviolet window to convert.</param>
        /// <returns>The window's underlying SDL2 pointer.</returns>
        public static explicit operator IntPtr(OpenGLUltravioletWindow window)
        {
            return (window == null) ? IntPtr.Zero : window.ptr;
        }

        /// <summary>
        /// Receives a message that has been published to a queue.
        /// </summary>
        /// <param name="type">The type of message that was received.</param>
        /// <param name="data">The data for the message that was received.</param>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type != SDL2UltravioletMessages.SDLEvent)
                return;

            var msg = (SDL2.Messages.SDL2EventMessageData)data;
            if (msg.Event.type != SDL_EventType.WINDOWEVENT || msg.Event.window.windowID != id)
                return;

            switch (msg.Event.window.@event)
            {
                case SDL_WindowEventID.SHOWN:
                    OnShown();
                    break;

                case SDL_WindowEventID.HIDDEN:
                    OnHidden();
                    break;

                case SDL_WindowEventID.MINIMIZED:
                    OnMinimized();
                    break;

                case SDL_WindowEventID.MAXIMIZED:
                    OnMaximized();
                    break;

                case SDL_WindowEventID.RESTORED:
                    OnRestored();
                    break;

                case SDL_WindowEventID.MOVED:
                    UpdateWindowedPosition(new Point2(msg.Event.window.data1, msg.Event.window.data2));
                    break;

                case SDL_WindowEventID.SIZE_CHANGED:
                    UpdateWindowedClientSize(new Size2(msg.Event.window.data1, msg.Event.window.data2));
                    break;
            }
        }

        /// <summary>
        /// Releases resources associated with the window.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                SDL.DestroyWindow(ptr);
                disposed = true;
            }
        }

        /// <summary>
        /// Sets the window's fullscreen display mode.
        /// </summary>
        /// <param name="displayMode">The fullscreen display mode to set, or null to use the desktop display mode.</param>
        public void SetFullscreenDisplayMode(DisplayMode displayMode)
        {
            Contract.EnsureNotDisposed(this, disposed);

            SetFullscreenDisplayModeInternal(displayMode);
        }

        /// <summary>
        /// Sets the window's fullscreen display mode.
        /// </summary>
        /// <param name="width">The display mode's width.</param>
        /// <param name="height">The display mode's height.</param>
        /// <param name="bpp">The display mode's bit depth.</param>
        /// <param name="refresh">The display mode's refresh rate in hertz.</param>
        public void SetFullscreenDisplayMode(Int32 width, Int32 height, Int32 bpp, Int32 refresh)
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");
            Contract.EnsureRange(bpp > 0, "bpp");
            Contract.EnsureRange(refresh > 0, "refresh");

            SetFullscreenDisplayModeInternal(new DisplayMode(width, height, bpp, refresh));
        }

        /// <summary>
        /// Gets the window's fullscreen display mode.
        /// </summary>
        /// <returns>The window's fullscreen display mode, or null if the window is using the desktop display mode.</returns>
        public DisplayMode GetFullscreenDisplayMode()
        {
            Contract.EnsureNotDisposed(this, disposed);

            return displayMode;
        }

        /// <summary>
        /// Sets the window's fullscreen/windowed mode.
        /// </summary>
        /// <param name="mode">The window mode to set.</param>
        public void SetWindowMode(WindowMode mode)
        {
            Contract.EnsureNotDisposed(this, disposed);

            if (windowMode != mode)
            {
                windowMode = mode;
                switch (mode)
                {
                    case WindowMode.Windowed:
                        SDL.SetWindowFullscreen(ptr, 0);
                        break;

                    case WindowMode.Fullscreen:
                        SDL.SetWindowFullscreen(ptr, (uint)SDL_WindowFlags.FULLSCREEN);
                        break;
                    
                    case WindowMode.FullscreenWindowed:
                        SDL.SetWindowFullscreen(ptr, (uint)SDL_WindowFlags.FULLSCREEN_DESKTOP);
                        break;
                    
                    default:
                        throw new NotSupportedException("mode");
                }
            }
        }

        /// <summary>
        /// Gets the fullscreen/windowed mode for this window.
        /// </summary>
        /// <returns>The window's current fullscreen/windowed mode.</returns>
        public WindowMode GetWindowMode()
        {
            Contract.EnsureNotDisposed(this, disposed);

            return windowMode;
        }

        /// <summary>
        /// Sets the window's maximization/minimization state.
        /// </summary>
        /// <param name="state">The window state to set.</param>
        public void SetWindowState(WindowState state)
        {
            switch (state)
            {
                case WindowState.Normal:
                    SDL.RestoreWindow(ptr);
                    break;

                case WindowState.Minimized:
                    SDL.MinimizeWindow(ptr);
                    break;

                case WindowState.Maximized:
                    SDL.MaximizeWindow(ptr);
                    break;

                default:
                    throw new NotSupportedException("state");
            }
        }

        /// <summary>
        /// Gets the window's maximization/minimization state.
        /// </summary>
        /// <returns>The window's current maximization/minimization state.</returns>
        public WindowState GetWindowState()
        {
            Contract.EnsureNotDisposed(this, disposed);

            var flags = SDL.GetWindowFlags(ptr);

            if ((flags & SDL_WindowFlags.MAXIMIZED) == SDL_WindowFlags.MAXIMIZED)
                return WindowState.Maximized;

            if ((flags & SDL_WindowFlags.MINIMIZED) == SDL_WindowFlags.MINIMIZED)
                return WindowState.Minimized;

            return WindowState.Normal;
        }

        /// <summary>
        /// Draws the window.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        public void Draw(UltravioletTime time)
        {
            OnDrawing(time);
            OnDrawingUI(time);
        }

        /// <summary>
        /// Gets the window's identifier within its windowing system.
        /// </summary>
        public Int32 ID
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return (Int32)SDL.GetWindowID(ptr);
            }
        }

        /// <summary>
        /// Gets or sets the window's caption.
        /// </summary>
        public String Caption
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return SDL.GetWindowTitle(ptr);
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                SDL.SetWindowTitle(ptr, value ?? String.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the window's position.
        /// </summary>
        public Point2 Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                Int32 x, y;
                SDL.GetWindowPosition(ptr, out x, out y);

                return new Point2(x, y);
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                SDL.SetWindowPosition(ptr, value.X, value.Y);
            }
        }

        /// <summary>
        /// Gets or sets the position to which the window will be restored upon entering non-maximized windowed mode.
        /// </summary>
        public Point2 WindowedPosition
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return windowedPosition.GetValueOrDefault();
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                windowedPosition = value;

                if (GetWindowMode() == WindowMode.Windowed && GetWindowState() == WindowState.Normal)
                {
                    Position = value;
                }
            }
        }

        /// <summary>
        /// Gets the window's client size.
        /// </summary>
        public Size2 ClientSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                Int32 w, h;
                SDL.GetWindowSize(ptr, out w, out h);

                return new Size2(w, h);
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                SDL.SetWindowSize(ptr, value.Width, value.Height);
            }
        }

        /// <summary>
        /// Gets or sets the client size to which the window will be restored upon entering non-maximized windowed mode.
        /// </summary>
        public Size2 WindowedClientSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return windowedClientSize.GetValueOrDefault();
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                windowedClientSize = value;

                if (GetWindowMode() == WindowMode.Windowed && GetWindowState() == WindowState.Normal)
                {
                    ClientSize = value;
                }
            }
        }

        /// <summary>
        /// Gets the window's maximum client size.
        /// </summary>
        public Size2 MaximumClientSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                Int32 w, h;
                SDL.GetWindowMaximumSize(ptr, out w, out h);

                return new Size2(w, h);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this window's rendering is synchronized
        /// with the vertical retrace (i.e, whether vsync is enabled).
        /// </summary>
        public Boolean SynchronizeWithVerticalRetrace
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return synchronizeWithVerticalRetrace;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                synchronizeWithVerticalRetrace = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the window is currently active.
        /// </summary>
        public Boolean Active
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                var flags = SDL.GetWindowFlags(ptr);

                return
                    ((flags & SDL_WindowFlags.MINIMIZED) != SDL_WindowFlags.MINIMIZED) &&
                    ((flags & SDL_WindowFlags.INPUT_FOCUS) != 0);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this window is visible.
        /// </summary>
        public Boolean Visible
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                var flags = SDL.GetWindowFlags(ptr);
                return (flags & SDL_WindowFlags.RESIZABLE) == SDL_WindowFlags.RESIZABLE;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                if (value)
                {
                    SDL.ShowWindow(ptr);
                }
                else
                {
                    SDL.HideWindow(ptr);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this window is resizable.
        /// </summary>
        public Boolean Resizable
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                var flags = SDL.GetWindowFlags(ptr);
                return (flags & SDL_WindowFlags.RESIZABLE) == SDL_WindowFlags.RESIZABLE;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this window is borderless.
        /// </summary>
        public Boolean Borderless
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                var flags = SDL.GetWindowFlags(ptr);
                return (flags & SDL_WindowFlags.BORDERLESS) == SDL_WindowFlags.BORDERLESS;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this window was created from a native window.
        /// </summary>
        public Boolean Native
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return native; 
            }
        }

        /// <summary>
        /// Gets or sets the window's icon.
        /// </summary>
        public Surface2D Icon
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return this.icon;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                SetIcon(value ?? DefaultWindowIcon);

                this.icon = value;
            }
        }

        /// <summary>
        /// Occurs when the window is shown.
        /// </summary>
        public event UltravioletWindowEventHandler Shown;

        /// <summary>
        /// Occurs when the window is hidden.
        /// </summary>
        public event UltravioletWindowEventHandler Hidden;

        /// <summary>
        /// Occurs when the window is minimized.
        /// </summary>
        public event UltravioletWindowEventHandler Minimized;

        /// <summary>
        /// Occurs when the window is maximized.
        /// </summary>
        public event UltravioletWindowEventHandler Maximized;

        /// <summary>
        /// Occurs when the window is restored.
        /// </summary>
        public event UltravioletWindowEventHandler Restored;

        /// <summary>
        /// Occurs when the window is rendered.
        /// </summary>
        public event UltravioletWindowDrawingEventHandler Drawing;

        /// <summary>
        /// Occurs when the window is drawing its UI layer.
        /// </summary>
        public event UltravioletWindowDrawingEventHandler DrawingUI;

        /// <summary>
        /// Loads the default window icon.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The default window icon.</returns>
        private static Surface2D LoadDefaultWindowIcon(UltravioletContext uv)
        {
            Contract.Require(uv, "uv");

            return IconLoader.Create().LoadIcon();
        }

        /// <summary>
        /// Fixes issues specific to the current platform.
        /// </summary>
        private void FixPlatformSpecificIssues()
        {
            SDL_SysWMinfo sysInfo;
            SDLMacro.SDL_VERSION(&sysInfo.version);

            if (!SDL.GetWindowWMInfo(ptr, &sysInfo))
                throw new SDL2Exception();

            switch (sysInfo.subsystem)
            {
                case SDL_SysWMType.WINDOWS:
                    FixPlatformSpecificIssues_Windows(ref sysInfo);
                    break;
            }
        }

        /// <summary>
        /// Fixes issues specific to the Windows platform.
        /// </summary>
        /// <param name="sysinfo">The current system information.</param>
        private void FixPlatformSpecificIssues_Windows(ref SDL_SysWMinfo sysinfo)
        {
            // NOTE: This fix prevents Windows from playing the "ding" sound when
            // a key binding involving the Alt modifier is pressed.
            wndProc = new Win32Native.WndProcDelegate((hWnd, msg, wParam, lParam) =>
            {
                const Int32 WM_SYSCOMMAND = 0x0112;
                const Int32 SC_KEYMENU = 0xF100;
                if (msg == WM_SYSCOMMAND && (wParam.ToInt64() & 0xfff0) == SC_KEYMENU)
                {
                    return IntPtr.Zero;
                }
                return Win32Native.CallWindowProc(wndProcPrev, hWnd, msg, wParam, lParam);
            });

            const int GWLP_WNDPROC = -4;
            wndProcPrev = Win32Native.SetWindowLongPtr(sysinfo.info.win.window, GWLP_WNDPROC,
                Marshal.GetFunctionPointerForDelegate(wndProc));
        }

        /// <summary>
        /// Sets the window's fullscreen display mode.
        /// </summary>
        /// <param name="displayMode">The fullscreen display mode to set, or null to use the desktop display mode.</param>
        private void SetFullscreenDisplayModeInternal(DisplayMode displayMode)
        {
            if (displayMode == null)
            {
                if (SDL.SetWindowDisplayMode(ptr, null) < 0)
                    throw new SDL2Exception();
            }
            else
            {
                SDL_DisplayMode sdlMode;
                sdlMode.w = displayMode.Width;
                sdlMode.h = displayMode.Height;
                sdlMode.refresh_rate = displayMode.RefreshRate;
                switch (displayMode.BitsPerPixel)
                {
                    case 16:
                        sdlMode.format = SDL_PixelFormatEnum.RGB565;
                        break;
                    
                    case 24:
                        sdlMode.format = SDL_PixelFormatEnum.RGB888;
                        break;

                    default:
                        sdlMode.format = SDL_PixelFormatEnum.ARGB8888;
                        break;
                }

                if (SDL.SetWindowDisplayMode(ptr, &sdlMode) < 0)
                    throw new SDL2Exception();

                if (SDL.GetWindowDisplayMode(ptr, &sdlMode) < 0)
                    throw new SDL2Exception();

                var bpp = SDLMacro.BITSPERPIXEL(sdlMode.format);
                displayMode = new DisplayMode(sdlMode.w, sdlMode.h, bpp, sdlMode.refresh_rate);
            }
            this.displayMode = displayMode;
        }

        /// <summary>
        /// Sets the window's icon.
        /// </summary>
        /// <param name="surface">The surface that contains the icon to set.</param>
        private void SetIcon(Surface2D surface)
        {
            var surfptr = (surface == null) ? null : ((OpenGLSurface2D)surface).Native;
            SDL.SetWindowIcon(ptr, (IntPtr)surfptr);
        }

        /// <summary>
        /// Raises the Drawing event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        private void OnDrawing(UltravioletTime time)
        {
            var temp = Drawing;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        /// <summary>
        /// Raises the DrawingUI event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        private void OnDrawingUI(UltravioletTime time)
        {
            var temp = DrawingUI;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        /// <summary>
        /// Raises the Shown event.
        /// </summary>
        private void OnShown()
        {
            var temp = Shown;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the Hidden event.
        /// </summary>
        private void OnHidden()
        {
            var temp = Hidden;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the Maximized event.
        /// </summary>
        private void OnMaximized()
        {
            var temp = Maximized;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the Minimized event.
        /// </summary>
        private void OnMinimized()
        {
            var temp = Minimized;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the Restored event.
        /// </summary>
        private void OnRestored()
        {
            var temp = Restored;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Updates the window's windowed position, if it is currently in the correct mode and state.
        /// </summary>
        /// <param name="position">The new windowed position.</param>
        private void UpdateWindowedPosition(Point2 position)
        {
            if (windowedPosition == null || (GetWindowState() == WindowState.Normal && GetWindowMode() == WindowMode.Windowed))
            {
                windowedPosition = position;
            }
        }

        /// <summary>
        /// Updates the window's windowed client size, if it is currently in the correct mode and state.
        /// </summary>
        /// <param name="size">The new windowed client size.</param>
        private void UpdateWindowedClientSize(Size2 size)
        {
            if (windowedClientSize == null || (GetWindowState() == WindowState.Normal && GetWindowMode() == WindowMode.Windowed))
            {
                windowedClientSize = size;
            }
        }

        // A custom Win32 windows procedure used to override SDL2's built-in functionality.
        private Win32Native.WndProcDelegate wndProc;
        private IntPtr wndProcPrev;

        // Property values.
        private readonly UInt32 id;
        private Point2? windowedPosition;
        private Size2? windowedClientSize;
        private Boolean synchronizeWithVerticalRetrace;
        private readonly Boolean native;
        private Surface2D icon;

        // State values.
        private readonly IntPtr ptr;
        private WindowMode windowMode = WindowMode.Windowed;
        private DisplayMode displayMode;
        private Boolean disposed;

        // The default window icon.
        private static readonly UltravioletSingleton<Surface2D> DefaultWindowIcon = new UltravioletSingleton<Surface2D>((uv) =>
        {
            return LoadDefaultWindowIcon(uv);
        });
    }
}

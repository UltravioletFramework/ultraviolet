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
    public sealed unsafe partial class OpenGLUltravioletWindow : UltravioletResource,
        IMessageSubscriber<UltravioletMessageID>,
        IUltravioletWindow        
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletWindow class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="ptr">The SDL2 pointer that represents the window.</param>
        /// <param name="native">A value indicating whether the window was created from a native pointer.</param>
        internal OpenGLUltravioletWindow(UltravioletContext uv, IntPtr ptr, Boolean native = false)
            : base(uv)
        {
            this.ptr = ptr;
            this.id = SDL.GetWindowID(ptr);
            this.native = native;

            SetIcon(DefaultWindowIcon.Value);

            FixPlatformSpecificIssues();

            UpdateWindowedPosition(Position);
            UpdateWindowedClientSize(ClientSize);

            var flags = SDL.GetWindowFlags(ptr);

            this.focused = (flags & SDL_WindowFlags.INPUT_FOCUS) == SDL_WindowFlags.INPUT_FOCUS;
            this.minimized = (flags & SDL_WindowFlags.MINIMIZED) == SDL_WindowFlags.MINIMIZED;

            ChangeCompositor(DefaultCompositor.Create(this));            
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

        /// <inheritdoc/>
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
                    minimized = true;
                    OnMinimized();
                    break;

                case SDL_WindowEventID.MAXIMIZED:
                    minimized = false;
                    OnMaximized();
                    break;

                case SDL_WindowEventID.RESTORED:
                    minimized = false;
                    OnRestored();
                    break;

                case SDL_WindowEventID.MOVED:
                    UpdateWindowedPosition(new Point2(msg.Event.window.data1, msg.Event.window.data2));
                    break;

                case SDL_WindowEventID.SIZE_CHANGED:
                    UpdateWindowedClientSize(new Size2(msg.Event.window.data1, msg.Event.window.data2));
                    break;

                case SDL_WindowEventID.FOCUS_GAINED:
                    focused = true;
                    break;

                case SDL_WindowEventID.FOCUS_LOST:
                    focused = false;
                    break;
            }
        }

        /// <inheritdoc/>
        public void SetFullscreenDisplayMode(DisplayMode displayMode)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SetFullscreenDisplayModeInternal(displayMode);
        }

        /// <inheritdoc/>
        public void SetFullscreenDisplayMode(Int32 width, Int32 height, Int32 bpp, Int32 refresh, Int32? displayIndex = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");
            Contract.EnsureRange(bpp > 0, "bpp");
            Contract.EnsureRange(refresh > 0, "refresh");

            if (displayIndex.HasValue)
            {
                var displayIndexValue = displayIndex.Value;
                if (displayIndexValue < 0 || displayIndexValue >= Ultraviolet.GetPlatform().Displays.Count)
                    throw new ArgumentOutOfRangeException(nameof(displayIndex));
            }

            SetFullscreenDisplayModeInternal(new DisplayMode(width, height, bpp, refresh, displayIndex));
        }

        /// <inheritdoc/>
        public DisplayMode GetFullscreenDisplayMode()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return displayMode;
        }

        /// <inheritdoc/>
        public void SetWindowMode(WindowMode mode)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (windowMode == mode)
                return;

            UpdateWindowedPosition(Position);
            UpdateWindowedClientSize(ClientSize);

            switch (mode)
            {
                case WindowMode.Windowed:
                    {
                        if (SDL.SetWindowFullscreen(ptr, 0) < 0)
                            throw new SDL2Exception();

                        var x = windowedPosition?.X ?? UltravioletConfiguration.DefaultWindowPositionX;
                        var y = windowedPosition?.Y ?? UltravioletConfiguration.DefaultWindowPositionY;
                        var w = windowedClientSize?.Width ?? UltravioletConfiguration.DefaultWindowClientWidth;
                        var h = windowedClientSize?.Height ?? UltravioletConfiguration.DefaultWindowClientHeight;

                        if (!ApplyWin32FullscreenWindowedFix_Windowed())
                            SDL.SetWindowBordered(ptr, true);

                        SDL.SetWindowSize(ptr, w, h);
                        SDL.SetWindowPosition(ptr, x, y);
                        
                        if (Ultraviolet.Platform == UltravioletPlatform.Windows)
                            win32CachedStyle = IntPtr.Zero;
                    }
                    break;

                case WindowMode.Fullscreen:
                    {
                        if (displayMode != null)
                        {
                            if (displayMode.DisplayIndex.HasValue)
                            {
                                var display = Ultraviolet.GetPlatform().Displays[displayMode.DisplayIndex.Value];
                                ChangeDisplay(display);
                            }
                        }
                        else
                        {
                            SetDesktopDisplayMode();
                        }

                        if (SDL.SetWindowFullscreen(ptr, (uint)SDL_WindowFlags.FULLSCREEN) < 0)
                            throw new SDL2Exception();

                        if (Ultraviolet.Platform == UltravioletPlatform.Windows)
                            win32CachedStyle = IntPtr.Zero;
                    }
                    break;

                case WindowMode.FullscreenWindowed:
                    {
                        if (SDL.SetWindowFullscreen(ptr, 0) < 0)
                            throw new SDL2Exception();

                        var displayBounds = Display.Bounds;

                        if (!ApplyWin32FullscreenWindowedFix_FullscreenWindowed())
                            SDL.SetWindowBordered(ptr, false);
                        
                        SDL.SetWindowSize(ptr, displayBounds.Width, displayBounds.Height);
                        SDL.SetWindowPosition(ptr, displayBounds.X, displayBounds.Y);
                    }
                    break;

                default:
                    throw new NotSupportedException(nameof(mode));
            }

            windowMode = mode;
            UpdateMouseGrab();
        }

        /// <inheritdoc/>
        public WindowMode GetWindowMode()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return windowMode;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public WindowState GetWindowState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var flags = SDL.GetWindowFlags(ptr);

            if ((flags & SDL_WindowFlags.MAXIMIZED) == SDL_WindowFlags.MAXIMIZED)
                return WindowState.Maximized;

            if ((flags & SDL_WindowFlags.MINIMIZED) == SDL_WindowFlags.MINIMIZED)
                return WindowState.Minimized;

            return WindowState.Normal;
        }

        /// <inheritdoc/>
        public void ChangeCompositor(Compositor compositor)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (compositor.Window != this)
                throw new InvalidOperationException(UltravioletStrings.CompositorAssociatedWithWrongWindow);

            if (IsCurrentWindow)
                throw new InvalidOperationException(UltravioletStrings.CannotChangeCompositorWhileCurrent);

            if (this.compositor != null)
                this.compositor.Dispose();

            this.compositor = compositor ?? DefaultCompositor.Create(this);
        }

        /// <inheritdoc/>
        public void ChangeDisplay(Int32 displayIndex)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (displayIndex < 0 || displayIndex >= Ultraviolet.GetPlatform().Displays.Count)
                displayIndex = 0;

            var display = Ultraviolet.GetPlatform().Displays[displayIndex];
            ChangeDisplay(display);
        }

        /// <inheritdoc/>
        public void ChangeDisplay(IUltravioletDisplay display)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(display, nameof(display));

            if (Display == display)
                return;

            var x = display.Bounds.Center.X - (ClientSize.Width / 2);
            var y = display.Bounds.Center.Y - (ClientSize.Height / 2);

            Position = new Point2(x, y);
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

        /// <inheritdoc/>
        public Int32 ID
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return (Int32)SDL.GetWindowID(ptr);
            }
        }

        /// <inheritdoc/>
        public String Caption
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return SDL.GetWindowTitle(ptr);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                SDL.SetWindowTitle(ptr, value ?? String.Empty);
            }
        }

        /// <inheritdoc/>
        public Point2 Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                Int32 x, y;
                SDL.GetWindowPosition(ptr, out x, out y);

                return new Point2(x, y);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (GetWindowMode() == WindowMode.Windowed && GetWindowState() == WindowState.Normal)
                {
                    windowedPosition = value;
                }

                SDL.SetWindowPosition(ptr, value.X, value.Y);
            }
        }

        /// <inheritdoc/>
        public Point2 WindowedPosition
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return windowedPosition.GetValueOrDefault();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                windowedPosition = value;

                if (GetWindowMode() == WindowMode.Windowed && GetWindowState() == WindowState.Normal)
                {
                    SDL.SetWindowPosition(ptr, value.X, value.Y);
                }
            }
        }

        /// <inheritdoc/>
        public Size2 ClientSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                Int32 w, h;
                SDL.GetWindowSize(ptr, out w, out h);

                return new Size2(w, h);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                if (GetWindowMode() == WindowMode.Windowed && GetWindowState() == WindowState.Normal)
                {
                    windowedClientSize = value;
                }

                SDL.SetWindowSize(ptr, value.Width, value.Height);
            }
        }
        
        /// <inheritdoc/>
        public Size2 WindowedClientSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return windowedClientSize.GetValueOrDefault();
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                windowedClientSize = value;

                if (GetWindowMode() == WindowMode.Windowed && GetWindowState() == WindowState.Normal)
                {
                    SDL.SetWindowSize(ptr, value.Width, value.Height);
                }
            }
        }

        /// <inheritdoc/>
        public Size2 MinimumClientSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                Int32 w, h;
                SDL.GetWindowMinimumSize(ptr, out w, out h);

                return new Size2(w, h);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                SDL.SetWindowMinimumSize(ptr, value.Width, value.Height);
            }
        }

        /// <inheritdoc/>
        public Size2 MaximumClientSize
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                Int32 w, h;
                SDL.GetWindowMaximumSize(ptr, out w, out h);

                return new Size2(w, h);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                SDL.SetWindowMaximumSize(ptr, value.Width, value.Height);
            }
        }

        /// <inheritdoc/>
        public Boolean SynchronizeWithVerticalRetrace
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return synchronizeWithVerticalRetrace;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                synchronizeWithVerticalRetrace = value;
            }
        }

        /// <inheritdoc/>
        public Boolean Active
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return focused && !minimized;
            }
        }

        /// <inheritdoc/>
        public Boolean Visible
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var flags = SDL.GetWindowFlags(ptr);
                return (flags & SDL_WindowFlags.RESIZABLE) == SDL_WindowFlags.RESIZABLE;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

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

        /// <inheritdoc/>
        public Boolean Resizable
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var flags = SDL.GetWindowFlags(ptr);
                return (flags & SDL_WindowFlags.RESIZABLE) == SDL_WindowFlags.RESIZABLE;
            }
        }

        /// <inheritdoc/>
        public Boolean Borderless
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var flags = SDL.GetWindowFlags(ptr);
                return (flags & SDL_WindowFlags.BORDERLESS) == SDL_WindowFlags.BORDERLESS;
            }
        }

        /// <inheritdoc/>
        public Boolean Native
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return native; 
            }
        }

        /// <inheritdoc/>
        public Boolean GrabsMouseWhenWindowed
        {
            get { return grabsMouseWhenWindowed; }
            set
            {
                grabsMouseWhenWindowed = value;
                UpdateMouseGrab();
            }
        }

        /// <inheritdoc/>
        public Boolean GrabsMouseWhenFullscreenWindowed
        {
            get { return grabsMouseWhenFullscreenWindowed; }
            set
            {
                grabsMouseWhenFullscreenWindowed = value;
                UpdateMouseGrab();
            }
        }

        /// <inheritdoc/>
        public Boolean GrabsMouseWhenFullscreen
        {
            get { return grabsMouseWhenFullscreen; }
            set
            {
                grabsMouseWhenFullscreen = value;
                UpdateMouseGrab();
            }
        }

        /// <inheritdoc/>
        public Surface2D Icon
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.icon;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                SetIcon(value ?? DefaultWindowIcon);

                this.icon = value;
            }
        }

        /// <inheritdoc/>
        public Compositor Compositor
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return compositor;
            }
        }

        /// <inheritdoc/>
        public IUltravioletDisplay Display
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                var index = SDL.GetWindowDisplayIndex(ptr);
                var platform = Ultraviolet.GetPlatform();
                if (platform != null)
                    return Ultraviolet.GetPlatform().Displays[index];

                return null;
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
        /// Gets or sets a value indicating whether this is the current window.
        /// </summary>
        internal Boolean IsCurrentWindow
        {
            get;
            set;
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(compositor);
            }
            SDL.DestroyWindow(ptr);
            base.Dispose(disposing);
        }

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
                return;

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
                SetDesktopDisplayMode();
            }
            else
            {
                SDL_DisplayMode sdlMode;
                sdlMode.w = displayMode.Width;
                sdlMode.h = displayMode.Height;
                sdlMode.refresh_rate = displayMode.RefreshRate;
                switch (displayMode.BitsPerPixel)
                {
                    case 15:
                        sdlMode.format = SDL_PixelFormatEnum.RGB555;
                        break;

                    case 16:
                        sdlMode.format = SDL_PixelFormatEnum.RGB565;
                        break;
                    
                    default:
                        sdlMode.format = SDL_PixelFormatEnum.RGB888;
                        break;
                }

                var wasFullscreen = windowMode == WindowMode.Fullscreen;
                if (wasFullscreen)
                    SetWindowMode(WindowMode.Windowed);

                if (SDL.SetWindowDisplayMode(ptr, &sdlMode) < 0)
                    throw new SDL2Exception();

                if (wasFullscreen)
                {
                    if (displayMode.DisplayIndex.HasValue)
                    {
                        ChangeDisplay(displayMode.DisplayIndex.Value);
                    }
                    SetWindowMode(WindowMode.Fullscreen);
                }

                if (SDL.GetWindowDisplayMode(ptr, &sdlMode) < 0)
                    throw new SDL2Exception();

                int bpp;
                uint Rmask, Gmask, Bmask, Amask;
                SDL.PixelFormatEnumToMasks((uint)sdlMode.format, &bpp, &Rmask, &Gmask, &Bmask, &Amask);

                var displayIndex = displayMode.DisplayIndex;
                if (displayIndex.HasValue)
                {
                    if (displayIndex < 0 || displayIndex >= Ultraviolet.GetPlatform().Displays.Count)
                        displayIndex = null;
                }

                displayMode = new DisplayMode(sdlMode.w, sdlMode.h, bpp, sdlMode.refresh_rate, displayIndex);
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

        /// <summary>
        /// Updates the window's mouse grab state.
        /// </summary>
        private void UpdateMouseGrab()
        {
            switch (windowMode)
            {
                case WindowMode.Windowed:
                    SDL.SetWindowGrab(ptr, grabsMouseWhenWindowed);
                    break;

                case WindowMode.Fullscreen:
                    SDL.SetWindowGrab(ptr, grabsMouseWhenFullscreen);
                    break;

                case WindowMode.FullscreenWindowed:
                    SDL.SetWindowGrab(ptr, grabsMouseWhenFullscreenWindowed);
                    break;
            }
        }

        /// <summary>
        /// Sets the window to use the desktop display mode for its current display.
        /// </summary>
        private void SetDesktopDisplayMode()
        {
            SDL_DisplayMode mode;
            if (SDL.GetDesktopDisplayMode(Display.Index, &mode) < 0)
                throw new SDL2Exception();

            if (SDL.SetWindowDisplayMode(ptr, &mode) < 0)
                throw new SDL2Exception();
        }

        /// <summary>
        /// Retrieves the HWND value for this window on the Windows platform.
        /// </summary>
        private Boolean GetHwnd(out IntPtr hwnd)
        {
            if (Ultraviolet.Platform != UltravioletPlatform.Windows)
                throw new NotSupportedException();

            SDL_SysWMinfo sysInfo;
            SDLMacro.SDL_VERSION(&sysInfo.version);

            if (!SDL.GetWindowWMInfo(ptr, &sysInfo))
            {
                hwnd = IntPtr.Zero;
                return false;
            }
            hwnd = sysInfo.info.win.window;
            return true;
        }

        /// <summary>
        /// Applies a fix to the window's styles on the Windows platform which addresses
        /// the flickering which has been observed when using ALT+TAB while the application
        /// is in fullscreen windowed mode.
        /// </summary>
        private Boolean ApplyWin32FullscreenWindowedFix_Windowed()
        {
            if (Ultraviolet.Platform != UltravioletPlatform.Windows || win32CachedStyle == IntPtr.Zero)
                return false;

            IntPtr hwnd;
            if (!GetHwnd(out hwnd))
                return false;

            Win32Native.SetWindowLongPtr(hwnd, Win32Native.GWL_STYLE, win32CachedStyle);
            return true;
        }

        /// <summary>
        /// Applies a fix to the window's styles on the Windows platform which addresses
        /// the flickering which has been observed when using ALT+TAB while the application
        /// is in fullscreen windowed mode.
        /// </summary>
        private Boolean ApplyWin32FullscreenWindowedFix_FullscreenWindowed()
        {
            if (Ultraviolet.Platform != UltravioletPlatform.Windows)
                return false;

            IntPtr hwnd;
            if (!GetHwnd(out hwnd))
                return false;

            win32CachedStyle = Win32Native.GetWindowLongPtr(hwnd, Win32Native.GWL_STYLE);

            var style = (UInt32)win32CachedStyle & ~(Win32Native.WS_DLGFRAME | Win32Native.WS_BORDER);
            Win32Native.SetWindowLongPtr(hwnd, Win32Native.GWL_STYLE, (IntPtr)style);

            return true;
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
        private Boolean grabsMouseWhenWindowed;
        private Boolean grabsMouseWhenFullscreenWindowed;
        private Boolean grabsMouseWhenFullscreen;
        private Surface2D icon;
        private Compositor compositor;

        // State values.
        private readonly IntPtr ptr;
        private WindowMode windowMode = WindowMode.Windowed;
        private DisplayMode displayMode;
        private Boolean focused;
        private Boolean minimized;

        // HACK: Cached style from before entering fullscreen windowed mode.
        private IntPtr win32CachedStyle;

        // The default window icon.
        private static readonly UltravioletSingleton<Surface2D> DefaultWindowIcon = new UltravioletSingleton<Surface2D>(
            uv => LoadDefaultWindowIcon(uv));
    }
}

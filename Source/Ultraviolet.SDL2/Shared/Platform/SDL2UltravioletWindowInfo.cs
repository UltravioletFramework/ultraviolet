using System;
using System.Collections;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Core.Text;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_Hint;
using static Ultraviolet.SDL2.Native.SDL_WindowFlags;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Platform
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="IUltravioletWindowInfo"/> interface.
    /// </summary>
    public abstract class SDL2UltravioletWindowInfo : IUltravioletWindowInfo, IUltravioletComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2UltravioletWindowInfo"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="uvconfig">The Ultraviolet configuration settings for the current context.</param>
        /// <param name="winconfig">The window configuration settings for the current context.</param>
        internal SDL2UltravioletWindowInfo(UltravioletContext uv, UltravioletConfiguration uvconfig, SDL2PlatformConfiguration winconfig)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(uvconfig, nameof(uvconfig));
            Contract.Require(winconfig, nameof(winconfig));

            this.uv = uv;

            InitializePrimaryWindow(uvconfig, winconfig);
        }
        
        /// <summary>
        /// Updates the state of the application's displays.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public void Update(UltravioletTime time)
        {
            foreach (var window in windows)
                ((SDL2UltravioletWindow)window).Update(time);
        }

        /// <summary>
        /// Designates the specified window as the primary window.
        /// </summary>
        /// <param name="window">The window to designate as the primary window, or null to clear the primary window.</param>
        public void DesignatePrimary(IUltravioletWindow window)
        {
            if (window != null && !windows.Contains(window))
                throw new InvalidOperationException(UltravioletStrings.InvalidResource);

            if (primary != window)
            {
                OnPrimaryWindowChanging();

                primary = window;

                OnPrimaryWindowChanged();
            }
        }
        
        /// <summary>
        /// Gets the window with the specified identifier.
        /// </summary>
        /// <returns>The window with the specified identifier, or null if no such window exists.</returns>
        public IUltravioletWindow GetByID(Int32 id)
        {
            var match = default(SDL2UltravioletWindow);
            foreach (SDL2UltravioletWindow window in windows)
            {
                if (SDL_GetWindowID((IntPtr)window) == (UInt32)id)
                {
                    match = window;
                    break;
                }
            }
            return match;
        }

        /// <summary>
        /// Gets a pointer to the SDL2 window object encapsulated by the window with the specified identifier.
        /// </summary>
        /// <returns>A pointer to the SDL2 window object encapsulated by the window with the specified identifier.</returns>
        public IntPtr GetPtrByID(Int32 id)
        {
            var window = GetByID(id);
            if (window != null)
            {
                return (IntPtr)(SDL2UltravioletWindow)window;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// Gets the context's master window, which is used to create the OpenGL context.
        /// </summary>
        /// <returns>The context's master window.</returns>
        public IUltravioletWindow GetMaster()
        {
            return master;
        }

        /// <summary>
        /// Gets a pointer to the SDL2 window object encapsulated by the master window.
        /// </summary>
        /// <returns>A pointer to the SDL2 window object encapsulated by the master window.</returns>
        public IntPtr GetMasterPointer()
        {
            return (IntPtr)(SDL2UltravioletWindow)master;
        }

        /// <summary>
        /// Gets the context's primary window.
        /// </summary>
        /// <returns>The context's primary window, or null if the context is headless.</returns>
        public IUltravioletWindow GetPrimary()
        {
            return primary;
        }

        /// <summary>
        /// Gets a pointer to the SDL2 window object encapsulated by the primary window.
        /// </summary>
        /// <returns>A pointer to the SDL2 window object encapsulated by the primary window.</returns>
        public IntPtr GetPrimaryPointer()
        {
            return (IntPtr)(SDL2UltravioletWindow)primary;
        }

        /// <summary>
        /// Gets the context's current window.
        /// </summary>
        /// <returns>The context's current window.</returns>
        public IUltravioletWindow GetCurrent()
        {
            return current;
        }

        /// <summary>
        /// Gets a pointer to the SDL2 window object encapsulated by the current window.
        /// </summary>
        /// <returns>A pointer to the SDL2 window object encapsulated by the current window.</returns>
        public IntPtr GetCurrentPointer()
        {
            return (IntPtr)(SDL2UltravioletWindow)current;
        }

        /// <summary>
        /// Creates a new window and attaches it to the current context.
        /// </summary>
        /// <param name="caption">The window's caption text.</param>
        /// <param name="x">The x-coordinate at which to position the window's top-left corner.</param>
        /// <param name="y">The y-coordinate at which to position the window's top-left corner.</param>
        /// <param name="width">The width of the window's client area in pixels.</param>
        /// <param name="height">The height of the window's client area in pixels.</param>
        /// <param name="flags">A set of WindowFlags values indicating how to create the window.</param>
        /// <returns>The Ultraviolet window that was created.</returns>
        public IUltravioletWindow Create(String caption, Int32 x, Int32 y, Int32 width, Int32 height, WindowFlags flags = WindowFlags.None)
        {
            var sdlflags = (renderingAPI == SDL2PlatformRenderingAPI.OpenGL) ? SDL_WINDOW_OPENGL : 0;

            if (Ultraviolet.Properties.SupportsHighDensityDisplayModes)
                sdlflags |= SDL_WINDOW_ALLOW_HIGHDPI;

            if ((flags & WindowFlags.Resizable) == WindowFlags.Resizable)
                sdlflags |= SDL_WINDOW_RESIZABLE;

            if ((flags & WindowFlags.Borderless) == WindowFlags.Borderless)
                sdlflags |= SDL_WINDOW_BORDERLESS;

            // We force it to be hidden so we can clear the window to the desired color first!
            sdlflags |= SDL_WINDOW_HIDDEN;

            var sdlptr = SDL_CreateWindow(caption ?? String.Empty,
                x == -1 ? (int) SDL_WINDOWPOS_CENTERED_MASK : x,
                y == -1 ? (int) SDL_WINDOWPOS_CENTERED_MASK : y,
                width, height, sdlflags);
            
            if (sdlptr == IntPtr.Zero)
                throw new SDL2Exception();

            var win = new SDL2UltravioletWindow(Ultraviolet, sdlptr, flags);
            windows.Add(win);

            Ultraviolet.Messages.Subscribe(win, SDL2UltravioletMessages.SDLEvent);

            OnWindowCreated(win);

            return win;
        }

        /// <summary>
        /// Creates a new Ultraviolet window from the specified native window and attaches it to the current context.
        /// </summary>
        /// <param name="ptr">A pointer that represents the native window to attach to the context.</param>
        /// <returns>The Ultraviolet window that was created.</returns>
        public IUltravioletWindow CreateFromNativePointer(IntPtr ptr)
        {
            var sdlptr = SDL_CreateWindowFrom(ptr);
            if (sdlptr == IntPtr.Zero)
                throw new SDL2Exception();

            var win = new SDL2UltravioletWindow(Ultraviolet, sdlptr, WindowFlags.None);
            windows.Add(win);

            Ultraviolet.Messages.Subscribe(win, SDL2UltravioletMessages.SDLEvent);

            OnWindowCreated(win);

            return win;
        }

        /// <summary>
        /// Destroys the specified window.
        /// </summary>
        /// <remarks>Windows which were created from native pointers are disassociated from the current context,
        /// but are not actually destroyed.  To destroy such windows, use the native framework which created them.</remarks>
        /// <param name="window">The Ultraviolet window to destroy.</param>
        /// <returns>true if the window was destroyed; false if the window was closed.</returns>
        public Boolean Destroy(IUltravioletWindow window)
        {
            Contract.Require(window, nameof(window));

            if (!windows.Remove(window))
                throw new InvalidOperationException(UltravioletStrings.InvalidResource);

            if (window == current)
                throw new InvalidOperationException();

            OnWindowDestroyed(window);

            if (window == primary)
                DesignatePrimary(null);

            OnWindowCleanup(window);

            var sdlwin = (SDL2UltravioletWindow)window;
            Ultraviolet.Messages.Unsubscribe(sdlwin, SDL2UltravioletMessages.SDLEvent);

            var native = sdlwin.Native;
            sdlwin.Dispose();

            return !native;
        }

        /// <summary>
        /// Destroys the window with the specified identifier.
        /// </summary>
        /// <param name="windowID">The identifier of the window to destroy.</param>
        /// <returns>true if the window was destroyed; false if the window was closed.</returns>
        public Boolean DestroyByID(Int32 windowID)
        {
            var window = GetByID(windowID);
            if (window != null)
            {
                Destroy(window);
            }
            return windows.Count == 0;
        }

        /// <summary>
        /// Gets the collection's enumerator.
        /// </summary>
        /// <returns>The collection's enumerator.</returns>
        public List<IUltravioletWindow>.Enumerator GetEnumerator()
        {
            return windows.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>The collection's enumerator.</returns>
        IEnumerator<IUltravioletWindow> IEnumerable<IUltravioletWindow>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection.
        /// </summary>
        /// <returns>The collection's enumerator.</returns>
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get { return uv; }
        }

        /// <summary>
        /// Occurs after a window has been created.
        /// </summary>
        public event UltravioletWindowInfoEventHandler WindowCreated;

        /// <summary>
        /// Occurs when a window is about to be destroyed.
        /// </summary>
        public event UltravioletWindowInfoEventHandler WindowDestroyed;

        /// <summary>
        /// Occurs when the primary window is about to change.
        /// </summary>
        public event UltravioletWindowInfoEventHandler PrimaryWindowChanging;

        /// <summary>
        /// Occurs when the primary window changes.
        /// </summary>
        public event UltravioletWindowInfoEventHandler PrimaryWindowChanged;

        /// <summary>
        /// Occurs when the current window is about to change.
        /// </summary>
        public event UltravioletWindowInfoEventHandler CurrentWindowChanged;

        /// <summary>
        /// Occurs when the current window changes.
        /// </summary>
        public event UltravioletWindowInfoEventHandler CurrentWindowChanging;

        /// <summary>
        /// Initializes the rendering API.
        /// </summary>
        /// <param name="sdlconfig">The platform configuration settings.</param>
        protected virtual void InitializeRenderingAPI(SDL2PlatformConfiguration sdlconfig)
        {

        }

        /// <summary>
        /// Initializes the rendering API using lower settings in the event that the initial attempt
        /// to initialize the API failed.
        /// </summary>
        /// <param name="sdlconfig">The platform configuration settings.</param>
        /// <param name="attempt">The number of attempts which have been made to find a working fallback configuration.</param>
        /// <returns><see langword="true"/> if a fallback configuration was provided; otherwise, <see langword="false"/>.</returns>
        protected virtual Boolean InitializeRenderingAPIFallback(SDL2PlatformConfiguration sdlconfig, Int32 attempt)
        {
            return false;
        }

        /// <summary>
        /// Cleans up a window's resources after it is destroyed.
        /// </summary>
        /// <param name="window">The window that was destroyed.</param>
        protected virtual void OnWindowCleanup(IUltravioletWindow window)
        { }

        /// <summary>
        /// Raises the WindowCreated event.
        /// </summary>
        /// <param name="window">The window that was created.</param>
        protected void OnWindowCreated(IUltravioletWindow window) =>
            WindowCreated?.Invoke(window);

        /// <summary>
        /// Raises the WindowDestroyed event.
        /// </summary>
        /// <param name="window">The window that is being destroyed.</param>
        protected void OnWindowDestroyed(IUltravioletWindow window) =>
            WindowDestroyed?.Invoke(window);

        /// <summary>
        /// Raises the PrimaryWindowChanging event.
        /// </summary>
        protected void OnPrimaryWindowChanging() =>
            PrimaryWindowChanging?.Invoke(primary);

        /// <summary>
        /// Raises the PrimaryWindowChanged event.
        /// </summary>
        protected void OnPrimaryWindowChanged() =>
            PrimaryWindowChanged?.Invoke(primary);

        /// <summary>
        /// Raises the CurrentWindowChanging event.
        /// </summary>
        protected void OnCurrentWindowChanging() =>
            CurrentWindowChanging?.Invoke(current);

        /// <summary>
        /// Raises the CurrentWindowChanged event.
        /// </summary>
        protected void OnCurrentWindowChanged() =>
            CurrentWindowChanged?.Invoke(current);

        /// <summary>
        /// Gets the window manager's list of windows.
        /// </summary>
        protected IList<IUltravioletWindow> Windows { get { return windows; } }

        /// <summary>
        /// Gets or sets the master window.
        /// </summary>
        protected IUltravioletWindow Master
        {
            get { return master; }
            set { master = value; }
        }

        /// <summary>
        /// Gets or sets the primary window.
        /// </summary>
        protected IUltravioletWindow Primary
        {
            get { return primary; }
            set { primary = value; }
        }

        /// <summary>
        /// Gets or sets the current window.
        /// </summary>
        protected IUltravioletWindow Current
        {
            get { return current; }
            set { current = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether vertical sync is enabled.
        /// </summary>
        protected Boolean VSync
        {
            get { return vsync; }
            set { vsync = value; }
        }

        /// <summary>
        /// Initializes the context's primary window.
        /// </summary>
        private void InitializePrimaryWindow(UltravioletConfiguration uvconfig, SDL2PlatformConfiguration sdlconfig)
        {
            // Initialize the rendering API.
            if (sdlconfig.RenderingAPI != SDL2PlatformRenderingAPI.OpenGL)
                throw new NotSupportedException();

            InitializeRenderingAPI(sdlconfig);
            renderingAPI = sdlconfig.RenderingAPI;

            // Retrieve the caption for our window.
            var caption = Localization.Strings.Contains("WINDOW_CAPTION") ?
                Localization.Get("WINDOW_CAPTION") : UltravioletStrings.DefaultWindowCaption.Value;

            // If we're running on Android or iOS, we can't create a headless context.
            var isRunningOnMobile = (Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS);
            if (isRunningOnMobile && uvconfig.Headless)
                throw new InvalidOperationException(SDL2Strings.CannotCreateHeadlessContextOnMobile);

            // Initialize the hidden master window used to create the OpenGL context.
            var masterWidth = 0;
            var masterHeight = 0;
            var masterFlags = (renderingAPI == SDL2PlatformRenderingAPI.OpenGL) ? SDL_WINDOW_OPENGL : 0;

            if (Ultraviolet.Properties.SupportsHighDensityDisplayModes)
                masterFlags |= SDL_WINDOW_ALLOW_HIGHDPI;

            if (isRunningOnMobile)
            {
                masterFlags |= SDL_WINDOW_FULLSCREEN | SDL_WINDOW_RESIZABLE;
            }
            else
            {
                masterFlags |= SDL_WINDOW_HIDDEN;
            }

            // Attempt to create the master window. If that fails, reduce our requirements and try again before failing.
            var masterptr = SDL_CreateWindow(isRunningOnMobile ? caption : String.Empty, 0, 0, masterWidth, masterHeight, masterFlags);
            if (masterptr == IntPtr.Zero)
            {
                var fallbackAttempt = 0;
                while (InitializeRenderingAPIFallback(sdlconfig, fallbackAttempt++))
                {
                    masterptr = SDL_CreateWindow(isRunningOnMobile ? caption : String.Empty, 0, 0, masterWidth, masterHeight, masterFlags);
                    if (masterptr != IntPtr.Zero)
                        break;
                }

                if (masterptr == IntPtr.Zero)
                    throw new SDL2Exception();

                // Fallback might have disabled sRGB, so update our configuration to reflect our current state.
                var srgbFramebufferEnabled = 0;
                unsafe
                {
                    if (SDL_GL_GetAttribute(SDL_GLattr.SDL_GL_FRAMEBUFFER_SRGB_CAPABLE, &srgbFramebufferEnabled) < 0)
                        throw new SDL2Exception();
                }
                uvconfig.SrgbBuffersEnabled = (srgbFramebufferEnabled > 0);
            }
            
            var flags = uvconfig.WindowIsVisible ? WindowFlags.None : WindowFlags.Hidden;

            if (uvconfig.WindowIsResizable)
                flags |= WindowFlags.Resizable;

            if (uvconfig.WindowIsBorderless)
                flags |= WindowFlags.Borderless;

            this.master = new SDL2UltravioletWindow(Ultraviolet, masterptr, flags);

            // Set SDL_HINT_VIDEO_WINDOW_SHARE_PIXEL_FORMAT so that enlisted windows
            // will be OpenGL-enabled and set to the correct pixel format.
            if (!SDL_SetHint(SDL_HINT_VIDEO_WINDOW_SHARE_PIXEL_FORMAT, masterptr.ToStringHex()))
                throw new SDL2Exception();

            // If this is not a headless context, create the primary application window.
            if (!uvconfig.Headless)
            {
                if (isRunningOnMobile)
                {
                    this.windows.Add(this.master);
                    DesignatePrimary(this.master);
                }
                else
                {
                    var primary = Create(caption,
                        uvconfig.InitialWindowPosition.X,
                        uvconfig.InitialWindowPosition.Y,
                        uvconfig.InitialWindowPosition.Width,
                        uvconfig.InitialWindowPosition.Height, flags);
                    DesignatePrimary(primary);
                }
            }
        }

        // The context's attached windows.
        private readonly List<IUltravioletWindow> windows = new List<IUltravioletWindow>();

        // The primary and active windows.
        private SDL2PlatformRenderingAPI renderingAPI;
        private IUltravioletWindow master;
        private IUltravioletWindow primary;
        private IUltravioletWindow current;
        private Boolean vsync;

        // The Ultraviolet context.
        private readonly UltravioletContext uv;
    }
}

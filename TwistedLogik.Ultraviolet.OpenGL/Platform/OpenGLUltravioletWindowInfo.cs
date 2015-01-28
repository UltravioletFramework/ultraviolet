using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.SDL2;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Platform
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the IUltravioletWindowInfo interface.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public sealed class OpenGLUltravioletWindowInfo : IUltravioletWindowInfo, IUltravioletComponent
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletWindowInfo class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        internal OpenGLUltravioletWindowInfo(UltravioletContext uv, OpenGLUltravioletConfiguration configuration)
        {
            Contract.Require(uv, "uv");
            Contract.Require(configuration, "configuration");

            this.uv = uv;

            InitializePrimaryWindow(configuration);
        }

        /// <summary>
        /// Draws the current window.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Draw.</param>
        public void Draw(UltravioletTime time)
        {
            var oglwin = (OpenGLUltravioletWindow)current;

            oglwin.Draw(time);
            SDL.GL_SwapWindow((IntPtr)oglwin);
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
        /// Makes the specified window the current window.
        /// </summary>
        /// <param name="window">The window to make current.</param>
        /// <param name="context">The OpenGL context.</param>
        public void DesignateCurrent(IUltravioletWindow window, IntPtr context)
        {
            if (current != window)
            {
                OnCurrentWindowChanging();

                current = window;

                if (window != null && (window != glwin || window.SynchronizeWithVerticalRetrace != vsync))
                {
                    DesignateCurrentOpenGLWindow(window, context);
                }

                OnCurrentWindowChanged();
            }

            if (windows.Count == 0 || (window == null && context == IntPtr.Zero))
            {
                DesignateCurrentOpenGLWindow(null, context);
            }
        }

        /// <summary>
        /// Gets the window with the specified identifier.
        /// </summary>
        /// <returns>The window with the specified identifier, or null if no such window exists.</returns>
        public IUltravioletWindow GetByID(Int32 id)
        {
            var match = default(OpenGLUltravioletWindow);
            foreach (OpenGLUltravioletWindow window in windows)
            {
                if (SDL.GetWindowID((IntPtr)window) == (UInt32)id)
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
                return (IntPtr)(OpenGLUltravioletWindow)window;
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
            return (IntPtr)(OpenGLUltravioletWindow)master;
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
            return (IntPtr)(OpenGLUltravioletWindow)primary;
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
            return (IntPtr)(OpenGLUltravioletWindow)current;
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
            var sdlflags = SDL_WindowFlags.OPENGL;

            if ((flags & WindowFlags.Resizable) == WindowFlags.Resizable)
                sdlflags |= SDL_WindowFlags.RESIZABLE;

            if ((flags & WindowFlags.Borderless) == WindowFlags.Borderless)
                sdlflags |= SDL_WindowFlags.BORDERLESS;

            if ((flags & WindowFlags.Hidden) == WindowFlags.Hidden)
                sdlflags |= SDL_WindowFlags.HIDDEN;
            else
                sdlflags |= SDL_WindowFlags.SHOWN;

            var sdlptr = SDL.CreateWindow(caption ?? String.Empty, x, y, width, height, sdlflags);
            if (sdlptr == IntPtr.Zero)
                throw new SDL2Exception();

            var win = new OpenGLUltravioletWindow(sdlptr);
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
            var sdlptr = SDL.CreateWindowFrom(ptr);
            if (sdlptr == IntPtr.Zero)
                throw new SDL2Exception();

            var win = new OpenGLUltravioletWindow(sdlptr);
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
            Contract.Require(window, "window");

            if (!windows.Remove(window))
                throw new InvalidOperationException(UltravioletStrings.InvalidResource);

            if (window == current)
                throw new InvalidOperationException();

            OnWindowDestroyed(window);

            if (window == primary)
                DesignatePrimary(null);

            if (window == glwin && glcontext != IntPtr.Zero)
                DesignateCurrentOpenGLWindow(null, glcontext);

            var sdlwin = (OpenGLUltravioletWindow)window;
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
        /// Initializes the context's primary window.
        /// </summary>
        /// <param name="configuration">The Ultraviolet Framg</param>
        private void InitializePrimaryWindow(OpenGLUltravioletConfiguration configuration)
        {
            // Retrieve the caption for our window.
            var caption = Localization.Strings.Contains("WINDOW_CAPTION") ? 
                Localization.Get("WINDOW_CAPTION") : UltravioletStrings.DefaultWindowCaption.Value;
            
            // Set the OpenGL attributes for the window we're about to create.
            if (SDL.GL_SetAttribute(SDL_GLattr.MULTISAMPLEBUFFERS, configuration.MultiSampleBuffers) < 0)
                throw new SDL2Exception();

            if (SDL.GL_SetAttribute(SDL_GLattr.MULTISAMPLESAMPLES, configuration.MultiSampleSamples) < 0)
                throw new SDL2Exception();

            /*
             * If we're running on Android, we need to do a few things differently.
             *  - Android uses OpenGL ES, so we need to ask for an ES context.
             *  - Android only supports a single window, so our master and primary are the same window.
             */
            var isRunningOnAndroid = (Ultraviolet.Platform == UltravioletPlatform.Android);
            if (isRunningOnAndroid)
            {
                if (configuration.Headless)
                {
                    throw new InvalidOperationException(OpenGLStrings.CannotCreateHeadlessContextOnAndroid);
                }
                const Int32 SDL_GL_CONTEXT_PROFILE_ES = (int)0x0004; 
                SDL.GL_SetAttribute(SDL_GLattr.CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_ES);
            }

            // Initialize the hidden master window used to create the OpenGL context.
            var masterWidth = 0;
            var masterHeight = 0;
            var masterFlags = SDL_WindowFlags.OPENGL;

            if (isRunningOnAndroid)
            {
                masterFlags |= SDL_WindowFlags.FULLSCREEN | SDL_WindowFlags.RESIZABLE;
            }
            else
            {
                masterFlags |= SDL_WindowFlags.HIDDEN;
            }

            var masterptr = SDL.CreateWindow(isRunningOnAndroid ? caption : String.Empty, 0, 0, masterWidth, masterHeight, masterFlags);
            if (masterptr == IntPtr.Zero)
                throw new SDL2Exception();

            this.master = new OpenGLUltravioletWindow(masterptr);

            // Set SDL_HINT_VIDEO_WINDOW_SHARE_PIXEL_FORMAT so that enlisted windows
            // will be OpenGL-enabled and set to the correct pixel format.
            if (!SDL.SetHint(SDL_Hint.VIDEO_WINDOW_SHARE_PIXEL_FORMAT, masterptr.ToStringHex()))
                throw new SDL2Exception();

            // If this is not a headless context, create the primary application window.
            if (!configuration.Headless)
            {
                if (isRunningOnAndroid)
                {
                    this.windows.Add(this.master);
                    DesignatePrimary(this.master);
                }
                else
                {
                    var flags = configuration.WindowIsVisible ? WindowFlags.None : WindowFlags.Hidden;

                    if (configuration.WindowIsResizable)
                        flags |= WindowFlags.Resizable;

                    if (configuration.WindowIsBorderless)
                        flags |= WindowFlags.Borderless;

                    var primary = Create(caption,
                        configuration.InitialWindowPosition.X,
                        configuration.InitialWindowPosition.Y,
                        configuration.InitialWindowPosition.Width,
                        configuration.InitialWindowPosition.Height, flags);
                    DesignatePrimary(primary);
                }
            }
        }

        /// <summary>
        /// Raises the WindowCreated event.
        /// </summary>
        /// <param name="window">The window that was created.</param>
        private void OnWindowCreated(IUltravioletWindow window)
        {
            var temp = WindowCreated;
            if (temp != null)
            {
                temp(window);
            }
        }

        /// <summary>
        /// Raises the WindowDestroyed event.
        /// </summary>
        /// <param name="window">The window that is being destroyed.</param>
        private void OnWindowDestroyed(IUltravioletWindow window)
        {
            var temp = WindowDestroyed;
            if (temp != null)
            {
                temp(window);
            }
        }

        /// <summary>
        /// Raises the PrimaryWindowChanging event.
        /// </summary>
        private void OnPrimaryWindowChanging()
        {
            var temp = PrimaryWindowChanging;
            if (temp != null)
            {
                temp(primary);
            }
        }

        /// <summary>
        /// Raises the PrimaryWindowChanged event.
        /// </summary>
        private void OnPrimaryWindowChanged()
        {
            var temp = PrimaryWindowChanged;
            if (temp != null)
            {
                temp(primary);
            }
        }

        /// <summary>
        /// Raises the CurrentWindowChanging event.
        /// </summary>
        private void OnCurrentWindowChanging()
        {
            var temp = CurrentWindowChanging;
            if (temp != null)
            {
                temp(current);
            }
        }

        /// <summary>
        /// Raises the CurrentWindowChanged event.
        /// </summary>
        private void OnCurrentWindowChanged()
        {
            var temp = CurrentWindowChanged;
            if (temp != null)
            {
                temp(current);
            }
        }

        /// <summary>
        /// Binds the OpenGL context to the specified window.
        /// </summary>
        private void DesignateCurrentOpenGLWindow(IUltravioletWindow window, IntPtr context)
        {
            if (context == IntPtr.Zero)
            {
                if (glcontext == IntPtr.Zero)
                {
                    return;
                }
                context = glcontext;
            }

            var win = (OpenGLUltravioletWindow)(window ?? master);
            var winptr = (IntPtr)win;
            if (SDL.GL_MakeCurrent(winptr, context) < 0)
                throw new SDL2Exception();

            if (SDL.GL_SetSwapInterval(win.SynchronizeWithVerticalRetrace ? 1 : 0) < 0)
                throw new SDL2Exception();

            glwin = win;
            glcontext = context;
            vsync = win.SynchronizeWithVerticalRetrace;
        }

        // The context's attached windows.
        private readonly List<IUltravioletWindow> windows = new List<IUltravioletWindow>();

        // The primary and active windows.
        private IUltravioletWindow master;
        private IUltravioletWindow primary;
        private IUltravioletWindow current;
        private IUltravioletWindow glwin;
        private IntPtr glcontext;
        private Boolean vsync;

        // The Ultraviolet context.
        private readonly UltravioletContext uv;
    }
}

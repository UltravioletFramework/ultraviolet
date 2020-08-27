using System;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Platform;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="IUltravioletPlatform"/> interface.
    /// </summary>
    public sealed class SDL2UltravioletPlatform : UltravioletResource, IUltravioletPlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2UltravioletPlatform"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework's configuration settings.</param>
        public SDL2UltravioletPlatform(UltravioletContext uv, UltravioletConfiguration configuration)
            : base(uv)
        {
            this.clipboard = ClipboardService.Create();
            this.messageBoxService = MessageBoxService.Create();
            this.windows = new SDL2UltravioletWindowInfoOpenGL(uv, configuration);
            this.displays = new SDL2UltravioletDisplayInfo(uv);
            this.isCursorVisible = SDL_ShowCursor(SDL_QUERY) != 0;
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.displays.Update(time);
            this.windows.Update(time);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public void InitializePrimaryWindow(UltravioletConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (IsPrimaryWindowInitialized)
                throw new InvalidOperationException();

            this.windows.InitializePrimaryWindow(Ultraviolet, configuration);
            this.IsPrimaryWindowInitialized = true;
        }

        /// <inheritdoc/>
        public void ShowMessageBox(MessageBoxType type, String title, String message, IUltravioletWindow parent = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (parent == null)
                parent = Windows.GetPrimary();

            var window = (parent == null) ? IntPtr.Zero : (IntPtr)((SDL2UltravioletWindow)parent);
            messageBoxService.ShowMessageBox(type, title, message, window);
        }

        /// <inheritdoc/>
        public Boolean IsPrimaryWindowInitialized
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public Boolean IsCursorVisible
        {
            get { return isCursorVisible; }
            set
            {
                if (value != isCursorVisible)
                {
                    var result = SDL_ShowCursor(value ? SDL_ENABLE : SDL_DISABLE);
                    if (result < 0)
                        throw new SDL2Exception();

                    isCursorVisible = SDL_ShowCursor(SDL_QUERY) != 0;
                }
            }
        }

        /// <inheritdoc/>
        public Cursor Cursor
        {
            get => cursor;
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                cursor = value;

                unsafe
                {
                    var sdlCursor = (value is SDL2Cursor sdl2cursor) ? sdl2cursor.Native : null;
                    if (sdlCursor == null)
                        sdlCursor = SDL_GetDefaultCursor();

                    SDL_SetCursor(sdlCursor);
                }
            }
        }

        /// <inheritdoc/>
        public ClipboardService Clipboard => clipboard;

        /// <inheritdoc/>
        public IUltravioletWindowInfo Windows => windows;

        /// <inheritdoc/>
        public IUltravioletDisplayInfo Displays => displays;

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && !Disposed)
            {
                windows.DesignateCurrent(null, IntPtr.Zero);
                foreach (SDL2UltravioletWindow window in windows.ToList())
                {
                    windows.Destroy(window);
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Raises the Updating event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        private void OnUpdating(UltravioletTime time) =>
            Updating?.Invoke(this, time);

        // Property values.
        private Boolean isCursorVisible = true;
        private Cursor cursor;
        private readonly ClipboardService clipboard;
        private readonly MessageBoxService messageBoxService;
        private readonly SDL2UltravioletWindowInfoOpenGL windows;
        private readonly SDL2UltravioletDisplayInfo displays;
    }
}

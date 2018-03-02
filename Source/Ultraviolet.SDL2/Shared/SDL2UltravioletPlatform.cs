using System;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Native;
using Ultraviolet.SDL2.Platform;

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
        /// <param name="uvconfig">The Ultraviolet Framework's configuration settings.</param>
        /// <param name="sdlconfig">The SDL2 platform configuration settings.</param>
        public SDL2UltravioletPlatform(UltravioletContext uv, UltravioletConfiguration uvconfig, SDL2PlatformConfiguration sdlconfig)
            : base(uv)
        {
            this.clipboard = ClipboardService.Create();
            this.messageBoxService = MessageBoxService.Create();
            this.windows = new SDL2UltravioletWindowInfoOpenGL(uv, uvconfig, sdlconfig);
            this.displays = new SDL2UltravioletDisplayInfo(uv);
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
        public void ShowMessageBox(MessageBoxType type, String title, String message, IUltravioletWindow parent = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (parent == null)
                parent = Windows.GetPrimary();

            var window = (parent == null) ? IntPtr.Zero : (IntPtr)((SDL2UltravioletWindow)parent);
            messageBoxService.ShowMessageBox(type, title, message, window);
        }

        /// <inheritdoc/>
        public Cursor Cursor
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cursor;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                cursor = value;

                unsafe
                {
                    var sdlCursor = (value == null) ? SDLNative.SDL_GetDefaultCursor() : ((SDL2Cursor)value).Native;
                    SDLNative.SDL_SetCursor(sdlCursor);
                }
            }
        }

        /// <inheritdoc/>
        public ClipboardService Clipboard
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return clipboard;
            }
        }

        /// <inheritdoc/>
        public IUltravioletWindowInfo Windows
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return windows;
            }
        }

        /// <inheritdoc/>
        public IUltravioletDisplayInfo Displays
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return displays;
            }
        }

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
        private Cursor cursor;
        private readonly ClipboardService clipboard;
        private readonly MessageBoxService messageBoxService;
        private readonly SDL2UltravioletWindowInfoOpenGL windows;
        private readonly SDL2UltravioletDisplayInfo displays;
    }
}

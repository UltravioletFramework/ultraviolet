using System;
using System.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.OpenGL.Platform;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the IUltravioletPlatform interface.
    /// </summary>
    public sealed unsafe class OpenGLUltravioletPlatform : UltravioletResource, IUltravioletPlatform
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletPlatform class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        public OpenGLUltravioletPlatform(UltravioletContext uv, OpenGLUltravioletConfiguration configuration)
            : base(uv)
        {
            this.clipboard = new OpenGLUltravioletClipboardInfo();
            this.windows   = new OpenGLUltravioletWindowInfo(uv, configuration);
            this.displays  = new OpenGLUltravioletDisplayInfo();
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnUpdating(time);
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
                cursor = value;

                if (OpenGLCursor.AreCursorsSupported(Ultraviolet))
                {
                    var oglcursor = (OpenGLCursor)value;
                    if (oglcursor != null)
                    {
                        SDL.SetCursor(oglcursor.Native);
                    }
                    else
                    {
                        SDL.SetCursor(SDL.GetDefaultCursor());
                    }
                }
            }
        }

        /// <inheritdoc/>
        public IUltravioletClipboardInfo Clipboard
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
                foreach (OpenGLUltravioletWindow window in windows.ToList())
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
        private void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        // Property values.
        private Cursor cursor;
        private readonly OpenGLUltravioletClipboardInfo clipboard;
        private readonly OpenGLUltravioletWindowInfo windows;
        private readonly OpenGLUltravioletDisplayInfo displays;
    }
}

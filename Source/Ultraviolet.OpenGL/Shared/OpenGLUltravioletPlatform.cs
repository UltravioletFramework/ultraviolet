using System;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.OpenGL.Platform;
using Ultraviolet.Platform;

namespace Ultraviolet.OpenGL
{
    /// <summary>
    /// Represents the OpenGL implementation of the IUltravioletPlatform interface.
    /// </summary>
    public sealed class OpenGLUltravioletPlatform : UltravioletResource, IUltravioletPlatform
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLUltravioletPlatform class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="configuration">The Ultraviolet Framework configuration settings for the current context.</param>
        public OpenGLUltravioletPlatform(UltravioletContext uv, OpenGLUltravioletConfiguration configuration)
            : base(uv)
        {
            this.clipboard = ClipboardService.Create();
            this.cursorService = CursorService.Create();
            this.messageBoxService = MessageBoxService.Create();
            this.windows = new OpenGLUltravioletWindowInfo(uv, configuration);
            this.displays = new OpenGLUltravioletDisplayInfo();
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public void ShowMessageBox(MessageBoxType type, String title, String message, IUltravioletWindow parent = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (parent == null)
                parent = Windows.GetPrimary();
            
            var window = (parent == null) ? IntPtr.Zero : (IntPtr)((OpenGLUltravioletWindow)parent);
            messageBoxService.ShowMessageBox(type, title, message, window);
        }

        /// <inheritdoc/>
        public Cursor Cursor
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return cursorService.Cursor;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                cursorService.Cursor = value;
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
        private void OnUpdating(UltravioletTime time) =>
            Updating?.Invoke(this, time);

        // Property values.
        private readonly ClipboardService clipboard;
        private readonly CursorService cursorService;
        private readonly MessageBoxService messageBoxService;
        private readonly OpenGLUltravioletWindowInfo windows;
        private readonly OpenGLUltravioletDisplayInfo displays;
    }
}

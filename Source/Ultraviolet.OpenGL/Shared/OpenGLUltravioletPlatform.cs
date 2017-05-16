using System;
using System.Linq;
using Ultraviolet.Core;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Platform;

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
            var winconfig = new SDL2WindowConfiguration();
            winconfig.WindowType = SDL2WindowType.OpenGL;
            winconfig.MultiSampleBuffers = configuration.MultiSampleBuffers;
            winconfig.MultiSampleSamples = configuration.MultiSampleSamples;

            this.clipboard = ClipboardService.Create();
            this.cursorService = CursorService.Create();
            this.messageBoxService = MessageBoxService.Create();
            this.windows = new SDL2UltravioletWindowInfo(uv, configuration, winconfig);
            this.displays = new SDL2UltravioletDisplayInfo();
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
            
            var window = (parent == null) ? IntPtr.Zero : (IntPtr)((SDL2UltravioletWindow)parent);
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
                ((SDL2UltravioletWindowInfo)windows).DesignateCurrent(null, IntPtr.Zero);
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
        private readonly ClipboardService clipboard;
        private readonly CursorService cursorService;
        private readonly MessageBoxService messageBoxService;
        private readonly IUltravioletWindowInfo windows;
        private readonly IUltravioletDisplayInfo displays;
    }
}

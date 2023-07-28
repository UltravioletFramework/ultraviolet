using System;
using Ultraviolet.Core;
using Ultraviolet.Platform;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="IUltravioletPlatform"/>.
    /// </summary>
    public class DummyUltravioletPlatform : UltravioletResource, IUltravioletPlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyUltravioletPlatform"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public DummyUltravioletPlatform(UltravioletContext uv)
            : base(uv)
        {
            this.clipboard = new DummyClipboardService();
            this.windows = new DummyUltravioletWindowInfo();
            this.displays = new DummyUltravioletDisplayInfo();
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            Updating?.Invoke(this, time);
        }

        /// <inheritdoc/>
        public void InitializePrimaryWindow(UltravioletConfiguration configuration)
        { }

        /// <inheritdoc/>
        public void ShowMessageBox(MessageBoxType type, String title, String message, IUltravioletWindow parent = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public Boolean IsPrimaryWindowInitialized
        {
            get { return true; }
        }

        /// <inheritdoc/>
        public Boolean IsCursorVisible
        {
            get { return false; }
            set { }
        }

        /// <inheritdoc/>
        public Cursor Cursor
        {
            get { return null; }
            set { }
        }

        /// <inheritdoc/>
        public ClipboardService Clipboard => clipboard;

        /// <inheritdoc/>
        public IUltravioletWindowInfo Windows => windows;

        /// <inheritdoc/>
        public IUltravioletDisplayInfo Displays => displays;

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        // Property values.
        private ClipboardService clipboard;
        private IUltravioletWindowInfo windows;
        private IUltravioletDisplayInfo displays;
    }
}

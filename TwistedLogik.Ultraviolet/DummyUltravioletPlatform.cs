using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet
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
            this.clipboard = new DummyUltravioletClipboardInfo();
            this.windows = new DummyUltravioletWindowInfo();
            this.displays = new DummyUltravioletDisplayInfo();
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        /// <inheritdoc/>
        public Cursor Cursor
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return default(Cursor);
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);
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

        // Property values.
        private IUltravioletClipboardInfo clipboard;
        private IUltravioletWindowInfo windows;
        private IUltravioletDisplayInfo displays;
    }
}

using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="IUltravioletInput"/>.
    /// </summary>
    public sealed class DummyUltravioletInput : UltravioletResource, IUltravioletInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyUltravioletInput"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public DummyUltravioletInput(UltravioletContext uv)
            : base(uv)
        {
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
        public void ShowSoftwareKeyboard()
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void ShowSoftwareKeyboard(KeyboardMode mode)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public void HideSoftwareKeyboard()
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <inheritdoc/>
        public Boolean IsKeyboardSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return false;
        }

        /// <inheritdoc/>
        public KeyboardDevice GetKeyboard()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return null;
        }

        /// <inheritdoc/>
        public Boolean IsMouseSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return false;
        }

        /// <inheritdoc/>
        public MouseDevice GetMouse()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return null;
        }

        /// <inheritdoc/>
        public Int32 GetGamePadCount()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return 0;
        }

        /// <inheritdoc/>
        public Boolean IsGamePadSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return false;
        }

        /// <inheritdoc/>
        public Boolean IsGamePadConnected(Int32 playerIndex)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return false;
        }

        /// <inheritdoc/>
        public GamePadDevice GetGamePadForPlayer(Int32 playerIndex)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return null;
        }

        /// <inheritdoc/>
        public GamePadDevice GetFirstConnectedGamePad()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return null;
        }

        /// <inheritdoc/>
        public Boolean IsTouchSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return false;
        }

        /// <inheritdoc/>
        public Boolean IsTouchDeviceConnected(Int32 index)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return false;
        }

        /// <inheritdoc/>
        public TouchDevice GetTouchDevice()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return null;
        }

        /// <inheritdoc/>
        public TouchDevice GetTouchDeviceByIndex(Int32 index)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return null;
        }

        /// <inheritdoc/>
        public Boolean EmulateMouseWithTouchInput
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return true;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);                
            }
        }

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <inheritdoc/>
        public event GamePadConnectionEventHandler GamePadConnected
        {
            add { }
            remove { }
        }

        /// <inheritdoc/>
        public event GamePadConnectionEventHandler GamePadDisconnected
        {
            add { }
            remove { }
        }
    }
}

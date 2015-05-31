using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.SDL2.Input;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.SDL2
{
    /// <summary>
    /// Represents the SDL2 implementation of the Ultraviolet Input subsystem.
    /// </summary>
    public sealed class SDL2UltravioletInput : UltravioletResource, IUltravioletInput
    {
        /// <summary>
        /// Initializes a new instance of the SDL2UltravioletInput class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public SDL2UltravioletInput(UltravioletContext uv)
            : base(uv)
        {
            this.keyboard = new SDL2KeyboardDevice(uv);
            this.mouse = new SDL2MouseDevice(uv);
            this.gamePadInfo = new GamePadDeviceInfo(uv);
            this.gamePadInfo.GamePadConnected += OnGamePadConnected;
            this.gamePadInfo.GamePadDisconnected += OnGamePadDisconnected;
            this.touchInfo = new TouchDeviceInfo(uv);
        }

        /// <summary>
        /// Resets the device's state in preparation for the next frame.
        /// </summary>
        public void ResetDeviceStates()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.keyboard.ResetDeviceState();
            this.mouse.ResetDeviceState();
            this.gamePadInfo.ResetDeviceStates();
            this.touchInfo.ResetDeviceStates();
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.keyboard.Update(time);
            this.mouse.Update(time);
            this.gamePadInfo.Update(time);
            this.touchInfo.Update(time);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public void ShowSoftwareKeyboard()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SDL.StartTextInput();
        }

        /// <inheritdoc/>
        public void HideSoftwareKeyboard()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            SDL.StopTextInput();
        }

        /// <inheritdoc/>
        public Boolean IsKeyboardSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return true;
        }

        /// <inheritdoc/>
        public KeyboardDevice GetKeyboard()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return keyboard;
        }

        /// <inheritdoc/>
        public Boolean IsMouseSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return true;
        }

        /// <inheritdoc/>
        public MouseDevice GetMouse()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return mouse;
        }

        /// <inheritdoc/>
        public Int32 GetGamePadCount()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return gamePadInfo.Count;
        }

        /// <inheritdoc/>
        public Boolean IsGamePadSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return true;
        }

        /// <inheritdoc/>
        public Boolean IsGamePadConnected(Int32 playerIndex)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return gamePadInfo.GetGamePadForPlayer(playerIndex) != null;
        }

        /// <inheritdoc/>
        public GamePadDevice GetGamePadForPlayer(Int32 playerIndex)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return gamePadInfo.GetGamePadForPlayer(playerIndex);
        }

        /// <inheritdoc/>
        public GamePadDevice GetFirstConnectedGamePad()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return gamePadInfo.GetFirstConnectedGamePad();
        }

        /// <inheritdoc/>
        public Boolean IsTouchSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return touchInfo.Count > 0;
        }

        /// <inheritdoc/>
        public Boolean IsTouchDeviceConnected(Int32 index)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(index >= 0, "index");

            return GetTouchDeviceByIndex(index) != null;
        }

        /// <inheritdoc/>
        public TouchDevice GetTouchDevice()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return touchInfo.Count > 0 ? touchInfo.GetTouchDeviceByIndex(0) : null;
        }

        /// <inheritdoc/>
        public TouchDevice GetTouchDeviceByIndex(Int32 index)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(index >= 0, "index");

            return index >= touchInfo.Count ? null : touchInfo.GetTouchDeviceByIndex(index);
        }

        /// <inheritdoc/>
        public Boolean EmulateMouseWithTouchInput
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.emulateMouseWithTouchInput;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.emulateMouseWithTouchInput = value;
            }
        }

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <inheritdoc/>
        public event GamePadConnectionEventHandler GamePadConnected;

        /// <inheritdoc/>
        public event GamePadConnectionEventHandler GamePadDisconnected;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                SafeDispose.DisposeRef(ref keyboard);
                SafeDispose.DisposeRef(ref mouse);
                SafeDispose.DisposeRef(ref gamePadInfo);
                SafeDispose.DisposeRef(ref touchInfo);
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

        /// <summary>
        /// Raises the <see cref="GamePadConnected"/> event.
        /// </summary>
        /// <param name="device">The device that was connected.</param>
        /// <param name="playerIndex">The player index associated with the game pad.</param>
        private void OnGamePadConnected(GamePadDevice device, Int32 playerIndex)
        {
            var temp = GamePadConnected;
            if (temp != null)
            {
                temp(device, playerIndex);
            }
        }

        /// <summary>
        /// Raises the <see cref="GamePadDisconnected"/> event.
        /// </summary>
        /// <param name="device">The device that was disconnected.</param>
        /// <param name="playerIndex">The player index associated with the game pad.</param>
        private void OnGamePadDisconnected(GamePadDevice device, Int32 playerIndex)
        {
            var temp = GamePadDisconnected;
            if (temp != null)
            {
                temp(device, playerIndex);
            }
        }

        // Input devices.
        private SDL2KeyboardDevice keyboard;
        private SDL2MouseDevice mouse;
        private GamePadDeviceInfo gamePadInfo;
        private TouchDeviceInfo touchInfo;

        // Property values.
        private Boolean emulateMouseWithTouchInput = true;
    }
}

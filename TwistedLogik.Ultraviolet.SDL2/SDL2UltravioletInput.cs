using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.SDL2.Input;

namespace TwistedLogik.Ultraviolet.SDL2
{
    /// <summary>
    /// Represents the SDL2 implementation of the Ultraviolet Input subsystem.
    /// </summary>
    public sealed class SDL2UltravioletInput : UltravioletResource, IUltravioletInput, IUltravioletSubsystem
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
        }

        /// <summary>
        /// Updates the subsystem's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            this.keyboard.Update(time);
            this.mouse.Update(time);

            OnUpdating(time);
        }

        /// <summary>
        /// Gets a value indicating whether the current platform supports keyboard input.
        /// </summary>
        /// <returns>true if the current platform supports keyboard input; otherwise, false.</returns>
        public Boolean IsKeyboardSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return true;
        }

        /// <summary>
        /// Gets the keyboard device, if keyboard input is supported.
        /// </summary>
        /// <remarks>If keyboard input is not supported on the current platform, this method will throw NotSupportedException.</remarks>
        /// <returns>The keyboard device.</returns>
        public KeyboardDevice GetKeyboard()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return keyboard;
        }

        /// <summary>
        /// Gets a value indicating whether the current platform supports mouse input.
        /// </summary>
        /// <returns>true if the current platform supports mouse input; otherwise, false.</returns>
        public Boolean IsMouseSupported()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return true;
        }

        /// <summary>
        /// Gets the mouse device, if mouse input is supported.
        /// </summary>
        /// <remarks>If mouse input is not supported on the current platform, this method will throw NotSupportedException.</remarks>
        /// <returns>The mouse device.</returns>
        public MouseDevice GetMouse()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return mouse;
        }

        /// <summary>
        /// Occurs when the subsystem is updating its state.
        /// </summary>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                SafeDispose.DisposeRef(ref keyboard);
                SafeDispose.DisposeRef(ref mouse);
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

        // Input devices.
        private KeyboardDevice keyboard;
        private MouseDevice mouse;
    }
}

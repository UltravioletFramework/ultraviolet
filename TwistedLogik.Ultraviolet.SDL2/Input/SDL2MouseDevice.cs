using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.SDL2.Messages;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Represents the SDL2 implementation of the MouseDevice class.
    /// </summary>
    public sealed class SDL2MouseDevice : MouseDevice, IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the SDL2MouseDevice class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public SDL2MouseDevice(UltravioletContext uv)
            : base(uv)
        {
            uv.Messages.Subscribe(this,
                SDL2UltravioletMessages.SDLEvent);
        }

        /// <summary>
        /// Receives a message that has been published to a queue.
        /// </summary>
        /// <param name="type">The type of message that was received.</param>
        /// <param name="data">The data for the message that was received.</param>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == SDL2UltravioletMessages.SDLEvent)
            {
                var evt = ((SDL2EventMessageData)data).Event;
                switch (evt.type)
                {
                    case SDL_EventType.MOUSEMOTION:
                        {
                            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.motion.windowID);
                            OnMoved(window, evt.motion.x, evt.motion.y, evt.motion.xrel, evt.motion.yrel);
                        }
                        break;

                    case SDL_EventType.MOUSEBUTTONDOWN:
                        {
                            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.button.windowID);
                            var button = GetUltravioletButton(evt.button.button);
                            OnButtonPressed(window, button);
                        }
                        break;

                    case SDL_EventType.MOUSEBUTTONUP:
                        {
                            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.button.windowID);
                            var button = GetUltravioletButton(evt.button.button);
                            OnButtonReleased(window, button);

                            if (evt.button.clicks == 1)
                            {
                                buttonStateClicks |= (uint)SDL_BUTTON(evt.button.button);
                                OnClick(window, button);
                            }

                            if (evt.button.clicks == 2)
                            {
                                buttonStateDoubleClicks |= (uint)SDL_BUTTON(evt.button.button);
                                OnDoubleClick(window, button);
                            }
                        }
                        break;

                    case SDL_EventType.MOUSEWHEEL:
                        {
                            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.wheel.windowID);
                            pendingWheelDeltaX = evt.wheel.x;
                            pendingWheelDeltaY = evt.wheel.y;
                            OnWheelScrolled(window, evt.wheel.x, evt.wheel.y);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the device's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            buttonStateOld = buttonStateNew;
            buttonStateNew = SDL.GetMouseState(out x, out y);

            buttonStateClicks = 0;
            buttonStateDoubleClicks = 0;

            wheelDeltaX = pendingWheelDeltaX.HasValue ? pendingWheelDeltaX.Value : 0;
            wheelDeltaY = pendingWheelDeltaY.HasValue ? pendingWheelDeltaY.Value : 0;
            pendingWheelDeltaX = null;
            pendingWheelDeltaY = null;
        }

        /// <summary>
        /// Gets a value indicating whether the specified button is currently down.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <returns>true if the button is down; otherwise, false.</returns>
        public override Boolean IsButtonDown(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return (buttonStateNew & SDL_BUTTON(button)) != 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified button is currently up.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <returns>true if the button is up; otherwise, false.</returns>
        public override Boolean IsButtonUp(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return (buttonStateNew & SDL_BUTTON(button)) == 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified button is currently pressed.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <param name="ignoreRepeats">A value indicating whether to ignore repeated button press events on devices which support them.</param>
        /// <returns>true if the button is pressed; otherwise, false.</returns>        
        public override Boolean IsButtonPressed(MouseButton button, Boolean ignoreRepeats = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return 
                ((buttonStateNew & SDL_BUTTON(button)) != 0) &&
                ((buttonStateOld & SDL_BUTTON(button)) == 0);
        }

        /// <summary>
        /// Gets a value indicating whether the specified button is currently released.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <returns>true if the button is released; otherwise, false.</returns>
        public override Boolean IsButtonReleased(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return
                ((buttonStateNew & SDL_BUTTON(button)) == 0) &&
                ((buttonStateOld & SDL_BUTTON(button)) != 0);
        }

        /// <summary>
        /// Gets a value indicating whether the specified button was clicked this frame.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <returns>true if the button was clicked this frame; otherwise, false.</returns>
        public override Boolean IsButtonClicked(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return (buttonStateClicks & SDL_BUTTON(button)) != 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified button was double clicked this frame.
        /// </summary>
        /// <param name="button">The button to evaluate.</param>
        /// <returns>true if the button was double clicked this frame; otherwise, false.</returns>
        public override Boolean IsButtonDoubleClicked(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return (buttonStateDoubleClicks & SDL_BUTTON(button)) != 0;
        }

        /// <summary>
        /// Gets the current state of the specified button.
        /// </summary>
        /// <param name="button">The button for which to retrieve a state.</param>
        /// <returns>The current state of the specified button.</returns>
        public override ButtonState GetButtonState(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var state = IsButtonDown(button) ? ButtonState.Down : ButtonState.Up;

            if (IsButtonPressed(button)) 
                state |= ButtonState.Pressed;

            if (IsButtonReleased(button)) 
                state |= ButtonState.Released;

            return state;
        }

        /// <summary>
        /// Gets the mouse's current position.
        /// </summary>
        public override Vector2 Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// Gets the mouse's current x-coordinate.
        /// </summary>
        public override Int32 X
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return x;
            }
        }

        /// <summary>
        /// Gets the mouse's current y-coordinate.
        /// </summary>
        public override Int32 Y
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return y;
            }
        }

        /// <summary>
        /// Gets the mouse's horizontal scroll wheel delta in the last frame.
        /// </summary>
        public override Int32 WheelDeltaX
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return wheelDeltaX;
            }
        }

        /// <summary>
        /// Gets the mouse's vertical scroll wheel delta in the last frame.
        /// </summary>
        public override Int32 WheelDeltaY
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return wheelDeltaY;
            }
        }

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
                if (!Ultraviolet.Disposed)
                {
                    Ultraviolet.Messages.Unsubscribe(this);
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates the SDL2 button state mask that corresponds to the specified button.
        /// </summary>
        /// <param name="button">The button for which to create a state mask.</param>
        /// <returns>The state mask for the specified button.</returns>
        private static Int32 SDL_BUTTON(Int32 button)
        {
            return 1 << (button - 1);
        }

        /// <summary>
        /// Creates the SDL2 button state mask that corresponds to the specified button.
        /// </summary>
        /// <param name="button">The button for which to create a state mask.</param>
        /// <returns>The state mask for the specified button.</returns>
        private static Int32 SDL_BUTTON(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.None:
                    return 0;
                case MouseButton.Left:
                    return SDL_BUTTON(1);
                case MouseButton.Middle:
                    return SDL_BUTTON(2);
                case MouseButton.Right:
                    return SDL_BUTTON(3);
                case MouseButton.XButton1:
                    return SDL_BUTTON(4);
                case MouseButton.XButton2:
                    return SDL_BUTTON(5);
            }
            throw new ArgumentException("button");
        }

        /// <summary>
        /// Gets the Ultraviolet MouseButton value that corresponds to the specified SDL2 button value.
        /// </summary>
        /// <param name="value">The SDL2 button value to convert.</param>
        /// <returns>The Ultraviolet MouseButton value that corresponds to the specified SDL2 button value.</returns>
        private static MouseButton GetUltravioletButton(Int32 value)
        {
            if (value == 0)
                return MouseButton.None;

            switch ((SDL_Button)value)
            {
                case SDL_Button.LEFT:
                    return MouseButton.Left;
                case SDL_Button.MIDDLE:
                    return MouseButton.Middle;
                case SDL_Button.RIGHT:
                    return MouseButton.Right;
                case SDL_Button.X1:
                    return MouseButton.XButton1;
                case SDL_Button.X2:
                    return MouseButton.XButton2;
            }
            throw new ArgumentException("value");
        }

        // Property values.
        private Int32 x;
        private Int32 y;
        private Int32 wheelDeltaX;
        private Int32 wheelDeltaY;

        // State values.
        private UInt32 buttonStateOld;
        private UInt32 buttonStateNew;
        private UInt32 buttonStateClicks;
        private UInt32 buttonStateDoubleClicks;
        private Int32? pendingWheelDeltaX;
        private Int32? pendingWheelDeltaY;
    }
}

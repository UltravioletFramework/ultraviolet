using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;
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
            this.window = Ultraviolet.GetPlatform().Windows.GetPrimary();

            var buttonCount = Enum.GetValues(typeof(MouseButton)).Length;            
            this.states = new InternalButtonState[buttonCount];

            uv.Messages.Subscribe(this,
                SDL2UltravioletMessages.SDLEvent);
        }

        /// <inheritdoc/>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == SDL2UltravioletMessages.SDLEvent)
            {
                var evt = ((SDL2EventMessageData)data).Event;
                switch (evt.type)
                {
                    case SDL_EventType.MOUSEMOTION:
                        OnMouseMotion(ref evt.motion);
                        break;

                    case SDL_EventType.MOUSEBUTTONDOWN:
                        OnMouseButtonDown(ref evt.button);
                        break;

                    case SDL_EventType.MOUSEBUTTONUP:
                        OnMouseButtonUp(ref evt.button);
                        break;

                    case SDL_EventType.MOUSEWHEEL:
                        OnMouseWheel(ref evt.wheel);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Resets the device's state in preparation for the next frame.
        /// </summary>
        public void ResetDeviceState()
        {
            buttonStateClicks       = 0;
            buttonStateDoubleClicks = 0;

            for (int i = 0; i < states.Length; i++)
            {
                states[i].Reset();
            }
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {

        }

        /// <summary>
        /// Gets the mouse cursor's position within the specified window.
        /// </summary>
        /// <param name="window">The window to evaluate.</param>
        /// <returns>The cursor's position within the specified window, or <c>null</c> if the cursor is outside of the window.</returns>
        public override Vector2? GetPositionInWindow(IUltravioletWindow window)
        {
            Contract.Require(window, "window");

            if (Window != window)
                return null;

            return Position;
        }

        /// <inheritdoc/>
        public override Boolean IsButtonDown(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return states[(int)button].Down;
        }

        /// <inheritdoc/>
        public override Boolean IsButtonUp(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return states[(int)button].Up;
        }

        /// <inheritdoc/>
        public override Boolean IsButtonPressed(MouseButton button, Boolean ignoreRepeats = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return states[(int)button].Pressed || (!ignoreRepeats && states[(int)button].Repeated);
        }

        /// <inheritdoc/>
        public override Boolean IsButtonReleased(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return states[(int)button].Released;
        }

        /// <inheritdoc/>
        public override Boolean IsButtonClicked(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return (buttonStateClicks & SDL_BUTTON(button)) != 0;
        }

        /// <inheritdoc/>
        public override Boolean IsButtonDoubleClicked(MouseButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return (buttonStateDoubleClicks & SDL_BUTTON(button)) != 0;
        }

        /// <inheritdoc/>
        public override IUltravioletWindow Window
        {
            get { return window; }
        }

        /// <inheritdoc/>
        public override Vector2 Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return new Vector2(x, y);
            }
        }

        /// <inheritdoc/>
        public override Int32 X
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return x;
            }
        }

        /// <inheritdoc/>
        public override Int32 Y
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return y;
            }
        }

        /// <inheritdoc/>
        public override Int32 WheelDeltaX
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return wheelDeltaX;
            }
        }

        /// <inheritdoc/>
        public override Int32 WheelDeltaY
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return wheelDeltaY;
            }
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Handles SDL2's MOUSEMOTION event.
        /// </summary>
        private void OnMouseMotion(ref SDL_MouseMotionEvent evt)
        {
            if (!Ultraviolet.GetInput().EmulateMouseWithTouchInput && evt.which == SDL_TOUCH_MOUSEID)
                return;

            this.window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.windowID);

            this.x = evt.x;
            this.y = evt.y;

            OnMoved(window, evt.x, evt.y, evt.xrel, evt.yrel);
        }

        /// <summary>
        /// Handles SDL2's MOUSEBUTTONDOWN event.
        /// </summary>
        private void OnMouseButtonDown(ref SDL_MouseButtonEvent evt)
        {
            if (!Ultraviolet.GetInput().EmulateMouseWithTouchInput && evt.which == SDL_TOUCH_MOUSEID)
                return;

            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.windowID);
            var button = GetUltravioletButton(evt.button);

            this.states[(int)button].OnDown(false);

            OnButtonPressed(window, button);
        }

        /// <summary>
        /// Handles SDL2's MOUSEBUTTONUP event.
        /// </summary>
        private void OnMouseButtonUp(ref SDL_MouseButtonEvent evt)
        {
            if (!Ultraviolet.GetInput().EmulateMouseWithTouchInput && evt.which == SDL_TOUCH_MOUSEID)
                return;

            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.windowID);
            var button = GetUltravioletButton(evt.button);

            this.states[(int)button].OnUp();
            
            OnButtonReleased(window, button);

            if (evt.clicks == 1)
            {
                buttonStateClicks |= (uint)SDL_BUTTON(evt.button);
                OnClick(window, button);
            }

            if (evt.clicks == 2)
            {
                buttonStateDoubleClicks |= (uint)SDL_BUTTON(evt.button);
                OnDoubleClick(window, button);
            }
        }

        /// <summary>
        /// Handles SDL2's MOUSEWHEEL event.
        /// </summary>
        private void OnMouseWheel(ref SDL_MouseWheelEvent evt)
        {
            if (!Ultraviolet.GetInput().EmulateMouseWithTouchInput && evt.which == SDL_TOUCH_MOUSEID)
                return;

            var window = Ultraviolet.GetPlatform().Windows.GetByID((int)evt.windowID);
            wheelDeltaX = evt.x;
            wheelDeltaY = evt.y;
            OnWheelScrolled(window, evt.x, evt.y);
        }

        // The device identifier of the touch-based mouse emulator.
        private const UInt32 SDL_TOUCH_MOUSEID = unchecked((UInt32)(-1));

        // Property values.
        private Int32 x;
        private Int32 y;
        private Int32 wheelDeltaX;
        private Int32 wheelDeltaY;
        private IUltravioletWindow window;

        // State values.
        private InternalButtonState[] states;
        private UInt32 buttonStateClicks;
        private UInt32 buttonStateDoubleClicks;
    }
}

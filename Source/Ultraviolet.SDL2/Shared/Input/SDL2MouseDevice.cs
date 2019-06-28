using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Input;
using Ultraviolet.Platform;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_Button;
using static Ultraviolet.SDL2.Native.SDL_EventType;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Input
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
                    case SDL_MOUSEMOTION:
                        {
                            // HACK: On iOS, for some goddamn reason, SDL2 sends us a spurious motion event
                            // with mouse ID 0 when you first touch the screen. This only seems to happen once
                            // so let's just ignore it.
                            if (!ignoredFirstMouseMotionEvent)
                            {
                                SetMousePositionFromDevicePosition(evt.motion.windowID);
                                ignoredFirstMouseMotionEvent = true;
                            }
                            else
                            {
                                if (!isRegistered && evt.motion.which != SDL_TOUCH_MOUSEID)
                                    Register(evt.motion.windowID);

                                OnMouseMotion(ref evt.motion);
                            }
                        }
                        break;

                    case SDL_MOUSEBUTTONDOWN:
                        {
                            if (!isRegistered && evt.button.which != SDL_TOUCH_MOUSEID)
                                Register(evt.button.windowID);

                            OnMouseButtonDown(ref evt.button);
                        }
                        break;

                    case SDL_MOUSEBUTTONUP:
                        {
                            if (!isRegistered && evt.button.which != SDL_TOUCH_MOUSEID)
                                Register(evt.button.windowID);

                            OnMouseButtonUp(ref evt.button);
                        }
                        break;

                    case SDL_MOUSEWHEEL:
                        {
                            if (!isRegistered && evt.wheel.which != SDL_TOUCH_MOUSEID)
                                Register(evt.wheel.windowID);

                            OnMouseWheel(ref evt.wheel);
                        }
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

        /// <inheritdoc/>
        public override void WarpToWindow(IUltravioletWindow window, Int32 x, Int32 y)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(window, nameof(window));

            window.WarpMouseWithinWindow(x, y);
        }

        /// <inheritdoc/>
        public override void WarpToWindowCenter(IUltravioletWindow window)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(window, nameof(window));

            var size = window.ClientSize;
            window.WarpMouseWithinWindow(size.Width / 2, size.Height / 2);
        }

        /// <inheritdoc/>
        public override void WarpToPrimaryWindow(Int32 x, Int32 y)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var primary = Ultraviolet.GetPlatform().Windows.GetPrimary();
            if (primary == null)
                throw new InvalidOperationException(UltravioletStrings.NoPrimaryWindow);

            primary.WarpMouseWithinWindow(x, y);
        }

        /// <inheritdoc/>
        public override void WarpToPrimaryWindowCenter()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var primary = Ultraviolet.GetPlatform().Windows.GetPrimary();
            if (primary == null)
                throw new InvalidOperationException(UltravioletStrings.NoPrimaryWindow);

            var size = primary.ClientSize;
            primary.WarpMouseWithinWindow(size.Width / 2, size.Height / 2);
        }

        /// <inheritdoc/>
        public override Point2? GetPositionInWindow(IUltravioletWindow window)
        {
            Contract.Require(window, nameof(window));

            if (Window != window)
                return null;

            var spos = (Point2)Position;
            var cpos = Window.Compositor.WindowToPoint(spos);

            return cpos;
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
        public override Boolean GetIsRelativeModeEnabled()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return SDL_GetRelativeMouseMode();
        }

        /// <inheritdoc/>
        public override Boolean SetIsRelativeModeEnabled(Boolean enabled)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var result = SDL_SetRelativeMouseMode(enabled);
            if (result == -1)
                return false;

            if (result < 0)
                throw new SDL2Exception();

            relativeMode = enabled;
            return true;
        }

        /// <inheritdoc/>
        public override IUltravioletWindow Window => window;

        /// <inheritdoc/>
        public override Point2 Position => new Point2(x, y);

        /// <inheritdoc/>
        public override Int32 X => x;

        /// <inheritdoc/>
        public override Int32 Y => y;

        /// <inheritdoc/>
        public override Int32 WheelDeltaX => wheelDeltaX;

        /// <inheritdoc/>
        public override Int32 WheelDeltaY => wheelDeltaY;

        /// <inheritdoc/>
        public override Boolean IsRegistered => isRegistered;

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
                case SDL_BUTTON_LEFT:
                    return MouseButton.Left;
                case SDL_BUTTON_MIDDLE:
                    return MouseButton.Middle;
                case SDL_BUTTON_RIGHT:
                    return MouseButton.Right;
                case SDL_BUTTON_X1:
                    return MouseButton.XButton1;
                case SDL_BUTTON_X2:
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

            if (relativeMode)
            {
                SetMousePosition(evt.windowID, evt.x, evt.y);
                OnMoved(window, evt.x, evt.y, evt.xrel, evt.yrel);
            }
            else
            {
                SetMousePosition(evt.windowID, evt.x, evt.y);
                OnMoved(window, evt.x, evt.y, evt.xrel, evt.yrel);
            }
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

            OnButtonReleased(window, button);
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

        /// <summary>
        /// Flags the device as registered.
        /// </summary>
        private void Register(UInt32 windowID)
        {
            var input = (SDL2UltravioletInput)Ultraviolet.GetInput();
            if (input.RegisterMouseDevice(this))
            {
                isRegistered = true;
            }
        }

        /// <summary>
        /// Sets the mouse cursor's position within its window.
        /// </summary>
        private void SetMousePosition(UInt32 windowID, Int32 x, Int32 y)
        {
            this.window = Ultraviolet.GetPlatform().Windows.GetByID((int)windowID);

            if (Ultraviolet.Properties.SupportsHighDensityDisplayModes)
            {
                var scale = window?.Display.DeviceScale ?? 1f;
                this.x = (Int32)(x * scale);
                this.y = (Int32)(y * scale);
            }
            else
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Sets the mouse cursor's position based on the device's physical position.
        /// </summary>
        private void SetMousePositionFromDevicePosition(UInt32 windowID)
        {
            Int32 x, y;
            SDL_GetMouseState(out x, out y);
            SetMousePosition(windowID, x, y);
        }

        // The device identifier of the touch-based mouse emulator.
        private const UInt32 SDL_TOUCH_MOUSEID = unchecked((UInt32)(-1));

        // Property values.
        private Int32 x;
        private Int32 y;
        private Int32 wheelDeltaX;
        private Int32 wheelDeltaY;
        private Boolean isRegistered;
        private IUltravioletWindow window;

        // State values.
        private InternalButtonState[] states;
        private UInt32 buttonStateClicks;
        private UInt32 buttonStateDoubleClicks;
        private Boolean ignoredFirstMouseMotionEvent;
        private Boolean relativeMode;
    }
}

using System;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Input;
using Ultraviolet.SDL2.Messages;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_EventType;
using static Ultraviolet.SDL2.Native.SDL_GameControllerAxis;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="GamePadDevice"/> class.
    /// </summary>
    public sealed class SDL2GamePadDevice : GamePadDevice,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2GamePadDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="joystickIndex">The index of the SDL2 joystick device.</param>
        /// <param name="playerIndex">The index of the player that owns the device.</param>
        public SDL2GamePadDevice(UltravioletContext uv, Int32 joystickIndex, Int32 playerIndex)
            : base(uv)
        {
            this.timeLastPressAxis = new Double[Enum.GetValues(typeof(GamePadAxis)).Length];
            this.timeLastPressButton = new Double[Enum.GetValues(typeof(GamePadButton)).Length];

            this.repeatingAxis = new Boolean[this.timeLastPressAxis.Length];
            this.repeatingButton = new Boolean[this.timeLastPressButton.Length];

            if ((this.controller = SDL_GameControllerOpen(joystickIndex)) == IntPtr.Zero)
            {
                throw new SDL2Exception();
            }

            this.name = SDL_GameControllerNameForIndex(joystickIndex);
            this.states = new InternalButtonState[Enum.GetValues(typeof(GamePadButton)).Length];
            this.playerIndex = playerIndex;

            var joystick = SDL_GameControllerGetJoystick(controller);

            if ((this.instanceID = SDL_JoystickInstanceID(joystick)) < 0)
            {
                throw new SDL2Exception();
            }

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
            if (type != SDL2UltravioletMessages.SDLEvent)
                return;
            
            var evt = ((SDL2EventMessageData)data).Event;
            switch (evt.type)
            {
                case SDL_CONTROLLERBUTTONDOWN:
                    {
                        if (evt.cbutton.which == instanceID)
                        {
                            if (!isRegistered)
                                Register();

                            var button = SDLToUltravioletButton((SDL_GameControllerButton)evt.cbutton.button);
                            PutButtonInDownState(button);
                        }
                    }
                    break;

                case SDL_CONTROLLERBUTTONUP:
                    {
                        if (evt.cbutton.which == instanceID)
                        {
                            if (!isRegistered)
                                Register();

                            var button = SDLToUltravioletButton((SDL_GameControllerButton)evt.cbutton.button);
                            PutButtonInUpState(button);
                        }
                    }
                    break;

                case SDL_CONTROLLERAXISMOTION:
                    {
                        if (evt.caxis.which == instanceID)
                        {
                            if (!isRegistered)
                                Register();

                            OnAxisMotion(evt.caxis);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Gets the pointer to the native SDL2 object that this object represents.
        /// </summary>
        /// <returns>A pointer to the native SDL2 object that this object represents.</returns>
        public IntPtr ToNative()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            return controller;
        }

        /// <summary>
        /// Resets the device's state in preparation for the next frame.
        /// </summary>
        public void ResetDeviceState()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            for (int i = 0; i < states.Length; i++)
            {
                states[i].Reset();
            }
        }

        /// <inheritdoc/>
        public override void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            lastUpdateTime = time.TotalTime.TotalMilliseconds;

            var leftJoystickVector = LeftJoystickVector;
            if (leftJoystickVector != leftJoystickVectorPrev)
            {
                leftJoystickVectorPrev = leftJoystickVector;
                OnLeftJoystickVectorChanged(leftJoystickVectorPrev);
            }

            var rightJoystickVector = RightJoystickVector;
            if (rightJoystickVector != rightJoystickVectorPrev)
            {
                rightJoystickVectorPrev = rightJoystickVector;
                OnRightJoystickVectorChanged(rightJoystickVector);
            }

            CheckForRepeatedPresses(time);
        }

        /// <inheritdoc/>
        public override bool IsButtonDown(GamePadButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var btnval = (Int32)button;
            return states[btnval].Down;
        }

        /// <inheritdoc/>
        public override bool IsButtonUp(GamePadButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var btnval = (Int32)button;
            return states[btnval].Up;
        }

        /// <inheritdoc/>
        public override bool IsButtonPressed(GamePadButton button, bool ignoreRepeats = true)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var btnval = (Int32)button;
            return states[btnval].Pressed || (!ignoreRepeats && states[btnval].Repeated);
        }

        /// <inheritdoc/>
        public override bool IsButtonReleased(GamePadButton button)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var btnval = (Int32)button;
            return states[btnval].Released;
        }

        /// <inheritdoc/>
        public override GamePadJoystickDirection GetJoystickDirection(GamePadJoystick joystick, Single? threshold = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var thresholdValue = Math.Abs(threshold ?? AxisDownThreshold);

            var hAxis = GamePadAxis.None;
            var vAxis = GamePadAxis.None;

            switch (joystick)
            {
                case GamePadJoystick.None:
                    return GamePadJoystickDirection.None;

                case GamePadJoystick.Left:
                    hAxis = GamePadAxis.LeftJoystickX;
                    vAxis = GamePadAxis.LeftJoystickY;
                    break;

                case GamePadJoystick.Right:
                    hAxis = GamePadAxis.RightJoystickX;
                    vAxis = GamePadAxis.RightJoystickY;
                    break;

                default:
                    throw new ArgumentException("joystick");
            }

            var hAxisValue = GetAxisValue(hAxis);
            var vAxisValue = GetAxisValue(vAxis);

            var result = GamePadJoystickDirection.None;

            if (hAxisValue <= -thresholdValue)
                result |= GamePadJoystickDirection.Left;
            else if (hAxisValue >= thresholdValue)
                result |= GamePadJoystickDirection.Right;

            if (vAxisValue <= -thresholdValue)
                result |= GamePadJoystickDirection.Up;
            else if (vAxisValue > thresholdValue)
                result |= GamePadJoystickDirection.Down;

            return result;
        }

        /// <inheritdoc/>
        public override GamePadJoystickDirection GetJoystickDirectionFromAxis(GamePadAxis axis, Single? threshold = null)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var thresholdValue = Math.Abs(threshold ?? AxisDownThreshold);

            switch (axis)
            {
                case GamePadAxis.None:
                    return GamePadJoystickDirection.None;

                case GamePadAxis.LeftTrigger:
                case GamePadAxis.RightTrigger:
                    return GamePadJoystickDirection.None;

                case GamePadAxis.LeftJoystickX:
                    if (leftJoystickX <= -thresholdValue)
                    {
                        return GamePadJoystickDirection.Left;
                    }
                    if (leftJoystickX >= thresholdValue)
                    {
                        return GamePadJoystickDirection.Right;
                    }
                    return GamePadJoystickDirection.None;

                case GamePadAxis.LeftJoystickY:
                    if (leftJoystickY <= -thresholdValue)
                    {
                        return GamePadJoystickDirection.Up;
                    }
                    if (leftJoystickY >= thresholdValue)
                    {
                        return GamePadJoystickDirection.Down;
                    }
                    return GamePadJoystickDirection.None;

                case GamePadAxis.RightJoystickX:
                    if (rightJoystickX <= -thresholdValue)
                    {
                        return GamePadJoystickDirection.Left;
                    }
                    if (rightJoystickX >= thresholdValue)
                    {
                        return GamePadJoystickDirection.Right;
                    }
                    return GamePadJoystickDirection.None;

                case GamePadAxis.RightJoystickY:
                    if (rightJoystickY <= -thresholdValue)
                    {
                        return GamePadJoystickDirection.Up;
                    }
                    if (rightJoystickY >= thresholdValue)
                    {
                        return GamePadJoystickDirection.Down;
                    }
                    return GamePadJoystickDirection.None;
            }

            throw new ArgumentException("axis");
        }

        /// <inheritdoc/>
        public override Single GetAxisValue(GamePadAxis axis)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            switch (axis)
            {
                case GamePadAxis.None:
                    return 0f;

                case GamePadAxis.LeftJoystickX:
                    return leftJoystickX;

                case GamePadAxis.LeftJoystickY:
                    return leftJoystickY;

                case GamePadAxis.RightJoystickX:
                    return rightJoystickX;

                case GamePadAxis.RightJoystickY:
                    return rightJoystickY;

                case GamePadAxis.LeftTrigger:
                    return leftTrigger;

                case GamePadAxis.RightTrigger:
                    return rightTrigger;
            }
            throw new ArgumentException("axis");
        }

        /// <inheritdoc/>
        public override Boolean IsAxisDown(GamePadAxis axis)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            switch (axis)
            {
                case GamePadAxis.None:
                    return false;

                case GamePadAxis.LeftJoystickX:
                    return IsAxisDown(leftJoystickX);

                case GamePadAxis.LeftJoystickY:
                    return IsAxisDown(leftJoystickY);

                case GamePadAxis.RightJoystickX:
                    return IsAxisDown(rightJoystickX);

                case GamePadAxis.RightJoystickY:
                    return IsAxisDown(rightJoystickY);

                case GamePadAxis.LeftTrigger:
                    return IsAxisDown(leftTrigger);

                case GamePadAxis.RightTrigger:
                    return IsAxisDown(rightTrigger);
            }

            throw new ArgumentException("axis");
        }

        /// <inheritdoc/>
        public override Boolean IsAxisUp(GamePadAxis axis)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            switch (axis)
            {
                case GamePadAxis.None:
                    return true;

                case GamePadAxis.LeftJoystickX:
                    return !IsAxisDown(leftJoystickX);

                case GamePadAxis.LeftJoystickY:
                    return !IsAxisDown(leftJoystickY);

                case GamePadAxis.RightJoystickX:
                    return !IsAxisDown(rightJoystickX);

                case GamePadAxis.RightJoystickY:
                    return !IsAxisDown(rightJoystickY);

                case GamePadAxis.LeftTrigger:
                    return !IsAxisDown(leftTrigger);

                case GamePadAxis.RightTrigger:
                    return !IsAxisDown(rightTrigger);
            }

            throw new ArgumentException("axis");
        }

        /// <inheritdoc/>
        public override Boolean IsAxisPressed(GamePadAxis axis)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            switch (axis)
            {
                case GamePadAxis.None:
                    return false;

                case GamePadAxis.LeftJoystickX:
                    return IsAxisPressed(prevLeftJoystickX, leftJoystickX);

                case GamePadAxis.LeftJoystickY:
                    return IsAxisPressed(prevLeftJoystickY, leftJoystickY);

                case GamePadAxis.RightJoystickX:
                    return IsAxisPressed(prevRightJoystickX, rightJoystickX);

                case GamePadAxis.RightJoystickY:
                    return IsAxisPressed(prevRightJoystickY, rightJoystickY);

                case GamePadAxis.LeftTrigger:
                    return IsAxisPressed(prevLeftTrigger, leftTrigger);

                case GamePadAxis.RightTrigger:
                    return IsAxisPressed(prevRightTrigger, rightTrigger);
            }

            throw new ArgumentException("axis");
        }

        /// <inheritdoc/>
        public override Boolean IsAxisReleased(GamePadAxis axis)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            switch (axis)
            {
                case GamePadAxis.None:
                    return false;

                case GamePadAxis.LeftJoystickX:
                    return IsAxisReleased(prevLeftJoystickX, leftJoystickX);

                case GamePadAxis.LeftJoystickY:
                    return IsAxisReleased(prevLeftJoystickY, leftJoystickY);

                case GamePadAxis.RightJoystickX:
                    return IsAxisReleased(prevRightJoystickX, rightJoystickX);

                case GamePadAxis.RightJoystickY:
                    return IsAxisReleased(prevRightJoystickY, rightJoystickY);

                case GamePadAxis.LeftTrigger:
                    return IsAxisReleased(prevLeftTrigger, leftTrigger);

                case GamePadAxis.RightTrigger:
                    return IsAxisReleased(prevRightTrigger, rightTrigger);
            }

            throw new ArgumentException("axis");
        }

        /// <inheritdoc/>
        public override String Name => name;

        /// <inheritdoc/>
        public override Int32 PlayerIndex => playerIndex;

        /// <inheritdoc/>
        public override Single AxisDownThreshold { get; set; }

        /// <inheritdoc/>
        public override Single LeftTrigger => leftTrigger;

        /// <inheritdoc/>
        public override Single RightTrigger => rightTrigger;

        /// <inheritdoc/>
        public override Single LeftJoystickX => leftJoystickX;

        /// <inheritdoc/>
        public override Single LeftJoystickY => leftJoystickY;

        /// <inheritdoc/>
        public override Vector2 LeftJoystickVector => new Vector2(leftJoystickX, leftJoystickY);

        /// <inheritdoc/>
        public override Single RightJoystickX => rightJoystickX;

        /// <inheritdoc/>
        public override Single RightJoystickY => rightJoystickY;

        /// <inheritdoc/>
        public override Vector2 RightJoystickVector => new Vector2(rightJoystickX, rightJoystickY);

        /// <inheritdoc/>
        public override Boolean IsRegistered => isRegistered;

        /// <summary>
        /// Gets the SDL2 instance identifier of the game pad device.
        /// </summary>
        internal Int32 InstanceID => instanceID;

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

                instanceID = 0;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Converts an SDL2 SDL_GameControllerButton value to an Ultraviolet GamePadButton value.
        /// </summary>
        /// <param name="button">The <see cref="SDL_GameControllerButton"/> value to convert.</param>
        /// <returns>The converted <see cref="GamePadButton"/> value.</returns>
        private static GamePadButton SDLToUltravioletButton(SDL_GameControllerButton button) => (GamePadButton)(1 + (int)button);

        /// <summary>
        /// Normalizes an SDL2 axis value.
        /// </summary>
        /// <param name="value">The SDL2 axis value to normalize.</param>
        /// <returns>The normalized value.</returns>
        private static Single NormalizeAxisValue(Int16 value)
        {
            return (value < 0) ? -(value / (Single)Int16.MinValue) : value / (Single)Int16.MaxValue;
        }

        /// <summary>
        /// Puts the specified button into the "Down" state if it isn't already in that state.
        /// </summary>
        private void PutButtonInDownState(GamePadButton button)
        {
            var buttonIndex = (Int32)button;
            if (states[buttonIndex].Down)
                return;

            states[buttonIndex].OnDown(false);
            timeLastPressButton[buttonIndex] = lastUpdateTime;
            repeatingButton[buttonIndex] = false;
            OnButtonPressed(button, false);
        }

        /// <summary>
        /// Puts the specified direction in the "Up" state if it isn't already in that state.
        /// </summary>
        private void PutButtonInUpState(GamePadButton button)
        {
            var buttonIndex = (int)button;
            if (states[buttonIndex].Up)
                return;

            states[buttonIndex].OnUp();
            timeLastPressButton[buttonIndex] = lastUpdateTime;
            repeatingButton[buttonIndex] = false;
            OnButtonReleased(button);
        }

        /// <summary>
        /// Handles an SDL2 axis motion event.
        /// </summary>
        /// <param name="evt">The SDL2 event data.</param>
        private void OnAxisMotion(SDL_ControllerAxisEvent evt)
        {
            var value = NormalizeAxisValue(evt.value);

            switch ((SDL_GameControllerAxis)evt.axis)
            {
                case SDL_CONTROLLER_AXIS_LEFTX:
                    prevLeftJoystickX = leftJoystickX;
                    leftJoystickX = value;
                    OnAxisChanged(GamePadAxis.LeftJoystickX, value);
                    CheckForAxisPresses(GamePadAxis.LeftJoystickX, prevLeftJoystickX, value);
                    break;

                case SDL_CONTROLLER_AXIS_LEFTY:
                    prevLeftJoystickY = leftJoystickY;
                    leftJoystickY = value;
                    OnAxisChanged(GamePadAxis.LeftJoystickY, value);
                    CheckForAxisPresses(GamePadAxis.LeftJoystickY, prevLeftJoystickY, value);
                    break;
                
                case SDL_CONTROLLER_AXIS_RIGHTX:
                    prevRightJoystickX = rightJoystickX;
                    rightJoystickX = value;
                    OnAxisChanged(GamePadAxis.RightJoystickX, value);
                    CheckForAxisPresses(GamePadAxis.RightJoystickX, prevRightJoystickX, value);
                    break;
                
                case SDL_CONTROLLER_AXIS_RIGHTY:
                    prevRightJoystickY = rightJoystickY;
                    rightJoystickY = value;
                    OnAxisChanged(GamePadAxis.RightJoystickY, value);
                    CheckForAxisPresses(GamePadAxis.RightJoystickY, prevRightJoystickY, value);
                    break;
                
                case SDL_CONTROLLER_AXIS_TRIGGERLEFT:
                    prevLeftTrigger = leftTrigger;
                    leftTrigger = value;
                    OnAxisChanged(GamePadAxis.LeftTrigger, value);
                    CheckForAxisPresses(GamePadAxis.LeftTrigger, prevLeftTrigger, value);
                    break;
                
                case SDL_CONTROLLER_AXIS_TRIGGERRIGHT:
                    prevRightTrigger = rightTrigger;
                    rightTrigger = value;
                    OnAxisChanged(GamePadAxis.RightTrigger, value);
                    CheckForAxisPresses(GamePadAxis.RightTrigger, prevRightTrigger, value);
                    break;
            }
        }

        /// <summary>
        /// Raises <see cref="GamePadDevice.AxisPressed"/> and <see cref="GamePadDevice.AxisReleased"/> in response
        /// to changes in a particular axis.
        /// </summary>
        /// <param name="axis">The axis to evaluate.</param>
        /// <param name="previousValue">The last known value of the axis.</param>
        /// <param name="currentValue">The current value of the axis.</param>
        private void CheckForAxisPresses(GamePadAxis axis, Single previousValue, Single currentValue)
        {
            var axisIndex = (int)axis;

            var axisWasDown = IsAxisDown(previousValue);
            var axisIsDown = IsAxisDown(currentValue);

            // Update button states.
            var btnPrev = ButtonFromAxis(axis, previousValue);
            var btnCurrent = ButtonFromAxis(axis, currentValue);

            if (btnPrev != GamePadButton.None && (btnPrev != btnCurrent || !axisIsDown))
                PutButtonInUpState(btnPrev);

            if (btnCurrent != GamePadButton.None && axisIsDown)
                PutButtonInDownState(btnCurrent);

            // Axis went from pressed->pressed but changed direction.
            if (axisIsDown && axisWasDown && Math.Sign(currentValue) != Math.Sign(previousValue))
            {
                timeLastPressAxis[axisIndex] = lastUpdateTime;
                repeatingAxis[axisIndex] = false;

                OnAxisReleased(axis, 0f);
                OnAxisPressed(axis, currentValue, false);
                      
                return;
            }

            // Axis went from pressed->released or released->pressed.
            if (axisWasDown != axisIsDown)
            {
                if (axisIsDown)
                {
                    timeLastPressAxis[axisIndex] = lastUpdateTime;
                    repeatingAxis[axisIndex] = false;

                    OnAxisPressed(axis, currentValue, false);
                }
                else
                {
                    OnAxisReleased(axis, currentValue);
                }
            }
        }

        /// <summary>
        /// Raises repeated <see cref="GamePadDevice.AxisPressed"/> and <see cref="GamePadDevice.ButtonPressed"/> events as necessary.
        /// </summary>
        private void CheckForRepeatedPresses(UltravioletTime time)
        {
            for (int i = 0; i < repeatingAxis.Length; i++)
            {
                var axis = (GamePadAxis)i;
                if (!IsAxisDown(axis))
                    continue;

                var delay = repeatingAxis[i] ? PressRepeatDelay : PressRepeatInitialDelay;
                if (delay <= time.TotalTime.TotalMilliseconds - timeLastPressAxis[i])
                {
                    repeatingAxis[i] = true;
                    timeLastPressAxis[i] = time.TotalTime.TotalMilliseconds;

                    var value = GetAxisValue(axis);
                    OnAxisPressed(axis, value, true);
                }
            }

            for (int i = 0; i < repeatingButton.Length; i++)
            {
                var button = (GamePadButton)i;
                if (!IsButtonDown(button))
                    continue;

                var delay = repeatingButton[i] ? PressRepeatDelay : PressRepeatInitialDelay;
                if (delay <= time.TotalTime.TotalMilliseconds - timeLastPressButton[i])
                {
                    repeatingButton[i] = true;
                    timeLastPressButton[i] = time.TotalTime.TotalMilliseconds;

                    OnButtonPressed(button, true);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified axis value counts as being "down."
        /// </summary>
        /// <param name="value">The axis' value.</param>
        /// <returns><see langword="true"/> if the axis is down; otherwise, <see langword="false"/>.</returns>
        private Boolean IsAxisDown(Single value)
        {
            return Math.Abs(value) >= AxisDownThreshold;
        }

        /// <summary>
        /// Gets a value indicating whether the specified axis values count as being "pressed."
        /// </summary>
        private Boolean IsAxisPressed(Single previousValue, Single currentValue)
        {
            var previousDown = IsAxisDown(previousValue);
            var currentDown = IsAxisDown(currentValue);

            if (currentDown && previousDown && Math.Sign(previousValue) != Math.Sign(currentValue))
                return true;

            if (currentDown && !previousDown)
                return true;

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified axis values count as being "released."
        /// </summary>
        private Boolean IsAxisReleased(Single previousValue, Single currentValue)
        {
            var previousDown = IsAxisDown(previousValue);
            var currentDown = IsAxisDown(currentValue);

            if (currentDown && previousDown && Math.Sign(previousValue) != Math.Sign(currentValue))
                return true;

            if (!currentDown && previousDown)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the <see cref="GamePadButton"/> value that corresponds to the specified axis and value.
        /// </summary>
        private GamePadButton ButtonFromAxis(GamePadAxis axis, Single value)
        {
            switch (axis)
            {
                case GamePadAxis.LeftJoystickX:
                    return Math.Sign(value) < 0 ? GamePadButton.LeftStickLeft : GamePadButton.LeftStickRight;

                case GamePadAxis.LeftJoystickY:
                    return Math.Sign(value) < 0 ? GamePadButton.LeftStickUp : GamePadButton.LeftStickDown;

                case GamePadAxis.RightJoystickX:
                    return Math.Sign(value) < 0 ? GamePadButton.RightStickLeft : GamePadButton.RightStickRight;

                case GamePadAxis.RightJoystickY:
                    return Math.Sign(value) < 0 ? GamePadButton.RightStickUp : GamePadButton.RightStickDown;

                case GamePadAxis.LeftTrigger:
                    return GamePadButton.LeftTrigger;

                case GamePadAxis.RightTrigger:
                    return GamePadButton.RightTrigger;
            }

            return GamePadButton.None;
        }

        /// <summary>
        /// Flags the device as registered.
        /// </summary>
        private void Register()
        {
            var input = (SDL2UltravioletInput)Ultraviolet.GetInput();
            if (input.RegisterGamePadDevice(this))
                isRegistered = true;
        }

        // State values.
        private Int32 instanceID;
        private readonly Int32 playerIndex;
        private readonly IntPtr controller;
        private readonly InternalButtonState[] states;
        private Double lastUpdateTime;

        // Property values.
        private readonly String name;
        private Single leftTrigger;
        private Single prevLeftTrigger;
        private Single rightTrigger;
        private Single prevRightTrigger;
        private Vector2 leftJoystickVectorPrev;
        private Single leftJoystickX;
        private Single prevLeftJoystickX;
        private Single leftJoystickY;
        private Single prevLeftJoystickY;
        private Vector2 rightJoystickVectorPrev;
        private Single rightJoystickX;
        private Single prevRightJoystickX;
        private Single rightJoystickY;
        private Single prevRightJoystickY;
        private Boolean isRegistered;

        // Press timers.
        private readonly Double[] timeLastPressAxis;
        private readonly Double[] timeLastPressButton;
        private readonly Boolean[] repeatingAxis;
        private readonly Boolean[] repeatingButton;

        // Repeat delays.
        private const Single PressRepeatInitialDelay = 500.0f;
        private const Single PressRepeatDelay = 33.0f;
    }
}

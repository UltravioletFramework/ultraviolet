using System;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Messages;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.SDL2.Messages;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="GamePadDevice"/> class.
    /// </summary>
    public sealed class SDL2GamePadDevice : GamePadDevice,
        IMessageSubscriber<UltravioletMessageID>
    {
        /// <summary>
        /// Initializes the <see cref="SDL2GamePadDevice"/> type.
        /// </summary>
        static SDL2GamePadDevice()
        {
            sdlButtons = (SDL_GameControllerButton[])Enum.GetValues(typeof(SDL_GameControllerButton));
        }

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

            if ((this.controller = SDL.GameControllerOpen(joystickIndex)) == IntPtr.Zero)
            {
                throw new SDL2Exception();
            }

            this.name = SDL.GameControllerNameForIndex(joystickIndex);
            this.states = new InternalButtonState[sdlButtons.Length];
            this.playerIndex = playerIndex;

            var joystick = SDL.GameControllerGetJoystick(controller);

            if ((this.instanceID = SDL.JoystickInstanceID(joystick)) < 0)
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
                case SDL_EventType.CONTROLLERBUTTONDOWN:
                    {
                        if (evt.cbutton.which == instanceID)
                        {
                            var button = SDLToUltravioletButton((SDL_GameControllerButton)evt.cbutton.button);
                            var buttonIndex = (int)button;
                            states[buttonIndex].OnDown(false);
                            timeLastPressButton[buttonIndex] = lastUpdateTime;
                            repeatingButton[buttonIndex] = false;
                            OnButtonPressed(button, false);
                        }
                    }
                    break;

                case SDL_EventType.CONTROLLERBUTTONUP:
                    {
                        if (evt.cbutton.which == instanceID)
                        {
                            var button = SDLToUltravioletButton((SDL_GameControllerButton)evt.cbutton.button);
                            var buttonIndex = (int)button;
                            states[(int)button].OnUp();
                            timeLastPressButton[buttonIndex] = lastUpdateTime;
                            repeatingButton[buttonIndex] = false;
                            OnButtonReleased(button);
                        }
                    }
                    break;

                case SDL_EventType.CONTROLLERAXISMOTION:
                    {
                        if (evt.caxis.which == instanceID)
                        {
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
        public override String Name
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return name;
            }
        }

        /// <inheritdoc/>
        public override Int32 PlayerIndex
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return playerIndex;
            }
        }

        /// <inheritdoc/>
        public override Single AxisDownThreshold
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return axisPressThreshold;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                axisPressThreshold = value;
            }
        }

        /// <inheritdoc/>
        public override Single LeftTrigger
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return leftTrigger;
            }
        }

        /// <inheritdoc/>
        public override Single RightTrigger
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return rightTrigger;
            }
        }

        /// <inheritdoc/>
        public override Single LeftJoystickX
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return leftJoystickX;
            }
        }

        /// <inheritdoc/>
        public override Single LeftJoystickY
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return leftJoystickY;
            }
        }

        /// <inheritdoc/>
        public override Vector2 LeftJoystickVector
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return new Vector2(leftJoystickX, leftJoystickY);
            }
        }

        /// <inheritdoc/>
        public override Single RightJoystickX
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return rightJoystickX;
            }
        }

        /// <inheritdoc/>
        public override Single RightJoystickY
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return rightJoystickY;
            }
        }

        /// <inheritdoc/>
        public override Vector2 RightJoystickVector
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return new Vector2(rightJoystickX, rightJoystickY);
            }
        }

        /// <summary>
        /// Gets the SDL2 instance identifier of the game pad device.
        /// </summary>
        internal Int32 InstanceID
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return instanceID;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
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
        /// Converts an SDL2 SDL_GameControllerButton value to an Ultraviolet GamePadButton value.
        /// </summary>
        /// <param name="button">The <see cref="SDL_GameControllerButton"/> value to convert.</param>
        /// <returns>The converted <see cref="GamePadButton"/> value.</returns>
        private static GamePadButton SDLToUltravioletButton(SDL_GameControllerButton button)
        {
            return (GamePadButton)(1 + (int)button);
        }

        /// <summary>
        /// Converts an SDL2 SDL_GameControllerAxis value to an Ultraviolet GamePadAxis value.
        /// </summary>
        /// <param name="axis">The <see cref="SDL_GameControllerAxis"/> value to convert.</param>
        /// <returns>The converted <see cref="GamePadAxis"/> value.</returns>
        private static GamePadAxis SDLToUltravioletAxis(SDL_GameControllerAxis axis)
        {
            return (GamePadAxis)(1 + (int)axis);
        }

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
        /// Handles an SDL2 axis motion event.
        /// </summary>
        /// <param name="evt">The SDL2 event data.</param>
        private void OnAxisMotion(SDL_ControllerAxisEvent evt)
        {
            var value = NormalizeAxisValue(evt.value);

            switch ((SDL_GameControllerAxis)evt.axis)
            {
                case SDL_GameControllerAxis.LEFTX:
                    prevLeftJoystickX = leftJoystickX;
                    leftJoystickX = value;
                    OnAxisChanged(GamePadAxis.LeftJoystickX, value);
                    CheckForAxisPresses(GamePadAxis.LeftJoystickX, prevLeftJoystickX, value);
                    break;

                case SDL_GameControllerAxis.LEFTY:
                    prevLeftJoystickY = leftJoystickY;
                    leftJoystickY = value;
                    OnAxisChanged(GamePadAxis.LeftJoystickY, value);
                    CheckForAxisPresses(GamePadAxis.LeftJoystickY, prevLeftJoystickY, value);
                    break;
                
                case SDL_GameControllerAxis.RIGHTX:
                    prevRightJoystickX = rightJoystickX;
                    rightJoystickX = value;
                    OnAxisChanged(GamePadAxis.RightJoystickX, value);
                    CheckForAxisPresses(GamePadAxis.RightJoystickX, prevRightJoystickX, value);
                    break;
                
                case SDL_GameControllerAxis.RIGHTY:
                    prevRightJoystickY = rightJoystickX;
                    rightJoystickY = value;
                    OnAxisChanged(GamePadAxis.RightJoystickY, value);
                    CheckForAxisPresses(GamePadAxis.RightJoystickY, prevRightJoystickY, value);
                    break;
                
                case SDL_GameControllerAxis.TRIGGERLEFT:
                    prevLeftTrigger = leftTrigger;
                    leftTrigger = value;
                    OnAxisChanged(GamePadAxis.LeftTrigger, value);
                    CheckForAxisPresses(GamePadAxis.LeftTrigger, prevLeftTrigger, value);
                    break;
                
                case SDL_GameControllerAxis.TRIGGERRIGHT:
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

            // Axis went from pressed->pressed but changed direction.
            if (Math.Sign(currentValue) != Math.Sign(previousValue))
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
                return;
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

        // The values of the SDL_GameControllerButton enumeration.
        private static readonly SDL_GameControllerButton[] sdlButtons;        

        // State values.
        private readonly Int32 instanceID;
        private readonly Int32 playerIndex;
        private readonly IntPtr controller;
        private readonly InternalButtonState[] states;
        private Double lastUpdateTime;

        // Property values.
        private readonly String name;
        private Single axisPressThreshold;
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

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
            if ((this.controller = SDL.GameControllerOpen(joystickIndex)) == IntPtr.Zero)
            {
                throw new SDL2Exception();
            }

            this.name        = SDL.GameControllerNameForIndex(joystickIndex);
            this.states      = new InternalButtonState[sdlButtons.Length];
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
                            states[(int)button].OnDown(false);
                            OnButtonPressed(button);
                        }
                    }
                    break;

                case SDL_EventType.CONTROLLERBUTTONUP:
                    {
                        if (evt.cbutton.which == instanceID)
                        {
                            var button = SDLToUltravioletButton((SDL_GameControllerButton)evt.cbutton.button);
                            states[(int)button].OnUp();
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
                    leftJoystickX = value;
                    OnAxisChanged(GamePadAxis.LeftJoystickX, value);
                    break;

                case SDL_GameControllerAxis.LEFTY:
                    leftJoystickY = value;
                    OnAxisChanged(GamePadAxis.LeftJoystickY, value);
                    break;
                
                case SDL_GameControllerAxis.RIGHTX:
                    rightJoystickX = value;
                    OnAxisChanged(GamePadAxis.RightJoystickX, value);
                    break;
                
                case SDL_GameControllerAxis.RIGHTY:
                    rightJoystickY = value;
                    OnAxisChanged(GamePadAxis.RightJoystickY, value);
                    break;
                
                case SDL_GameControllerAxis.TRIGGERLEFT:
                    leftTrigger = value;
                    OnAxisChanged(GamePadAxis.LeftTrigger, value);
                    break;
                
                case SDL_GameControllerAxis.TRIGGERRIGHT:
                    rightTrigger = value;
                    OnAxisChanged(GamePadAxis.RightTrigger, value);
                    break;
            }
        }

        // The values of the SDL_GameControllerButton enumeration.
        private static readonly SDL_GameControllerButton[] sdlButtons;
        
        // State values.
        private readonly Int32 instanceID;
        private readonly Int32 playerIndex;
        private readonly IntPtr controller;
        private readonly InternalButtonState[] states;

        // Property values.
        private readonly String name;
        private Single leftTrigger;
        private Single rightTrigger;
        private Vector2 leftJoystickVectorPrev;
        private Single leftJoystickX;
        private Single leftJoystickY;
        private Vector2 rightJoystickVectorPrev;
        private Single rightJoystickX;
        private Single rightJoystickY;
    }
}

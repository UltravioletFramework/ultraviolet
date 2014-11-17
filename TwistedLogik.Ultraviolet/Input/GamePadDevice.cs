using System;

namespace TwistedLogik.Ultraviolet.Input
{
    /// <summary>
    /// Represents the method that is called when a game pad button is pressed or released.
    /// </summary>
    /// <param name="device">The <see cref="GamePadDevice"/> that raised the event.</param>
    /// <param name="button">The <see cref="GamePadButton"/> value that represents the button that was pressed.</param>
    public delegate void GamePadButtonEventHandler(GamePadDevice device, GamePadButton button);

    /// <summary>
    /// Represents the method that is called when a game pad axis changes its value.
    /// </summary>
    /// <param name="device">The <see cref="GamePadDevice"/> that raised the event.</param>
    /// <param name="axis">The <see cref="GamePadAxis"/> value that represents the axis that changed.</param>
    /// <param name="value">The axis' value.</param>
    public delegate void GamePadAxisEventHandler(GamePadDevice device, GamePadAxis axis, Single value);

    /// <summary>
    /// Represents the method that is called when a game pad axis vector changes its value.
    /// </summary>
    /// <param name="device">The <see cref="GamePadDevice"/> that raised the event.</param>
    /// <param name="vector">The axis' vector.</param>
    public delegate void GamePadAxisVectorEventHandler(GamePadDevice device, Vector2 vector);

    /// <summary>
    /// Represents a game pad input device.
    /// </summary>
    public abstract class GamePadDevice : InputDevice<GamePadButton>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePadDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public GamePadDevice(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Gets the device's name.
        /// </summary>
        public abstract String Name
        {
            get;
        }

        /// <summary>
        /// Gets the index of the player that corresponds to this device.
        /// </summary>
        public abstract Int32 PlayerIndex
        {
            get;
        }

        /// <summary>
        /// Gets the normalized value of the left trigger axis.
        /// </summary>
        public abstract Single LeftTrigger
        {
            get;
        }

        /// <summary>
        /// Gets the normalized value of the right trigger axis.
        /// </summary>
        public abstract Single RightTrigger
        {
            get;
        }

        /// <summary>
        /// Gets the normalized value of the left joystick x-axis.
        /// </summary>
        public abstract Single LeftJoystickX
        {
            get;
        }

        /// <summary>
        /// Gets the normalized value of the left joystick y-axis.
        /// </summary>
        public abstract Single LeftJoystickY
        {
            get;
        }

        /// <summary>
        /// Gets a vector representing the position of the left joystick.
        /// </summary>
        public abstract Vector2 LeftJoystickVector
        {
            get;
        }

        /// <summary>
        /// Gets the normalized value of the right joystick x-axis.
        /// </summary>
        public abstract Single RightJoystickX
        {
            get;
        }

        /// <summary>
        /// Gets the normalized value of the right joystick y-axis.
        /// </summary>
        public abstract Single RightJoystickY
        {
            get;
        }

        /// <summary>
        /// Gets a vector representing the position of the right joystick.
        /// </summary>
        public abstract Vector2 RightJoystickVector
        {
            get;
        }

        /// <summary>
        /// Occurs when one of the game pad's buttons is pressed.
        /// </summary>
        public event GamePadButtonEventHandler ButtonPressed;

        /// <summary>
        /// Occurs when one of the game pad's buttons is released.
        /// </summary>
        public event GamePadButtonEventHandler ButtonReleased;

        /// <summary>
        /// Occurs when the value of one of the game pad's axes changes.
        /// </summary>
        public event GamePadAxisEventHandler AxisChanged;

        /// <summary>
        /// Occurs when the value of the game pad's left joystick vector changes.
        /// </summary>
        public event GamePadAxisVectorEventHandler LeftJoystickVectorChanged;

        /// <summary>
        /// Occurs when the value of the game pad's right joystick vector changes.
        /// </summary>
        public event GamePadAxisVectorEventHandler RightJoystickVectorChanged;

        /// <summary>
        /// Raises the <see cref="ButtonPressed"/> event.
        /// </summary>
        /// <param name="button">The <see cref="GamePadButton"/> value that represents the button that was pressed.</param>
        protected virtual void OnButtonPressed(GamePadButton button)
        {
            var temp = ButtonPressed;
            if (temp != null)
            {
                temp(this, button);
            }
        }

        /// <summary>
        /// Raises the <see cref="ButtonReleased"/> event.
        /// </summary>
        /// <param name="button">The <see cref="GamePadButton"/> value that represents the button that was pressed.</param>
        protected virtual void OnButtonReleased(GamePadButton button)
        {
            var temp = ButtonReleased;
            if (temp != null)
            {
                temp(this, button);
            }
        }

        /// <summary>
        /// Raises the AxisChanged event.
        /// </summary>
        /// <param name="axis">The <see cref="GamePadAxis"/> value that represents the axis that changed.</param>
        /// <param name="value">The axis' value.</param>
        protected virtual void OnAxisChanged(GamePadAxis axis, Single value)
        {
            var temp = AxisChanged;
            if (temp != null)
            {
                temp(this, axis, value);
            }
        }

        /// <summary>
        /// Raises the LeftJoystickVectorChanged event.
        /// </summary>
        /// <param name="vector">The axis' vector.</param>
        protected virtual void OnLeftJoystickVectorChanged(Vector2 vector)
        {
            var temp = LeftJoystickVectorChanged;
            if (temp != null)
            {
                temp(this, vector);
            }
        }

        /// <summary>
        /// Raises the RightJoystickVectorChanged event.
        /// </summary>
        /// <param name="vector">The axis' vector.</param>
        protected virtual void OnRightJoystickVectorChanged(Vector2 vector)
        {
            var temp = RightJoystickVectorChanged;
            if (temp != null)
            {
                temp(this, vector);
            }
        }
    }
}

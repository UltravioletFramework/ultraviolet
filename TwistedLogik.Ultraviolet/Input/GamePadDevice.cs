using System;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Input
{
    /// <summary>
    /// Represents the method that is called when a game pad button is pressed or released.
    /// </summary>
    /// <param name="device">The <see cref="GamePadDevice"/> that raised the event.</param>
    /// <param name="button">The <see cref="GamePadButton"/> value that represents the button that was pressed.</param>
    public delegate void GamePadButtonEventHandler(GamePadDevice device, GamePadButton button);

    /// <summary>
    /// Represents a game pad input device.
    /// </summary>
    public abstract class GamePadDevice : InputDevice<GamePadButton>
    {
        /// <summary>
        /// Initializes a new instance of the GamePadDevice class.
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
        /// Occurs when one of the game pad's buttons is pressed.
        /// </summary>
        public event GamePadButtonEventHandler ButtonPressed;

        /// <summary>
        /// Occurs when one of the game pad's buttons is released.
        /// </summary>
        public event GamePadButtonEventHandler ButtonReleased;

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
    }
}

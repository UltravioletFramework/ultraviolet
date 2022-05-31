using System;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents the directions in which a game pad joystick can be oriented.
    /// </summary>
    [Flags]
    public enum GamePadJoystickDirection
    {
        /// <summary>
        /// The joystick is at rest.
        /// </summary>
        None,

        /// <summary>
        /// The joystick is pointed up.
        /// </summary>
        Up = 0x01,
         
        /// <summary>
        /// The joystick is pointed down.
        /// </summary>
        Down = 0x02,

        /// <summary>
        /// The joystick is pointed left.
        /// </summary>
        Left = 0x04,

        /// <summary>
        /// The joystick is pointed right.
        /// </summary>
        Right = 0x08,
    }
}

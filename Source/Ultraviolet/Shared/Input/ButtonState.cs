using System;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents the state of an input button.
    /// </summary>
    [Flags]
    public enum ButtonState
    {
        /// <summary>
        /// The button is up.
        /// </summary>
        Up = 0x0001,

        /// <summary>
        /// The button is down.
        /// </summary>
        Down = 0x0002,

        /// <summary>
        /// The button was pressed this frame.
        /// </summary>
        Pressed = 0x0004,

        /// <summary>
        /// The button was released this frame.
        /// </summary>
        Released = 0x0008,
    }
}

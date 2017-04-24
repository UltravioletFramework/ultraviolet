using System;

namespace Ultraviolet.Presentation.Input
{
    /// <summary>
    /// Represents the keyboard's modifier keys.
    /// </summary>
    [Flags]
    public enum ModifierKeys
    {
        /// <summary>
        /// No modifiers are pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// One of the ALT keys is pressed.
        /// </summary>
        Alt = 1,

        /// <summary>
        /// One of the CTRL keys is pressed.
        /// </summary>
        Control = 2,

        /// <summary>
        /// One of the SHIFT keys is pressed.
        /// </summary>
        Shift = 4,

        /// <summary>
        /// Indicates that the key event has been repeated.
        /// </summary>
        Repeat = 8,
    }
}

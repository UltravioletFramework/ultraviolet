using System;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents the set of flags which specify how to create a new Ultraviolet window.
    /// </summary>
    [Flags]
    public enum WindowFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// The window is resizable.
        /// </summary>
        Resizable = 0x0001,

        /// <summary>
        /// The window is borderless.
        /// </summary>
        Borderless = 0x00002,

        /// <summary>
        /// The window is initially hidden.
        /// </summary>
        Hidden = 0x0004,
    }
}

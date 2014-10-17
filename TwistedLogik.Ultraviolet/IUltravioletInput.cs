using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the Ultraviolet Framework's input subsystem.
    /// </summary>
    public interface IUltravioletInput : IUltravioletSubsystem
    {
        /// <summary>
        /// Gets a value indicating whether the current platform supports keyboard input.
        /// </summary>
        /// <returns>true if the current platform supports keyboard input; otherwise, false.</returns>
        Boolean IsKeyboardSupported();

        /// <summary>
        /// Gets the keyboard device, if keyboard input is supported.
        /// </summary>
        /// <remarks>If keyboard input is not supported on the current platform, this method will throw NotSupportedException.</remarks>
        /// <returns>The keyboard device.</returns>
        KeyboardDevice GetKeyboard();

        /// <summary>
        /// Gets a value indicating whether the current platform supports mouse input.
        /// </summary>
        /// <returns>true if the current platform supports mouse input; otherwise, false.</returns>
        Boolean IsMouseSupported();

        /// <summary>
        /// Gets the mouse device, if mouse input is supported.
        /// </summary>
        /// <remarks>If mouse input is not supported on the current platform, this method will throw NotSupportedException.</remarks>
        /// <returns>The mouse device.</returns>
        MouseDevice GetMouse();
    }
}

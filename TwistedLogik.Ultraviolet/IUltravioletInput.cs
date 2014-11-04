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
        /// <returns><c>true</c> if the current platform supports keyboard input; otherwise, <c>false</c>.</returns>
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
        /// <returns><c>true</c> if the current platform supports mouse input; otherwise, <c>false</c>.</returns>
        Boolean IsMouseSupported();

        /// <summary>
        /// Gets the mouse device, if mouse input is supported.
        /// </summary>
        /// <remarks>If mouse input is not supported on the current platform, this method will throw NotSupportedException.</remarks>
        /// <returns>The mouse device.</returns>
        MouseDevice GetMouse();

        /// <summary>
        /// Gets the number of game pads that are currently connected.
        /// </summary>
        /// <returns>The number of game pads that are currently connected.</returns>
        Int32 GetGamePadCount();

        /// <summary>
        /// Gets the highest index belonging to a connected game pad.
        /// </summary>
        /// <returns>The highest index belonging to a connected game pad.</returns>
        Int32 GetGamePadMaxIndex();

        /// <summary>
        /// Gets a value indicating whether the current platform supports game pad input.
        /// </summary>
        /// <returns><c>true</c> if the current platform supports game pad input; otherwise, <c>false</c>.</returns>
        Boolean IsGamePadSupported();

        /// <summary>
        /// Gets a value indicating whether the game pad with the specified index is connected.
        /// </summary>
        /// <param name="index">The index of the game pad to evaluate.</param>
        /// <returns><c>true</c> if the specified game pad is connected; otherwise, <c>false</c>.</returns>
        Boolean IsGamePadConnected(Int32 index);

        /// <summary>
        /// Gets the game pad device with the specified index.
        /// </summary>
        /// <param name="index">The index of the game pad to retrieve.</param>
        /// <returns>The <see cref="GamePadDevice"/> with the specified index, or <c>null</c> if no such game pad exists.</returns>
        GamePadDevice GetGamePad(Int32 index);

        /// <summary>
        /// Gets the first available game pad device.
        /// </summary>
        /// <returns>The first available game pad device, or null if no game pads are available.</returns>
        GamePadDevice GetFirstGamePad();
    }
}

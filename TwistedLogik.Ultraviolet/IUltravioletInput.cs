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
        /// Gets a value indicating whether the game pad for the specified player is connected.
        /// </summary>
        /// <param name="playerIndex">The index of the player to evaluate.</param>
        /// <returns><c>true</c> if the specified player's game pad is connected; otherwise, <c>false</c>.</returns>
        Boolean IsGamePadConnected(Int32 playerIndex);

        /// <summary>
        /// Gets the game pad that belongs to the specified player.
        /// </summary>
        /// <param name="playerIndex">The index of the player for which to retrieve a game pad.</param>
        /// <returns>The game pad that belongs to the specified player, or <c>null</c> if no such game pad exists.</returns>
        GamePadDevice GetGamePadForPlayer(Int32 playerIndex);

        /// <summary>
        /// Gets the first connected game pad device.
        /// </summary>
        /// <returns>The first connected game pad device, or <c>null</c> if no game pads are connected.</returns>
        GamePadDevice GetFirstConnectedGamePad();
    }
}

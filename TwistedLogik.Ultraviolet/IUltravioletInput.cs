using System;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents the method that is called when a <see cref="GamePadDevice"/> is connected or disconnected.
    /// </summary>
    /// <param name="device">The device that was connected or disconnected.</param>
    /// <param name="playerIndex">The player index associated with the game pad.</param>
    public delegate void GamePadConnectionEventHandler(GamePadDevice device, Int32 playerIndex);

    /// <summary>
    /// Represents the Ultraviolet Framework's input subsystem.
    /// </summary>
    public interface IUltravioletInput : IUltravioletSubsystem
    {
        /// <summary>
        /// Displays the software keyboard, if one is available.
        /// </summary>
        void ShowSoftwareKeyboard();

        /// <summary>
        /// Hides the software keyboard.
        /// </summary>
        void HideSoftwareKeyboard();

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

        /// <summary>
        /// Gets a value indicating whether touch input is supported.
        /// </summary>
        /// <returns><c>true</c> if touch input is supported; otherwise, <c>false</c>.</returns>
        Boolean IsTouchSupported();

        /// <summary>
        /// Gets a value indicating whether a touch device with the specified index exists.
        /// </summary>
        /// <param name="index">The touch device index to evaluate.</param>
        /// <returns><c>true</c> if there is a touch device at the specified index; otherwise, false.</returns>
        Boolean IsTouchDeviceConnected(Int32 index);

        /// <summary>
        /// Gets the first available touch device.
        /// </summary>
        /// <returns>The first available touch device.</returns>
        TouchDevice GetTouchDevice();

        /// <summary>
        /// Gets the touch device with the specified device index.
        /// </summary>
        /// <param name="index">The index of the device to retrieve.</param>
        /// <returns>The touch device with the specified device index, or <c>null</c> if no such device exists.</returns>
        TouchDevice GetTouchDeviceByIndex(Int32 index);

        /// <summary>
        /// Gets or sets a value indicating whether the input subsystem should emulate
        /// mouse inputs using touch inputs.
        /// </summary>
        Boolean EmulateMouseWithTouchInput
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when a game pad is connected to the system.
        /// </summary>
        event GamePadConnectionEventHandler GamePadConnected;

        /// <summary>
        /// Occurs when a game pad is disconnected from the system.
        /// </summary>
        event GamePadConnectionEventHandler GamePadDisconnected;
    }
}

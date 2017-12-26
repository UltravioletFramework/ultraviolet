using System;
using Ultraviolet.Input;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the method that is called when a <see cref="KeyboardDevice"/> is registered
    /// with the Ultraviolet context as a result of receiving input for the first time.
    /// </summary>
    /// <param name="device">The device that was registered.</param>
    public delegate void KeyboardRegistrationEventHandler(KeyboardDevice device);

    /// <summary>
    /// Represents the method that is called when a <see cref="MouseDevice"/> is registered
    /// with the Ultraviolet context as a result of receiving input for the first time.
    /// </summary>
    /// <param name="device">The device that was registered.</param>
    public delegate void MouseRegistrationEventHandler(MouseDevice device);

    /// <summary>
    /// Represents the method that is called when a <see cref="GamePadDevice"/> is 
    /// connected or disconnected.
    /// </summary>
    /// <param name="device">The device that was connected or disconnected.</param>
    /// <param name="playerIndex">The player index associated with the game pad.</param>
    public delegate void GamePadConnectionEventHandler(GamePadDevice device, Int32 playerIndex);

    /// <summary>
    /// Represents the method that is called when a <see cref="GamePadDevice"/> is registered
    /// with the Ultraviolet context as a result of receiving input for the first time.
    /// </summary>
    /// <param name="device">The device that was registered.</param>
    /// <param name="playerIndex">The player index associated with the game pad.</param>
    public delegate void GamePadRegistrationEventHandler(GamePadDevice device, Int32 playerIndex);

    /// <summary>
    /// Represents the method that is called when a <see cref="TouchDevice"/> is registered
    /// with the Ultraviolet context as a result of receiving input for the first time.
    /// </summary>
    /// <param name="device">The device that was registered.</param>
    public delegate void TouchDeviceRegistrationEventHandler(TouchDevice device);

    /// <summary>
    /// Represents the Ultraviolet Framework's input subsystem.
    /// </summary>
    public interface IUltravioletInput : IUltravioletSubsystem
    {
        /// <summary>
        /// Displays the software keyboard, if one is available, 
        /// using <see cref="KeyboardMode.Text"/> as the keyboard mode.
        /// </summary>
        void ShowSoftwareKeyboard();

        /// <summary>
        /// Displays the software keyboard, if one is available, using 
        /// the specified keyboard mode.
        /// </summary>
        /// <param name="mode">A <see cref="KeyboardMode"/> value which specifies 
        /// the type of software keyboard to display.</param>
        void ShowSoftwareKeyboard(KeyboardMode mode);

        /// <summary>
        /// Hides the software keyboard.
        /// </summary>
        void HideSoftwareKeyboard();

        /// <summary>
        /// Gets a value indicating whether the current platform supports
        /// keyboard input.
        /// </summary>
        /// <returns><see langword="true"/> if the current platform supports 
        /// keyboard input; otherwise, <see langword="false"/>.</returns>
        Boolean IsKeyboardSupported();

        /// <summary>
        /// Gets a value indicating whether a keyboard device has been
        /// registered with the context.
        /// </summary>
        /// <returns><see langword="true"/> if a keyboard device is registered 
        /// with the context; otherwise, <see langword="false"/>.</returns>
        Boolean IsKeyboardRegistered();

        /// <summary>
        /// Gets the keyboard device, if keyboard input is supported.
        /// </summary>
        /// <remarks>If keyboard input is not supported on the current platform, 
        /// this method will throw <see cref="NotSupportedException"/>.</remarks>
        /// <returns>The keyboard device.</returns>
        KeyboardDevice GetKeyboard();

        /// <summary>
        /// Gets a value indicating whether the current platform supports 
        /// mouse input.
        /// </summary>
        /// <returns><see langword="true"/> if the current platform supports 
        /// mouse input; otherwise, <see langword="false"/>.</returns>
        Boolean IsMouseSupported();

        /// <summary>
        /// Gets a value indicating whether a mouse device has been registered 
        /// with the context.
        /// </summary>
        /// <returns><see langword="true"/> if a mouse device is registered with 
        /// the context; otherwise, <see langword="false"/></returns>
        Boolean IsMouseRegistered();

        /// <summary>
        /// Gets the mouse device, if mouse input is supported.
        /// </summary>
        /// <remarks>If mouse input is not supported on the current platform, 
        /// this method will throw <see cref="NotSupportedException"/>.</remarks>
        /// <returns>The mouse device.</returns>
        MouseDevice GetMouse();

        /// <summary>
        /// Gets the number of game pads that are currently connected.
        /// </summary>
        /// <returns>The number of game pads that are currently connected.</returns>
        Int32 GetGamePadCount();

        /// <summary>
        /// Gets a value indicating whether the current platform supports 
        /// game pad input.
        /// </summary>
        /// <returns><see langword="true"/> if the current platform supports 
        /// game pad input; otherwise, <see langword="false"/>.</returns>
        Boolean IsGamePadSupported();

        /// <summary>
        /// Gets a value indicating whether any game pad is connected.
        /// </summary>
        /// <returns><see langword="true"/> if ta game pad is connected; 
        /// otherwise, <see langword="false"/>.</returns>
        Boolean IsGamePadConnected();

        /// <summary>
        /// Gets a value indicating whether the game pad for the specified 
        /// player is connected.
        /// </summary>
        /// <param name="playerIndex">The index of the player to evaluate.</param>
        /// <returns><see langword="true"/> if the specified player's game pad 
        /// is connected; otherwise, <see langword="false"/>.</returns>
        Boolean IsGamePadConnected(Int32 playerIndex);

        /// <summary>
        /// Gets a value indicating whether any game pad has been registered 
        /// with the context.
        /// </summary>
        /// <returns><see langword="true"/> if a game pad has been registered; 
        /// otherwise, <see langword="false"/>.</returns>
        Boolean IsGamePadRegistered();

        /// <summary>
        /// Gets a value indicating whether the game pad for the specified player has 
        /// been registered with the context.
        /// </summary>
        /// <returns><see langword="true"/> if the game pad for the specified player
        /// is registered with the context; otherwise, <see langword="false"/></returns>
        Boolean IsGamePadRegistered(Int32 playerIndex);

        /// <summary>
        /// Gets the game pad that belongs to the specified player.
        /// </summary>
        /// <param name="playerIndex">The index of the player for which to 
        /// retrieve a game pad.</param>
        /// <returns>The game pad that belongs to the specified player,
        /// or <see langword="null"/> if no such game pad exists.</returns>
        GamePadDevice GetGamePadForPlayer(Int32 playerIndex);

        /// <summary>
        /// Gets the first connected game pad device.
        /// </summary>
        /// <returns>The first connected game pad device, 
        /// or <see langword="null"/> if no game pads are connected.</returns>
        GamePadDevice GetFirstConnectedGamePad();

        /// <summary>
        /// Gets the first registered game pad device.
        /// </summary>
        /// <returns>The first registered game pad device, 
        /// or <see langword="null"/> if no game pads are registered.</returns>
        GamePadDevice GetFirstRegisteredGamePad();

        /// <summary>
        /// Gets the primary game pad.
        /// </summary>
        /// <remarks>The primary game pad is the first device which was registered as a
        /// result of receiving user input.</remarks>
        /// <returns>The primary game pad, or <see langword="null"/> if there is no
        /// primary game pad.</returns>
        GamePadDevice GetPrimaryGamePad();

        /// <summary>
        /// Gets a value indicating whether touch input is supported.
        /// </summary>
        /// <returns><see langword="true"/> if touch input is supported; 
        /// otherwise, <see langword="false"/>.</returns>
        Boolean IsTouchSupported();

        /// <summary>
        /// Gets a value indicating whether any touch devices are connected.
        /// </summary>
        /// <returns><see langword="true"/> if a touch device is connected; 
        /// otherwise, <see langword="false"/></returns>
        Boolean IsTouchDeviceConnected();

        /// <summary>
        /// Gets a value indicating whether the touch device with the specified 
        /// index is connected.
        /// </summary>
        /// <param name="index">The index of the touch device to evaluate.</param>
        /// <returns><see langword="true"/> if a touch device with the specified index 
        /// is connected; otherwise, <see langword="false"/></returns>
        Boolean IsTouchDeviceConnected(Int32 index);

        /// <summary>
        /// Gets a value indicating whether a touch device device has been registered 
        /// with the context.
        /// </summary>
        /// <returns><see langword="true"/> if a touch device is registered with the 
        /// context; otherwise, <see langword="false"/></returns>
        Boolean IsTouchDeviceRegistered();

        /// <summary>
        /// Gets a value indicating whether the touch device with the specified index has 
        /// been registered with the context.
        /// </summary>
        /// <returns><see langword="true"/> if the touch device with the specified index
        /// is registered with the context; otherwise, <see langword="false"/></returns>
        Boolean IsTouchDeviceRegistered(Int32 index);
        
        /// <summary>
        /// Gets the touch device with the specified device index.
        /// </summary>
        /// <param name="index">The index of the device to retrieve.</param>
        /// <returns>The touch device with the specified device index,
        /// or <see langword="null"/> if no such device exists.</returns>
        TouchDevice GetTouchDevice(Int32 index);

        /// <summary>
        /// Gets the first connected touch device.
        /// </summary>
        /// <returns>The first connected touch device, or <see langword="null"/> if no
        /// touch devices have been connected.</returns>
        TouchDevice GetFirstConnectedTouchDevice();

        /// <summary>
        /// Gets the first registered touch device.
        /// </summary>
        /// <returns>The first registered touch device, or <see langword="null"/> if no
        /// touch devices have been registered.</returns>
        TouchDevice GetFirstRegisteredTouchDevice();

        /// <summary>
        /// Gets the primary touch device.
        /// </summary>
        /// <remarks>The primary touch device is the first device which was registered as a
        /// result of receiving user input.</remarks>
        /// <returns>The primary touch device, or <see langword="null"/> if there is no 
        /// primary touch device.</returns>
        TouchDevice GetPrimaryTouchDevice();

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
        /// Gets a value indicating whether a mouse cursor is available, either from a physical
        /// device or from touch emulation.
        /// </summary>
        Boolean IsMouseCursorAvailable
        {
            get;            
        }

        /// <summary>
        /// Occurs when a keyboard device is registered as a result of receiving
        /// user input for the first time.
        /// </summary>
        event KeyboardRegistrationEventHandler KeyboardRegistered;

        /// <summary>
        /// Occurs when a mouse device is registered as a result of receiving
        /// user input for the first time.
        /// </summary>
        event MouseRegistrationEventHandler MouseRegistered;

        /// <summary>
        /// Occurs when a game pad is connected.
        /// </summary>
        event GamePadConnectionEventHandler GamePadConnected;

        /// <summary>
        /// Occurs when a game pad is disconnected.
        /// </summary>
        event GamePadConnectionEventHandler GamePadDisconnected;

        /// <summary>
        /// Occurs when a game pad device is registered as a result of receiving
        /// user input for the first time.
        /// </summary>
        event GamePadRegistrationEventHandler GamePadRegistered;

        /// <summary>
        /// Occurs when a touch device is registered as a result of receiving
        /// user input for the first time.
        /// </summary>
        event TouchDeviceRegistrationEventHandler TouchDeviceRegistered;
    }
}

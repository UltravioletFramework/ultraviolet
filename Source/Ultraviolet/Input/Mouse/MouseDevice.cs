using System;
using Ultraviolet.Platform;

namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents the method that is called when a mouse button is pressed or released.
    /// </summary>
    /// <param name="window">The window in which the input event took place.</param>
    /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
    /// <param name="button">The <see cref="MouseButton"/> value that represents the button that was pressed or released.</param>
    public delegate void MouseButtonEventHandler(IUltravioletWindow window, MouseDevice device, MouseButton button);

    /// <summary>
    /// Represents the method that is called when the mouse is moved.
    /// </summary>
    /// <param name="window">The window in which the input event took place.</param>
    /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
    /// <param name="x">The x-coordinate of the mouse's current position.</param>
    /// <param name="y">The y-coordinate of the mouse's current position.</param>
    /// <param name="dx">The difference between the x-coordinate of the mouse's 
    /// current position and the x-coordinate of the mouse's previous position.</param>
    /// <param name="dy">The difference between the y-coordinate of the mouse's 
    /// current position and the y-coordinate of the mouse's previous position.</param>
    public delegate void MouseMoveEventHandler(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy);

    /// <summary>
    /// Represents the method that is called when the mouse's wheel is scrolled.
    /// </summary>
    /// <param name="window">The window in which the input event took place.</param>
    /// <param name="device">The <see cref="MouseDevice"/> that raised the event.</param>
    /// <param name="x">The amount that the wheel was scrolled along the horizontal axis.</param>
    /// <param name="y">The amount that the wheel was scrolled along the vertical axis.</param>
    public delegate void MouseWheelEventHandler(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y);

    /// <summary>
    /// Represents a mouse device.
    /// </summary>
    public abstract class MouseDevice : InputDevice<MouseButton>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public MouseDevice(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Sets the mouse cursor's position to the specified point within the specified window.
        /// </summary>
        /// <param name="window">The window within which to place the mouse cursor.</param>
        /// <param name="x">The x-coordinate within <paramref name="window"/> at which to position the cursor.</param>
        /// <param name="y">The y-coordinate within <paramref name="window"/> at which to position the cursor.</param>
        public abstract void WarpToWindow(IUltravioletWindow window, Int32 x, Int32 y);

        /// <summary>
        /// Sets the mouse cursor's position to the center of the specified window.
        /// </summary>
        /// <param name="window">The window within which to place the mouse cursor.</param>
        public abstract void WarpToWindowCenter(IUltravioletWindow window);

        /// <summary>
        /// Sets the mouse cursor's position to the specified point within the application's primary window.
        /// </summary>
        /// <param name="x">The x-coordinate within the primary window at which to position the cursor.</param>
        /// <param name="y">The y-coordinate within the primary window at which to position the cursor.</param>
        public abstract void WarpToPrimaryWindow(Int32 x, Int32 y);

        /// <summary>
        /// Sets the mouse cursor's position to the center of the application's primary window.
        /// </summary>
        public abstract void WarpToPrimaryWindowCenter();

        /// <summary>
        /// Gets the mouse cursor's position within the specified window.
        /// </summary>
        /// <param name="window">The window to evaluate.</param>
        /// <returns>The cursor's compositor-space position within the specified 
        /// window, or <see langword="null"/> if the cursor is outside of the window.</returns>
        public abstract Point2? GetPositionInWindow(IUltravioletWindow window);

        /// <summary>
        /// Gets a value indicating whether the specified button was clicked this frame.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> value that represents the button to evaluate.</param>
        /// <returns><see langword="true"/> if the button was clicked this frame; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean IsButtonClicked(MouseButton button);

        /// <summary>
        /// Gets a value indicating whether the specified button was double clicked this frame.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> value that represents the button to evaluate.</param>
        /// <returns><see langword="true"/> if the button was double clicked this frame; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean IsButtonDoubleClicked(MouseButton button);

        /// <summary>
        /// Gets a value indicating whether relative mouse mode is currently enabled.
        /// </summary>
        /// <returns><see langword="true"/> if relative mouse mode is currently enabled; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean GetIsRelativeModeEnabled();

        /// <summary>
        /// Enables or disables relative mouse mode. In relative mode, the cursor is hidden, and the driver will try
        /// to report continuous motion in the current window. Only relative motion events will be delivered, and
        /// the mouse position will not change.
        /// </summary>
        /// <param name="enabled"><see langword="true"/> to enable relative mode; otherwise, <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if relative mode was enabled or disabled; <see langword="false"/> if relative mode is not supported.</returns>
        public abstract Boolean SetIsRelativeModeEnabled(Boolean enabled);

        /// <summary>
        /// Gets the window that currently contains the mouse cursor.
        /// </summary>
        public abstract IUltravioletWindow Window
        {
            get;
        }

        /// <summary>
        /// Gets the mouse's current position.
        /// </summary>
        public abstract Point2 Position
        {
            get;
        }

        /// <summary>
        /// Gets the x-coordinate of the mouse's current position.
        /// </summary>
        public abstract Int32 X
        {
            get;
        }

        /// <summary>
        /// Gets the y-coordinate of the mouse's current position.
        /// </summary>
        public abstract Int32 Y
        {
            get;
        }

        /// <summary>
        /// Gets the mouse's horizontal scroll wheel delta in the last frame.
        /// </summary>
        public abstract Int32 WheelDeltaX
        {
            get;
        }

        /// <summary>
        /// Gets the mouse's vertical scroll wheel delta in the last frame.
        /// </summary>
        public abstract Int32 WheelDeltaY
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether one of the Control modifier keys is currently down.
        /// </summary>
        public Boolean IsControlDown
        {
            get
            {
                var input = Ultraviolet.GetInput();

                if (!input.IsKeyboardSupported())
                    return false;

                return input.GetKeyboard().IsControlDown;
            }
        }

        /// <summary>
        /// Gets a value indicating whether one of the Alt modifier keys is currently down.
        /// </summary>
        public Boolean IsAltDown
        {
            get
            {
                var input = Ultraviolet.GetInput();

                if (!input.IsKeyboardSupported())
                    return false;

                return input.GetKeyboard().IsAltDown;
            }
        }

        /// <summary>
        /// Gets a value indicating whether one of the Shift modifier keys is currently down.
        /// </summary>
        public Boolean IsShiftDown
        {
            get
            {
                var input = Ultraviolet.GetInput();

                if (!input.IsKeyboardSupported())
                    return false;

                return input.GetKeyboard().IsShiftDown;
            }
        }

        /// <summary>
        /// Occurs when a mouse button is pressed.
        /// </summary>
        public event MouseButtonEventHandler ButtonPressed;

        /// <summary>
        /// Occurs when a mouse button is released.
        /// </summary>
        public event MouseButtonEventHandler ButtonReleased;

        /// <summary>
        /// Occurs when a mouse button is clicked.
        /// </summary>
        public event MouseButtonEventHandler Click;

        /// <summary>
        /// Occurs when a mouse button is double clicked.
        /// </summary>
        public event MouseButtonEventHandler DoubleClick;

        /// <summary>
        /// Occurs when the mouse is moved.
        /// </summary>
        public event MouseMoveEventHandler Moved;

        /// <summary>
        /// Occurs when the mouse wheel is scrolled.
        /// </summary>
        public event MouseWheelEventHandler WheelScrolled;

        /// <summary>
        /// Raises the <see cref="ButtonPressed"/> event.
        /// </summary>
        /// <param name="window">The window in which the input event took place.</param>
        /// <param name="button">The mouse button that was pressed.</param>
        protected virtual void OnButtonPressed(IUltravioletWindow window, MouseButton button) =>
            ButtonPressed?.Invoke(window, this, button);

        /// <summary>
        /// Raises the <see cref="ButtonReleased"/> event.
        /// </summary>
        /// <param name="window">The window in which the input event took place.</param>
        /// <param name="button">The mouse button that was released.</param>
        protected virtual void OnButtonReleased(IUltravioletWindow window, MouseButton button) =>
            ButtonReleased?.Invoke(window, this, button);

        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// </summary>
        /// <param name="window">The window in which the input event took place.</param>
        /// <param name="button">The mouse button that was clicked.</param>
        protected virtual void OnClick(IUltravioletWindow window, MouseButton button) =>
            Click?.Invoke(window, this, button);

        /// <summary>
        /// Raises the <see cref="DoubleClick"/> event.
        /// </summary>
        /// <param name="window">The window in which the input event took place.</param>
        /// <param name="button">The mouse button that was clicked.</param>
        protected virtual void OnDoubleClick(IUltravioletWindow window, MouseButton button) =>
            DoubleClick?.Invoke(window, this, button);

        /// <summary>
        /// Raises the <see cref="Moved"/> event.
        /// </summary>
        /// <param name="window">The window in which the input event took place.</param>
        /// <param name="x">The x-coordinate of the mouse's current position.</param>
        /// <param name="y">The y-coordinate of the mouse's current position.</param>
        /// <param name="dx">The difference between the x-coordinate of the mouse's 
        /// current position and the x-coordinate of the mouse's previous position.</param>
        /// <param name="dy">The difference between the y-coordinate of the mouse's 
        /// current position and the y-coordinate of the mouse's previous position.</param>
        protected virtual void OnMoved(IUltravioletWindow window, Int32 x, Int32 y, Int32 dx, Int32 dy) =>
            Moved?.Invoke(window, this, x, y, dx, dy);

        /// <summary>
        /// Raises the <see cref="WheelScrolled"/> event.
        /// </summary>
        /// <param name="window">The window in which the input event took place.</param>
        /// <param name="x">The amount that the wheel was scrolled along the horizontal axis.</param>
        /// <param name="y">The amount that the wheel was scrolled along the vertical axis.</param>
        protected virtual void OnWheelScrolled(IUltravioletWindow window, Int32 x, Int32 y) =>
            WheelScrolled?.Invoke(window, this, x, y);
    }
}

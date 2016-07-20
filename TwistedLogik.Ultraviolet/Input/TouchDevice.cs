using System;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Input
{
    /// <summary>
    /// Represents the method that is called when a touch input begins.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="touchID">The unique identifier of the touch input.</param>
    /// <param name="fingerID">The unique identifier of the finger which caused the touch.</param>
    /// <param name="x">The normalized x-coordinate of the touch.</param>
    /// <param name="y">The normalized y-coordinate of the touch.</param>
    /// <param name="pressure">The normalized pressure of the touch.</param>
    public delegate void TouchDownEventHandler(TouchDevice device,
        Int64 touchID, Int64 fingerID, Single x, Single y, Single pressure);

    /// <summary>
    /// Represents the method that is called when a touch input ends.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="touchID">The unique identifier of the touch input.</param>
    /// <param name="fingerID">The unique identifier of the finger which caused the touch.</param>
    public delegate void TouchUpEventHandler(TouchDevice device,
        Int64 touchID, Int64 fingerID);

    /// <summary>
    /// Represents the method that is called when a touch input moves.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="touchID">The unique identifier of the touch input.</param>
    /// <param name="fingerID">The unique identifier of the finger which caused the touch.</param>
    /// <param name="x">The normalized x-coordinate of the touch.</param>
    /// <param name="y">The normalized y-coordinate of the touch.</param>
    /// <param name="dx">The distance that the touch has moved along the x-axis in normalized coordinates.</param>
    /// <param name="dy">The distance that the touch has moved along the y-axis in normalized coordinates.</param>
    /// <param name="pressure">The normalized pressure of the touch.</param>
    public delegate void TouchMotionEventHandler(TouchDevice device, 
        Int64 touchID, Int64 fingerID, Single x, Single y, Single dx, Single dy, Single pressure);

    /// <summary>
    /// Represents the method that is called when a touch is interpreted as a tap.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="fingerID">The unique identifier of the finger which caused the tap.</param>
    /// <param name="x">The normalized x-coordinate of the tap.</param>
    /// <param name="y">The normalized y-coordinate of the tap.</param>
    public delegate void TouchTapEventHandler(TouchDevice device,
        Int64 fingerID, Single x, Single y);

    /// <summary>
    /// Represents the method that is called when a multiple-finger touch gesture is performed.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="x">The normalized x-coordinate of the gesture's centroid.</param>
    /// <param name="y">The normalized y-coordinate of the gesture's centroid.</param>
    /// <param name="theta">The amount that the fingers rotated during the gesture.</param>
    /// <param name="distance">The amount that the fingers pinched during the gesture.</param>
    /// <param name="fingers">The number of fingers involved in the gesture.</param>
    public delegate void MultiGestureEventHandler(TouchDevice device,
        Single x, Single y, Single theta, Single distance, Int32 fingers);

    /// <summary>
    /// Represents a touch input device.
    /// </summary>
    public abstract class TouchDevice : InputDevice
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public TouchDevice(UltravioletContext uv) :
            base(uv)
        { }
        
        /// <summary>
        /// Converts the specified window coordinates into normalized touch coordinates.
        /// </summary>
        /// <param name="window">The window for which to perform the conversion.</param>
        /// <param name="coordinates">The window-space coordinates to convert to normalized touch coordinates.</param>
        /// <returns>The normalized touch coordinates which correspond to the specified window-space coordinates.</returns>
        public abstract Point2F NormalizeCoordinates(IUltravioletWindow window, Point2 coordinates);

        /// <summary>
        /// Converts the specified window coordinates into normalized touch coordinates.
        /// </summary>
        /// <param name="window">The window for which to perform the conversion.</param>
        /// <param name="x">The window-space x-coordinate to convert to a normalized touch coordinate.</param>
        /// <param name="y">The window-space y-coordinate to convert to a normalized touch coordinate.</param>
        /// <returns>The normalized touch coordinates which correspond to the specified window-space coordinates.</returns>
        public abstract Point2F NormalizeCoordinates(IUltravioletWindow window, Int32 x, Int32 y);

        /// <summary>
        /// Converts the specified normalized touch coordinates into window-space coordinates.
        /// </summary>
        /// <param name="window">The window for which to perform the conversion.</param>
        /// <param name="coordinates">The normalized touch coordinates to convert to window-space coordinates.</param>
        /// <returns>The window-space coordinates which correspond to the specified normalized touch coordinates.</returns>
        public abstract Point2 DenormalizeCoordinates(IUltravioletWindow window, Point2F coordinates);

        /// <summary>
        /// Converts the specified normalized touch coordinates into window-space coordinates.
        /// </summary>
        /// <param name="window">The window for which to perform the conversion.</param>
        /// <param name="x">The normalized touch x-coordinate to convert to a window-space coordinate.</param>
        /// <param name="y">The normalized touch y-coordinate to convert to a window-space coordinate.</param>
        /// <returns>The window-space coordinates which correspond to the specified normalized touch coordinates.</returns>
        public abstract Point2 DenormalizeCoordinates(IUltravioletWindow window, Single x, Single y);

        /// <summary>
        /// Gets a value indicating whether the specified touch is currently active.
        /// </summary>
        /// <param name="touchID">The identifier of the touch to evaluate.</param>
        /// <returns><see langword="true"/> if the specified touch is currently active; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean IsActive(Int64 touchID);

        /// <summary>
        /// Attempts to retrieve the parameters of the specified touch.
        /// </summary>
        /// <param name="touchID">The identifier of the touch to retrieve.</param>
        /// <param name="touchInfo">The parameters of the specified touch.</param>
        /// <returns><see langword="true"/> if the touch is active and its parameters were retrieved; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean TryGetTouch(Int64 touchID, out TouchInfo touchInfo);

        /// <summary>
        /// Retrieves the parameters of the specified touch, throwing an exception if the touch is not active.
        /// </summary>
        /// <param name="touchID">The identifier of the touch to retrieve.</param>
        /// <returns>The parameters for the specified touch.</returns>
        public abstract TouchInfo GetTouch(Int64 touchID);

        /// <summary>
        /// Retrieves the parameters of the specified touch, throwing an exception if the touch is not active.
        /// </summary>
        /// <param name="index">The index of the touch to retrieve. The value of this parameter must
        /// be greater than or equal to 0 and less than <see cref="TouchCount"/>.</param>
        /// <returns>The parameters for the specified touch.</returns>
        public abstract TouchInfo GetTouchByIndex(Int32 index);
        
        /// <summary>
        /// Gets a value indicating whether a tap was performed in the previous frame.
        /// </summary>
        /// <returns><see langword="true"/> if a tap was performed; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean WasTapped();

        /// <summary>
        /// Gets a value indicating whether a tap was performed in the previous frame which fell within
        /// the specified region (given in normalized coordinates).
        /// </summary>
        /// <param name="area">The area to evaluate, given in normalized coordinates.</param>
        /// <returns><see langword="true"/> if a tap was performed in the specified area; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean WasTappedWithin(RectangleF area);

        /// <summary>
        /// Gets a value indicating whether any active touch is currently within
        /// the specified region (given in normalized coordinates).
        /// </summary>
        /// <param name="area">The area to evaluate, given in normalized coordinates.</param>
        /// <returns><see langword="true"/> if a touch is within the specified area; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean IsTouchWithin(RectangleF area);

        /// <summary>
        /// Gets a value indicating whether the specified touch is currently within
        /// the specified region (given in normalized coordinates).
        /// </summary>
        /// <param name="touchID">The identifier of the touch to evaluate.</param>
        /// <param name="area">The area to evaluate, given in normalized coordinates.</param>
        /// <returns><see langword="true"/> if the specified touch is within the specified area; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean IsTouchWithin(Int64 touchID, RectangleF area);

        /// <summary>
        /// Gets the number of currently active touches.
        /// </summary>
        public abstract Int32 TouchCount { get; }

        /// <summary>
        /// Gets the number of taps in the last frame.
        /// </summary>
        public abstract Int32 TapCount { get; }

        /// <summary>
        /// Gets the maximum distance that a finger can move between its up and down events in order
        /// for the input to be considered a "tap." This value is in normalized units; i.e. a value
        /// of 0.05 means that the finger can move across 5% of the device and still be considered a tap.
        /// </summary>
        public Single MaximumTapDistance { get; set; } = 0.05f;

        /// <summary>
        /// Gets the maximum delay in milliseconds between a finger's up and down events in order
        /// for the input to be considered a "tap."
        /// </summary>
        public Double MaximumTapDelay { get; set; } = 500.0;

        /// <summary>
        /// Occurs when a touch input begins.
        /// </summary>
        public event TouchDownEventHandler TouchDown;

        /// <summary>
        /// Occurs when a touch input ends.
        /// </summary>
        public event TouchUpEventHandler TouchUp;

        /// <summary>
        /// Occurs when a touch input moves.
        /// </summary>
        public event TouchMotionEventHandler TouchMotion;

        /// <summary>
        /// Occurs when atouch input is interpreted as a tap.
        /// </summary>
        public event TouchTapEventHandler Tap;

        /// <summary>
        /// Occurs when a multiple-finger touch gesture is performed.
        /// </summary>
        public event MultiGestureEventHandler MultiGesture;

        /// <summary>
        /// Raises the <see cref="TouchDown"/> event.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch input.</param>
        /// <param name="fingerID">The unique identifier of the finger which caused the touch.</param>
        /// <param name="x">The normalized x-coordinate of the touch.</param>
        /// <param name="y">The normalized y-coordinate of the touch.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        protected virtual void OnTouchDown(Int64 touchID, Int64 fingerID, Single x, Single y, Single pressure)
        {
            TouchDown?.Invoke(this, touchID, fingerID, x, y, pressure);
        }

        /// <summary>
        /// Raises the <see cref="TouchUp"/> event.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch input.</param>
        /// <param name="fingerID">The unique identifier of the finger which caused the touch.</param>
        protected virtual void OnTouchUp(Int64 touchID, Int64 fingerID)
        {
            TouchUp?.Invoke(this, touchID, fingerID);
        }

        /// <summary>
        /// Raises the <see cref="TouchMotion"/> event.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch input.</param>
        /// <param name="fingerID">The unique identifier of the finger which caused the touch.</param>
        /// <param name="x">The normalized x-coordinate of the touch.</param>
        /// <param name="y">The normalized y-coordinate of the touch.</param>
        /// <param name="dx">The distance that the touch has moved along the x-axis in normalized coordinates.</param>
        /// <param name="dy">The distance that the touch has moved along the y-axis in normalized coordinates.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        protected virtual void OnTouchMotion(Int64 touchID, Int64 fingerID, Single x, Single y, Single dx, Single dy, Single pressure)
        {
            TouchMotion?.Invoke(this, touchID, fingerID, x, y, dx, dy, pressure);
        }

        /// <summary>
        /// Raises the <see cref="Tap"/> event.
        /// </summary>
        /// <param name="fingerID">The unique identifier of the finger which caused the tap.</param>
        /// <param name="x">The normalized x-coordinate of the tap.</param>
        /// <param name="y">The normalized y-coordinate of the tap.</param>
        protected virtual void OnTap(Int64 fingerID, Single x, Single y)
        {
            Tap?.Invoke(this, fingerID, x, y);
        }

        /// <summary>
        /// Raises the <see cref="MultiGesture"/> event.
        /// </summary>
        /// <param name="x">The normalized x-coordinate of the gesture's centroid.</param>
        /// <param name="y">The normalized y-coordinate of the gesture's centroid.</param>
        /// <param name="theta">The amount that the fingers rotated during the gesture.</param>
        /// <param name="distance">The amount that the fingers pinched during the gesture.</param>
        /// <param name="fingers">The number of fingers involved in the gesture.</param>
        protected virtual void OnMultiGesture(Single x, Single y, Single theta, Single distance, Int32 fingers)
        {
            MultiGesture?.Invoke(this, x, y, theta, distance, fingers);
        }
    }
}

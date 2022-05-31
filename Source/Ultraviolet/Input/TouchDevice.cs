using System;
using System.IO;
using System.Threading.Tasks;
using Ultraviolet.Platform;

namespace Ultraviolet.Input
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
    /// <param name="touchID">The unique identifier of the touch which caused the tap.</param>
    /// <param name="fingerID">The unique identifier of the finger which caused the tap.</param>
    /// <param name="x">The normalized x-coordinate of the tap.</param>
    /// <param name="y">The normalized y-coordinate of the tap.</param>
    public delegate void TouchTapEventHandler(TouchDevice device,
        Int64 touchID, Int64 fingerID, Single x, Single y);

    /// <summary>
    /// Represents the method that is called when a touch becomes a long press.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="touchID">The unique identifier of the touch input.</param>
    /// <param name="fingerID">The unique identifier of the finger which caused the touch.</param>
    /// <param name="x">The normalized x-coordinate of the touch.</param>
    /// <param name="y">The normalized y-coordinate of the touch.</param>
    /// <param name="pressure">The normalized pressure of the touch.</param>
    public delegate void TouchLongPressEventHandler(TouchDevice device,
        Int64 touchID, Int64 fingerID, Single x, Single y, Single pressure);

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
    /// Represents the method that is called when a $1 gesture is recorded.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="gestureID">The unique identifier of the recorded gesture.</param>
    public delegate void DollarGestureRecordedEventHandler(TouchDevice device,
        Int64 gestureID);

    /// <summary>
    /// Represents the method that is called when a $1 gesture is performed.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="gestureID">The unique identifier of the gesture which was performed.</param>
    /// <param name="x">The normalized x-coordinate of the gesture's centroid.</param>
    /// <param name="y">The normalized y-coordinate of the gesture's centroid.</param>
    /// <param name="error">The difference between the gesture template and the actual performed gesture; lower is better.</param>
    /// <param name="fingers">The number of fingers used to perform the gesture.</param>
    public delegate void DollarGestureEventHandler(TouchDevice device,
        Int64 gestureID, Single x, Single y, Single error, Int32 fingers);

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
        /// Binds the touch device to the specified window. This window's coordinate system is used
        /// to denormalize touch coordinates. If no window is bound to the device, the primary window is used.
        /// </summary>
        /// <param name="window">The window to bind to the device.</param>
        public abstract void BindToWindow(IUltravioletWindow window);

        /// <summary>
        /// Converts the specified window coordinates into normalized touch coordinates.
        /// </summary>
        /// <param name="coordinates">The window-space coordinates to convert to normalized touch coordinates.</param>
        /// <returns>The normalized touch coordinates which correspond to the specified window-space coordinates.</returns>
        public abstract Point2F NormalizeCoordinates(Point2 coordinates);

        /// <summary>
        /// Converts the specified window coordinates into normalized touch coordinates.
        /// </summary>
        /// <param name="x">The window-space x-coordinate to convert to a normalized touch coordinate.</param>
        /// <param name="y">The window-space y-coordinate to convert to a normalized touch coordinate.</param>
        /// <returns>The normalized touch coordinates which correspond to the specified window-space coordinates.</returns>
        public abstract Point2F NormalizeCoordinates(Int32 x, Int32 y);

        /// <summary>
        /// Converts the specified normalized touch coordinates into window-space coordinates.
        /// </summary>
        /// <param name="coordinates">The normalized touch coordinates to convert to window-space coordinates.</param>
        /// <returns>The window-space coordinates which correspond to the specified normalized touch coordinates.</returns>
        public abstract Point2 DenormalizeCoordinates(Point2F coordinates);

        /// <summary>
        /// Converts the specified normalized touch coordinates into window-space coordinates.
        /// </summary>
        /// <param name="x">The normalized touch x-coordinate to convert to a window-space coordinate.</param>
        /// <param name="y">The normalized touch y-coordinate to convert to a window-space coordinate.</param>
        /// <returns>The window-space coordinates which correspond to the specified normalized touch coordinates.</returns>
        public abstract Point2 DenormalizeCoordinates(Single x, Single y);

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
        /// Gets a value indicating whether the specified touch was the first touch
        /// in the currently active gesture.
        /// </summary>
        /// <remarks>A gesture begins when the first finger is pressed against the device, and only
        /// ends when all fingers are removed from the device.</remarks>
        /// <param name="touchID">The unique identifier of the touch to evaluate.</param>
        /// <returns><see langword="true"/> if the specified touch is the first touch in 
        /// the currently active gesture; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean IsFirstTouchInGesture(Int64 touchID);

        /// <summary>
        /// Gets the index of the specified touch within the currently active gesture.
        /// </summary>
        /// <remarks>A gesture begins when the first finger is pressed against the device, and only
        /// ends when all fingers are removed from the device.</remarks>
        /// <param name="touchID">The unique identifier of the touch to evaluate.</param>
        /// <returns>The index of the specified touch, or -1 if the touch is not active.</returns>
        public abstract Int32 GetTouchIndex(Int64 touchID);

        /// <summary>
        /// Instructs the device to begin recording a $1 gesture.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> which produces the identifier of the recorded gesture.</returns>
        public abstract Task<Int64> RecordDollarGestureAsync();

        /// <summary>
        /// Instructs the device to begin recording a $1 gesture.
        /// </summary>
        /// <returns><see langword="true"/> if recording started successfully; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean RecordDollarGesture();

        /// <summary>
        /// Loads $1 gestures for the device from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which to load the device's $1 gestures.</param>
        public abstract void LoadDollarGestures(Stream stream);

        /// <summary>
        /// Saves $1 gestures for the device to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which to save the device's $1 gestures.</param>
        public abstract void SaveDollarGestures(Stream stream);

        /// <summary>
        /// Gets the window which is bound to the device.
        /// </summary>
        public abstract IUltravioletWindow BoundWindow { get; }

        /// <summary>
        /// Gets a value indicating whether the device is recording a $1 gesture.
        /// </summary>
        public abstract Boolean IsRecordingDollarGesture { get; }

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
        /// for the input to be considered a "tap." This value is in device-independent pixels (1/96 of an inch).
        /// </summary>
        public Double TapMaximumDistance { get; set; } = 5.0;

        /// <summary>
        /// Gets the maximum delay in milliseconds between a finger's up and down events in order
        /// for the input to be considered a "tap."
        /// </summary>
        public Double TapDelay { get; set; } = 500.0;

        /// <summary>
        /// Gets the maximum distance that a finger can move before a touch can no longer be considered a long press.
        /// This value is in device independent pixels (1/96 of an inch).
        /// </summary>
        public Double LongPressMaximumDistance { get; set; } = 5.0;

        /// <summary>
        /// Gets the delay in milliseconds before a touch is considered a long tap.
        /// </summary>
        public Double LongPressDelay { get; set; } = 1000.0;
        
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
        /// Occurs when a touch input is interpreted as a tap.
        /// </summary>
        public event TouchTapEventHandler Tap;

        /// <summary>
        /// Occurs when a touch input becomes a long press.
        /// </summary>
        public event TouchLongPressEventHandler LongPress;

        /// <summary>
        /// Occurs when a multiple-finger touch gesture is performed.
        /// </summary>
        public event MultiGestureEventHandler MultiGesture;

        /// <summary>
        /// Occurs when a $1 gesture is recorded.
        /// </summary>
        public event DollarGestureRecordedEventHandler DollarGestureRecorded;

        /// <summary>
        /// Occurs when a $1 gesture is performed.
        /// </summary>
        public event DollarGestureEventHandler DollarGesture;

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
        /// <param name="touchID">The unique identifier of the touch which caused the tap.</param>
        /// <param name="fingerID">The unique identifier of the finger which caused the tap.</param>
        /// <param name="x">The normalized x-coordinate of the tap.</param>
        /// <param name="y">The normalized y-coordinate of the tap.</param>
        protected virtual void OnTap(Int64 touchID, Int64 fingerID, Single x, Single y)
        {
            Tap?.Invoke(this, touchID, fingerID, x, y);
        }

        /// <summary>
        /// Raises the <see cref="LongPress"/> event.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch input.</param>
        /// <param name="fingerID">The unique identifier of the finger which caused the touch.</param>
        /// <param name="x">The normalized x-coordinate of the touch.</param>
        /// <param name="y">The normalized y-coordinate of the touch.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        protected virtual void OnLongPress(Int64 touchID, Int64 fingerID, Single x, Single y, Single pressure)
        {
            LongPress?.Invoke(this, touchID, fingerID, x, y, pressure);
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

        /// <summary>
        /// Raises the <see cref="DollarGestureRecorded"/> event.
        /// </summary>
        /// <param name="gestureID">The unique identifier of the recorded gesture.</param>
        protected virtual void OnDollarGestureRecorded(Int64 gestureID)
        {
            DollarGestureRecorded?.Invoke(this, gestureID);
        }

        /// <summary>
        /// Raises the <see cref="DollarGesture"/> event.
        /// </summary>
        /// <param name="gestureID">The unique identifier of the gesture which was performed.</param>
        /// <param name="x">The normalized x-coordinate of the gesture's centroid.</param>
        /// <param name="y">The normalized y-coordinate of the gesture's centroid.</param>
        /// <param name="error">The difference between the gesture template and the actual performed gesture; lower is better.</param>
        /// <param name="fingers">The number of fingers used to perform the gesture.</param>
        protected virtual void OnDollarGesture(Int64 gestureID, Single x, Single y, Single error, Int32 fingers)
        {
            DollarGesture?.Invoke(this, gestureID, x, y, error, fingers);
        }

        /// <summary>
        /// Updates the position and pressure of a touch.
        /// </summary>
        /// <param name="touch">The touch to update.</param>
        /// <param name="x">The x-coordinate of the touch.</param>
        /// <param name="y">The y-coordinate of the touch.</param>
        /// <param name="dx">The change in the touch's x-coordinate.</param>
        /// <param name="dy">The change in the touch's y-coordinate.</param>
        /// <param name="pressure">The pressure of the touch.</param>
        protected void SetTouchPosition(ref TouchInfo touch, Single x, Single y, out Single dx, out Single dy, Single pressure)
        {
            dx = x - touch.CurrentX;
            dy = y - touch.CurrentY;

            touch.CurrentX = x;
            touch.CurrentY = y;
            touch.Pressure = pressure;

            var dpixels = DenormalizeCoordinates(dx, dy);
            var distance = (Single)Math.Sqrt((dpixels.X * dpixels.X) + (dpixels.Y * dpixels.Y));

            touch.Distance += distance;
        }

        /// <summary>
        /// Updates the value of the <see cref="TouchInfo.IsLongPress"/> property for a touch.
        /// </summary>
        /// <param name="touch">The touch to update.</param>
        /// <param name="value">The value to set.</param>
        protected void SetTouchIsLongPress(ref TouchInfo touch, Boolean value)
        {
            touch.IsLongPress = value;
        }
    }
}

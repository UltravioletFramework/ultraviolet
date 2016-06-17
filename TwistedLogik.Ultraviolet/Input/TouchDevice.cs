using System;

namespace TwistedLogik.Ultraviolet.Input
{
    /// <summary>
    /// Represents the method that is called when a finger is tapped against a touch device.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="fingerID">A value which identifies the finger which was tapped.</param>
    /// <param name="x">The x-coordinate at which the tap began.</param>
    /// <param name="y">The y-coordinate at which the tap began.</param>
    public delegate void TouchTapEventHandler(TouchDevice device, Int64 fingerID, Single x, Single y);

    /// <summary>
    /// Represents the method that is called when a finger is pressed against or removed from a touch device. 
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="fingerID">A value which identifies the finger which was pressed or removed.</param>
    /// <param name="x">The normalized x-coordinate at which the finger was pressed or removed.</param>
    /// <param name="y">The normalized y-coordinate at which the finger was pressed or removed.</param>
    /// <param name="pressure">The normalized pressure with which the touch was applied.</param>
    public delegate void TouchFingerEventHandler(TouchDevice device, Int64 fingerID, Single x, Single y, Single pressure);

    /// <summary>
    /// Represents the method that is called when a finger is moved across a touch device.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="fingerID">A value which identifies the finger which was moved.</param>
    /// <param name="x">The finger's normalized x-coordinate on the touch device.</param>
    /// <param name="y">The finger's normalized y-coordinate on the touch device.</param>
    /// <param name="dx">The change in the finger's x-coordinate.</param>
    /// <param name="dy">The change in the finger's y-coordinate.</param>
    /// <param name="pressure">The normalized pressure of the finger against the device.</param>
    public delegate void TouchFingerMotionEventHandler(TouchDevice device, Int64 fingerID, Single x, Single y, Single dx, Single dy, Single pressure);

    /// <summary>
    /// Represents the method that is called when a multitouch gesture is performed.
    /// </summary>
    /// <param name="device">The <see cref="TouchDevice"/> that raised the event.</param>
    /// <param name="x">The x-coordinate of the normalized center of the gesture.</param>
    /// <param name="y">The y-coordinate of the normalized center of the gesture.</param>
    /// <param name="theta">The amount that the fingers rotated during the gesture.</param>
    /// <param name="distance">The amount that the fingers pinched during the gesture.</param>
    /// <param name="numfingers">The number of fingers that were used in the gesture.</param>
    public delegate void MultiTouchEventHandler(TouchDevice device, Single x, Single y, Single theta, Single distance, Int32 numfingers);

    /// <summary>
    /// Represents touch input.
    /// </summary>
    public abstract class TouchDevice : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchDevice"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public TouchDevice(UltravioletContext uv)
            : base(uv)
        {
            MaximumTapDistance = 0.05f;
            MaximumTapDelay    = 500.0;
        }

        /// <summary>
        /// Updates the device's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        public abstract void Update(UltravioletTime time);

        /// <summary>
        /// Gets a value indicating whether the device was tapped anywhere on its surface
        /// during the previous frame.
        /// </summary>
        /// <returns><see langword="true"/> if the device was tapped; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean WasTapped();

        /// <summary>
        /// Gets a value indicating whether the device was tapped anywhere on its surface
        /// during the previous frame by the finger with the specified index.
        /// </summary>
        /// <param name="index">The index of the finger to evaluate.</param>
        /// <returns><see langword="true"/> if the device was tapped; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean WasTappedBy(Int32 index);

        /// <summary>
        /// Gets a value indicating whether the device was tapped anywhere in the specified
        /// area on its surface during the previous frame.
        /// </summary>
        /// <param name="area">A <see cref="RectangleF"/> structure containing the area to 
        /// evaluate, specified in normalized coordinates.</param>
        /// <returns><see langword="true"/> if the device was tapped; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean WasTapped(RectangleF area);

        /// <summary>
        /// Gets a value indicating whether the device was tapped anywhere in the specified
        /// area on its surface during the previous frame by the finger with the specified index.
        /// </summary>
        /// <param name="index">The index of the finger to evaluate.</param>
        /// <param name="area">A <see cref="RectangleF"/> structure containing the area to 
        /// evaluate, specified in normalized coordinates.</param>
        /// <returns><see langword="true"/> if the device was tapped; otherwise, <see langword="false"/>.</returns>
        public abstract Boolean WasTappedBy(Int32 index, RectangleF area);

        /// <summary>
        /// Gets the current values for the specified active touch event.
        /// </summary>
        /// <param name="index">The index of the touch to retrieve.</param>
        /// <param name="info">The touch information.</param>
        /// <returns><see langword="true"/> if there is an active touch with the specified index;
        /// otherwise, <see langword="false"/>.</returns>
        public abstract Boolean GetActiveTouch(Int32 index, out TouchInfo info);

        /// <summary>
        /// Gets the identifier of the finger which started the touch with the
        /// specified index.
        /// </summary>
        /// <param name="index">The index of the touch for which to retrieve a finger identifier.</param>
        /// <returns>The finger identifier of the touch, or <see langword="null"/> if there is no
        /// active touch with the specified index.</returns>
        public abstract Int64? GetFingerIDFromIndex(Int32 index);

        /// <summary>
        /// Gets the index of the touch started by the finger with the
        /// specified identifier.
        /// </summary>
        /// <param name="fingerID">The identifier of the finger for which to retrieve a touch index.</param>
        /// <returns>The index of the touch, or <see langword="null"/> if there is no
        /// active touch with the specified identifier.</returns>
        public abstract Int32? GetIndexFromFingerID(Int64 fingerID);

        /// <summary>
        /// Gets the maximum distance that a finger can move between its up and down events in order
        /// for the input to be considered a "tap." This value is in normalized units; i.e. a value
        /// of 0.05 means that the finger can move across 5% of the device and still be considered a tap.
        /// </summary>
        public Single MaximumTapDistance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the maximum delay in milliseconds between a finger's up and down events in order
        /// for the input to be considered a "tap."
        /// </summary>
        public Double MaximumTapDelay
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when a finger is tapped against the touch surface.
        /// </summary>
        public event TouchTapEventHandler Tap;

        /// <summary>
        /// Occurs when a finger is pressed against the touch surface.
        /// </summary>
        public event TouchFingerEventHandler FingerDown;

        /// <summary>
        /// Occurs when a finger is removed from the touch surface.
        /// </summary>
        public event TouchFingerEventHandler FingerUp;

        /// <summary>
        /// Occurs when a finger is moved across the touch surface.
        /// </summary>
        public event TouchFingerMotionEventHandler FingerMotion;

        /// <summary>
        /// Occurs when a multi-touch gesture is performed.
        /// </summary>
        public event MultiTouchEventHandler MultiTouchGesture;

        /// <summary>
        /// Raises the <see cref="Tap"/> event.
        /// </summary>
        /// <param name="fingerID">A value which identifies the finger which was tapped.</param>
        /// <param name="x">The x-coordinate at which the finger was tapped.</param>
        /// <param name="y">The y-coordinate at which the finger was tapped.</param>
        protected virtual void OnTap(Int64 fingerID, Single x, Single y) =>
            Tap?.Invoke(this, fingerID, x, y);

        /// <summary>
        /// Raises the <see cref="FingerDown"/> event.
        /// </summary>
        /// <param name="fingerID">A value which identifies the finger which was pressed or removed.</param>
        /// <param name="x">The normalized x-coordinate at which the finger was pressed or removed.</param>
        /// <param name="y">The normalized y-coordinate at which the finger was pressed or removed.</param>
        /// <param name="pressure">The normalized pressure with which the touch was applied.</param>
        protected virtual void OnFingerDown(Int64 fingerID, Single x, Single y, Single pressure) =>
            FingerDown?.Invoke(this, fingerID, x, y, pressure);

        /// <summary>
        /// Raises the <see cref="FingerUp"/> event.
        /// </summary>
        /// <param name="fingerID">A value which identifies the finger which was pressed or removed.</param>
        /// <param name="x">The normalized x-coordinate at which the finger was pressed or removed.</param>
        /// <param name="y">The normalized y-coordinate at which the finger was pressed or removed.</param>
        /// <param name="pressure">The normalized pressure with which the touch was applied.</param>
        protected virtual void OnFingerUp(Int64 fingerID, Single x, Single y, Single pressure) =>
            FingerUp?.Invoke(this, fingerID, x, y, pressure);

        /// <summary>
        /// Raises the <see cref="FingerMotion"/> event.
        /// </summary>
        /// <param name="fingerID">A value which identifies the finger which was moved.</param>
        /// <param name="x">The finger's normalized x-coordinate on the touch device.</param>
        /// <param name="y">The finger's normalized y-coordinate on the touch device.</param>
        /// <param name="dx">The change in the finger's x-coordinate.</param>
        /// <param name="dy">The change in the finger's y-coordinate.</param>
        /// <param name="pressure">The normalized pressure of the finger against the device.</param>
        protected virtual void OnFingerMotion(Int64 fingerID, Single x, Single y, Single dx, Single dy, Single pressure) =>
            FingerMotion?.Invoke(this, fingerID, x, y, dx, dy, pressure);

        /// <summary>
        /// Raises the <see cref="MultiTouchGesture"/> event.
        /// </summary>
        /// <param name="x">The x-coordinate of the normalized center of the gesture.</param>
        /// <param name="y">The y-coordinate of the normalized center of the gesture.</param>
        /// <param name="theta">The amount that the fingers rotated during the gesture.</param>
        /// <param name="distance">The amount that the fingers pinched during the gesture.</param>
        /// <param name="numfingers">The number of fingers that were used in the gesture.</param>
        protected virtual void OnMultiTouchGesture(Single x, Single y, Single theta, Single distance, Int32 numfingers) =>
            MultiTouchGesture?.Invoke(this, x, y, theta, distance, numfingers);
    }
}

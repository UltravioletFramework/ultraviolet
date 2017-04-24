using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the parameters of an active touch input.
    /// </summary>
    public struct TouchInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchInfo"/> structure.
        /// </summary>
        /// <param name="timestamp">The timestamp, in ticks, at which the touch began.</param>
        /// <param name="touchID">The unique identifier of the touch event.</param>
        /// <param name="touchIndex">The index of the touch within the current gesture.</param>
        /// <param name="fingerID">The unique identifier of the finger which caused the touch event.</param>
        /// <param name="originX">The normalized x-coordinate at which the touch originated.</param>
        /// <param name="originY">The normalized x-coordinate at which the touch originated.</param>
        /// <param name="currentX">The normalized x-coordinate of the touch.</param>
        /// <param name="currentY">The normalized y-coordinate of the touch.</param>
        /// <param name="pressure">The normalized pressure of the touch.</param>
        /// <param name="isLongPress">A value indicating whether the touch is a long press.</param>
        public TouchInfo(Int64 timestamp, Int64 touchID, Int32 touchIndex, Int64 fingerID,
            Single originX, Single originY, Single currentX, Single currentY, Single pressure, Boolean isLongPress)
        {
            this.timestamp = timestamp;
            this.touchID = touchID;
            this.touchIndex = touchIndex;
            this.fingerID = fingerID;
            this.originX = originX;
            this.originY = originY;
            this.currentX = currentX;
            this.currentY = currentY;
            this.pressure = pressure;
            this.distance = 0;
            this.isLongPress = isLongPress;
        }

        /// <summary>
        /// Gets the timestamp, in ticks, at which the touch began.
        /// </summary>
        public Int64 Timestamp
        {
            get { return timestamp; }
            internal set { timestamp = value; }
        }

        /// <summary>
        /// Gets the unique identifier of the touch event.
        /// </summary>
        public Int64 TouchID
        {
            get { return touchID; }
            internal set { touchID = value; }
        }

        /// <summary>
        /// Gets the index of the touch within the current gesture.
        /// </summary>
        public Int32 TouchIndex
        {
            get { return touchIndex; }
            internal set { touchIndex = value; }
        }

        /// <summary>
        /// Gets the internal identifier of the finger which caused the touch event.
        /// </summary>
        public Int64 FingerID
        {
            get { return fingerID; }
            internal set { fingerID = value; }
        }

        /// <summary>
        /// Gets the normalized coordinates of the position at which the touch originated.
        /// </summary>
        public Point2F OriginPosition => new Point2F(originX, originY);

        /// <summary>
        /// Gets the normalized x-coordinate at which the touch originated.
        /// </summary>
        public Single OriginX
        {
            get { return originX; }
            internal set { originX = value; }
        }

        /// <summary>
        /// Gets the normalized y-coordinate at which the touch originated.
        /// </summary>
        public Single OriginY
        {
            get { return originY; }
            internal set { originY = value; }
        }

        /// <summary>
        /// Gets the normalized coordinates of the touch's current position.
        /// </summary>
        public Point2F CurrentPosition => new Point2F(currentX, currentY);

        /// <summary>
        /// Gets the normalized x-coordinate of the touch.
        /// </summary>
        public Single CurrentX
        {
            get { return currentX; }
            internal set { currentX = value; }
        }

        /// <summary>
        /// Gets the normalized y-coordinate of the touch.
        /// </summary>
        public Single CurrentY
        {
            get { return currentY; }
            internal set { currentY = value; }
        }

        /// <summary>
        /// Gets the normalized pressure of the touch.
        /// </summary>
        public Single Pressure
        {
            get { return pressure; }
            internal set { pressure = value; }
        }

        /// <summary>
        /// Gets the total distance that the touch has moved.
        /// </summary>
        public Single Distance
        {
            get { return distance; }
            internal set { distance = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the touch is a long press.
        /// </summary>
        public Boolean IsLongPress
        {
            get { return isLongPress; }
            internal set { isLongPress = value; }
        }

        // Property values.
        private Int64 timestamp;
        private Int64 touchID;
        private Int32 touchIndex;
        private Int64 fingerID;
        private Single originX;
        private Single originY;
        private Single currentX;
        private Single currentY;
        private Single pressure;
        private Single distance;
        private Boolean isLongPress;
    }
}
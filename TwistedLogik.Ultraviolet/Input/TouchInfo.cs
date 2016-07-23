using System;

namespace TwistedLogik.Ultraviolet
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
        public TouchInfo(Int64 timestamp, Int64 touchID, Int32 touchIndex, Int64 fingerID,
            Single originX, Single originY, Single currentX, Single currentY, Single pressure)
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
        }

        /// <summary>
        /// Gets the timestamp, in ticks, at which the touch began.
        /// </summary>
        public Int64 Timestamp => timestamp;

        /// <summary>
        /// Gets the unique identifier of the touch event.
        /// </summary>
        public Int64 TouchID => touchID;

        /// <summary>
        /// Gets the index of the touch within the current gesture.
        /// </summary>
        public Int32 TouchIndex => touchIndex;

        /// <summary>
        /// Gets the internal identifier of the finger which caused the touch event.
        /// </summary>
        public Int64 FingerID => fingerID;

        /// <summary>
        /// Gets the normalized coordinates of the position at which the touch originated.
        /// </summary>
        public Point2F OriginPosition => new Point2F(originX, originY);

        /// <summary>
        /// Gets the normalized x-coordinate at which the touch originated.
        /// </summary>
        public Single OriginX => originX;

        /// <summary>
        /// Gets the normalized y-coordinate at which the touch originated.
        /// </summary>
        public Single OriginY => originY;

        /// <summary>
        /// Gets the normalized coordinates of the touch's current position.
        /// </summary>
        public Point2F CurrentPosition => new Point2F(currentX, currentY);

        /// <summary>
        /// Gets the normalized x-coordinate of the touch.
        /// </summary>
        public Single CurrentX => currentX;

        /// <summary>
        /// Gets the normalized y-coordinate of the touch.
        /// </summary>
        public Single CurrentY => currentY;

        /// <summary>
        /// Gets the normalized pressure of the touch.
        /// </summary>
        public Single Pressure => pressure;

        // Property values.
        private readonly Int64 timestamp;
        private readonly Int64 touchID;
        private readonly Int32 touchIndex;
        private readonly Int64 fingerID;
        private readonly Single originX;
        private readonly Single originY;
        private readonly Single currentX;
        private readonly Single currentY;
        private readonly Single pressure;
    }
}
using System;

namespace TwistedLogik.Ultraviolet.SDL2.Input
{
    /// <summary>
    /// Represents the metadata for a tap event.
    /// </summary>
    internal struct TouchTapData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchTapData"/> structure.
        /// </summary>
        /// <param name="fingerID">A value which identifies the finger being tapped.</param>
        /// <param name="x">The x-coordinate at which the tap began.</param>
        /// <param name="y">The y-coordinate at which the tap began.</param>
        /// <param name="timestamp">The timestamp, in milliseconds, at which the tap began.</param>
        public TouchTapData(Int64 fingerID, Single x, Single y, Double timestamp)
        {
            this.fingerID  = fingerID;
            this.x         = x;
            this.y         = y;
            this.timestamp = timestamp;
        }

        /// <summary>
        /// Gets the value which identifies the finger being tapped.
        /// </summary>
        public Int64 FingerID
        {
            get { return fingerID; }
        }

        /// <summary>
        /// Gets the x-coordinate at which the tap began.
        /// </summary>
        public Single X
        {
            get { return x; }
        }

        /// <summary>
        /// Gets the y-coordinate at which the tap began.
        /// </summary>
        public Single Y
        {
            get { return y; }
        }

        /// <summary>
        /// Gets the timestamp in milliseconds at which the tap began.
        /// </summary>
        public Double Timestamp
        {
            get { return timestamp; }
        }

        // Property values.
        private Int64 fingerID;
        private Single x;
        private Single y;
        private Double timestamp;
    }
}

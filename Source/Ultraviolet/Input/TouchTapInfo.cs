using System;

namespace Ultraviolet
{
    /// <summary>
    /// Represents the parameters of a tap event.
    /// </summary>
    public struct TouchTapInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchTapInfo"/> structure.
        /// </summary>
        /// <param name="touchID">The unique identifier of the touch which caused the tap.</param>
        /// <param name="fingerID">The unique identifier of the finger which caused the tap.</param>
        /// <param name="x">The normalized x-coordinate at which the tap originated.</param>
        /// <param name="y">The normalized x-coordinate at which the tap originated.</param>
        public TouchTapInfo(Int64 touchID, Int64 fingerID, Single x, Single y)
        {
            this.touchID = touchID;
            this.fingerID = fingerID;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets the unique identifier of the finger which caused the tap.
        /// </summary>
        public Int64 TouchID => touchID;

        /// <summary>
        /// Gets the unique identifier of the finger which caused the tap.
        /// </summary>
        public Int64 FingerID => fingerID;

        /// <summary>
        /// Gets the normalized x-coordinate at which the tap originated.
        /// </summary>
        public Single X => x;

        /// <summary>
        /// Gets the normalized y-coordinate at which the tap originated.
        /// </summary>
        public Single Y => y;

        // Property values.
        private readonly Int64 touchID;
        private readonly Int64 fingerID;
        private readonly Single x;
        private readonly Single y;
    }
}
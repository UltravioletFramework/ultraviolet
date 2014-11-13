using System;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// The <see cref="MutableVector3"/> structure is a mutable version of the <see cref="Vector3"/> structure used 
    /// primarily for performance micro-optimizations within the Ultraviolet Framework.
    /// </summary>
    public struct MutableVector3
    {
        /// <summary>
        /// The vector's x-coordinate.
        /// </summary>
        public Single X;

        /// <summary>
        /// The vector's y-coordinate.
        /// </summary>
        public Single Y;

        /// <summary>
        /// The vector's z-coordinate.
        /// </summary>
        public Single Z;
    }
}

using System;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// The <see cref="MutableVector2"/> structure is a mutable version of the <see cref="Vector2"/> structure used 
    /// primarily for performance micro-optimizations within the Ultraviolet Framework.
    /// </summary>
    public struct MutableVector2
    {
        /// <summary>
        /// The vector's x-coordinate.
        /// </summary>
        public Single X;

        /// <summary>
        /// The vector's y-coordinate.
        /// </summary>
        public Single Y;
    }
}

using System;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents source data from which a <see cref="Surface2D"/> object can be created.
    /// </summary>
    public abstract class SurfaceSource
    {
        /// <summary>
        /// Gets the color of the pixel at the specified position on the surface source.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel to retrieve.</param>
        /// <param name="y">The y-coordinate of the pixel to retrieve.</param>
        /// <returns>The color of the specified pixel.</returns>
        public abstract Color this[Int32 x, Int32 y]
        {
            get;
        }

        /// <summary>
        /// Gets the width of the surface source in pixels.
        /// </summary>
        public abstract Int32 Width
        {
            get;
        }

        /// <summary>
        /// Gets the height of the surface source.
        /// </summary>
        public abstract Int32 Height
        {
            get;
        }
    }
}

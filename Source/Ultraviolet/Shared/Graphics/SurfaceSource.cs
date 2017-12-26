using System;
using System.IO;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="SurfaceSource"/> class.
    /// </summary>
    /// <param name="stream">The stream from which to load the surface.</param>
    /// <returns>The instance of <see cref="SurfaceSource"/> that was created.</returns>
    public delegate SurfaceSource SurfaceSourceFactory(Stream stream);

    /// <summary>
    /// Represents source data from which a <see cref="Surface2D"/> object can be created.
    /// </summary>
    public abstract class SurfaceSource : IDisposable
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SurfaceSource"/> class.
        /// </summary>
        /// <param name="stream">Th <see cref="Stream"/> that contains the data to load.</param>
        /// <returns>The instance of <see cref="SurfaceSource"/> that was created.</returns>
        public static SurfaceSource Create(Stream stream)
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SurfaceSourceFactory>()(stream);
        }

        /// <inheritdoc/>
        public abstract void Dispose();

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
        /// Gets a pointer to the beginning of the surface data.
        /// </summary>
        public abstract IntPtr Data
        {
            get;
        }

        /// <summary>
        /// Gets the size, in bytes, of a single scanline of image data from this source.
        /// </summary>
        public abstract Int32 Stride
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

        /// <summary>
        /// Gets a <see cref="SurfaceSourceDataFormat"/> value which indicates the format
        /// in which this surface source's data is stored.
        /// </summary>
        public abstract SurfaceSourceDataFormat DataFormat
        {
            get;
        }
    }
}

using System;
using System.IO;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="PlatformNativeSurface"/> class.
    /// </summary>
    /// <param name="source">The surface source from which to create the surface.</param>
    /// <returns>The instance of <see cref="PlatformNativeSurface"/> that was created.</returns>
    public delegate PlatformNativeSurface PlatformNativeSurfaceFactory(SurfaceSource source);

    /// <summary>
    /// Represents native platform-specific surface data.
    /// </summary>
    public abstract class PlatformNativeSurface : IDisposable
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PlatformNativeSurface"/> class.
        /// </summary>
        /// <param name="source">The surface source from which to create the surface.</param>
        /// <returns>The instance of <see cref="PlatformNativeSurface"/> that was created.</returns>
        public static PlatformNativeSurface Create(SurfaceSource source)
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<PlatformNativeSurfaceFactory>()(source);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PlatformNativeSurface"/> class.
        /// </summary>
        /// <param name="stream">The stream from which to load the surface.</param>
        /// <returns>The instance of <see cref="PlatformNativeSurface"/> that was created.</returns>
        public static PlatformNativeSurface Create(Stream stream)
        {
            var uv = UltravioletContext.DemandCurrent();
            var factory = uv.GetFactoryMethod<PlatformNativeSurfaceFactory>();

            var data = new Byte[stream.Length];
            stream.Read(data, 0, data.Length);

            using (var mstream = new MemoryStream(data))
            using (var src = SurfaceSource.Create(mstream))
            {
                return factory(src);
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Flips the surface in the specified direction.
        /// </summary>
        /// <param name="direction">A <see cref="SurfaceFlipDirection"/> value which specifies the 
        /// direction in which to flip the surface.</param>
        public abstract void Flip(SurfaceFlipDirection direction);

        /// <summary>
        /// Flips the surface in the specified direction and processes its alpha, optionally premultiplying it
        /// and performing color keying. If the surface is already premultiplied, it will not be premultiplied again.
        /// </summary>
        /// <param name="direction">A <see cref="SurfaceFlipDirection"/> value which specifies the 
        /// direction in which to flip the surface.</param>
        /// <param name="premultiply">A value indicating whether to premultiply the surface's alpha.</param>
        /// <param name="keycolor">A key color to substitute with a transparent color.</param>
        public abstract void FlipAndProcessAlpha(SurfaceFlipDirection direction, Boolean premultiply, Color? keycolor = null);

        /// <summary>
        /// Processes the surface's alpha, optionally premultiplying it and performing color keying.
        /// If the surface is already premultiplied, it will not be premultiplied again.
        /// </summary>
        /// <param name="premultiply">A value indicating whether to premultiply the surface's alpha.</param>
        /// <param name="keycolor">A key color to substitute with a transparent color.</param>
        public abstract void ProcessAlpha(Boolean premultiply, Color? keycolor = null);

        /// <summary>
        /// Gets the surface's data.
        /// </summary>
        /// <param name="data">An array to populate with the surface's data.</param>
        /// <param name="region">The region of the surface from which to retrieve data.</param>
        public abstract void GetData(Color[] data, Rectangle region);

        /// <summary>
        /// Sets the surface's data in the specified region of the surface.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="region">The region of the surface to populate with data.</param>
        public abstract void SetData(Color[] data, Rectangle region);

        /// <summary>
        /// Sets the surface's data from the data at the specified pointer.
        /// </summary>
        /// <param name="data">A pointer to the data to set.</param>
        /// <param name="srcOffsetInBytes">The offset from the beginning of the source data, in bytes, at which to begin copying.</param>
        /// <param name="dstOffsetInBytes">The offset from the beginning of the surface, in bytes, at which to begin copying.</param>
        /// <param name="sizeInBytes">The number of bytes to copy.</param>
        public abstract void SetRawData(IntPtr data, Int32 srcOffsetInBytes, Int32 dstOffsetInBytes, Int32 sizeInBytes);

        /// <summary>
        /// Creates a copy of the surface.
        /// </summary>
        /// <returns>A new surface that is a copy of this surface.</returns>
        public abstract PlatformNativeSurface CreateCopy();

        /// <summary>
        /// Gets a value indicating whether the surface has been flipped horizontally.
        /// </summary>
        public abstract Boolean IsFlippedHorizontally { get; }

        /// <summary>
        /// Gets a value indicating whether the surface has been flipped vertically.
        /// </summary>
        public abstract Boolean IsFlippedVertically { get; }

        /// <summary>
        /// Gets a value indicating whether the surface's alpha has been premultiplied.
        /// </summary>
        public abstract Boolean IsAlphaPremultiplied { get; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the surface's color is sRGB encoded.
        /// </summary>
        public abstract Boolean SrgbEncoded { get; set; }

        /// <summary>
        /// Gets the number of bytes per pixel on this surface.
        /// </summary>
        public abstract Int32 BytesPerPixel { get; }

        /// <summary>
        /// Gets the surface's width in pixels.
        /// </summary>
        public abstract Int32 Width { get; }

        /// <summary>
        /// Gets the surface's height in pixels.
        /// </summary>
        public abstract Int32 Height { get; }

        /// <summary>
        /// Gets the surface's pitch.
        /// </summary>
        public abstract Int32 Pitch { get; }

        /// <summary>
        /// Gets a pointer to the native structure which represents this surface.
        /// </summary>
        public abstract IntPtr Native { get; }

        /// <summary>
        /// Gets a value indicating whether the object has been disposed.
        /// </summary>
        public Boolean Disposed { get; private set; }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected virtual void Dispose(Boolean disposing) => Disposed = true;
    }
}

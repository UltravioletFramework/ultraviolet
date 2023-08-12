using System;
using System.IO;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Surface2D"/> class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The surface's width in pixels.</param>
    /// <param name="height">The surface's height in pixels.</param>
    /// <param name="options">The surface's configuration options.</param>
    /// <returns>The instance of <see cref="Surface2D"/> that was created.</returns>
    public delegate Surface2D Surface2DFactory(UltravioletContext uv, Int32 width, Int32 height, SurfaceOptions options);

    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Surface2D"/> class from an instance of <see cref="SurfaceSource"/>.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="source">The surface source from which to create the surface.</param>
    /// <param name="options">The surface's configuration options.</param>
    /// <returns>The instance of <see cref="Surface2D"/> that was created.</returns>
    public delegate Surface2D Surface2DFromSourceFactory(UltravioletContext uv, SurfaceSource source, SurfaceOptions options);

    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="Surface2D"/> class from an instance of <see cref="PlatformNativeSurface"/>.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="surface">The native surface from which to create the surface.</param>
    /// <param name="options">The surface's configuration options.</param>
    /// <returns>The instance of <see cref="Surface2D"/> that was created.</returns>
    public delegate Surface2D Surface2DFromNativeSurfaceFactory(UltravioletContext uv, PlatformNativeSurface surface, SurfaceOptions options);

    /// <summary>
    /// Represents a two-dimensional image which is held in CPU memory.
    /// </summary>
    /// <remarks>A <see cref="Surface2D"/> operates similarly to a <see cref="Texture2D"/>, except that it is held in CPU memory rather
    /// than GPU memory. This makes it useful for manipulating image data prior to turning it into a texture.</remarks>
    public abstract class Surface2D : Surface
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Surface2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Surface2D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="Surface2D"/> class.
        /// </summary>
        /// <param name="width">The surface's width in pixels.</param>
        /// <param name="height">The surface's height in pixels.</param>
        /// <param name="options">The surface's configuration options.</param>
        /// <returns>The instance of <see cref="Surface2D"/> that was created.</returns>
        public static Surface2D Create(Int32 width, Int32 height, SurfaceOptions options = SurfaceOptions.Default)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Surface2DFactory>()(uv, width, height, options);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Surface2D"/> class.
        /// </summary>
        /// <param name="source">The <see cref="SurfaceSource"/> from which to create the surface.</param>
        /// <param name="options">The surface's configuration options.</param>
        /// <returns>The instance of <see cref="Surface2D"/> that was created.</returns>
        public static Surface2D Create(SurfaceSource source, SurfaceOptions options = SurfaceOptions.Default)
        {
            Contract.Require(source, nameof(source));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Surface2DFromSourceFactory>()(uv, source, options);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Surface2D"/> class.
        /// </summary>
        /// <param name="surface">The <see cref="PlatformNativeSurface"/> from which to create the surface.</param>
        /// <param name="options">The surface's configuration options.</param>
        /// <returns>The instance of <see cref="Surface2D"/> that was created.</returns>
        public static Surface2D Create(PlatformNativeSurface surface, SurfaceOptions options = SurfaceOptions.Default)
        {
            Contract.Require(surface, nameof(surface));

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Surface2DFromNativeSurfaceFactory>()(uv, surface, options);
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
        /// Clears the surface to the specified color.
        /// </summary>
        /// <param name="color">The color to which the surface will be cleared.</param>
        public abstract void Clear(Color color);

        /// <summary>
        /// Gets the surface's data.
        /// </summary>
        /// <param name="data">An array to populate with the surface's data.</param>
        public abstract void GetData(Color[] data);

        /// <summary>
        /// Gets the surface's data.
        /// </summary>
        /// <param name="data">An array to populate with the surface's data.</param>
        /// <param name="region">The region of the surface from which to retrieve data.</param>
        public abstract void GetData(Color[] data, Rectangle region);

        /// <summary>
        /// Sets the surface's data.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        public abstract void SetData(Color[] data);

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
        /// Blits the surface onto the specified destination surface.
        /// </summary>
        /// <param name="dst">The destination surface.</param>
        public abstract void Blit(Surface2D dst);

        /// <summary>
        /// Blits the surface onto the specified destination surface.
        /// </summary>
        /// <param name="dst">The destination surface.</param>
        /// <param name="dstRect">The area on the destination surface to which this surface will be copied.</param>
        public abstract void Blit(Surface2D dst, Rectangle dstRect);

        /// <summary>
        /// Blits the surface onto the specified destination surface.
        /// </summary>
        /// <param name="srcRect">The area of this surface that will be copied to the destination surface.</param>
        /// <param name="dst">The destination surface.</param>
        /// <param name="dstRect">The area on the destination surface to which this surface will be copied.</param>
        public abstract void Blit(Rectangle srcRect, Surface2D dst, Rectangle dstRect);

        /// <summary>
        /// Blits the surface onto the specified destination surface at the given location relative
        /// to the destination's top-left corner, optionally flipping this surface in the process.
        /// </summary>
        /// <param name="dst">The destination surface.</param>
        /// <param name="position">The position at which to blit the surface.</param>
        public abstract void Blit(Surface2D dst, Point2 position);

        /// <summary>
        /// Blits the surface onto the specified destination surface at the given location relative
        /// to the destination's top-left corner, optionally flipping this surface in the process.
        /// </summary>
        /// <param name="dst">The destination surface.</param>
        /// <param name="position">The position at which to blit the surface.</param>
        /// <param name="direction">The direction in which to flip the surface.</param>
        public abstract void Blit(Surface2D dst, Point2 position, SurfaceFlipDirection direction);

        /// <summary>
        /// Creates a copy of the surface.
        /// </summary>
        /// <returns>A new <see cref="Surface2D"/> which is a copy of this surface.</returns>
        public abstract Surface2D CreateSurface();

        /// <summary>
        /// Creates a copy of a region of this surface.
        /// </summary>
        /// <param name="region">The region of this surface to copy.</param>
        /// <returns>A new <see cref="Surface2D"/> which is a copy of the specified region of this surface.</returns>
        public abstract Surface2D CreateSurface(Rectangle region);

        /// <summary>
        /// Saves the surface as a JPEG image to the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which to save the image data.</param>
        public abstract void SaveAsJpeg(Stream stream);

        /// <summary>
        /// Saves the surface as a PNG image to the specified stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to which to save the image data.</param>
        public abstract void SaveAsPng(Stream stream);

        /// <summary>
        /// Gets or sets a value indicating whether the surface's data is SRGB encoded. The default value of this property
        /// is determined by the value of the <see cref="UltravioletContextProperties.SrgbDefaultForSurface2D"/> property.
        /// </summary>
        public abstract Boolean SrgbEncoded { get; set; }

        /// <summary>
        /// Gets the surface's width in pixels.
        /// </summary>
        public abstract Int32 Width
        {
            get;
        }

        /// <summary>
        /// Gets the surface's height in pixels.
        /// </summary>
        public abstract Int32 Height
        {
            get;
        }

        /// <summary>
        /// Gets the length of a surface scanline in bytes.
        /// </summary>
        public abstract Int32 Pitch
        {
            get;
        }

        /// <summary>
        /// Gets the number of bytes used to represent a single pixel on this surface.
        /// </summary>
        public abstract Int32 BytesPerPixel
        {
            get;
        }
        
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
        /// Gets a pointer to the surface's pixel data.
        /// </summary>
        public abstract IntPtr Pixels { get; }
    }
}

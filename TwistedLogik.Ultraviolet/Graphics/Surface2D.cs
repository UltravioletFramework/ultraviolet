using System;
using System.Drawing;
using System.IO;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a factory method which constructs instances of the Surface2D class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="width">The surface's width in pixels.</param>
    /// <param name="height">The surface's height in pixels.</param>
    /// <returns>The instance of Surface2D that was created.</returns>
    public delegate Surface2D Surface2DFactory(UltravioletContext uv, Int32 width, Int32 height);

    /// <summary>
    /// Represents a factory method which constructs instances of the Surface2D class from an instance of <see cref="System.Drawing.Bitmap"/>.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <param name="bitmap">The bitmap from which to create the surface.</param>
    /// <returns>The instance of Surface2D that was created.</returns>
    public delegate Surface2D Surface2DFromBitmapFactory(UltravioletContext uv, Bitmap bitmap);

    /// <summary>
    /// Represents a two-dimensional image which is held in CPU memory.
    /// </summary>
    /// <remarks>A Surface2D operates similarly to a Texture2D, except that it is held in CPU memory rather
    /// than GPU memory.  This makes it useful for manipulating image data prior to turning it into a texture.</remarks>
    public abstract class Surface2D : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the Surface2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public Surface2D(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the Surface2D class.
        /// </summary>
        /// <param name="width">The surface's width in pixels.</param>
        /// <param name="height">The surface's height in pixels.</param>
        /// <returns>The instance of Surface2D that was created.</returns>
        public static Surface2D Create(Int32 width, Int32 height)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Surface2DFactory>()(uv, width, height);
        }

        /// <summary>
        /// Creates a new instance of the Surface2D class.
        /// </summary>
        /// <param name="bitmap">The bitmap from which to create the surface.</param>
        /// <returns>The instance of Surface2D that was created.</returns>
        public static Surface2D Create(Bitmap bitmap)
        {
            Contract.Require(bitmap, "bitmap");

            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<Surface2DFromBitmapFactory>()(uv, bitmap);
        }

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
        /// Creates a copy of the surface.
        /// </summary>
        /// <returns>A new surface which is a copy of this surface.</returns>
        public abstract Surface2D CreateSurface();

        /// <summary>
        /// Creates a copy of a region of this surface.
        /// </summary>
        /// <param name="region">The region of this surface to copy.</param>
        /// <returns>A new surface which is a copy of the specified region of this surface.</returns>
        public abstract Surface2D CreateSurface(Rectangle region);

        /// <summary>
        /// Creates a texture from the surface.
        /// </summary>
        /// <param name="premultiplyAlpha">A value indicating whether to premultiply the surface's alpha when creating the texture.</param>
        /// <returns>The texture that was created from the surface.</returns>
        public abstract Texture2D CreateTexture(Boolean premultiplyAlpha = true);

        /// <summary>
        /// Saves the surface as a JPEG image to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which to save the image data.</param>
        public abstract void SaveAsJpeg(Stream stream);

        /// <summary>
        /// Saves the surface as a PNG image to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which to save the image data.</param>
        public abstract void SaveAsPng(Stream stream);

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
    }
}

using System;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    /// <summary>
    /// Represents an SDL surface.
    /// </summary>
    public unsafe sealed partial class SDL_Surface : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the SDL_Surface class.
        /// </summary>
        /// <param name="width">The surface's width.</param>
        /// <param name="height">The surface's height.</param>
        public SDL_Surface(Int32 width, Int32 height)
        {
            if ((this.ptr = SDL.CreateRGBSurface(0, width, height, 32, rmask, gmask, bmask, amask)) == null)
                throw new SDL2Exception();
        }

        /// <summary>
        /// Initializes a new instance of the SDL_Surface class.
        /// </summary>
        /// <param name="src">A pointer to the native SDL surface.</param>
        public SDL_Surface(SDL_Surface_Native* src)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            var dst = SDL.CreateRGBSurface(0, src->w, src->h, 32, rmask, gmask, bmask, amask);
            if (dst == null)
                throw new SDL2Exception();

            if (SDL.BlitSurface(src, null, dst, null) < 0)
                throw new SDL2Exception();

            this.ptr = dst;
        }

        /// <summary>
        /// Creates a new instance of SDL_Surface from the image data contained in the specified stream.
        /// </summary>
        /// <param name="stream">The stream that contains the image data from which to create the surface.</param>
        /// <returns>The instance of SDL_Surface that was created.</returns>
        public static SDL_Surface CreateFromStream(Stream stream)
        {
            Contract.Require(stream, "stream");

            var data = new Byte[stream.Length];
            stream.Read(data, 0, data.Length);

            using (var mstream = new MemoryStream(data))
            {
                using (var src = SurfaceSource.Create(mstream))
                {
                    return CreateFromSurfaceSource(src);
                }
            }
        }

        /// <summary>
        /// Creates a new instance of SDL_Surface from the image data contained in the specified bitmap.
        /// </summary>
        /// <param name="source">The surface source that contains the image data from which to create the surface.</param>
        /// <returns>The instance of SDL_Surface that was created.</returns>
        public static SDL_Surface CreateFromSurfaceSource(SurfaceSource source)
        {
            Contract.Require(source, "source");

            var width  = source.Width;
            var height = source.Height;

            var bmpSurface = new SDL_Surface(width, height);
            for (int y = 0; y < height; y++)
            {
                var pDst = (uint*)((byte*)bmpSurface.Native->pixels + (bmpSurface.Native->pitch * y));
                for (int x = 0; x < width; x++)
                {
                    *pDst++ = source[x, y].ToArgb();
                }
            }

            return bmpSurface;
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
        /// Prepares the SDL surface to be exported as OpenGL texture data by
        /// flipping it upside down and premultiplying its alpha values.
        /// </summary>
        /// <param name="premultiplyAlpha">A value indicating whether to premultiply the surface's alpha.</param>
        public void PrepareForTextureExport(Boolean premultiplyAlpha)
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.EnsureNot(readyForTextureExport, SDL2Strings.SurfaceAlreadyPreparedForExport);

            var rowsToProcess = (ptr->h % 2 == 0) ? ptr->h / 2 : 1 + ptr->h / 2;
            for (var y = 0; y < rowsToProcess; y++)
            {
                var y1 = (y);
                var y2 = (ptr->h - 1) - y;

                var pSrc = (UInt32*)((Byte*)ptr->pixels + (y1 * Pitch));
                var pDst = (UInt32*)((Byte*)ptr->pixels + (y2 * Pitch));

                for (var x = 0; x < ptr->w; x++)
                {
                    var colorSrc = Color.FromRgba(*pSrc);
                    var colorDst = Color.FromRgba(*pDst);

                    if (colorSrc.Equals(Color.Magenta))
                    {
                        colorSrc = Color.Transparent;
                    }
                    if (colorDst.Equals(Color.Magenta))
                    {
                        colorDst = Color.Transparent;
                    }

                    if (premultiplyAlpha)
                    {
                        colorSrc = PremultiplyColor(colorSrc);
                        colorDst = PremultiplyColor(colorDst);
                    }

                    *pSrc++ = colorDst.PackedValue;
                    *pDst++ = colorSrc.PackedValue;
                }
            }

            readyForTextureExport = true;
        }

        /// <summary>
        /// Gets the surface's data.
        /// </summary>
        /// <param name="data">An array to populate with the surface's data.</param>
        /// <param name="region">The region of the surface from which to retrieve data.</param>
        public void GetData(Color[] data, Rectangle region)
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.Require(data, "data");

            var top = region.Top;
            var left = region.Left;
            var bottom = region.Bottom;
            var right = region.Right;

            if (top < 0 || left < 0 || right > Width || bottom > Height || region.Width <= 0 || region.Height <= 0)
                throw new ArgumentOutOfRangeException("region");

            if (data.Length < region.Width * region.Height)
                throw new ArgumentException(SDL2Strings.BufferIsTooSmall.Format("data"));

            fixed (Color* pDst1 = data)
            {
                var pDst = (UInt32*)pDst1;
                for (int sy = top; sy < bottom; sy++)
                {
                    var pSrc = (UInt32*)((Byte*)ptr->pixels + (sy * Pitch) + (left * BytesPerPixel));
                    for (int sx = left; sx < right; sx++)
                    {
                        *pDst++ = *pSrc++;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the surface's data in the specified region of the surface.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="region">The region of the surface to populate with data.</param>
        public void SetData(Color[] data, Rectangle region)
        {
            Contract.EnsureNotDisposed(this, disposed);
            Contract.Require(data, "data");

            var top = region.Top;
            var left = region.Left;
            var bottom = region.Bottom;
            var right = region.Right;

            if (top < 0 || left < 0 || right > Width || bottom > Height || region.Width <= 0 || region.Height <= 0)
                throw new ArgumentOutOfRangeException("region");

            if (data.Length < region.Width * region.Height)
                throw new ArgumentException(SDL2Strings.BufferIsTooSmall.Format("data"));

            fixed (Color* pSrc1 = data)
            {
                var pSrc = (UInt32*)pSrc1;
                for (int sy = top; sy < bottom; sy++)
                {
                    var pDst = (UInt32*)((Byte*)ptr->pixels + (sy * Pitch) + (left * BytesPerPixel));
                    for (int sx = left; sx < right; sx++)
                    {
                        *pDst++ = *pSrc++;
                    }
                }
            }
        }

        /// <summary>
        /// Creates a copy of the surface.
        /// </summary>
        /// <returns>A new surface that is a copy of this surface.</returns>
        public SDL_Surface CreateCopy()
        {
            Contract.EnsureNotDisposed(this, disposed);

            var copy = new SDL_Surface(Width, Height);

            if (SDL.BlitSurface(ptr, null, copy.ptr, null) < 0)
                throw new SDL2Exception();

            return copy;
        }

        /// <summary>
        /// Gets a value indicating whether the surface has been prepared to be exported as an OpenGL texture.
        /// </summary>
        public Boolean ReadyForTextureExport
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return readyForTextureExport;
            }
        }

        /// <summary>
        /// Gets the number of bytes per pixel on this surface.
        /// </summary>
        public Int32 BytesPerPixel
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return ptr->format->BytesPerPixel;
            }
        }

        /// <summary>
        /// Gets the surface's width in pixels.
        /// </summary>
        public Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return ptr->w;
            }
        }

        /// <summary>
        /// Gets the surface's height in pixels.
        /// </summary>
        public Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return ptr->h;
            }
        }

        /// <summary>
        /// Gets the surface's pitch.
        /// </summary>
        public Int32 Pitch
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return ptr->pitch;
            }
        }

        /// <summary>
        /// Gets the native surface pointer.
        /// </summary>
        public SDL_Surface_Native* Native
        {
            get { return ptr; }
        }

        /// <summary>
        /// Premultiplies the specified color's alpha.
        /// </summary>
        /// <param name="color">The color to premultiply.</param>
        /// <returns>The premultiplied color.</returns>
        private static Color PremultiplyColor(Color color)
        {
            var factor = color.A / 255f;

            return new Color(
                (byte)(color.R * factor),
                (byte)(color.G * factor),
                (byte)(color.B * factor), color.A);
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        private void Dispose(Boolean disposing)
        {
            if (disposed)
                return;

            SDL.FreeSurface(ptr);

            disposed = true;
        }

        // The mask values for each color channel.
        private static readonly UInt32 rmask = 0x000000ffu;
        private static readonly UInt32 gmask = 0x0000ff00u;
        private static readonly UInt32 bmask = 0x00ff0000u;
        private static readonly UInt32 amask = 0xff000000u;

        // State values.
        private SDL_Surface_Native* ptr;
        private Boolean disposed;
        private Boolean readyForTextureExport;
    }
}

using System;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.SDL2.Native
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
            Contract.Require(stream, nameof(stream));

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
            Contract.Require(source, nameof(source));

            var width  = source.Width;
            var height = source.Height;

            var bmpSurface = new SDL_Surface(width, height);

            var pDstData = (byte*)bmpSurface.Native->pixels;
            var pSrcData = (byte*)source.Data;

            var dstExtraBytes = bmpSurface.Native->pitch - (bmpSurface.Native->w * 4);
            var srcExtraBytes = source.Stride - (source.Width * 4);

            byte srcR, srcG, srcB, srcA;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    srcR = *pSrcData++;
                    srcG = *pSrcData++;
                    srcB = *pSrcData++;
                    srcA = *pSrcData++;

                    if (source.DataFormat == SurfaceSourceDataFormat.BGRA)
                    {
                        var temp = srcR;
                        srcR = srcB;
                        srcB = temp;
                    }

                    *pDstData++ = srcB;
                    *pDstData++ = srcG;
                    *pDstData++ = srcR;
                    *pDstData++ = srcA;
                }

                pDstData += dstExtraBytes;
                pSrcData += srcExtraBytes;
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

            var pitch = Pitch;
            var magenta = Color.Magenta.PackedValue;
            var transparent = Color.Transparent.PackedValue;

            uint srcValue;
            uint dstValue;

            byte srcR, srcG, srcB, srcA;
            byte dstR, dstG, dstB, dstA;

            var rowsToProcess = (ptr->h % 2 == 0) ? ptr->h / 2 : 1 + ptr->h / 2;
            for (var y = 0; y < rowsToProcess; y++)
            {
                var y1 = (y);
                var y2 = (ptr->h - 1) - y;

                var pSrc = (UInt32*)((Byte*)ptr->pixels + (y1 * pitch));
                var pDst = (UInt32*)((Byte*)ptr->pixels + (y2 * pitch));

                for (var x = 0; x < ptr->w; x++)
                {
                    srcValue = *pSrc;
                    dstValue = *pDst;

                    if (srcValue == magenta)
                    {
                        *pDst = transparent;
                    }
                    else
                    {
                        srcA = (byte)(srcValue >> 24);
                        srcB = (byte)(srcValue >> 16);
                        srcG = (byte)(srcValue >> 8);
                        srcR = (byte)(srcValue);

                        if (premultiplyAlpha)
                        {
                            var factor = srcA / 255f;
                            srcR = (byte)(srcR * factor);
                            srcG = (byte)(srcG * factor);
                            srcB = (byte)(srcB * factor);
                        }

                        *pDst = (uint)((srcR) | (srcG << 8) | (srcB << 16) | (srcA << 24));
                    }

                    if (dstValue == magenta)
                    {
                        *pSrc = transparent;
                    }
                    else
                    {
                        dstA = (byte)(dstValue >> 24);
                        dstB = (byte)(dstValue >> 16);
                        dstG = (byte)(dstValue >> 8);
                        dstR = (byte)(dstValue);

                        if (premultiplyAlpha)
                        {
                            var factor = dstA / 255f;
                            dstR = (byte)(dstR * factor);
                            dstG = (byte)(dstG * factor);
                            dstB = (byte)(dstB * factor);
                        }

                        *pSrc = (uint)((dstR) | (dstG << 8) | (dstB << 16) | (dstA << 24));
                    }
                    
                    pDst++;
                    pSrc++;
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
            Contract.Require(data, nameof(data));

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
            Contract.Require(data, nameof(data));

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

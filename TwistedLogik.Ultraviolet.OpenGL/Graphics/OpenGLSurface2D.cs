using System;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using TwistedLogik.Ultraviolet.SDL2;
using TwistedLogik.Ultraviolet.SDL2.Native;

namespace TwistedLogik.Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL/SDL2 implementation of the Surface2D class.
    /// </summary>
    public unsafe sealed class OpenGLSurface2D : Surface2D
    {
        /// <summary>
        /// Initializes a new instance of the OpenGLSurface2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="source">The surface source from which to create the surface.</param>
        public OpenGLSurface2D(UltravioletContext uv, SurfaceSource source)
            : this(uv, SDL_Surface.CreateFromSurfaceSource(source))
        {

        }

        /// <summary>
        /// Initializes a new instance of the OpenGLSurface2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="nativesurf">The native SDL surface that this object represents.</param>
        public OpenGLSurface2D(UltravioletContext uv, SDL_Surface nativesurf)
            : base(uv)
        {
            if (nativesurf == null)
                throw new ArgumentNullException("nativesurf");

            this.nativesurf = nativesurf;
        }

        /// <summary>
        /// Initializes a new instance of the OpenGLSurface2D class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The width of the surface in pixels.</param>
        /// <param name="height">The height of the surface in pixels.</param>
        public OpenGLSurface2D(UltravioletContext uv, Int32 width, Int32 height)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, "width");
            Contract.EnsureRange(height > 0, "height");

            this.nativesurf = new SDL_Surface(width, height);
        }

        /// <summary>
        /// Modifies the surface's pixel data so that it can be written to disk as preprocessed texture data.
        /// </summary>
        /// <param name="premultiplyAlpha">A value indicating whether to premultiply the surface's alpha.</param>
        public void PrepareForTextureExport(Boolean premultiplyAlpha)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            nativesurf.PrepareForTextureExport(premultiplyAlpha);
        }

        /// <summary>
        /// Gets the surface's data.
        /// </summary>
        /// <param name="data">An array to populate with the surface's data.</param>
        public override void GetData(Color[] data)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, "data");

            nativesurf.GetData(data, new Rectangle(0, 0, Width, Height));
        }

        /// <summary>
        /// Gets the surface's data.
        /// </summary>
        /// <param name="data">An array to populate with the surface's data.</param>
        /// <param name="region">The region of the surface from which to retrieve data.</param>
        public override void GetData(Color[] data, Rectangle region)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, "data");

            nativesurf.GetData(data, region);
        }

        /// <summary>
        /// Sets the surface's data.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        public override void SetData(Color[] data)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, "data");

            nativesurf.SetData(data, new Rectangle(0, 0, Width, Height));
        }

        /// <summary>
        /// Sets the surface's data in the specified region of the surface.
        /// </summary>
        /// <param name="data">An array containing the data to set.</param>
        /// <param name="region">The region of the surface to populate with data.</param>
        public override void SetData(Color[] data, Rectangle region)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, "data");

            nativesurf.SetData(data, region);
        }
        
        /// <summary>
        /// Blits the surface onto the specified destination surface.
        /// </summary>
        /// <param name="dst">The destination surface.</param>
        public override void Blit(Surface2D dst)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dst, "dst");

            Ultraviolet.ValidateResource(dst);

            BlitInternal(this, new Rectangle(0, 0, Width, Height), (OpenGLSurface2D)dst, new Rectangle(0, 0, dst.Width, dst.Height));
        }

        /// <summary>
        /// Blits the surface onto the specified destination surface.
        /// </summary>
        /// <param name="dst">The destination surface.</param>
        /// <param name="dstRect">The area on the destination surface to which this surface will be copied.</param>
        public override void Blit(Surface2D dst, Rectangle dstRect)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dst, "dst");

            Ultraviolet.ValidateResource(dst);

            BlitInternal(this, new Rectangle(0, 0, Width, Height), (OpenGLSurface2D)dst, dstRect);
        }

        /// <summary>
        /// Blits the surface onto the specified destination surface.
        /// </summary>
        /// <param name="srcRect">The area of this surface that will be copied to the destination surface.</param>
        /// <param name="dst">The destination surface.</param>
        /// <param name="dstRect">The area on the destination surface to which this surface will be copied.</param>
        public override void Blit(Rectangle srcRect, Surface2D dst, Rectangle dstRect)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dst, "dst");

            Ultraviolet.ValidateResource(dst);

            BlitInternal(this, srcRect, (OpenGLSurface2D)dst, dstRect);
        }

        /// <summary>
        /// Creates a copy of the surface.
        /// </summary>
        /// <returns>A new surface which is a copy of this surface.</returns>
        public override Surface2D CreateSurface()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var copysurf = nativesurf.CreateCopy();
            return new OpenGLSurface2D(Ultraviolet, copysurf);
        }

        /// <summary>
        /// Creates a copy of a region of this surface.
        /// </summary>
        /// <param name="region">The region of this surface to copy.</param>
        /// <returns>A new surface which is a copy of the specified region of this surface.</returns>
        public override Surface2D CreateSurface(Rectangle region)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (region.Left < 0 || region.Top < 0 || region.Right > Width || region.Bottom > Height || region.Width <= 0 || region.Height <= 0)
                throw new ArgumentOutOfRangeException("region");

            var copysurf = new SDL_Surface(region.Width, region.Height);

            var srcrect = new SDL_Rect() { x = region.X, y = region.Y, w = region.Width, h = region.Height };
            var dstrect = new SDL_Rect() { x = 0, y = 0, w = region.Width, h = region.Height };

            if (SDL.BlitSurface(nativesurf.Native, &srcrect, copysurf.Native, &dstrect) < 0)
                throw new SDL2Exception();
            
            return new OpenGLSurface2D(Ultraviolet, copysurf);
        }

        /// <summary>
        /// Creates a texture from the surface.
        /// </summary>
        /// <param name="premultiplyAlpha">A value indicating whether to premultiply the surface's alpha when creating the texture.</param>
        /// <returns>The texture that was created from the surface.</returns>
        public override Texture2D CreateTexture(Boolean premultiplyAlpha)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            using (var copysurf = new SDL_Surface(Width, Height))
            {
                if (SDL.SetSurfaceBlendMode(nativesurf.Native, SDL_BlendMode.NONE) < 0)
                    throw new SDL2Exception();

                if (SDL.BlitSurface(nativesurf.Native, null, copysurf.Native, null) < 0)
                    throw new SDL2Exception();

                copysurf.PrepareForTextureExport(premultiplyAlpha);
                return new OpenGLTexture2D(Ultraviolet, copysurf);
            }
        }

        /// <summary>
        /// Saves the surface as a JPEG image to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which to save the image data.</param>
        public override void SaveAsJpeg(Stream stream)
        {
            Contract.Require(stream, "stream");

            var saver = SurfaceSaver.Create();
            saver.SaveAsJpeg(this, stream);
        }

        /// <summary>
        /// Saves the surface as a PNG image to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to which to save the image data.</param>
        public override void SaveAsPng(Stream stream)
        {
            Contract.Require(stream, "stream");

            var saver = SurfaceSaver.Create();
            saver.SaveAsPng(this, stream);
        }

        /// <summary>
        /// Gets the surface's width in pixels.
        /// </summary>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.Width;
            }
        }

        /// <summary>
        /// Gets the surface's height in pixels.
        /// </summary>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.Height;
            }
        }

        /// <summary>
        /// Gets the length of a surface scanline in bytes.
        /// </summary>
        public override Int32 Pitch
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.Pitch;
            }
        }

        /// <summary>
        /// Gets the number of bytes used to represent a single pixel on this surface.
        /// </summary>
        public override Int32 BytesPerPixel
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.BytesPerPixel;
            }
        }

        /// <summary>
        /// Gets a pointer to the native SDL surface that is encapsulated by this object.
        /// </summary>
        public SDL_Surface_Native* Native
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.Native;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            SafeDispose.Dispose(nativesurf);

            base.Dispose(disposing);
        }

        /// <summary>
        /// Blits the surface onto the specified destination surface.
        /// </summary>
        /// <param name="src">The source surface.</param>
        /// <param name="srcRect">The area of this surface that will be copied to the destination surface.</param>
        /// <param name="dst">The destination surface.</param>
        /// <param name="dstRect">The area on the destination surface to which this surface will be copied.</param>
        private static void BlitInternal(OpenGLSurface2D src, Rectangle srcRect, OpenGLSurface2D dst, Rectangle dstRect)
        {
            var sdlSrcRect = new SDL_Rect() { x = srcRect.X, y = srcRect.Y, w = srcRect.Width, h = srcRect.Height };
            var sdlDstRect = new SDL_Rect() { x = dstRect.X, y = dstRect.Y, w = dstRect.Width, h = dstRect.Height };

            if (SDL.SetSurfaceBlendMode(src.nativesurf.Native, SDL_BlendMode.NONE) < 0)
                throw new SDL2Exception();

            if (srcRect.Width != dstRect.Width || srcRect.Height != dstRect.Height)
            {
                if (SDL.BlitScaled(src.nativesurf.Native, &sdlSrcRect, dst.nativesurf.Native, &sdlDstRect) < 0)
                    throw new SDL2Exception();
            }
            else
            {
                if (SDL.BlitSurface(src.nativesurf.Native, &sdlSrcRect, dst.nativesurf.Native, &sdlDstRect) < 0)
                    throw new SDL2Exception();
            }
        }

        // State values.
        private readonly SDL_Surface nativesurf;
    }
}

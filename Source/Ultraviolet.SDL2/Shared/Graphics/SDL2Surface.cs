using System;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2.Native;

namespace Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Represents the SDL2 implementation of the <see cref="Surface2D"/> class.
    /// </summary>
    public unsafe sealed class SDL2Surface2D : Surface2D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2Surface2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="source">The surface source from which to create the surface.</param>
        public SDL2Surface2D(UltravioletContext uv, SurfaceSource source)
            : this(uv, new SDL2PlatformNativeSurface(source))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2Surface2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="nativesurf">The native SDL surface that this object represents.</param>
        public SDL2Surface2D(UltravioletContext uv, PlatformNativeSurface nativesurf)
            : base(uv)
        {
            if (nativesurf == null)
                throw new ArgumentNullException("nativesurf");

            this.nativesurf = (SDL2PlatformNativeSurface)nativesurf;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2Surface2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The width of the surface in pixels.</param>
        /// <param name="height">The height of the surface in pixels.</param>
        public SDL2Surface2D(UltravioletContext uv, Int32 width, Int32 height)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            this.nativesurf = new SDL2PlatformNativeSurface(width, height);
        }

        /// <inheritdoc/>
        public override void PrepareForTextureExport(Boolean premultiply, Boolean flip)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            nativesurf.PrepareForTextureExport(premultiply, flip);
        }

        /// <inheritdoc/>
        public override void GetData(Color[] data)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));

            nativesurf.GetData(data, new Rectangle(0, 0, Width, Height));
        }

        /// <inheritdoc/>
        public override void GetData(Color[] data, Rectangle region)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));

            nativesurf.GetData(data, region);
        }

        /// <inheritdoc/>
        public override void SetData(Color[] data)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));

            nativesurf.SetData(data, new Rectangle(0, 0, Width, Height));
        }

        /// <inheritdoc/>
        public override void SetData(Color[] data, Rectangle region)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(data, nameof(data));

            nativesurf.SetData(data, region);
        }

        /// <inheritdoc/>
        public override void Blit(Surface2D dst)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dst, nameof(dst));

            Ultraviolet.ValidateResource(dst);

            BlitInternal(this, new Rectangle(0, 0, Width, Height), (SDL2Surface2D)dst, new Rectangle(0, 0, dst.Width, dst.Height));
        }

        /// <inheritdoc/>
        public override void Blit(Surface2D dst, Rectangle dstRect)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dst, nameof(dst));

            Ultraviolet.ValidateResource(dst);

            BlitInternal(this, new Rectangle(0, 0, Width, Height), (SDL2Surface2D)dst, dstRect);
        }

        /// <inheritdoc/>
        public override void Blit(Rectangle srcRect, Surface2D dst, Rectangle dstRect)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dst, nameof(dst));

            Ultraviolet.ValidateResource(dst);

            BlitInternal(this, srcRect, (SDL2Surface2D)dst, dstRect);
        }

        /// <inheritdoc/>
        public override Surface2D CreateSurface()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var copysurf = nativesurf.CreateCopy();
            return new SDL2Surface2D(Ultraviolet, copysurf);
        }

        /// <inheritdoc/>
        public override Surface2D CreateSurface(Rectangle region)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            if (region.Left < 0 || region.Top < 0 || region.Right > Width || region.Bottom > Height || region.Width <= 0 || region.Height <= 0)
                throw new ArgumentOutOfRangeException("region");

            var copysurf = new SDL2PlatformNativeSurface(region.Width, region.Height);

            var srcrect = new SDL_Rect() { x = region.X, y = region.Y, w = region.Width, h = region.Height };
            var dstrect = new SDL_Rect() { x = 0, y = 0, w = region.Width, h = region.Height };

            if (SDL.BlitSurface(nativesurf.NativePtr, &srcrect, copysurf.NativePtr, &dstrect) < 0)
                throw new SDL2Exception();

            return new SDL2Surface2D(Ultraviolet, copysurf);
        }

        /// <inheritdoc/>
        public override Texture2D CreateTexture(Boolean premultiply, Boolean flip)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            using (var copysurf = new SDL2PlatformNativeSurface(Width, Height))
            {
                if (SDL.SetSurfaceBlendMode(nativesurf.NativePtr, SDL_BlendMode.NONE) < 0)
                    throw new SDL2Exception();

                if (SDL.BlitSurface(nativesurf.NativePtr, null, copysurf.NativePtr, null) < 0)
                    throw new SDL2Exception();

                copysurf.PrepareForTextureExport(premultiply, flip);
                return Texture2D.Create((IntPtr)copysurf.NativePtr->pixels, copysurf.Width, copysurf.Height, copysurf.BytesPerPixel);
            }
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var saver = SurfaceSaver.Create();
            saver.SaveAsJpeg(this, stream);
        }

        /// <inheritdoc/>
        public override void SaveAsPng(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var saver = SurfaceSaver.Create();
            saver.SaveAsPng(this, stream);
        }

        /// <inheritdoc/>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.Width;
            }
        }

        /// <inheritdoc/>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.Height;
            }
        }

        /// <inheritdoc/>
        public override Int32 Pitch
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.Pitch;
            }
        }

        /// <inheritdoc/>
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
        public SDL_Surface* NativePtr
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return nativesurf.NativePtr;
            }
        }

        /// <inheritdoc/>
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
        private static void BlitInternal(SDL2Surface2D src, Rectangle srcRect, SDL2Surface2D dst, Rectangle dstRect)
        {
            var sdlSrcRect = new SDL_Rect() { x = srcRect.X, y = srcRect.Y, w = srcRect.Width, h = srcRect.Height };
            var sdlDstRect = new SDL_Rect() { x = dstRect.X, y = dstRect.Y, w = dstRect.Width, h = dstRect.Height };

            if (SDL.SetSurfaceBlendMode(src.nativesurf.NativePtr, SDL_BlendMode.NONE) < 0)
                throw new SDL2Exception();

            if (srcRect.Width != dstRect.Width || srcRect.Height != dstRect.Height)
            {
                if (SDL.BlitScaled(src.nativesurf.NativePtr, &sdlSrcRect, dst.nativesurf.NativePtr, &sdlDstRect) < 0)
                    throw new SDL2Exception();
            }
            else
            {
                if (SDL.BlitSurface(src.nativesurf.NativePtr, &sdlSrcRect, dst.nativesurf.NativePtr, &sdlDstRect) < 0)
                    throw new SDL2Exception();
            }
        }

        // State values.
        private readonly SDL2PlatformNativeSurface nativesurf;
    }
}

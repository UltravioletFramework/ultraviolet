using System;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_BlendMode;
using static Ultraviolet.SDL2.Native.SDLNative;

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
        /// <param name="options">The surface's configuration options.</param>
        public SDL2Surface2D(UltravioletContext uv, SurfaceSource source, SurfaceOptions options)
            : this(uv, new SDL2PlatformNativeSurface(source), options)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2Surface2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="nativesurf">The native SDL surface that this object represents.</param>
        /// <param name="options">The surface's configuration options.</param>
        public SDL2Surface2D(UltravioletContext uv, PlatformNativeSurface nativesurf, SurfaceOptions options)
            : base(uv)
        {
            if (nativesurf == null)
                throw new ArgumentNullException(nameof(nativesurf));

            var isSrgb = (options & SurfaceOptions.SrgbColor) == SurfaceOptions.SrgbColor;
            var isLinear = (options & SurfaceOptions.LinearColor) == SurfaceOptions.LinearColor;
            if (isSrgb && isLinear)
                throw new ArgumentException(UltravioletStrings.SurfaceCannotHaveMultipleEncodings);

            this.nativesurf = (SDL2PlatformNativeSurface)nativesurf;
            this.SrgbEncoded = isLinear ? false : (isSrgb ? true : uv.Properties.SrgbDefaultForSurface2D);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2Surface2D"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="width">The width of the surface in pixels.</param>
        /// <param name="height">The height of the surface in pixels.</param>
        /// <param name="options">The surface's configuration options.</param>
        public SDL2Surface2D(UltravioletContext uv, Int32 width, Int32 height, SurfaceOptions options)
            : base(uv)
        {
            Contract.EnsureRange(width > 0, nameof(width));
            Contract.EnsureRange(height > 0, nameof(height));

            var isSrgb = (options & SurfaceOptions.SrgbColor) == SurfaceOptions.SrgbColor;
            var isLinear = (options & SurfaceOptions.LinearColor) == SurfaceOptions.LinearColor;
            if (isSrgb && isLinear)
                throw new ArgumentException(UltravioletStrings.SurfaceCannotHaveMultipleEncodings);

            this.nativesurf = new SDL2PlatformNativeSurface(width, height);
            this.SrgbEncoded = isLinear ? false : (isSrgb ? true : uv.Properties.SrgbDefaultForSurface2D);
        }

        /// <inheritdoc/>
        public override void Flip(SurfaceFlipDirection direction)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            nativesurf.Flip(direction);
        }

        /// <inheritdoc/>
        public override void FlipAndProcessAlpha(SurfaceFlipDirection direction, Boolean premultiply, Color? keycolor)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            nativesurf.FlipAndProcessAlpha(direction, premultiply, keycolor);
        }

        /// <inheritdoc/>
        public override void ProcessAlpha(Boolean premultiply, Color? keycolor)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            nativesurf.ProcessAlpha(premultiply, keycolor);
        }

        /// <inheritdoc/>
        public override void Clear(Color color)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var rect = new SDL_Rect() { x = 0, y = 0, w = Width, h = Height };
            var colorval = color.PackedValue;

            if (SDL_FillRect(nativesurf.NativePtr, &rect, colorval) < 0)
                throw new SDL2Exception();
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
        public override void SetRawData(IntPtr data, Int32 srcOffsetInBytes, Int32 dstOffsetInBytes, Int32 sizeInBytes)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(srcOffsetInBytes >= 0, nameof(srcOffsetInBytes));
            Contract.EnsureRange(dstOffsetInBytes >= 0, nameof(dstOffsetInBytes));
            Contract.EnsureRange(sizeInBytes >= 0, nameof(sizeInBytes));

            nativesurf.SetRawData(data, srcOffsetInBytes, dstOffsetInBytes, sizeInBytes);
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
        public override void Blit(Surface2D dst, Point2 position)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dst, nameof(dst));

            Ultraviolet.ValidateResource(dst);

            BlitInternal(this, (SDL2Surface2D)dst, position, SurfaceFlipDirection.None);
        }

        /// <inheritdoc/>
        public override void Blit(Surface2D dst, Point2 position, SurfaceFlipDirection direction)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(dst, nameof(dst));

            Ultraviolet.ValidateResource(dst);

            BlitInternal(this, (SDL2Surface2D)dst, position, direction);
        }

        /// <inheritdoc/>
        public override Surface2D CreateSurface()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var options = SrgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;
            var copysurf = nativesurf.CreateCopy();

            return new SDL2Surface2D(Ultraviolet, copysurf, options);
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

            if (SDL_BlitSurface(nativesurf.NativePtr, &srcrect, copysurf.NativePtr, &dstrect) < 0)
                throw new SDL2Exception();

            var options = SrgbEncoded ? SurfaceOptions.SrgbColor : SurfaceOptions.LinearColor;
            var result = new SDL2Surface2D(Ultraviolet, copysurf, options);

            return result;
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Stream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(stream, nameof(stream));

            var saver = SurfaceSaver.Create();
            saver.SaveAsJpeg(this, stream);
        }

        /// <inheritdoc/>
        public override void SaveAsPng(Stream stream)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.Require(stream, nameof(stream));

            var saver = SurfaceSaver.Create();
            saver.SaveAsPng(this, stream);
        }

        /// <inheritdoc/>
        public override Boolean SrgbEncoded
        {
            get => nativesurf.SrgbEncoded;
            set => nativesurf.SrgbEncoded = value;
        }

        /// <inheritdoc/>
        public override Int32 Width => nativesurf.Width;

        /// <inheritdoc/>
        public override Int32 Height => nativesurf.Height;

        /// <inheritdoc/>
        public override Int32 Pitch => nativesurf.Pitch;

        /// <inheritdoc/>
        public override Int32 BytesPerPixel => nativesurf.BytesPerPixel;

        /// <inheritdoc/>
        public override Boolean IsFlippedHorizontally => nativesurf.IsFlippedHorizontally;

        /// <inheritdoc/>
        public override Boolean IsFlippedVertically => nativesurf.IsFlippedVertically;

        /// <inheritdoc/>
        public override Boolean IsAlphaPremultiplied => nativesurf.IsAlphaPremultiplied;

        /// <inheritdoc/>
        public override IntPtr Pixels => (IntPtr)nativesurf.NativePtr->pixels;

        /// <summary>
        /// Gets a pointer to the native SDL surface that is encapsulated by this object.
        /// </summary>
        public SDL_Surface* NativePtr => nativesurf.NativePtr;

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
        private static void BlitInternal(SDL2Surface2D src, Rectangle srcRect, SDL2Surface2D dst, Rectangle dstRect)
        {
            var sdlSrcRect = new SDL_Rect() { x = srcRect.X, y = srcRect.Y, w = srcRect.Width, h = srcRect.Height };
            var sdlDstRect = new SDL_Rect() { x = dstRect.X, y = dstRect.Y, w = dstRect.Width, h = dstRect.Height };

            if (SDL_SetSurfaceBlendMode(src.nativesurf.NativePtr, SDL_BLENDMODE_NONE) < 0)
                throw new SDL2Exception();

            if (srcRect.Width != dstRect.Width || srcRect.Height != dstRect.Height)
            {
                if (SDL_BlitScaled(src.nativesurf.NativePtr, &sdlSrcRect, dst.nativesurf.NativePtr, &sdlDstRect) < 0)
                    throw new SDL2Exception();
            }
            else
            {
                if (SDL_BlitSurface(src.nativesurf.NativePtr, &sdlSrcRect, dst.nativesurf.NativePtr, &sdlDstRect) < 0)
                    throw new SDL2Exception();
            }
        }

        /// <summary>
        /// Blits the surface onto the specified destination surface.
        /// </summary>
        private static void BlitInternal(SDL2Surface2D src, SDL2Surface2D dst, Point2 position, SurfaceFlipDirection direction)
        {
            switch (direction)
            {
                case SurfaceFlipDirection.None:
                    BlitInternal(src, new Rectangle(0, 0, src.Width, src.Height), dst, new Rectangle(position.X, position.Y, src.Width, src.Height));
                    break;

                case SurfaceFlipDirection.Horizontal:
                    BlitInternalFlipH(src, dst, position);
                    break;

                case SurfaceFlipDirection.Vertical:
                    BlitInternalFlipV(src, dst, position);
                    break;
            }
        }
        
        /// <summary>
        /// Blits the surface onto the specified destination surface, flipping it horizontally.
        /// </summary>
        private static void BlitInternalFlipH(SDL2Surface2D src, SDL2Surface2D dst, Point2 position)
        {
            var srcNative = src.NativePtr;
            var dstNative = dst.NativePtr;

            var srcPitch = srcNative->pitch;
            var dstPitch = dstNative->pitch;

            var srcPtr = (UInt32*)srcNative->pixels;
            var dstPtr = (UInt32*)dstNative->pixels;

            var srcX = 0;
            var srcY = 0;

            var dstX = 0;
            var dstY = 0;

            for (var x = 0; x < srcNative->w; x++)
            {
                srcX = (srcNative->w - 1) - x;
                dstX = (position.X + x);

                if (dstX >= dstNative->w)
                    break;

                var pixelsRemaining = dstNative->h - position.Y;
                var pixelsBlitted = (pixelsRemaining > srcNative->h) ? srcNative->h : pixelsRemaining;

                for (var y = 0; y < pixelsBlitted; y++)
                {
                    srcY = y;
                    dstY = position.Y + y;

                    srcPtr = (UInt32*)((Byte*)srcNative->pixels + (srcY * srcPitch)) + srcX;
                    dstPtr = (UInt32*)((Byte*)dstNative->pixels + (dstY * dstPitch)) + dstX;

                    *dstPtr = *srcPtr;
                }
            }
        }

        /// <summary>
        /// Blits the surface onto the specified destination surface, flipping it vertically.
        /// </summary>
        private static void BlitInternalFlipV(SDL2Surface2D src, SDL2Surface2D dst, Point2 position)
        {
            var srcNative = src.NativePtr;
            var dstNative = dst.NativePtr;

            var srcPitch = srcNative->pitch;
            var dstPitch = dstNative->pitch;

            var srcPtr = (UInt32*)srcNative->pixels;
            var dstPtr = (UInt32*)dstNative->pixels;

            var srcY = 0;
            var dstY = 0;

            for (var y = 0; y < srcNative->h; y++)
            {
                srcY = (srcNative->h - 1) - y;
                dstY = (position.Y + y);

                if (dstY >= dstNative->h)
                    break;

                srcPtr = (UInt32*)((Byte*)srcNative->pixels + (srcY * srcPitch));
                dstPtr = (UInt32*)((Byte*)dstNative->pixels + (dstY * dstPitch)) + position.X;

                var pixelsRemaining = dstNative->w - position.X;
                var pixelsBlitted = (pixelsRemaining > srcNative->w) ? srcNative->w : pixelsRemaining;

                for (var x = 0; x < pixelsBlitted; x++)
                    *dstPtr++ = *srcPtr++;
            }
        }

        // State values.
        private readonly SDL2PlatformNativeSurface nativesurf;
    }
}

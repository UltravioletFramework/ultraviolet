using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_BlendMode;
using static Ultraviolet.SDL2.Native.SDLNative;

namespace Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Represents an SDL native surface.
    /// </summary>
    public unsafe sealed class SDL2PlatformNativeSurface : PlatformNativeSurface
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2PlatformNativeSurface"/> class.
        /// </summary>
        /// <param name="width">The surface's width.</param>
        /// <param name="height">The surface's height.</param>
        public SDL2PlatformNativeSurface(Int32 width, Int32 height)
        {
            if ((this.ptr = SDL_CreateRGBSurface(0, width, height, 32, rmask, gmask, bmask, amask)) == null)
                throw new SDL2Exception();
            
            if (SDL_SetSurfaceBlendMode(this.ptr, SDL_BLENDMODE_NONE) < 0)
                throw new SDL2Exception();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2PlatformNativeSurface"/> class.
        /// </summary>
        /// <param name="src">A pointer to the native SDL surface.</param>
        public SDL2PlatformNativeSurface(SDL_Surface* src)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            var dst = SDL_CreateRGBSurface(0, src->w, src->h, 32, rmask, gmask, bmask, amask);
            if (dst == null)
                throw new SDL2Exception();

            if (SDL_SetSurfaceBlendMode(dst, SDL_BLENDMODE_NONE) < 0)
                throw new SDL2Exception();

            if (SDL_BlitSurface(src, null, dst, null) < 0)
                throw new SDL2Exception();

            this.ptr = dst;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SDL2PlatformNativeSurface"/> class.
        /// </summary>
        /// <param name="source">The <see cref="SurfaceSource"/> from which to create the surface.</param>
        public SDL2PlatformNativeSurface(SurfaceSource source)
        {
            Contract.Require(source, nameof(source));

            var width = source.Width;
            var height = source.Height;

            if ((this.ptr = SDL_CreateRGBSurface(0, width, height, 32, rmask, gmask, bmask, amask)) == null)
                throw new SDL2Exception();
            
            if (SDL_SetSurfaceBlendMode(this.ptr, SDL_BLENDMODE_NONE) < 0)
                throw new SDL2Exception();

            var pDstData = (byte*)ptr->pixels;
            var pSrcData = (byte*)source.Data;

            var dstExtraBytes = ptr->pitch - (ptr->w * 4);
            var srcExtraBytes = source.Stride - (source.Width * 4);

            byte srcR, srcG, srcB, srcA;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    srcB = *pSrcData++;
                    srcG = *pSrcData++;
                    srcR = *pSrcData++;
                    srcA = *pSrcData++;

                    if (source.DataFormat == SurfaceSourceDataFormat.RGBA)
                    {
                        var temp = srcR;
                        srcR = srcB;
                        srcB = temp;
                    }

                    *pDstData++ = srcR;
                    *pDstData++ = srcG;
                    *pDstData++ = srcB;
                    *pDstData++ = srcA;
                }

                pDstData += dstExtraBytes;
                pSrcData += srcExtraBytes;
            }
        }
        
        /// <inheritdoc/>
        public override void Flip(SurfaceFlipDirection direction)
        {
            switch (direction)
            {
                case SurfaceFlipDirection.Horizontal:
                    FlipHAndProcessAlpha(premultiply: false, keycolor: null);
                    break;

                case SurfaceFlipDirection.Vertical:
                    FlipVAndProcessAlpha(premultiply: false, keycolor: null);
                    break;

                case SurfaceFlipDirection.None:
                    return;
            }
        }

        /// <inheritdoc/>
        public override void FlipAndProcessAlpha(SurfaceFlipDirection direction, Boolean premultiply, Color? keycolor)
        {
            switch (direction)
            {
                case SurfaceFlipDirection.Horizontal:
                    FlipHAndProcessAlpha(premultiply: premultiply, keycolor: keycolor);
                    break;

                case SurfaceFlipDirection.Vertical:
                    FlipVAndProcessAlpha(premultiply: premultiply, keycolor: keycolor);
                    break;

                case SurfaceFlipDirection.None:
                    ProcessAlpha(premultiply, keycolor);
                    return;
            }
        }

        /// <inheritdoc/>
        public override void ProcessAlpha(Boolean premultiply, Color? keycolor)
        {
            if (isAlphaPremultiplied && !keycolor.HasValue)
                return;

            var pitch = Pitch;

            var srfPtr = (UInt32*)ptr->pixels;
            var srfColor = 0u;

            var colorKeyEnabled = keycolor.HasValue;
            var colorKeyValue = keycolor.GetValueOrDefault().PackedValue;
            var colorKeyTransparent = 0u;

            for (var y = 0; y < ptr->h; y++)
            {
                srfPtr = (UInt32*)((Byte*)ptr->pixels + (y * pitch));

                for (var x = 0; x < ptr->w; x++)
                {
                    srfColor = *srfPtr;

                    if (colorKeyEnabled && srfColor == colorKeyValue)
                    {
                        *srfPtr++ = colorKeyTransparent;
                    }
                    else
                    {
                        if (!isAlphaPremultiplied)
                        {
                            *srfPtr++ = GammaCorrectedPremultiply(srfColor);
                        }
                        else srfPtr++;
                    }
                }
            }

            isAlphaPremultiplied = true;
        }

        /// <inheritdoc/>
        public override void GetData(Color[] data, Rectangle region)
        {
            Contract.EnsureNotDisposed(this, Disposed);
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

        /// <inheritdoc/>
        public override void SetData(Color[] data, Rectangle region)
        {
            Contract.EnsureNotDisposed(this, Disposed);
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

        /// <inheritdoc/>
        public override void SetRawData(IntPtr data, Int32 srcOffsetInBytes, Int32 dstOffsetInBytes, Int32 sizeInBytes)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureRange(srcOffsetInBytes >= 0, nameof(srcOffsetInBytes));
            Contract.EnsureRange(dstOffsetInBytes >= 0, nameof(dstOffsetInBytes));
            Contract.EnsureRange(sizeInBytes >= 0, nameof(sizeInBytes));

            var copied = 0;

            var pSrc = (UInt32*)data.ToPointer();
            for (int sy = 0; sy < ptr->h; sy++)
            {
                var pDst = (UInt32*)((Byte*)ptr->pixels + (sy * Pitch));
                for (int sx = 0; sx < ptr->w; sx++)
                {
                    if (copied >= sizeInBytes)
                        return;

                    *pDst++ = *pSrc++;
                    copied++;
                }
            }
        }

        /// <inheritdoc/>
        public override PlatformNativeSurface CreateCopy()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var copy = new SDL2PlatformNativeSurface(Width, Height);

            if (SDL_BlitSurface(ptr, null, copy.ptr, null) < 0)
                throw new SDL2Exception();

            copy.isReadyForTextureExport = isReadyForTextureExport;
            copy.isFlippedVertically = isFlippedVertically;
            copy.isFlippedHorizontally = isFlippedHorizontally;
            copy.isAlphaPremultiplied = isAlphaPremultiplied;

            return copy;
        }

        /// <inheritdoc/>
        public override Boolean SrgbEncoded { get; set; }

        /// <inheritdoc/>
        public override Boolean IsFlippedHorizontally => isFlippedHorizontally;

        /// <inheritdoc/>
        public override Boolean IsFlippedVertically => isFlippedVertically;

        /// <inheritdoc/>
        public override Boolean IsAlphaPremultiplied => isAlphaPremultiplied;

        /// <inheritdoc/>
        public override Int32 BytesPerPixel => NativePtr->format->BytesPerPixel;

        /// <inheritdoc/>
        public override Int32 Width => NativePtr->w;

        /// <inheritdoc/>
        public override Int32 Height => NativePtr->h;

        /// <inheritdoc/>
        public override Int32 Pitch => NativePtr->pitch;

        /// <inheritdoc/>
        public override IntPtr Native => (IntPtr)ptr;

        /// <summary> 
        /// Gets a pointer to the surface's underlying <see cref="SDL_Surface"/> structure.
        /// </summary>
        public SDL_Surface* NativePtr => 
            Disposed ? throw new ObjectDisposedException(GetType().Name) : ptr;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;

            SDL_FreeSurface(ptr);
            ptr = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Premultiplies the specified color.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private UInt32 GammaCorrectedPremultiply(UInt32 color)
        {
            var a = (Byte)(color >> 24) / 255f;
            var b = (Byte)(color >> 16) / 255f;
            var g = (Byte)(color >> 8) / 255f;
            var r = (Byte)(color) / 255f;

            if (SrgbEncoded)
            {
                b = Color.ConvertSrgbColorChannelToLinear(b);
                g = Color.ConvertSrgbColorChannelToLinear(g);
                r = Color.ConvertSrgbColorChannelToLinear(r);
            }

            b *= a;
            g *= a;
            r *= a;

            if (SrgbEncoded)
            {
                b = Color.ConvertLinearColorChannelToSrgb(b);
                g = Color.ConvertLinearColorChannelToSrgb(g);
                r = Color.ConvertLinearColorChannelToSrgb(r);
            }

            return (UInt32)(((Byte)(255f * r)) | ((Byte)(255f * g) << 8) | ((Byte)(255f * b) << 16) | ((Byte)(255f * a) << 24));
        }

        /// <summary>
        /// Horizontally flips the surface and optionally premultiplies its alpha.
        /// </summary>
        private void FlipHAndProcessAlpha(Boolean premultiply, Color? keycolor)
        {
            var pitch = Pitch;

            var srcPtr = (UInt32*)ptr->pixels;
            var srcColor = 0u;

            var dstPtr = (UInt32*)ptr->pixels;
            var dstColor = 0u;

            var colorKeyEnabled = keycolor.HasValue;
            var colorKeyValue = keycolor.GetValueOrDefault().PackedValue;

            var colsToProcess = (ptr->w % 2 == 0) ? ptr->w / 2 : 1 + ptr->w / 2;
            for (var x = 0; x < colsToProcess; x++)
            {
                var x1 = (x);
                var x2 = (ptr->w - 1) - x;

                for (var y = 0; y < ptr->h; y++)
                {
                    srcPtr = (UInt32*)((Byte*)ptr->pixels + (y * pitch)) + x1;
                    dstPtr = (UInt32*)((Byte*)ptr->pixels + (y * pitch)) + x2;

                    srcColor = *srcPtr;
                    dstColor = *dstPtr;

                    // src -> dst
                    if (colorKeyEnabled && srcColor == colorKeyValue)
                    {
                        *dstPtr = 0u;
                    }
                    else
                    {
                        *dstPtr = (premultiply && !isAlphaPremultiplied) ? GammaCorrectedPremultiply(srcColor) : srcColor;
                    }

                    // dst -> src
                    if (colorKeyEnabled && dstColor == colorKeyValue)
                    {
                        *srcPtr = 0u;
                    }
                    else
                    {
                        *srcPtr = (premultiply && !isAlphaPremultiplied) ? GammaCorrectedPremultiply(dstColor) : dstColor;
                    }
                }
            }

            isFlippedHorizontally = !isFlippedHorizontally;

            if (premultiply)
                isAlphaPremultiplied = true;
        }

        /// <summary>
        /// Vertically flips the surface and optionally premultiplies its alpha.
        /// </summary>
        private void FlipVAndProcessAlpha(Boolean premultiply, Color? keycolor)
        {
            var pitch = Pitch;

            var srcPtr = (UInt32*)ptr->pixels;
            var srcColor = 0u;

            var dstPtr = (UInt32*)ptr->pixels;
            var dstColor = 0u;

            var colorKeyEnabled = premultiply && keycolor.HasValue;
            var colorKeyValue = keycolor.GetValueOrDefault().PackedValue;
            var colorKeyTransparent = 0u;

            var rowsToProcess = (ptr->h % 2 == 0) ? ptr->h / 2 : 1 + ptr->h / 2;
            for (var y = 0; y < rowsToProcess; y++)
            {
                var y1 = (y);
                var y2 = (ptr->h - 1) - y;

                srcPtr = (UInt32*)((Byte*)ptr->pixels + (y1 * pitch));
                dstPtr = (UInt32*)((Byte*)ptr->pixels + (y2 * pitch));

                for (var x = 0; x < ptr->w; x++)
                {
                    srcColor = *srcPtr;
                    dstColor = *dstPtr;

                    // src -> dst
                    if (colorKeyEnabled && srcColor == colorKeyValue)
                    {
                        *dstPtr++ = colorKeyTransparent;
                    }
                    else
                    {
                        *dstPtr++ = (premultiply && !isAlphaPremultiplied) ? GammaCorrectedPremultiply(srcColor) : srcColor;
                    }

                    // dst -> src
                    if (colorKeyEnabled && dstColor == colorKeyValue)
                    {
                        *srcPtr++ = colorKeyTransparent;
                    }
                    else
                    {
                        *srcPtr++ = (premultiply && !isAlphaPremultiplied) ? GammaCorrectedPremultiply(dstColor) : dstColor;
                    }
                }
            }

            isFlippedVertically = !isFlippedVertically;

            if (premultiply)
                isAlphaPremultiplied = true;
        }

        // The mask values for each color channel.
        private static readonly UInt32 rmask = 0x000000ffu;
        private static readonly UInt32 gmask = 0x0000ff00u;
        private static readonly UInt32 bmask = 0x00ff0000u;
        private static readonly UInt32 amask = 0xff000000u;

        // State values.
        private SDL_Surface* ptr;
        private Boolean isReadyForTextureExport;
        private Boolean isFlippedHorizontally;
        private Boolean isFlippedVertically;
        private Boolean isAlphaPremultiplied;
    }
}

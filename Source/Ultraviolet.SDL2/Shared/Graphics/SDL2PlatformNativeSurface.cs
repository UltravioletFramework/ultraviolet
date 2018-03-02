using System;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.SDL2.Native;
using static Ultraviolet.SDL2.Native.SDL_BlendMode;

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
            if ((this.ptr = SDLNative.SDL_CreateRGBSurface(0, width, height, 32, rmask, gmask, bmask, amask)) == null)
                throw new SDL2Exception();
            
            if (SDLNative.SDL_SetSurfaceBlendMode(this.ptr, SDL_BLENDMODE_NONE) < 0)
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

            var dst = SDLNative.SDL_CreateRGBSurface(0, src->w, src->h, 32, rmask, gmask, bmask, amask);
            if (dst == null)
                throw new SDL2Exception();

            if (SDLNative.SDL_SetSurfaceBlendMode(dst, SDL_BLENDMODE_NONE) < 0)
                throw new SDL2Exception();

            if (SDLNative.SDL_BlitSurface(src, null, dst, null) < 0)
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

            if ((this.ptr = SDLNative.SDL_CreateRGBSurface(0, width, height, 32, rmask, gmask, bmask, amask)) == null)
                throw new SDL2Exception();
            
            if (SDLNative.SDL_SetSurfaceBlendMode(this.ptr, SDL_BLENDMODE_NONE) < 0)
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
        public override void PrepareForTextureExport(Boolean premultiply, Boolean flip, Boolean opaque)
        {
            Contract.EnsureNotDisposed(this, Disposed);
            Contract.EnsureNot(isReadyForTextureExport, SDL2Strings.SurfaceAlreadyPreparedForExport);

            var pitch = Pitch;
            var magenta = Color.Magenta.PackedValue;
            var transparent = Color.Transparent.PackedValue;

            uint srcValue;
            uint dstValue;

            byte srcR, srcG, srcB, srcA;
            byte dstR, dstG, dstB, dstA;

            if (flip)
            {
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

                        if (!opaque && srcValue == magenta)
                        {
                            *pDst = transparent;
                        }
                        else
                        {
                            srcA = (byte)(srcValue >> 24);
                            srcB = (byte)(srcValue >> 16);
                            srcG = (byte)(srcValue >> 8);
                            srcR = (byte)(srcValue);

                            if (premultiply)
                            {
                                var factor = srcA / 255f;
                                srcR = (byte)(srcR * factor);
                                srcG = (byte)(srcG * factor);
                                srcB = (byte)(srcB * factor);
                            }

                            *pDst = (uint)((srcR) | (srcG << 8) | (srcB << 16) | (srcA << 24));
                        }

                        if (!opaque && dstValue == magenta)
                        {
                            *pSrc = transparent;
                        }
                        else
                        {
                            dstA = (byte)(dstValue >> 24);
                            dstB = (byte)(dstValue >> 16);
                            dstG = (byte)(dstValue >> 8);
                            dstR = (byte)(dstValue);

                            if (premultiply)
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
            }
            else
            {
                for (var y = 0; y < ptr->h; y++)
                {
                    var pSrc = (UInt32*)((Byte*)ptr->pixels + (y * pitch));
                    var pDst = pSrc;

                    for (var x = 0; x < ptr->w; x++)
                    {
                        srcValue = *pSrc;

                        if (!opaque && srcValue == magenta)
                        {
                            *pDst = transparent;
                        }
                        else
                        {
                            srcA = (byte)(srcValue >> 24);
                            srcB = (byte)(srcValue >> 16);
                            srcG = (byte)(srcValue >> 8);
                            srcR = (byte)(srcValue);

                            if (premultiply)
                            {
                                var factor = srcA / 255f;
                                srcR = (byte)(srcR * factor);
                                srcG = (byte)(srcG * factor);
                                srcB = (byte)(srcB * factor);
                            }

                            *pDst = (uint)((srcR) | (srcG << 8) | (srcB << 16) | (srcA << 24));
                        }

                        pSrc++;
                        pDst++;
                    }
                }
            }

            isReadyForTextureExport = true;
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
        public override PlatformNativeSurface CreateCopy()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            var copy = new SDL2PlatformNativeSurface(Width, Height);

            if (SDLNative.SDL_BlitSurface(ptr, null, copy.ptr, null) < 0)
                throw new SDL2Exception();

            return copy;
        }

        /// <inheritdoc/>
        public override Boolean IsReadyForTextureExport
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return isReadyForTextureExport;
            }
        }

        /// <inheritdoc/>
        public override Int32 BytesPerPixel
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return ptr->format->BytesPerPixel;
            }
        }

        /// <inheritdoc/>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return ptr->w;
            }
        }

        /// <inheritdoc/>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return ptr->h;
            }
        }

        /// <inheritdoc/>
        public override Int32 Pitch
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return ptr->pitch;
            }
        }

        /// <inheritdoc/>
        public override IntPtr Native
        {
            get { return (IntPtr)ptr; }
        }

        /// <summary> 
        /// Gets a pointer to the surface's underlying <see cref="SDL_Surface"/> structure.
        /// </summary>
        public SDL_Surface* NativePtr => ptr;

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
                SDLNative.SDL_FreeSurface(ptr);

            base.Dispose(disposing);
        }

        // The mask values for each color channel.
        private static readonly UInt32 rmask = 0x000000ffu;
        private static readonly UInt32 gmask = 0x0000ff00u;
        private static readonly UInt32 bmask = 0x00ff0000u;
        private static readonly UInt32 amask = 0xff000000u;

        // State values.
        private SDL_Surface* ptr;
        private Boolean isReadyForTextureExport;
    }
}

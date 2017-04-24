using System;
using System.IO;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using UIKit;

namespace Ultraviolet.iOS.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSource"/> class for the iOS platform.
    /// </summary>
    public sealed unsafe class iOSSurfaceSource : SurfaceSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iOSSurfaceSource"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the surface data.</param>
        public iOSSurfaceSource(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            using (var data = NSData.FromStream(stream))
            {
                using (var img = UIImage.LoadFromData(data))
                {
                    this.width = (Int32)img.Size.Width;
                    this.height = (Int32)img.Size.Height;
                    this.stride = (Int32)img.CGImage.BytesPerRow;

                    this.bmpData = Marshal.AllocHGlobal(stride * height);

                    using (var colorSpace = CGColorSpace.CreateDeviceRGB())
                    {
                        using (var bmp = new CGBitmapContext(bmpData, width, height, 8, stride, colorSpace, CGImageAlphaInfo.PremultipliedLast))
                        {
                            bmp.ClearRect(new CGRect(0, 0, width, height));
                            bmp.DrawImage(new CGRect(0, 0, width, height), img.CGImage);
                        }
                    }
                }
            }

            ReversePremultiplication();
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public override Color this[int x, int y]
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                unsafe
                {
                    var pixel = ((byte*)bmpData.ToPointer()) + (stride * y) + (x * sizeof(UInt32));
                    var a = *pixel++;
                    var b = *pixel++;
                    var g = *pixel++;
                    var r = *pixel++;
                    return new Color(r, g, b, a);
                }
            }
        }

        /// <inheritdoc/>
        public override IntPtr Data
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return bmpData;
            }
        }

        /// <inheritdoc/>
        public override Int32 Stride
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return stride;
            }
        }

        /// <inheritdoc/>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return width;
            }
        }

        /// <inheritdoc/>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return height;
            }
        }

        /// <inheritdoc/>
        public override SurfaceSourceDataFormat DataFormat => SurfaceSourceDataFormat.BGRA;

        /// <summary>
        /// Reverses the premultiplication which is automatically applied by the iOS API's...
        /// ...so that Ultraviolet can re-premultiply it later. Yeah.
        /// </summary>
        private void ReversePremultiplication()
        {
            var pBmpData = (Byte*)bmpData.ToPointer();
            for (int i = 0; i < width * height; i++)
            {
                var a = *(pBmpData + 3) / 255f;
                *(pBmpData + 0) = (Byte)(*(pBmpData + 0) / a);
                *(pBmpData + 1) = (Byte)(*(pBmpData + 1) / a);
                *(pBmpData + 2) = (Byte)(*(pBmpData + 2) / a);

                pBmpData += 4;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        private void Dispose(Boolean disposing)
        {
            if (disposed)
                return;

            if (bmpData != IntPtr.Zero)
                Marshal.FreeHGlobal(bmpData);

            disposed = true;
        }

        // State values.
        private readonly IntPtr bmpData;
        private readonly Int32 width;
        private readonly Int32 height;
        private readonly Int32 stride;
        private Boolean disposed;
    }
}

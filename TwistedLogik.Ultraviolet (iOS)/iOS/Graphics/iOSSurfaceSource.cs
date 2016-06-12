using System;
using System.IO;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using UIKit;

namespace TwistedLogik.Ultraviolet.iOS.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSource"/> class for the iOS platform.
    /// </summary>
    public sealed class iOSSurfaceSource : SurfaceSource
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

                    this.bmpData = new Byte[stride * height];
                    this.bmpDataHandle = GCHandle.Alloc(bmpData, GCHandleType.Pinned);

                    using (var bmp = new CGBitmapContext(bmpData, width, height, 8, 4 * width, CGColorSpace.CreateDeviceRGB(), CGImageAlphaInfo.First))
                    {
                        bmp.DrawImage(new CGRect(0, 0, width, height), img.CGImage);
                    }
                }
            }            
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
                    fixed (Byte* pixel = &bmpData[y * stride + (x * sizeof(UInt32))])
                        return Color.FromArgb(*(UInt32*)pixel);
                }
            }
        }

        /// <inheritdoc/>
        public override IntPtr Data
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return bmpDataHandle.AddrOfPinnedObject();
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
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        private void Dispose(Boolean disposing)
        {
            if (disposed)
                return;

            bmpDataHandle.Free();

            disposed = true;
        }

        // State values.
        private readonly Byte[] bmpData;
        private readonly GCHandle bmpDataHandle;
        private readonly Int32 width;
        private readonly Int32 height;
        private readonly Int32 stride;
        private Boolean disposed;
    }
}

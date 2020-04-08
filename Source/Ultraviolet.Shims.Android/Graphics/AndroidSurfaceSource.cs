using System;
using System.IO;
using System.Runtime.InteropServices;
using Android.Graphics;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.Shims.Android.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSource"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidSurfaceSource : SurfaceSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidSurfaceSource"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the surface data.</param>
        public AndroidSurfaceSource(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var opts = new BitmapFactory.Options();
            opts.InPreferredConfig = Bitmap.Config.Argb8888;

            using (var bmp = BitmapFactory.DecodeStream(stream, new Rect(), opts))
            {
                this.width = bmp.Width;
                this.height = bmp.Height;
                this.stride = bmp.RowBytes;

                this.bmpData = new Byte[stride * height];
                this.bmpDataHandle = GCHandle.Alloc(bmpData, GCHandleType.Pinned);

                var pixels = bmp.LockPixels();

                Marshal.Copy(pixels, bmpData, 0, bmpData.Length);

                bmp.UnlockPixels();
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
                    var pixel = ((byte*)Data.ToPointer()) + (stride * y) + (x * sizeof(UInt32));
                    var r = *pixel++;
                    var g = *pixel++;
                    var b = *pixel++;
                    var a = *pixel++;
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
        public override SurfaceSourceDataFormat DataFormat => SurfaceSourceDataFormat.RGBA;

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
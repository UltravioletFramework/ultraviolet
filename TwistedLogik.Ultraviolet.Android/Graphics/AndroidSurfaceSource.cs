using System;
using System.IO;
using Android.Graphics;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.Android.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSource"/> class for the Android platform.
    /// </summary>
    public sealed unsafe class AndroidSurfaceSource : SurfaceSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidSurfaceSource"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the surface data.</param>
        public AndroidSurfaceSource(Stream stream)
        {
            Contract.Require(stream, "stream");

            this.bmp = BitmapFactory.DecodeStream(stream);
            this.bmpData = this.bmp.LockPixels();
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

                var pixel = ((byte*)bmpData) + (bmp.RowBytes * y) + (x * sizeof(UInt32));
                return Color.FromArgb(*(uint*)pixel);
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

                return bmp.RowBytes;
            }
        }

        /// <inheritdoc/>
        public override Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return bmp.Width;
            }
        }

        /// <inheritdoc/>
        public override Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return bmp.Height;
            }
        }

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><c>true</c> if the object is being disposed; <c>false</c> if the object is being finalized.</param>
        private void Dispose(Boolean disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                bmp.UnlockPixels();
                bmp.Dispose();
            }

            disposed = true;
        }

        // State values.
        private readonly Bitmap bmp;
        private readonly IntPtr bmpData;
        private Boolean disposed;
    }
}

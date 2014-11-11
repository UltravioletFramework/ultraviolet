using System;
using System.Drawing;
using System.Drawing.Imaging;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.SDL2.Graphics
{
    /// <summary>
    /// Represents a <see cref="SurfaceSource"/> which retrieves pixel data from a <see cref="Bitmap"/> object.
    /// </summary>
    public unsafe class BitmapSurfaceSource : SurfaceSource, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapSurfaceSource"/> class.
        /// </summary>
        /// <param name="bmp">The <see cref="Bitmap"/> from which to read surface data.</param>
        public BitmapSurfaceSource(Bitmap bmp)
        {
            Contract.Require(bmp, "bmp");

            this.bmp = bmp;
            this.bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        }

        /// <inheritdoc/>
        public void Dispose()
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

                var pixel = ((byte*)bmpData.Scan0) + (bmpData.Stride * y) + (x * sizeof(UInt32));
                return Color.FromRgba(*(uint*)pixel);
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
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                bmp.UnlockBits(bmpData);
                bmp.Dispose();
            }

            disposed = true;
        }

        // State values.
        private readonly Bitmap bmp;
        private readonly BitmapData bmpData;
        private Boolean disposed;
    }
}

using System;
using System.Buffers;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Image;

namespace Ultraviolet.Shims.NETCore.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSource"/> class for the .NET Core platform.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class NETCoreSurfaceSource : SurfaceSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NETCoreSurfaceSource"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the surface data.</param>
        public NETCoreSurfaceSource(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var data = new Byte[stream.Length];
            stream.Read(data, 0, data.Length);

            using (var mstream = new MemoryStream(data))
            {
                this.image = UltravioletImage.FromStream(mstream);
                this.imageMemory = new Memory<byte>(this.image.Data);
                this.imageMemoryHandle = this.imageMemory.Pin();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NETCoreSurfaceSource"/> class.
        /// </summary>
        /// <param name="sourceImage">The image from which to read surface data.</param>
        public NETCoreSurfaceSource(UltravioletImage sourceImage)
        {
            Contract.Require(sourceImage, nameof(sourceImage));

            this.image = UltravioletImage.FromMemory(sourceImage.Data);
            this.imageMemory = new Memory<byte>(this.image.Data);
            this.imageMemoryHandle = this.imageMemory.Pin();
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

                this.image.GetPixel(x, y, out Pixel4 pixel);
                return new Color(pixel.R, pixel.G, pixel.B, pixel.A);
            }
        }

        /// <inheritdoc/>
        public unsafe override IntPtr Data => (IntPtr)imageMemoryHandle.Pointer;

        /// <inheritdoc/>
        public override Int32 Stride => image.GetStride();

        /// <inheritdoc/>
        public override Int32 Width => image.Width;

        /// <inheritdoc/>
        public override Int32 Height => image.Height;

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

            if (disposing)
            {
                imageMemoryHandle.Dispose();
                image.Dispose();
            }

            disposed = true;
        }

        // State values.
        private readonly UltravioletImage image; 
        private readonly MemoryHandle imageMemoryHandle;
        private readonly Memory<byte> imageMemory;
        private Boolean disposed;
    }
}

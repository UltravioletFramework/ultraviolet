using System;
using System.Buffers;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

/// <summary>
/// A 3 component pixel.
/// </summary>
public struct Pixel3
{
    /// <summary>
    /// First component.
    /// </summary>
    public byte R;

    /// <summary>
    /// Second component.
    /// </summary>
    public byte G;

    /// <summary>
    /// Third component.
    /// </summary>
    public byte B;
}
/// <summary>
/// A 4 component pixel.
/// </summary>
public struct Pixel4
{
    /// <summary>
    /// First component.
    /// </summary>
    public byte R;

    /// <summary>
    /// Second component.
    /// </summary>
    public byte G;

    /// <summary>
    /// Third component.
    /// </summary>
    public byte B;

    /// <summary>
    /// Fourth component.
    /// </summary>
    public byte A;
}

/// <summary>
/// Extensions for STB_Image.
/// </summary>
public static partial class StbExtensions
{
    /// <summary>
    /// Get a pixel at the specified coordinates.
    /// </summary>
#pragma warning disable CS3001 // Argument type is not CLS-compliant
    public static void GetPixel(this StbImageSharp.ImageResult image, int x, int y, out Pixel3 pixel)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
    {
        int bpp = 3;

        int pixelOffset = (x + image.Width * y) * bpp;

        pixel = new Pixel3();
        pixel.R = image.Data[pixelOffset + 0];
        pixel.G = image.Data[pixelOffset + 1];
        pixel.B = image.Data[pixelOffset + 2];
    }

    /// <summary>
    /// Get a pixel at the specified coordinates.
    /// </summary>
#pragma warning disable CS3001 // Argument type is not CLS-compliant
    public static void GetPixel(this StbImageSharp.ImageResult image, int x, int y, out Pixel4 pixel)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
    {
        int bpp = 4;

        int pixelOffset = (y * image.Width + x) * bpp;

        pixel = new Pixel4();
        pixel.R = image.Data[pixelOffset + 0];
        pixel.G = image.Data[pixelOffset + 1];
        pixel.B = image.Data[pixelOffset + 2];
        pixel.A = image.Data[pixelOffset + 3];
    }

    /// <summary>
    /// Set a pixel at the specified coordinates.
    /// </summary>
#pragma warning disable CS3001 // Argument type is not CLS-compliant
    public static void SetPixel(this StbImageSharp.ImageResult image, int x, int y, byte r, byte g, byte b, byte a)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
    {
        int bpp = 4;

        int pixelOffset = (x + image.Width * y) * bpp;

        if (image.Comp != StbImageSharp.ColorComponents.RedGreenBlueAlpha)
        {
            throw new Exception("Image comp is not expected.");
        }

        image.Data[pixelOffset + 0] = r;
        image.Data[pixelOffset + 1] = g;
        image.Data[pixelOffset + 2] = b;
        image.Data[pixelOffset + 3] = a;
    }

    /// <summary>
    /// Set a pixel at the specified coordinates.
    /// </summary>
#pragma warning disable CS3001 // Argument type is not CLS-compliant
    public static void SetPixel(this StbImageSharp.ImageResult image, int x, int y, byte r, byte g, byte b)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
    {
        int bpp = 3;

        int pixelOffset = (x + image.Width * y) * bpp;

        if (image.Comp != StbImageSharp.ColorComponents.RedGreenBlue)
        {
            throw new Exception("Image comp is not expected.");
        }

        image.Data[pixelOffset + 0] = r;
        image.Data[pixelOffset + 1] = g;
        image.Data[pixelOffset + 2] = b;
    }
    /// <summary>
    /// Gets the image stride.
    /// </summary>
#pragma warning disable CS3001 // Argument type is not CLS-compliant
    public static int GetStride(this StbImageSharp.ImageResult image)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
    {
        switch (image.Comp)
        {
            case StbImageSharp.ColorComponents.Grey:
                return 1 * image.Width;
            case StbImageSharp.ColorComponents.GreyAlpha:
                return 2 * image.Width;
            case StbImageSharp.ColorComponents.RedGreenBlue:
                return 3 * image.Width;
            case StbImageSharp.ColorComponents.RedGreenBlueAlpha:
                return 4 * image.Width;
        }

        return 0;
    }
}

namespace Ultraviolet.Desktop.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSource"/> class for the .NET Core 3.0 platform.
    /// </summary>
    [CLSCompliant(false)]
    public sealed class NETCore3SurfaceSource : SurfaceSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NETCore3SurfaceSource"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that contains the surface data.</param>
        public NETCore3SurfaceSource(Stream stream)
        {
            Contract.Require(stream, nameof(stream));

            var data = new Byte[stream.Length];
            stream.Read(data, 0, data.Length);

            using (var mstream = new MemoryStream(data)) 
            {
                this.bmp = StbImageSharp.ImageResult.FromStream(mstream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);
                imageMemory = new Memory<byte>(this.bmp.Data);

                this.imageMemoryHandle = imageMemory.Pin();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NETCore3SurfaceSource"/> class.
        /// </summary>
        /// <param name="bmp">The bitmap from which to read surface data.</param>
        public NETCore3SurfaceSource(StbImageSharp.ImageResult bmp)
        {
            Contract.Require(bmp, nameof(bmp));

            this.bmp = bmp;
            imageMemory = new Memory<byte>(this.bmp.Data);

            this.imageMemoryHandle = imageMemory.Pin();
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

                this.bmp.GetPixel(x, y, out Pixel4 pixel);
                return new Color(pixel.R, pixel.G, pixel.B, pixel.A);
            }
        }

        /// <inheritdoc/>
        public unsafe override IntPtr Data => (IntPtr)imageMemoryHandle.Pointer;

        /// <inheritdoc/>
        public override Int32 Stride => bmp.GetStride();

        /// <inheritdoc/>
        public override Int32 Width => bmp.Width;

        /// <inheritdoc/>
        public override Int32 Height => bmp.Height;

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
            }

            disposed = true;
        }

        // State values.
        private readonly StbImageSharp.ImageResult bmp;
        private readonly MemoryHandle imageMemoryHandle;
        private readonly Memory<byte> imageMemory;
        private Boolean disposed;
    }
}

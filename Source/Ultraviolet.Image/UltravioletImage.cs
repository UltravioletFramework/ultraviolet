using System.Diagnostics;
using System.IO;
using System;

namespace Ultraviolet.Image
{

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
    /// The color components
    /// </summary>
    public enum ColorComponents
    {
        /// <summary>
        /// 
        /// </summary>
        Default,

        /// <summary>
        /// 
        /// </summary>
        Grey,

        /// <summary>
        /// 
        /// </summary>
        GreyAlpha,

        /// <summary>
        /// 
        /// </summary>
        RedGreenBlue,

        /// <summary>
        /// 
        /// </summary>
        RedGreenBlueAlpha
    }

    /// <summary>
    /// Represent an image used by the Ultraviolet Framework
    /// </summary>
    public class UltravioletImage : IDisposable
    {
        private StbImageSharp.ImageResult _image;

        /// <summary>
        /// The width of the image in pixels
        /// </summary>
        public int Width
        {
            get => _image.Width;
            private set => _image.Width = value;
        }

        /// <summary>
        /// The height of the image in pixels
        /// </summary>
        public int Height
        {
            get => _image.Height;
            private set => _image.Height = value;
        }

        /// <summary>
        /// The color components as read from the source
        /// </summary>
        public ColorComponents SourceComp
        {
            get => (ColorComponents)_image.SourceComp;
            private set => _image.SourceComp = (StbImageSharp.ColorComponents)value;
        }

        /// <summary>
        /// The color components
        /// </summary>
        public ColorComponents Comp
        {
            get => (ColorComponents)_image.Comp;
            private set => _image.Comp = (StbImageSharp.ColorComponents)value;
        }

        /// <summary>
        /// The image raw bytes
        /// </summary>
        public byte[] Data
        {
            get => _image.Data;
            private set => _image.Data = value;
        }

        /// <summary>
        /// Loads an image from a file
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns></returns>
        public static UltravioletImage FromFile(String fileName)
        {
            var stream = File.Open(fileName, FileMode.Open);
            var image = StbImageSharp.ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);

            return new UltravioletImage { _image = image };
        }

        /// <summary>
        /// Loads an image from stream
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <returns></returns>
        public static UltravioletImage FromStream(Stream stream)
        {
            var image = StbImageSharp.ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);

            return new UltravioletImage { _image = image };
        }

        /// <summary>
        /// Creates an image from raw bytes
        /// </summary>
        /// <param name="data">The raw bytes</param>
        /// <returns></returns>
        public static UltravioletImage FromMemory(byte[] data)
        {
            var image = StbImageSharp.ImageResult.FromMemory(data, StbImageSharp.ColorComponents.RedGreenBlueAlpha);
            return new UltravioletImage { _image = image };
        }

        private UltravioletImage() { }

        /// <summary>
        /// Creates a new image
        /// </summary>
        /// <param name="width">The width in pixels</param>
        /// <param name="height">The height in pixels</param>
        /// <param name="data">Optional raw bytes</param>
        public UltravioletImage(int width, int height, byte[] data = null)
        {
            _image = new StbImageSharp.ImageResult
            {
                Width = width,
                Height = height,
                Comp = StbImageSharp.ColorComponents.RedGreenBlueAlpha
            };
            int componentCount = GetComponentCount((ColorComponents)_image.Comp);
            _image.Data = new byte[width * height * componentCount];
            if (data != null)
            {
                data.CopyTo(_image.Data, 0);
            }
        }

        /// <summary>
        /// Get a pixel at the specified coordinates.
        /// </summary>
        public void GetPixel(int x, int y, out Pixel3 pixel)
        {
            int bpp = 3;

            int pixelOffset = (x + _image.Width * y) * bpp;

            pixel = new Pixel3();
            pixel.R = _image.Data[pixelOffset + 0];
            pixel.G = _image.Data[pixelOffset + 1];
            pixel.B = _image.Data[pixelOffset + 2];
        }

        /// <summary>
        /// Get a pixel at the specified coordinates.
        /// </summary>
        public void GetPixel(int x, int y, out Pixel4 pixel)
        {
            int bpp = 4;

            int pixelOffset = (y * _image.Width + x) * bpp;

            pixel = new Pixel4();
            pixel.R = _image.Data[pixelOffset + 0];
            pixel.G = _image.Data[pixelOffset + 1];
            pixel.B = _image.Data[pixelOffset + 2];
            pixel.A = _image.Data[pixelOffset + 3];
        }

        /// <summary>
        /// Set a pixel at the specified coordinates.
        /// </summary>
        public void SetPixel(int x, int y, Pixel4 pixel)
        {
            int bpp = 4;

            int pixelOffset = (x + _image.Width * y) * bpp;

            if (_image.Comp != StbImageSharp.ColorComponents.RedGreenBlueAlpha)
            {
                throw new Exception("Image comp is not expected.");
            }

            _image.Data[pixelOffset + 0] = pixel.R;
            _image.Data[pixelOffset + 1] = pixel.G;
            _image.Data[pixelOffset + 2] = pixel.B;
            _image.Data[pixelOffset + 3] = pixel.A;
        }

        /// <summary>
        /// Set a pixel at the specified coordinates.
        /// </summary>
        public void SetPixel(int x, int y, byte r, byte g, byte b, byte a)
        {
            int bpp = 4;

            int pixelOffset = (x + _image.Width * y) * bpp;

            if (_image.Comp != StbImageSharp.ColorComponents.RedGreenBlueAlpha)
            {
                throw new Exception("Image comp is not expected.");
            }

            _image.Data[pixelOffset + 0] = r;
            _image.Data[pixelOffset + 1] = g;
            _image.Data[pixelOffset + 2] = b;
            _image.Data[pixelOffset + 3] = a;
        }

        /// <summary>
        /// Set a pixel at the specified coordinates.
        /// </summary>
        public void SetPixel(int x, int y, Pixel3 pixel)
        {
            int bpp = 3;

            int pixelOffset = (x + _image.Width * y) * bpp;

            if (_image.Comp != StbImageSharp.ColorComponents.RedGreenBlue)
            {
                throw new Exception("Image comp is not expected.");
            }

            _image.Data[pixelOffset + 0] = pixel.R;
            _image.Data[pixelOffset + 1] = pixel.G;
            _image.Data[pixelOffset + 2] = pixel.B;
        }

        /// <summary>
        /// Set a pixel at the specified coordinates.
        /// </summary>
        public void SetPixel(int x, int y, byte r, byte g, byte b)
        {
            int bpp = 3;

            int pixelOffset = (x + _image.Width * y) * bpp;

            if (_image.Comp != StbImageSharp.ColorComponents.RedGreenBlue)
            {
                throw new Exception("Image comp is not expected.");
            }

            _image.Data[pixelOffset + 0] = r;
            _image.Data[pixelOffset + 1] = g;
            _image.Data[pixelOffset + 2] = b;
        }
        /// <summary>
        /// Gets the image stride.
        /// </summary>
        public int GetStride()
        {
            return GetStride(Width, Comp);
        }

        private static int GetStride(int width, ColorComponents components)
        {
            switch (components)
            {
                case ColorComponents.Grey:
                    return 1 * width;
                case ColorComponents.GreyAlpha:
                    return 2 * width;
                case ColorComponents.RedGreenBlue:
                    return 3 * width;
                case ColorComponents.RedGreenBlueAlpha:
                    return 4 * width;
            }

            return 0;
        }

        private static int GetComponentCount(ColorComponents components)
        {
            switch (components)
            {
                case ColorComponents.Grey:
                    return 1;
                case ColorComponents.GreyAlpha:
                    return 2;
                case ColorComponents.RedGreenBlue:
                    return 3;
                case ColorComponents.RedGreenBlueAlpha:
                    return 4;
            }

            return 0;
        }

        /// <summary>
        /// Saves the image as JPEG to the stream
        /// </summary>
        /// <param name="stream">The stream to write the image to</param>
        /// <param name="quality">The quality of the jpeg image (0-100)</param>
        public void SaveAsJpeg(Stream stream, int quality)
        {
            var imageWriter = new StbImageWriteSharp.ImageWriter();
            imageWriter.WriteJpg(_image.Data, _image.Width, _image.Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream, quality);
        }

        /// <summary>
        /// Saves the image as PNG to the stream
        /// </summary>
        /// <param name="stream">The stream</param>
        public void SaveAsPng(Stream stream)
        {
            var imageWriter = new StbImageWriteSharp.ImageWriter();
            imageWriter.WritePng(_image.Data, _image.Width, _image.Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream);
        }

        /// <summary>
        /// Saves the image in the given format to a stream
        /// </summary>
        /// <param name="stream">The stream the image is written to</param>
        /// <param name="format">The format of the image</param>
        /// <exception cref="ArgumentException"></exception>
        public void Save(Stream stream, UltravioletImageFormat format)
        {
            switch (format)
            {
                case UltravioletImageFormat.PNG:
                    SaveAsPng(stream);
                    break;

                case UltravioletImageFormat.JPEG:
                    SaveAsJpeg(stream, 100);
                    break;

                default:
                    throw new ArgumentException(nameof(format));
            }
        }

        /// <summary>
        /// Disposes the image
        /// </summary>
        public void Dispose()
        {
        }
    }
}

using System;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.Shims.NETCore3.Graphics
{
    /// <summary>
    /// Surface image format
    /// </summary>
    public enum SurfaceImageFormat
    {
        /// <summary>
        /// PNG Surface image format
        /// </summary>
        PNG,

        /// <summary>
        /// JPEG Surface image format
        /// </summary>
        JPEG
    }

    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSaver"/> class for the .NET Core 3.0 platform.
    /// </summary>
    public sealed class NETCore3SurfaceSaver : SurfaceSaver
    {
        /// <inheritdoc/>
        public override void SaveAsPng(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, nameof(surface));
            Contract.Require(stream, nameof(stream));

            Save(surface, stream, SurfaceImageFormat.PNG);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, nameof(surface));
            Contract.Require(stream, nameof(stream));

            Save(surface, stream, SurfaceImageFormat.JPEG);
        }

        /// <inheritdoc/>
        public override void SaveAsPng(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, nameof(renderTarget));
            Contract.Require(stream, nameof(stream));

            Save(renderTarget, stream, SurfaceImageFormat.PNG);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, nameof(renderTarget));
            Contract.Require(stream, nameof(stream));

            Save(renderTarget, stream, SurfaceImageFormat.JPEG);
        }

        /// <summary>
        /// Saves the specified surface as an image with the specified format.
        /// </summary>
        /// <param name="surface">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        /// <param name="format">The format with which to save the image.</param>
        private void Save(Surface2D surface, Stream stream, SurfaceImageFormat format)
        {
            var data = new Color[surface.Width * surface.Height];
            surface.GetData(data);

            Save(data, surface.Width, surface.Height, stream, format);
        }

        /// <summary>
        /// Saves the specified render target as an image with the specified format.
        /// </summary>
        /// <param name="renderTarget">The render target to save.</param>
        /// <param name="stream">The stream to which to save the render target data.</param>
        /// <param name="format">The format with which to save the image.</param>
        private void Save(RenderTarget2D renderTarget, Stream stream, SurfaceImageFormat format)
        {
            var data = new Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(data);

            Save(data, renderTarget.Width, renderTarget.Height, stream, format);
        }



        /// <summary>
        /// Saves the specified color data as an image with the specified format.
        /// </summary>
        /// <param name="data">An array containing the image's color data.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="stream">The stream to which to save the image data.</param>
        /// <param name="format">The format with which to save the image.</param>
        private unsafe void Save(Color[] data, Int32 width, Int32 height, Stream stream, SurfaceImageFormat format)
        {
            var image = new StbImageSharp.ImageResult()
            {
                Width = width,
                Height = height,
                Comp = StbImageSharp.ColorComponents.RedGreenBlueAlpha,
                Data = new byte[width * height * 4]
            };

            fixed (Color* pData = data)
            {
                for (int y = 0; y < height; y++)
                {
                    var pSrc = pData + (y * width);

                    for (int x = 0; x < width; x++)
                    {
                        var color = *pSrc++;
                        image.SetPixel(x, y, color.R, color.G, color.B, color.A);
                    }
                }
            }

            var imageWriter = new StbImageWriteSharp.ImageWriter();
            if(format == SurfaceImageFormat.PNG)
                imageWriter.WritePng(image.Data, image.Width, image.Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream);
            else
                imageWriter.WriteJpg(image.Data, image.Width, image.Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream, 100);
        }
    }
}

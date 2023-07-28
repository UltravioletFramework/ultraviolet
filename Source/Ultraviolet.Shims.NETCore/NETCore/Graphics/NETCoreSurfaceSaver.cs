using System;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Image;

namespace Ultraviolet.Shims.NETCore.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSaver"/> class for the .NET Core platform.
    /// </summary>
    public sealed class NETCoreSurfaceSaver : SurfaceSaver
    {
        /// <inheritdoc/>
        public override void SaveAsPng(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, nameof(surface));
            Contract.Require(stream, nameof(stream));

            Save(surface, stream, UltravioletImageFormat.PNG);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, nameof(surface));
            Contract.Require(stream, nameof(stream));

            Save(surface, stream, UltravioletImageFormat.JPEG);
        }

        /// <inheritdoc/>
        public override void SaveAsPng(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, nameof(renderTarget));
            Contract.Require(stream, nameof(stream));

            Save(renderTarget, stream, UltravioletImageFormat.PNG);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, nameof(renderTarget));
            Contract.Require(stream, nameof(stream));

            Save(renderTarget, stream, UltravioletImageFormat.JPEG);
        }

        /// <summary>
        /// Saves the specified surface as an image with the specified format.
        /// </summary>
        /// <param name="surface">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        /// <param name="format">The format with which to save the image.</param>
        private void Save(Surface2D surface, Stream stream, UltravioletImageFormat format)
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
        private void Save(RenderTarget2D renderTarget, Stream stream, UltravioletImageFormat format)
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
        private unsafe void Save(Color[] data, Int32 width, Int32 height, Stream stream, UltravioletImageFormat format)
        {
            using (var image = new UltravioletImage(width, height))
            {
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

                image.Save(stream, format);
            }
        }
    }
}

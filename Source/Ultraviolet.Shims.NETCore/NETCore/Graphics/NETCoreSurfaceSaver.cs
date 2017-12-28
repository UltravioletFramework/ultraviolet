using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using GDIRect = System.Drawing.Rectangle;

namespace Ultraviolet.Shims.NETCore.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSaver"/> class for the .NET Standard 2.0 platform.
    /// </summary>
    public sealed class NETCoreSurfaceSaver : SurfaceSaver
    {
        /// <inheritdoc/>
        public override void SaveAsPng(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, nameof(surface));
            Contract.Require(stream, nameof(stream));

            Save(surface, stream, ImageFormat.Png);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, nameof(surface));
            Contract.Require(stream, nameof(stream));

            Save(surface, stream, ImageFormat.Jpeg);
        }

        /// <inheritdoc/>
        public override void SaveAsPng(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, nameof(renderTarget));
            Contract.Require(stream, nameof(stream));

            Save(renderTarget, stream, ImageFormat.Png);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, nameof(renderTarget));
            Contract.Require(stream, nameof(stream));

            Save(renderTarget, stream, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Saves the specified surface as an image with the specified format.
        /// </summary>
        /// <param name="surface">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        /// <param name="format">The format with which to save the image.</param>
        private void Save(Surface2D surface, Stream stream, ImageFormat format)
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
        private void Save(RenderTarget2D renderTarget, Stream stream, ImageFormat format)
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
        private unsafe void Save(Color[] data, Int32 width, Int32 height, Stream stream, ImageFormat format)
        {
            using (var bmp = new Bitmap(width, height))
            {
                var bmpData = bmp.LockBits(new GDIRect(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                fixed (Color* pData = data)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var pSrc = pData + (y * width);
                        var pDst = (Byte*)bmpData.Scan0 + (y * bmpData.Stride);

                        for (int x = 0; x < width; x++)
                        {
                            var color = *pSrc++;
                            *pDst++ = color.B;
                            *pDst++ = color.G;
                            *pDst++ = color.R;
                            *pDst++ = color.A;
                        }
                    }
                }

                bmp.UnlockBits(bmpData);
                bmp.Save(stream, format);
            }
        }
    }
}

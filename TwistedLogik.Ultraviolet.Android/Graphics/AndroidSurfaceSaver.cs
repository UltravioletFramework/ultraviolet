using System;
using System.IO;
using Android.Graphics;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;

namespace TwistedLogik.Ultraviolet.Android.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSaver"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidSurfaceSaver : SurfaceSaver
    {
        /// <inheritdoc/>
        public override void SaveAsPng(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, "surface");
            Contract.Require(stream, "stream");

            Save(surface, stream, Bitmap.CompressFormat.Png);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, "surface");
            Contract.Require(stream, "stream");

            Save(surface, stream, Bitmap.CompressFormat.Jpeg);
        }

        /// <inheritdoc/>
        public override void SaveAsPng(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, "renderTarget");
            Contract.Require(stream, "stream");

            Save(renderTarget, stream, Bitmap.CompressFormat.Png);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, "renderTarget");
            Contract.Require(stream, "stream");

            Save(renderTarget, stream, Bitmap.CompressFormat.Jpeg);
        }

        /// <summary>
        /// Saves the specified surface as an image with the specified format.
        /// </summary>
        /// <param name="renderTarget">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        /// <param name="format">The format with which to save the image.</param>
        private void Save(Surface2D surface, Stream stream, Bitmap.CompressFormat format)
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
        private void Save(RenderTarget2D renderTarget, Stream stream, Bitmap.CompressFormat format)
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
        private unsafe void Save(Color[] data, Int32 width, Int32 height, Stream stream, Bitmap.CompressFormat format)
        {
            using (var bmp = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888))
            {
                var bmpData = bmp.LockPixels();

                fixed (Color* pData = data)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var pSrc = pData + (y * width);
                        var pDst = (UInt32*)((Byte*)bmpData + (y * bmp.RowBytes));

                        for (int x = 0; x < width; x++)
                        {
                            *pDst++ = (*pSrc++).ToArgb();
                        }
                    }
                }

                bmp.UnlockPixels();
                bmp.Compress(format, 100, stream);
            }
        }
    }
}

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
    public sealed unsafe class AndroidSurfaceSaver : SurfaceSaver
    {
        /// <summary>
        /// Saves the specified surface as a PNG image.
        /// </summary>
        /// <param name="surface">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        public override void SaveAsPng(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, "surface");
            Contract.Require(stream, "stream");

            Save(surface, stream, Bitmap.CompressFormat.Png);
        }

        /// <summary>
        /// Saves the specified surface as a JPEG image.
        /// </summary>
        /// <param name="surface">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        public override void SaveAsJpeg(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, "surface");
            Contract.Require(stream, "stream");

            Save(surface, stream, Bitmap.CompressFormat.Jpeg);
        }

        /// <summary>
        /// Saves the specified surface as an image with the specified format.
        /// </summary>
        /// <param name="surface">The surface to save.</param>
        /// <param name="stream">The stream to which to save the surface data.</param>
        /// <param name="format">The format with which to save the image.</param>
        private void Save(Surface2D surface, Stream stream, Bitmap.CompressFormat format)
        {
            var data = new Color[surface.Width * surface.Height];
            surface.GetData(data);

            using (var bmp = Bitmap.CreateBitmap(surface.Width, surface.Height, Bitmap.Config.Argb8888))
            {
                var bmpData = bmp.LockPixels();

                fixed (Color* pData = data)
                {
                    for (int y = 0; y < surface.Height; y++)
                    {
                        var pSrc = pData + (y * surface.Width);
                        var pDst = (UInt32*)((Byte*)bmpData + (y * bmp.RowBytes));

                        for (int x = 0; x < surface.Width; x++)
                        {
                            *pDst++ = (*pSrc++).ToRgba();
                        }
                    }
                }

                bmp.UnlockPixels();
                bmp.Compress(format, 100, stream);
            }
        }
    }
}

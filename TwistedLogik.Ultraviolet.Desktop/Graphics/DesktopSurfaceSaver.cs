using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Graphics;
using GDIRect = System.Drawing.Rectangle;

namespace TwistedLogik.Ultraviolet.Desktop.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSaver"/> class for desktop platforms.
    /// </summary>
    public sealed unsafe class DesktopSurfaceSaver : SurfaceSaver
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

            Save(surface, stream, ImageFormat.Png);
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

            Save(surface, stream, ImageFormat.Jpeg);
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

            using (var bmp = new Bitmap(surface.Width, surface.Height))
            {
                var bmpData = bmp.LockBits(new GDIRect(0, 0, surface.Width, surface.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                fixed (Color* pData = data)
                {
                    for (int y = 0; y < surface.Height; y++)
                    {
                        var pSrc = pData + (y * surface.Width);
                        var pDst = (UInt32*)((Byte*)bmpData.Scan0 + (y * bmpData.Stride));

                        for (int x = 0; x < surface.Width; x++)
                        {
                            *pDst++ = (*pSrc++).ToRgba();
                        }
                    }
                }

                bmp.UnlockBits(bmpData);
                bmp.Save(stream, format);
            }
        }
    }
}

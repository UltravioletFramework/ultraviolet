using System;
using System.IO;
using AppKit;
using Foundation;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.Shims.macOS.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSaver"/> class for macOS.
    /// </summary>
    public sealed unsafe class macOSSurfaceSaver : SurfaceSaver
    {
        /// <inheritdoc/>
        public override void SaveAsPng(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, "surface");
            Contract.Require(stream, "stream");

            var data = new Color[surface.Width * surface.Height];
            surface.GetData(data);

            Save(data, surface.Width, surface.Height, stream, asPng: true);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, "surface");
            Contract.Require(stream, "stream");

            var data = new Color[surface.Width * surface.Height];
            surface.GetData(data);

            Save(data, surface.Width, surface.Height, stream, asPng: false);
        }

        /// <inheritdoc/>
        public override void SaveAsPng(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, "renderTarget");
            Contract.Require(stream, "stream");

            var data = new Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(data);

            Save(data, renderTarget.Width, renderTarget.Height, stream, asPng: true);
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, "renderTarget");
            Contract.Require(stream, "stream");

            var data = new Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(data);

            Save(data, renderTarget.Width, renderTarget.Height, stream, asPng: false);
        }

        /// <summary>
        /// Saves the specified color data as an image with the specified format.
        /// </summary>
        private void Save(Color[] data, Int32 width, Int32 height, Stream stream, Boolean asPng)
        {	
            using (var rep = new NSBitmapImageRep(IntPtr.Zero, width, height, 8, 4, true, false, "NSCalibratedRGBColorSpace", 0, 0)) 
            {
                fixed (Color* pData = data)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var pSrc = pData + (y * width);
                        var pDst = (Byte*)rep.BitmapData + (y * rep.BytesPerRow);

                        for (int x = 0; x < width; x++)
                        {
                            var color = *pSrc++;
                            *pDst++ = color.R;
                            *pDst++ = color.G;
                            *pDst++ = color.B;
                            *pDst++ = color.A;
                        }
                    }
                }

                var filetype = asPng ? NSBitmapImageFileType.Png : NSBitmapImageFileType.Jpeg;
                var properties = new NSDictionary();

                using (var imgData = rep.RepresentationUsingTypeProperties(filetype, properties))
                {
                    using (var imgStream = imgData.AsStream())
                    {
                        imgStream.CopyTo(stream);
                    }
                }
            }
        }
    }
}


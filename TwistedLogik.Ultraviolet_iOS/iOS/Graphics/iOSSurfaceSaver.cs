using System;
using System.IO;
using CoreGraphics;
using Foundation;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using UIKit;

namespace Ultraviolet.iOS.Graphics
{
    /// <summary>
    /// Represents an implementation of the <see cref="SurfaceSaver"/> class for the iOS platform.
    /// </summary>
    public sealed class iOSSurfaceSaver : SurfaceSaver
    {
        /// <inheritdoc/>
        public override void SaveAsPng(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, nameof(surface));
            Contract.Require(stream, nameof(stream));

            var data = new Color[surface.Width * surface.Height];
            surface.GetData(data);

            Save(data, surface.Width, surface.Height, stream, img => img.AsPNG());
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(Surface2D surface, Stream stream)
        {
            Contract.Require(surface, nameof(surface));
            Contract.Require(stream, nameof(stream));

            var data = new Color[surface.Width * surface.Height];
            surface.GetData(data);

            Save(data, surface.Width, surface.Height, stream, img => img.AsJPEG());
        }

        /// <inheritdoc/>
        public override void SaveAsPng(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, nameof(renderTarget));
            Contract.Require(stream, nameof(stream));

            var data = new Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(data);

            Save(data, renderTarget.Width, renderTarget.Height, stream, img => img.AsPNG());
        }

        /// <inheritdoc/>
        public override void SaveAsJpeg(RenderTarget2D renderTarget, Stream stream)
        {
            Contract.Require(renderTarget, nameof(renderTarget));
            Contract.Require(stream, nameof(stream));

            var data = new Color[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(data);

            Save(data, renderTarget.Width, renderTarget.Height, stream, img => img.AsJPEG());
        }

        /// <summary>
        /// Saves the specified color data as an image using the specified encoding.
        /// </summary>
        /// <param name="data">An array containing the image's color data.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="stream">The stream to which to save the image data.</param>
        /// <param name="encoder">A function which produces an encoded data stream for the image.</param>
        private unsafe void Save(Color[] data, Int32 width, Int32 height, Stream stream, Func<UIImage, NSData> encoder)
        {
            fixed (Color* pdata = data)
            {
                using (var cgColorSpace = CGColorSpace.CreateDeviceRGB())
                using (var cgDataProvider = new CGDataProvider((IntPtr)pdata, sizeof(Color) * width * height, false))
                using (var cgImage = new CGImage(width, height, 8, 32, width * 4, cgColorSpace, CGBitmapFlags.PremultipliedLast, cgDataProvider, null, false, CGColorRenderingIntent.Default))
                {
                    using (var img = UIImage.FromImage(cgImage))
                    {
                        using (var imgData = encoder(img))
                        using (var imgStream = imgData.AsStream())
                        {
                            imgStream.CopyTo(stream);
                        }
                    }
                }
            }
        }
    }
}

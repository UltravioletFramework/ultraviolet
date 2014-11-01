using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TwistedLogik.Ultraviolet.Testing
{
    /// <summary>
    /// Represents a unit test result containing a bitmap image.
    /// </summary>
    public sealed class BitmapResult
    {
        /// <summary>
        /// Initializes a new instance of the BitmapResult class.
        /// </summary>
        /// <param name="bitmap">The bitmap being examined.</param>
        internal BitmapResult(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified linear distance.
        /// </summary>
        /// <param name="distance">The distance value to set.</param>
        /// <returns>The result object.</returns>
        public BitmapResult WithinDistance(Single distance)
        {
            this.distance = distance;
            return this;
        }

        /// <summary>
        /// Asserts that the bitmap matches the image in the specified file.
        /// </summary>
        /// <param name="filename">The filename of the image to match against the bitmap.</param>
        public void ShouldMatch(String filename)
        {
            var expected = (Bitmap)Bitmap.FromFile(filename);
            if (expected.Width != bitmap.Width || expected.Height != bitmap.Height)
            {
                Assert.Fail("Images do not match");
            }

            for (int y = 0; y < expected.Height; y++)
            {
                for (int x = 0; x < expected.Width; x++)
                {
                    var pixelExpected = expected.GetPixel(x, y);
                    var pixelActual   = bitmap.GetPixel(x, y);
                    if (CalculateColorDistance(pixelExpected, pixelActual) > distance)
                    {
                        var filenameNoExtension = Path.GetFileNameWithoutExtension(filename);
                        var filenameActual = Path.ChangeExtension(filenameNoExtension + "_Failed", "png");
                        bitmap.Save(filenameActual, ImageFormat.Png);

                        Assert.Fail("Images do not match");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Bitmap Bitmap
        {
            get { return bitmap; }
        }

        /// <summary>
        /// Calculates the linear distance between two colors.
        /// </summary>
        /// <param name="c1">The first <see cref="Color"/> to compare.</param>
        /// <param name="c2">The second <see cref="Color"/> to compare.</param>
        /// <returns>The linear distance between the specified color.</returns>
        private static Single CalculateColorDistance(System.Drawing.Color c1, System.Drawing.Color c2)
        {
            return (Single)Math.Sqrt(
                Math.Pow(c1.A - c2.A, 2) +
                Math.Pow(c1.R - c2.R, 2) +
                Math.Pow(c1.G - c2.G, 2) +
                Math.Pow(c1.B - c2.B, 2)
            );
        }

        // State values.
        private readonly Bitmap bitmap;
        private Single distance = 5.0f;
    }
}

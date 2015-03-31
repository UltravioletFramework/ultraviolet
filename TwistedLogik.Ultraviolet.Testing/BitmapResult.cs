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
        /// Specifies that subsequent comparisons should have the specified threshold value.
        /// The threshold value is the number of pixels which must match the expected image
        /// in order for the images to be considered a match.
        /// </summary>
        /// <param name="threshold">The threshold value to set.</param>
        /// <returns>The result object.</returns>
        public BitmapResult WithinThreshold(Single threshold)
        {
            this.threshold = threshold;
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
                Assert.Fail("Images do not match due to differing dimensions");
            }

            var mismatchesFound    = 0;
            var mismatchesRequired = (Int32)((bitmap.Width * bitmap.Height) * threshold);

            using (var diff = new Bitmap(expected.Width, expected.Height))
            {
                for (int y = 0; y < expected.Height; y++)
                {
                    for (int x = 0; x < expected.Width; x++)
                    {
                        var pixelExpected = expected.GetPixel(x, y);
                        var pixelActual   = bitmap.GetPixel(x, y);

                        var diffR = pixelExpected.R + pixelActual.R - 2 * Math.Min(pixelExpected.R, pixelActual.R);
                        var diffG = pixelExpected.G + pixelActual.G - 2 * Math.Min(pixelExpected.G, pixelActual.G);
                        var diffB = pixelExpected.B + pixelActual.B - 2 * Math.Min(pixelExpected.B, pixelActual.B);

                        if (diffR != 0 || diffG != 0 || diffB != 0)
                        {
                            mismatchesFound++;
                        }

                        diff.SetPixel(x, y, System.Drawing.Color.FromArgb(255, diffR, diffG, diffB));
                    }
                }

                var filenameNoExtension = Path.GetFileNameWithoutExtension(filename);

                var filenameExpected = Path.ChangeExtension(filenameNoExtension + "_Expected", "png");
                expected.Save(filenameExpected);

                var filenameActual = Path.ChangeExtension(filenameNoExtension + "_Actual", "png");
                bitmap.Save(filenameActual, ImageFormat.Png);

                var filenameDiff = Path.ChangeExtension(filenameNoExtension + "_Diff", "png");
                diff.Save(filenameDiff, ImageFormat.Png);

                if (mismatchesFound >= mismatchesRequired)
                {
                    Assert.Fail("Images do not match");
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

        // State values.
        private readonly Bitmap bitmap;
        private Single threshold = 0.01f;
    }
}

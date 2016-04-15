using NUnit.Framework;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

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
        /// The threshold value is the percentage of pixels which must match the expected image
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
            var machineName = UltravioletTestFramework.GetSanitizedMachineName();
            Directory.CreateDirectory(machineName);

            var expected = (Bitmap)Image.FromFile(filename);

            var filenameNoExtension = Path.GetFileNameWithoutExtension(filename);

            var filenameExpected = Path.ChangeExtension(filenameNoExtension + "_Expected", "png");
            SaveBitmap(expected, Path.Combine(machineName, filenameExpected));

            var filenameActual = Path.ChangeExtension(filenameNoExtension + "_Actual", "png");
            SaveBitmap(bitmap, Path.Combine(machineName, filenameActual));

            if (expected.Width != bitmap.Width || expected.Height != bitmap.Height)
            {
                Assert.Fail("Images do not match due to differing dimensions");
            }

            var mismatchesFound    = 0;
            var mismatchesRequired = (Int32)((bitmap.Width * bitmap.Height) * threshold);

            using (var diff = new Bitmap(expected.Width, expected.Height))
            {
                // Ignore pixels that are within about 1% of the expected value.
                const Int32 PixelDiffThreshold = 2;

                for (int y = 0; y < expected.Height; y++)
                {
                    for (int x = 0; x < expected.Width; x++)
                    {
                        var pixelExpected = expected.GetPixel(x, y);
                        var pixelActual   = bitmap.GetPixel(x, y);

                        var diffR = Math.Abs(pixelExpected.R + pixelActual.R - 2 * Math.Min(pixelExpected.R, pixelActual.R));
                        var diffG = Math.Abs(pixelExpected.G + pixelActual.G - 2 * Math.Min(pixelExpected.G, pixelActual.G));
                        var diffB = Math.Abs(pixelExpected.B + pixelActual.B - 2 * Math.Min(pixelExpected.B, pixelActual.B));

                        if (diffR > PixelDiffThreshold || diffG > PixelDiffThreshold || diffB > PixelDiffThreshold)
                        {
                            mismatchesFound++;
                        }

                        diff.SetPixel(x, y, System.Drawing.Color.FromArgb(255, diffR, diffG, diffB));
                    }
                }

                var filenameDiff = Path.ChangeExtension(filenameNoExtension + "_Diff", "png");
                SaveBitmap(diff, Path.Combine(machineName, filenameDiff));

                if (mismatchesFound > mismatchesRequired)
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

        /// <summary>
        /// Saves a bitmap to thhe specified file.
        /// </summary>
        private void SaveBitmap(Bitmap bitmap, String filename)
        {
            // NOTE: We first open a FileStream because it gives us potentially more
            // useful exception information than "A generic error occurred in GDI+".
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                bitmap.Save(fs, ImageFormat.Png);
            }
        }

        // State values.
        private readonly Bitmap bitmap;
        private Single threshold = 0.0f;
    }
}

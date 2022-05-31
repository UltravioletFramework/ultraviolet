using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.TestApplication
{
    /// <summary>
    /// Represents a unit test result containing a bitmap image.
    /// </summary>
    public sealed class BitmapResult
    {
        /// <summary>
        /// Represents the type of threshold that is used to compare bitmap results to expected images.
        /// </summary>
        private enum BitmapResultThresholdType { Percentage, Count };

        /// <summary>
        /// Initializes a new instance of the BitmapResult class.
        /// </summary>
        /// <param name="bitmap">The bitmap being examined.</param>
        internal BitmapResult(StbImageSharp.ImageResult bitmap)
        {
            this.Bitmap = bitmap;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should have the specified percentage threshold value.
        /// The threshold value is the percentage of pixels which must differ from the expected image
        /// in order for the images to be considered not a match.
        /// </summary>
        /// <param name="threshold">The threshold value to set.</param>
        /// <returns>The result object.</returns>
        public BitmapResult WithinPercentageThreshold(Single threshold)
        {
            this.thresholdType = BitmapResultThresholdType.Percentage;
            this.threshold = threshold;
            return this;
        }

        /// <summary>
        /// Specifies the subsequent comparisons should have the specified absolute threshold value.
        /// The threshold value is the number of pixels which must differ from the expected image
        /// in order for the images to be considered not a match.
        /// </summary>
        /// <param name="threshold">The threshold value to set.</param>
        /// <returns>The result object.</returns>
        public BitmapResult WithinAbsoluteThreshold(Int32 threshold)
        {
            this.thresholdType = BitmapResultThresholdType.Count;
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

            var fileStream = File.Open(filename, FileMode.Open);

            var expected = StbImageSharp.ImageResult.FromStream(fileStream, StbImageSharp.ColorComponents.RedGreenBlueAlpha);

            var filenameNoExtension = Path.GetFileNameWithoutExtension(filename);

            var filenameExpected = Path.ChangeExtension(filenameNoExtension + "_Expected", "png");
            SaveBitmap(expected, Path.Combine(machineName, filenameExpected));

            var filenameActual = Path.ChangeExtension(filenameNoExtension + "_Actual", "png");
            SaveBitmap(Bitmap, Path.Combine(machineName, filenameActual));

            if (expected.Width != Bitmap.Width || expected.Height != Bitmap.Height)
            {
                Assert.Fail("Images do not match due to differing dimensions");
            }

            var mismatchesFound = 0;
            var mismatchesRequired = (thresholdType == BitmapResultThresholdType.Percentage) ?
                (Int32)((Bitmap.Width * Bitmap.Height) * threshold) : (Int32)threshold;

            var diff = new StbImageSharp.ImageResult() { 
                Width = expected.Width, 
                Height = expected.Height, 
                Comp = StbImageSharp.ColorComponents.RedGreenBlueAlpha, 
                Data = new byte[expected.Width * expected.Height * 4] 
            };

            {
                // Ignore pixels that are within about 1% of the expected value.
                const Int32 PixelDiffThreshold = 2;

                for (int y = 0; y < expected.Height; y++)
                {
                    for (int x = 0; x < expected.Width; x++)
                    {
                        expected.GetPixel(x, y, out Pixel4 pixelExpected);
                        Bitmap.GetPixel(x, y, out Pixel4 pixelActual);

                        var diffR = Math.Abs(pixelExpected.R + pixelActual.R - 2 * Math.Min(pixelExpected.R, pixelActual.R));
                        var diffG = Math.Abs(pixelExpected.G + pixelActual.G - 2 * Math.Min(pixelExpected.G, pixelActual.G));
                        var diffB = Math.Abs(pixelExpected.B + pixelActual.B - 2 * Math.Min(pixelExpected.B, pixelActual.B));

                        if (diffR > PixelDiffThreshold || diffG > PixelDiffThreshold || diffB > PixelDiffThreshold)
                        {
                            mismatchesFound++;
                        }

                        diff.SetPixel(x, y, (byte)diffR, (byte)diffG, (byte)diffB, 255);
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
        //public Bitmap Bitmap { get; }
        public StbImageSharp.ImageResult Bitmap { get; }

        /// <summary>
        /// Saves a bitmap to thhe specified file.
        /// </summary>
        private void SaveBitmap(StbImageSharp.ImageResult bitmap, String filename)
        {
            // NOTE: We first open a FileStream because it gives us potentially more
            // useful exception information than "A generic error occurred in GDI+".
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                StbImageWriteSharp.ImageWriter writer = new StbImageWriteSharp.ImageWriter();
                writer.WritePng(bitmap.Data, bitmap.Width, bitmap.Height, StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, fs);
            }
        }

        // Image comparison threshold.
        private BitmapResultThresholdType thresholdType = BitmapResultThresholdType.Percentage;
        private Single threshold = 0.0f;
    }
}

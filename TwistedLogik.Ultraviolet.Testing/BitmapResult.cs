using System;
using System.Drawing;
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
                    if (pixelExpected != pixelActual)
                    {
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

        // State values.
        private readonly Bitmap bitmap;
    }
}

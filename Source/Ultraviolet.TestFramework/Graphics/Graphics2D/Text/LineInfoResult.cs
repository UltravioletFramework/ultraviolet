using System;
using NUnit.Framework;
using Ultraviolet.Graphics.Graphics2D.Text;

namespace Ultraviolet.TestFramework.Graphics.Graphics2D.Text
{
    /// <summary>
    /// Represents a unit test result containing a <see cref="LineInfo"/> structure.
    /// </summary>
    public sealed class LineInfoResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineInfoResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        public LineInfoResult(LineInfo value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that the line should have the specified parameters
        /// </summary>
        /// <param name="x">The expected x-coordinate of the line's top-left corner.</param>
        /// <param name="y">The expected y-coordinate of the line's top-left corner.</param>
        /// <param name="width">The expected width of the line in pixels.</param>
        /// <param name="height">The expected height of the line in pixels.</param>
        /// <param name="lengthInCommands">The expected length of the line in commands.</param>
        /// <param name="lengthInGlyphs">The expected length of the line in glyphs.</param>
        /// <returns>The result object.</returns>
        public LineInfoResult ShouldBe(Int32 x, Int32 y, Int32 width, Int32 height, Int32 lengthInCommands, Int32 lengthInGlyphs)
        {
            return this
                .ShouldHavePosition(x, y)
                .ShouldHaveSize(width, height)
                .ShouldHaveLengthInCommands(lengthInCommands)
                .ShouldHaveLengthInGlyphs(lengthInGlyphs);
        }

        /// <summary>
        /// Asserts that the line should have the specified position.
        /// </summary>
        /// <param name="x">The expected x-coordinate of the line's top-left corner.</param>
        /// <param name="y">The expected y-coordinate of the line's top-left corner.</param>
        /// <returns>The result object.</returns>
        public LineInfoResult ShouldHavePosition(Int32 x, Int32 y)
        {
            Assert.AreEqual(x, value.X, "Position does not match.");
            Assert.AreEqual(y, value.Y, "Position does not match.");
            return this;
        }

        /// <summary>
        /// Asserts that the line should have the specified size.
        /// </summary>
        /// <param name="width">The expected width of the line in pixels.</param>
        /// <param name="height">The expected height of the line in pixels.</param>
        /// <returns>The result object.</returns>
        public LineInfoResult ShouldHaveSize(Int32 width, Int32 height)
        {
            Assert.AreEqual(width, value.Width, "Size does not match.");
            Assert.AreEqual(height, value.Height, "Size does not match.");
            return this;
        }

        /// <summary>
        /// Asserts that the line should have the specified length in commands.
        /// </summary>
        /// <param name="length">The expected length of the line in commands.</param>
        /// <returns>The result object.</returns>
        public LineInfoResult ShouldHaveLengthInCommands(Int32 length)
        {
            Assert.AreEqual(length, value.LengthInCommands, "Length in commands does not match.");
            return this;
        }

        /// <summary>
        /// Asserts that the line should have the specified length in source characters.
        /// </summary>
        /// <param name="length">The expected length of the line in source characters.</param>
        /// <returns>The result object.</returns>
        public LineInfoResult ShouldHaveLengthInSource(Int32 length)
        {
            Assert.AreEqual(length, value.LengthInSource, "Length in source characters does not match.");
            return this;
        }

        /// <summary>
        /// Asserts that the line should have the specified length in glyphs.
        /// </summary>
        /// <param name="length">The expected length of the line in glyphs.</param>
        /// <returns>The result object.</returns>
        public LineInfoResult ShouldHaveLengthInGlyphs(Int32 length)
        {
            Assert.AreEqual(length, value.LengthInGlyphs, "Length in glyphs does not match.");
            return this;
        }

        // State values.
        private LineInfo value;
    }
}

using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a rectangle value.
    /// </summary>
    public sealed class RectangleResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal RectangleResult(Rectangle value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should have the specified position.
        /// </summary>
        /// <param name="x">The expected x-coordinate of the rectangle's top-left corner.</param>
        /// <param name="y">The expected y-coordinate of the rectangle's top-left corner.</param>
        /// <returns>The result object.</returns>
        public RectangleResult ShouldHavePosition(Int32 x, Int32 y)
        {
            Assert.AreEqual(x, value.X);
            Assert.AreEqual(y, value.Y);
            return this;
        }

        /// <summary>
        /// Asserts that this value should have the specified dimensions.
        /// </summary>
        /// <param name="width">The expected width of the rectangle.</param>
        /// <param name="height">The expected height of the rectangle.</param>
        /// <returns>The result object.</returns>
        public RectangleResult ShouldHaveDimensions(Int32 width, Int32 height)
        {
            Assert.AreEqual(width, value.Width);
            Assert.AreEqual(height, value.Height);
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Rectangle Value
        {
            get { return value; }
        }

        // State values.
        private readonly Rectangle value;
    }
}

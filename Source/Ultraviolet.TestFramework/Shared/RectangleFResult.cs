using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a floating point rectangle value.
    /// </summary>
    public sealed class RectangleFResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleFResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal RectangleFResult(RectangleF value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should have the specified position.
        /// </summary>
        /// <param name="x">The expected x-coordinate of the rectangle's top-left corner.</param>
        /// <param name="y">The expected y-coordinate of the rectangle's top-left corner.</param>
        /// <returns>The result object.</returns>
        public RectangleFResult ShouldHavePosition(Single x, Single y)
        {
            Assert.AreEqual(x, value.X, delta);
            Assert.AreEqual(y, value.Y, delta);
            return this;
        }

        /// <summary>
        /// Asserts that this value should have the specified dimensions.
        /// </summary>
        /// <param name="width">The expected width of the rectangle.</param>
        /// <param name="height">The expected height of the rectangle.</param>
        /// <returns>The result object.</returns>
        public RectangleFResult ShouldHaveDimensions(Single width, Single height)
        {
            Assert.AreEqual(width, value.Width, delta);
            Assert.AreEqual(height, value.Height, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public RectangleFResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public RectangleF Value
        {
            get { return value; }
        }

        // State values.
        private readonly RectangleF value;
        private Single delta = 0.001f;
    }
}

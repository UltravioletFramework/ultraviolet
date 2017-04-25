using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a double-precision floating point rectangle value.
    /// </summary>
    public sealed class RectangleDResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleDResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal RectangleDResult(RectangleD value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should have the specified position.
        /// </summary>
        /// <param name="x">The expected x-coordinate of the rectangle's top-left corner.</param>
        /// <param name="y">The expected y-coordinate of the rectangle's top-left corner.</param>
        /// <returns>The result object.</returns>
        public RectangleDResult ShouldHavePosition(Double x, Double y)
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
        public RectangleDResult ShouldHaveDimensions(Double width, Double height)
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
        public RectangleDResult WithinDelta(Double delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public RectangleD Value
        {
            get { return value; }
        }

        // State values.
        private readonly RectangleD value;
        private Double delta = 0.001;
    }
}

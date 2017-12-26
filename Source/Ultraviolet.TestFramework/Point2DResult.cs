using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a two-dimensional point value with floating point components.
    /// </summary>
    public sealed class Point2DResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2FResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Point2DResult(Point2D value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this point should have the specified values.
        /// </summary>
        /// <param name="x">The expected x-coordinate.</param>
        /// <param name="y">The expected y-coordinate.</param>
        /// <returns>The result object.</returns>
        public Point2DResult ShouldBe(Double x, Double y)
        {
            Assert.AreEqual(x, value.X, delta);
            Assert.AreEqual(y, value.Y, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public Point2DResult WithinDelta(Double delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Point2D Value
        {
            get { return value; }
        }

        // State values.
        private readonly Point2D value;
        private Double delta = 0.001;
    }
}

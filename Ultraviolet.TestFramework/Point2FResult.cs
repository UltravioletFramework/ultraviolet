using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a two-dimensional point value with floating point components.
    /// </summary>
    public sealed class Point2FResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2FResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Point2FResult(Point2F value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this point should have the specified values.
        /// </summary>
        /// <param name="x">The expected x-coordinate.</param>
        /// <param name="y">The expected y-coordinate.</param>
        /// <returns>The result object.</returns>
        public Point2FResult ShouldBe(Single x, Single y)
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
        public Point2FResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Point2F Value
        {
            get { return value; }
        }

        // State values.
        private readonly Point2F value;
        private Single delta = 0.001f;
    }
}

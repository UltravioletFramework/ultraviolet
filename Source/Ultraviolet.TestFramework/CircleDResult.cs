using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a double-precision floating-point circle value.
    /// </summary>
    public sealed class CircleDResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleDResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal CircleDResult(CircleD value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should have the specified position.
        /// </summary>
        /// <param name="x">The expected x-coordinate of the circle's center.</param>
        /// <param name="y">The expected y-coordinate of the circle's center.</param>
        /// <returns>The result object.</returns>
        public CircleDResult ShouldHavePosition(Double x, Double y)
        {
            Assert.AreEqual(x, value.X, delta);
            Assert.AreEqual(y, value.Y, delta);
            return this;
        }

        /// <summary>
        /// Asserts that this value should have the specified radius.
        /// </summary>
        /// <param name="radius">The circle's expected radius.</param>
        /// <returns>The result object.</returns>
        public CircleDResult ShouldHaveRadius(Double radius)
        {
            Assert.AreEqual(radius, value.Radius, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public CircleDResult WithinDelta(Double delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public CircleD Value
        {
            get { return value; }
        }

        // State values.
        private CircleD value;
        private Double delta = 0.001;
    }
}

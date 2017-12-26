using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a floating-point circle value.
    /// </summary>
    public sealed class CircleFResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleFResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal CircleFResult(CircleF value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should have the specified position.
        /// </summary>
        /// <param name="x">The expected x-coordinate of the circle's center.</param>
        /// <param name="y">The expected y-coordinate of the circle's center.</param>
        /// <returns>The result object.</returns>
        public CircleFResult ShouldHavePosition(Single x, Single y)
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
        public CircleFResult ShouldHaveRadius(Single radius)
        {
            Assert.AreEqual(radius, value.Radius, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public CircleFResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public CircleF Value
        {
            get { return value; }
        }

        // State values.
        private CircleF value;
        private Single delta = 0.001f;
    }
}

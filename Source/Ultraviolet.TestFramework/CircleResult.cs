using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a circle value.
    /// </summary>
    public sealed class CircleResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircleResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal CircleResult(Circle value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should have the specified position.
        /// </summary>
        /// <param name="x">The expected x-coordinate of the circle's center.</param>
        /// <param name="y">The expected y-coordinate of the circle's center.</param>
        /// <returns>The result object.</returns>
        public CircleResult ShouldHavePosition(Int32 x, Int32 y)
        {
            Assert.AreEqual(x, value.X);
            Assert.AreEqual(y, value.Y);
            return this;
        }

        /// <summary>
        /// Asserts that this value should have the specified radius.
        /// </summary>
        /// <param name="radius">The circle's expected radius.</param>
        /// <returns>The result object.</returns>
        public CircleResult ShouldHaveRadius(Int32 radius)
        {
            Assert.AreEqual(radius, value.Radius);
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Circle Value
        {
            get { return value; }
        }

        // State values.
        private readonly Circle value;
    }
}

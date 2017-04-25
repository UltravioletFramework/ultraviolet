using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a two-dimensional vector value.
    /// </summary>
    public sealed class Vector2Result
    {
        /// <summary>
        /// Initializes a new instance of the Vector2Result class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Vector2Result(Vector2 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this vector should have the specified components.
        /// </summary>
        /// <param name="x">The expected x-component.</param>
        /// <param name="y">The expected y-component.</param>
        /// <returns>The result object.</returns>
        public Vector2Result ShouldBe(Single x, Single y)
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
        public Vector2Result WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Vector2 Value
        {
            get { return value; }
        }

        // State values.
        private readonly Vector2 value;
        private Single delta = 0.001f;
    }
}

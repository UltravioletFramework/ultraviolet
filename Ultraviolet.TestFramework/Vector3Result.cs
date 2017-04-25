using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a three-dimensional vector value.
    /// </summary>
    public sealed class Vector3Result 
    {
        /// <summary>
        /// Initializes a new instance of the Vector3Result class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Vector3Result(Vector3 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this vector should have the specified components.
        /// </summary>
        /// <param name="x">The expected x-component.</param>
        /// <param name="y">The expected y-component.</param>
        /// <param name="z">The expected z-component.</param>
        /// <returns>The result object.</returns>
        public Vector3Result ShouldBe(Single x, Single y, Single z)
        {
            Assert.AreEqual(x, Value.X, delta);
            Assert.AreEqual(y, Value.Y, delta);
            Assert.AreEqual(z, Value.Z, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public Vector3Result WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Vector3 Value
        {
            get { return value; }
        }

        // State values.
        private readonly Vector3 value;
        private Single delta = 0.001f;
    }
}

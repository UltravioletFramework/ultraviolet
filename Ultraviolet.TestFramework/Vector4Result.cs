using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a four-dimensional vector value.
    /// </summary>
    public sealed class Vector4Result
    {
        /// <summary>
        /// Initializes a new instance of the Vector4Result class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Vector4Result(Vector4 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this vector should have the specified components.
        /// </summary>
        /// <param name="x">The expected x-component.</param>
        /// <param name="y">The expected y-component.</param>
        /// <param name="z">The expected z-component.</param>
        /// <param name="w">The expected w-component.</param>
        /// <returns>The result object.</returns>
        public Vector4Result ShouldBe(Single x, Single y, Single z, Single w)
        {
            Assert.AreEqual(x, value.X, delta);
            Assert.AreEqual(y, value.Y, delta);
            Assert.AreEqual(z, value.Z, delta);
            Assert.AreEqual(w, value.W, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public Vector4Result WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Vector4 Value
        {
            get { return value; }
        }

        // State values.
        private readonly Vector4 value;
        private Single delta = 0.001f;
    }
}

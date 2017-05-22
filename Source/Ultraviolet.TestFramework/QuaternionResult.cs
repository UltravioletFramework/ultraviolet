using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a quaternion value.
    /// </summary>
    public sealed class QuaternionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal QuaternionResult(Quaternion value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this quaternion should have the specified components.
        /// </summary>
        /// <param name="x">The expected x-component.</param>
        /// <param name="y">The expected y-component.</param>
        /// <param name="z">The expected z-component.</param>
        /// <param name="w">The expected w-component.</param>
        /// <returns>The result object.</returns>
        public QuaternionResult ShouldBe(Single x, Single y, Single z, Single w)
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
        public QuaternionResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Quaternion Value
        {
            get { return value; }
        }

        // State values.
        private readonly Quaternion value;
        private Single delta = 0.001f;
    }
}

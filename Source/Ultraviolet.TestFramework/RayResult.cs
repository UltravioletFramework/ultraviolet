using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a ray value.
    /// </summary>
    public sealed class RayResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RayResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal RayResult(Ray value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this ray should have the specified position.
        /// </summary>
        /// <param name="x">The expected x-component of the ray's position.</param>
        /// <param name="y">The expected y-component of the ray's position.</param>
        /// <param name="z">The expected z-component of the ray's position.</param>
        /// <returns>The result object.</returns>
        public RayResult ShouldHavePosition(Single x, Single y, Single z)
        {
            Assert.AreEqual(x, value.Position.X, delta);
            Assert.AreEqual(y, value.Position.Y, delta);
            Assert.AreEqual(z, value.Position.Z, delta);
            return this;
        }

        /// <summary>
        /// Asserts that this ray should have the specified direction.
        /// </summary>
        /// <param name="x">The expected x-component of the ray's direction.</param>
        /// <param name="y">The expected y-component of the ray's direction.</param>
        /// <param name="z">The expected z-component of the ray's direction.</param>
        /// <returns>The result object.</returns>
        public RayResult ShouldHaveDirection(Single x, Single y, Single z)
        {
            Assert.AreEqual(x, value.Direction.X, delta);
            Assert.AreEqual(y, value.Direction.Y, delta);
            Assert.AreEqual(z, value.Direction.Z, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public RayResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Ray Value
        {
            get { return value; }
        }

        // State values.
        private readonly Ray value;
        private Single delta = 0.001f;
    }
}

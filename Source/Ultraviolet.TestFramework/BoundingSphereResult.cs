using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a bounding sphere value.
    /// </summary>
    public sealed class BoundingSphereResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingSphereResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal BoundingSphereResult(BoundingSphere value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this bounding sphere should have the specified center point.
        /// </summary>
        /// <param name="x">The expected x-component of the sphere's center point.</param>
        /// <param name="y">The expected y-component of the sphere's center point.</param>
        /// <param name="z">The expected z-component of the sphere's center point.</param>
        /// <returns>The result object.</returns>
        public BoundingSphereResult ShouldHaveCenter(Single x, Single y, Single z)
        {
            Assert.AreEqual(x, value.Center.X, delta);
            Assert.AreEqual(y, value.Center.Y, delta);
            Assert.AreEqual(z, value.Center.Z, delta);
            return this;
        }

        /// <summary>
        /// Asserts that this bounding sphere should have the specified radius.
        /// </summary>
        /// <param name="radius">The expected radius.</param>
        /// <returns>The result object.</returns>
        public BoundingSphereResult ShouldHaveRadius(Single radius)
        {
            Assert.AreEqual(radius, value.Radius, delta);
            return this;
        }
  
        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public BoundingSphereResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public BoundingSphere Value
        {
            get { return value; }
        }

        // State values.
        private readonly BoundingSphere value;
        private Single delta = 0.001f;
    }
}

using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a plane value.
    /// </summary>
    public sealed class PlaneResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal PlaneResult(Plane value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should have the specified normal vector.
        /// </summary>
        /// <param name="x">The expected x-coordinate of the plane's normal.</param>
        /// <param name="y">The expected y-coordinate of the plane's normal.</param>
        /// <param name="z">The expected z-coordinate of the plane's normal.</param>
        /// <returns>The result object.</returns>
        public PlaneResult ShouldHaveNormal(Single x, Single y, Single z)
        {
            Assert.AreEqual(x, value.Normal.X, delta);
            Assert.AreEqual(y, value.Normal.Y, delta);
            Assert.AreEqual(z, value.Normal.Z, delta);
            return this;
        }

        /// <summary>
        /// Asserts that this value should have the specified normal vector.
        /// </summary>
        /// <param name="normal">The expected coordinates of the plane's normal.</param>
        /// <returns>The result object.</returns>
        public PlaneResult ShouldHaveNormal(Vector3 normal)
        {
            Assert.AreEqual(normal.X, value.Normal.X, delta);
            Assert.AreEqual(normal.Y, value.Normal.Y, delta);
            Assert.AreEqual(normal.Z, value.Normal.Z, delta);
            return this;
        }

        /// <summary>
        /// Asserts that this value should have the specified distance from the origin.
        /// </summary>
        /// <param name="d">The plane's expected distance from the origin.</param>
        /// <returns>The result object.</returns>
        public PlaneResult ShouldHaveDistance(Single d)
        {
            Assert.AreEqual(d, value.D, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public PlaneResult WithinDelta(Double delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Plane Value
        {
            get { return value; }
        }

        // State values.
        private Plane value;
        private Double delta = 0.001;
    }
}

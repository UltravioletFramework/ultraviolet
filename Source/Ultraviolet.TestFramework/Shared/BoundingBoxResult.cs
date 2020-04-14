using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a bounding box value.
    /// </summary>
    public sealed class BoundingBoxResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBoxResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal BoundingBoxResult(BoundingBox value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this bounding box should have the specified minimum point.
        /// </summary>
        /// <param name="x">The expected x-component of the box's minimum point.</param>
        /// <param name="y">The expected y-component of the box's minimum point.</param>
        /// <param name="z">The expected z-component of the box's minimum point.</param>
        /// <returns>The result object.</returns>
        public BoundingBoxResult ShouldHaveMin(Single x, Single y, Single z)
        {
            Assert.AreEqual(x, value.Min.X, delta);
            Assert.AreEqual(y, value.Min.Y, delta);
            Assert.AreEqual(z, value.Min.Z, delta);
            return this;
        }

        /// <summary>
        /// Asserts that this bounding box should have the specified maximum point.
        /// </summary>
        /// <param name="x">The expected x-component of the box's maximum point.</param>
        /// <param name="y">The expected y-component of the box's maximum point.</param>
        /// <param name="z">The expected z-component of the box's maximum point.</param>
        /// <returns>The result object.</returns>
        public BoundingBoxResult ShouldHaveMax(Single x, Single y, Single z)
        {
            Assert.AreEqual(x, value.Max.X, delta);
            Assert.AreEqual(y, value.Max.Y, delta);
            Assert.AreEqual(z, value.Max.Z, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public BoundingBoxResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public BoundingBox Value
        {
            get { return value; }
        }

        // State values.
        private readonly BoundingBox value;
        private Single delta = 0.001f;
    }
}

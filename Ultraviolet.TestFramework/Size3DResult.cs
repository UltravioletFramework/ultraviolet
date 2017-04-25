using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a three-dimensional floating point size value.
    /// </summary>
    public sealed class Size3DResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size3DResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Size3DResult(Size3D value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this size should have the specified values.
        /// </summary>
        /// <param name="width">The expected width.</param>
        /// <param name="height">The expected height.</param>
        /// <param name="depth">The expected depth.</param>
        /// <returns>The result object.</returns>
        public Size3DResult ShouldBe(Double width, Double height, Double depth)
        {
            Assert.AreEqual(width, Value.Width, delta);
            Assert.AreEqual(height, Value.Height, delta);
            Assert.AreEqual(depth, Value.Depth, delta);
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public Size3DResult WithinDelta(Double delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Size3D Value
        {
            get { return value; }
        }

        // State values.
        private readonly Size3D value;
        private Double delta = 0.001f;
    }
}

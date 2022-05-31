using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a three-dimensional floating point size value.
    /// </summary>
    public sealed class Size3FResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size3FResult"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Size3FResult(Size3F value)
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
        public Size3FResult ShouldBe(Single width, Single height, Single depth)
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
        public Size3FResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Size3F Value
        {
            get { return value; }
        }

        // State values.
        private readonly Size3F value;
        private Single delta = 0.001f;
    }
}

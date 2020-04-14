using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a three-dimensional size value.
    /// </summary>
    public sealed class Size3Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Size3Result"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Size3Result(Size3 value)
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
        public Size3Result ShouldBe(Int32 width, Int32 height, Int32 depth)
        {
            Assert.AreEqual(width, value.Width);
            Assert.AreEqual(height, value.Height);
            Assert.AreEqual(depth, value.Depth);
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Size3 Value
        {
            get { return value; }
        }

        // State values.
        private readonly Size3 value;
    }
}

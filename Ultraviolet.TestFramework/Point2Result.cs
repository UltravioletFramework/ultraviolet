using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a two-dimensional point value.
    /// </summary>
    public sealed class Point2Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point2Result"/> class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal Point2Result(Point2 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this point should have the specified values.
        /// </summary>
        /// <param name="x">The expected x-coordinate.</param>
        /// <param name="y">The expected y-coordinate.</param>
        /// <returns>The result object.</returns>
        public Point2Result ShouldBe(Int32 x, Int32 y)
        {
            Assert.AreEqual(x, value.X);
            Assert.AreEqual(y, value.Y);
            return this;
        }
        
        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Point2 Value
        {
            get { return value; }
        }

        // State values.
        private readonly Point2 value;
    }
}

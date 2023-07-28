using System;
using NUnit.Framework;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a 32-bit floating point value.
    /// </summary>
    public sealed class SingleResult
    {
        /// <summary>
        /// Initializes a new instance of the SingleResult class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal SingleResult(Single value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that the value satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the value.</param>
        /// <returns>The result object.</returns>
        public SingleResult ShouldSatisfyTheCondition(Func<Single, Boolean> condition)
        {
            Assert.IsTrue(condition(value));
            return this;
        }

        /// <summary>
        /// Asserts that this value should be equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The result object.</returns>
        public SingleResult ShouldBe(Single expected)
        {
            if (delta.HasValue)
            {
                Assert.That(value, Is.EqualTo(expected).Within(delta));
            }
            else
            {
                Assert.That(value, Is.EqualTo(expected));
            }
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public SingleResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Single Value
        {
            get { return value; }
        }

        // State values.
        private Single value;
        private Single? delta;
    }
}

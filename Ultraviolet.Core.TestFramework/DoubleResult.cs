using System;
using NUnit.Framework;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a 32-bit floating point value.
    /// </summary>
    public sealed class DoubleResult
    {
        /// <summary>
        /// Initializes a new instance of the DoubleResult class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal DoubleResult(Double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that the value satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the value.</param>
        /// <returns>The result object.</returns>
        public DoubleResult ShouldSatisfyTheCondition(Func<Double, Boolean> condition)
        {
            Assert.IsTrue(condition(value));
            return this;
        }

        /// <summary>
        /// Asserts that this value should be equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The result object.</returns>
        public DoubleResult ShouldBe(Double expected)
        {
            if (delta.HasValue)
            {
                Assert.AreEqual(expected, value, delta.Value);
            }
            else
            {
                Assert.AreEqual(expected, value);
            }
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public DoubleResult WithinDelta(Double delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Double Value
        {
            get { return value; }
        }

        // State values.
        private Double value;
        private Double? delta;
    }
}

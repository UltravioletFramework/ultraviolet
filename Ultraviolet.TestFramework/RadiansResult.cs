using System;
using NUnit.Framework;

namespace Ultraviolet.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing an angle in radians.
    /// </summary>
    public sealed class RadiansResult
    {
        /// <summary>
        /// Initializes a new instance of the RadiansResult class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal RadiansResult(Radians value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that the value satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the value.</param>
        /// <returns>The result object.</returns>
        public RadiansResult ShouldSatisfyTheCondition(Func<Radians, Boolean> condition)
        {
            Assert.IsTrue(condition(value));
            return this;
        }

        /// <summary>
        /// Asserts that this value should be equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The result object.</returns>
        public RadiansResult ShouldBe(Single expected)
        {
            if (delta.HasValue)
            {
                Assert.That(expected, Is.EqualTo(value.Value).Within(delta.Value));
            }
            else
            {
                Assert.That(expected, Is.EqualTo(value.Value));
            }
            return this;
        }

        /// <summary>
        /// Specifies that subsequent comparisons should be within the specified delta.
        /// </summary>
        /// <param name="delta">The delta value to set.</param>
        /// <returns>The result object.</returns>
        public RadiansResult WithinDelta(Single delta)
        {
            this.delta = delta;
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Radians Value
        {
            get { return value; }
        }

        // State values.
        private Radians value;
        private Single? delta;
    }
}

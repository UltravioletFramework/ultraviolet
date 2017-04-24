using System;
using NUnit.Framework;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a value.
    /// </summary>
    public sealed class ValueResult<T> where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the ValueResult class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal ValueResult(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that the value satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the value.</param>
        /// <returns>The result object.</returns>
        public ValueResult<T> ShouldSatisfyTheCondition(Func<T, Boolean> condition)
        {
            Assert.IsTrue(condition(value));
            return this;
        }

        /// <summary>
        /// Asserts that this value should be equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The result object.</returns>
        public ValueResult<T> ShouldBe(T expected)
        {
            Assert.AreEqual(expected, value);
            return this;
        }

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public T Value
        {
            get { return value; }
        }

        // Property values.
        private readonly T value;
    }
}

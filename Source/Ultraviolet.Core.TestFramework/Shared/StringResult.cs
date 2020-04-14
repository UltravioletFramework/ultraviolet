using System;
using NUnit.Framework;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a localized string.
    /// </summary>
    public sealed class StringResult
    {
        /// <summary>
        /// Initializes a new instance of the StringResult class.
        /// </summary>
        /// <param name="str">The string being examined.</param>
        internal StringResult(String str)
        {
            this.str = str;
        }

        /// <summary>
        /// Asserts that the string satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the string.</param>
        /// <returns>The result object.</returns>
        public StringResult ShouldSatisfyTheCondition(Func<String, Boolean> condition)
        {
            Assert.IsTrue(condition(str));
            return this;
        }

        /// <summary>
        /// Asserts that this string should be equal to the expected string.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <returns>The result object.</returns>
        public StringResult ShouldBe(String expected)
        {
            Assert.AreEqual(expected, str);
            return this;
        }

        /// <summary>
        /// Asserts that this string should be null.
        /// </summary>
        /// <returns>The result object.</returns>
        public StringResult ShouldBeNull()
        {
            Assert.IsNull(str);
            return this;
        }

        /// <summary>
        /// Asserts that this string should not be null.
        /// </summary>
        /// <returns>The result object.</returns>
        public StringResult ShouldNotBeNull()
        {
            Assert.IsNotNull(str);
            return this;
        }

        /// <summary>
        /// Gets the underlying string.
        /// </summary>
        public String String
        {
            get { return str; }
        }

        // Property values.
        private readonly String str;
    }
}

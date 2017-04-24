using System;
using NUnit.Framework;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a localized string.
    /// </summary>
    public sealed class LocalizedStringResult
    {
        /// <summary>
        /// Initializes a new instance of the LocalizedStringResult class.
        /// </summary>
        /// <param name="str">The string being examined.</param>
        internal LocalizedStringResult(LocalizedString str)
        {
            this.str = str;
        }

        /// <summary>
        /// Asserts that the string satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the string.</param>
        /// <returns>The result object.</returns>
        public LocalizedStringResult ShouldSatisfyTheCondition(Func<LocalizedString, Boolean> condition)
        {
            Assert.IsTrue(condition(str));
            return this;
        }

        /// <summary>
        /// Asserts that this string should be equal to the expected string.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <returns>The result object.</returns>
        public LocalizedStringResult ShouldBe(String expected)
        {
            Assert.AreEqual(expected, (String)str);
            return this;
        }

        /// <summary>
        /// Asserts that this string should be null.
        /// </summary>
        /// <returns>The result object.</returns>
        public LocalizedStringResult ShouldBeNull()
        {
            Assert.IsNull(str);
            return this;
        }

        /// <summary>
        /// Asserts that this string should not be null.
        /// </summary>
        /// <returns>The result object.</returns>
        public LocalizedStringResult ShouldNotBeNull()
        {
            Assert.IsNotNull(str);
            return this;
        }

        /// <summary>
        /// Asserts that the string has the specified property.
        /// </summary>
        /// <param name="property">The property to evaluate.</param>
        /// <returns>The result object.</returns>
        public LocalizedStringResult ShouldHaveProperty(String property)
        {
            Assert.IsTrue(str.HasProperty(property));
            return this;
        }

        /// <summary>
        /// Asserts that the string does not have the specified property.
        /// </summary>
        /// <param name="property">The property to evaluate.</param>
        /// <returns>The result object.</returns>
        public LocalizedStringResult ShouldNotHaveProperty(String property)
        {
            Assert.IsFalse(str.HasProperty(property));
            return this;
        }

        /// <summary>
        /// Gets the underlying string.
        /// </summary>
        public LocalizedString String
        {
            get { return str; }
        }

        // Property values.
        private readonly LocalizedString str;
    }
}

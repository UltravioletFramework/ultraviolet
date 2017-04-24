using System;
using NUnit.Framework;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a localized string.
    /// </summary>
    public sealed class LocalizedStringVariantResult
    {
        /// <summary>
        /// Initializes a new instance of the LocalizedStringVariantResult class.
        /// </summary>
        /// <param name="variant">The string variant being examined.</param>
        internal LocalizedStringVariantResult(LocalizedStringVariant variant)
        {
            this.variant = variant;
        }

        /// <summary>
        /// Asserts that the string variant satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the string variant.</param>
        /// <returns>The result object.</returns>
        public LocalizedStringVariantResult ShouldSatisfyTheCondition(Func<LocalizedStringVariant, Boolean> condition)
        {
            Assert.IsTrue(condition(variant));
            return this;
        }

        /// <summary>
        /// Asserts that this string variant should be equal to the expected string.
        /// </summary>
        /// <param name="expected">The expected string.</param>
        /// <returns>The result object.</returns>
        public LocalizedStringVariantResult ShouldBe(String expected)
        {
            Assert.AreEqual(expected, variant.Value);
            return this;
        }

        /// <summary>
        /// Asserts that this string variant should be null.
        /// </summary>
        /// <returns>The result object.</returns>
        public LocalizedStringVariantResult ShouldBeNull()
        {
            Assert.IsNull(variant);
            return this;
        }

        /// <summary>
        /// Asserts that this string variant should not be null.
        /// </summary>
        /// <returns>The result object.</returns>
        public LocalizedStringVariantResult ShouldNotBeNull()
        {
            Assert.IsNotNull(variant);
            return this;
        }

        /// <summary>
        /// Asserts that the string variant has the specified property.
        /// </summary>
        /// <param name="property">The property to evaluate.</param>
        /// <returns>The result object.</returns>
        public LocalizedStringVariantResult ShouldHaveProperty(String property)
        {
            Assert.IsTrue(variant.HasProperty(property));
            return this;
        }

        /// <summary>
        /// Asserts that the string variant does not have the specified property.
        /// </summary>
        /// <param name="property">The property to evaluate.</param>
        /// <returns>The result object.</returns>
        public LocalizedStringVariantResult ShouldNotHaveProperty(String property)
        {
            Assert.IsFalse(variant.HasProperty(property));
            return this;
        }

        /// <summary>
        /// Gets the underlying string variant.
        /// </summary>
        public LocalizedStringVariant Variant
        {
            get { return variant; }
        }

        // Property values.
        private readonly LocalizedStringVariant variant;
    }
}

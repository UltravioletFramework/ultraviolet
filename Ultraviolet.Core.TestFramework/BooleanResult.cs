using System;
using NUnit.Framework;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing a boolean value.
    /// </summary>
    public sealed class BooleanResult
    {
        /// <summary>
        /// Initializes a new instance of the BooleanResult class.
        /// </summary>
        /// <param name="value">The value being examined.</param>
        internal BooleanResult(Boolean value)
        {
            this.value = value;
        }

        /// <summary>
        /// Asserts that this value should be equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The result object.</returns>
        public BooleanResult ShouldBe(Boolean expected)
        {
            if (expected)
            {
                Assert.IsTrue(value);
            }
            else
            {
                Assert.IsFalse(value);
            }
            return this;
        }
        
        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public Boolean Value
        {
            get { return value; }
        }

        // Property values.
        private readonly Boolean value;
    }
}

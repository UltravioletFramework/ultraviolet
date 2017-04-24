using System;
using NUnit.Framework;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing an object reference.
    /// </summary>
    public sealed class ObjectResult<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the ObjectResult class.
        /// </summary>
        /// <param name="obj">The object being examined.</param>
        internal ObjectResult(T obj)
        {
            this.obj = obj;
        }

        /// <summary>
        /// Asserts that the object satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the object.</param>
        /// <returns>The result object.</returns>
        public ObjectResult<T> ShouldSatisfyTheCondition(Func<T, Boolean> condition)
        {
            Assert.IsTrue(condition(obj));
            return this;
        }

        /// <summary>
        /// Asserts that this object should be equal to the expected object.
        /// </summary>
        /// <param name="expected">The expected object.</param>
        /// <returns>The result object.</returns>
        public ObjectResult<T> ShouldBe(T expected)
        {
            Assert.AreEqual(expected, obj);
            return this;
        }

        /// <summary>
        /// Asserts that this object should be null.
        /// </summary>
        /// <returns>The result object.</returns>
        public ObjectResult<T> ShouldBeNull()
        {
            Assert.IsNull(obj);
            return this;
        }

        /// <summary>
        /// Asserts that this object should not be null.
        /// </summary>
        /// <returns>The result object.</returns>
        public ObjectResult<T> ShouldNotBeNull()
        {
            Assert.IsNotNull(obj);
            return this;
        }

        /// <summary>
        /// Gets the underlying object.
        /// </summary>
        public T Object
        {
            get { return obj; }
        }

        // Property values.
        private readonly T obj;
    }
}

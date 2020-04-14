using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Ultraviolet.Core.TestFramework
{
    /// <summary>
    /// Represents a unit test result containing an object collection.
    /// </summary>
    public sealed class CollectionResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the CollectionResult class.
        /// </summary>
        /// <param name="collection">The collection being examined.</param>
        internal CollectionResult(IEnumerable<T> collection)
        {
            this.collection = collection;
        }

        /// <summary>
        /// Asserts that the collection satisfies the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the collection.</param>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldSatisfyTheCondition(Func<IEnumerable<T>, Boolean> condition)
        {
            Assert.IsTrue(condition(collection));
            return this;
        }

        /// <summary>
        /// Asserts that the collection contains the same items as the specified collection.
        /// </summary>
        /// <param name="expected">The expected collection.</param>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldBe(IEnumerable<T> expected)
        {
            CollectionAssert.AreEquivalent(expected.ToList(), collection.ToList());
            return this;
        }

        /// <summary>
        /// Asserts that the collection contains the same items as the specified collection.
        /// </summary>
        /// <param name="expected">The expected collection.</param>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldBe(params T[] expected)
        {
            CollectionAssert.AreEquivalent(expected.ToList(), collection.ToList());
            return this;
        }

        /// <summary>
        /// Asserts that the collection contains the same items, in the same order, as the specified collection.
        /// </summary>
        /// <param name="expected">The expected collection.</param>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldBeExactly(IEnumerable<T> expected)
        {
            CollectionAssert.AreEqual(expected.ToList(), collection.ToList());
            return this;
        }

        /// <summary>
        /// Asserts that the collection contains the same items, in the same order, as the specified collection.
        /// </summary>
        /// <param name="expected">The expected collection.</param>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldBeExactly(params T[] expected)
        {
            CollectionAssert.AreEqual(expected.ToList(), collection.ToList());
            return this;
        }

        /// <summary>
        /// Asserts that the collection is null.
        /// </summary>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldBeNull()
        {
            Assert.IsNull(collection);
            return this;
        }

        /// <summary>
        /// Asserts that the collection is not null.
        /// </summary>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldNotBeNull()
        {
            Assert.IsNotNull(collection);
            return this;
        }

        /// <summary>
        /// Asserts that the collection is empty.
        /// </summary>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldBeEmpty()
        {
            Assert.IsTrue(!collection.Any());
            return this;
        }

        /// <summary>
        /// Asserts that the collection is not empty.
        /// </summary>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldNotBeEmpty()
        {
            Assert.IsTrue(collection.Any());
            return this;
        }

        /// <summary>
        /// Asserts that the collection contains exactly the specified number of items.
        /// </summary>
        /// <param name="count">The expected number of items.</param>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldContainTheSpecifiedNumberOfItems(Int32 count)
        {
            Assert.AreEqual(collection.Count(), count);
            return this;
        }

        /// <summary>
        /// Asserts that the items in the collection satisfy the specified condition.
        /// </summary>
        /// <param name="condition">The condition against which to evaluate the collection's items.</param>
        /// <returns>The result object.</returns>
        public CollectionResult<T> ShouldContainItemsSatisfyingTheCondition(Func<T, Boolean> condition)
        {
            foreach (var item in collection)
            {
                Assert.IsTrue(condition(item));
            }
            return this;
        }

        /// <summary>
        /// Gets the underlying collection.
        /// </summary>
        public IEnumerable<T> Collection
        {
            get { return collection; }
        }

        // State values
        private readonly IEnumerable<T> collection;
    }
}

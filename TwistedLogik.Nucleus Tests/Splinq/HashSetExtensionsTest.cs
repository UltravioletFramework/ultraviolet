using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Splinq;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Splinq
{
    [TestClass]
    public class HashSetExtensionsTest : NucleusTestFramework
    {
        [TestMethod]
        public void HashSetExtensions_Any_ReturnsTrueIfHashSetContainsItems()
        {
            var set = new HashSet<Int32>() { 1 };

            var result = set.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void HashSetExtensions_Any_ReturnsFalseIfHashSetDoesNotContainItems()
        {
            var set = new HashSet<Int32>();

            var result = set.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void HashSetExtensions_AnyWithPredicate_ReturnsTrueIfHashSetContainsMatchingItems()
        {
            var set = new HashSet<Int32>() { 1, 2, 3 };

            var result = set.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void HashSetExtensions_AnyWithPredicate_ReturnsFalseIfHashSetDoesNotContainMatchingItems()
        {
            var set = new HashSet<Int32>() { 1, 3 };

            var result = set.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void HashSetExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var set = new HashSet<Int32>() { 2, 4, 6 };

            var result = set.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void HashSetExtensions_All_ReturnsTrueIfHashSetIsEmpty()
        {
            var set = new HashSet<Int32>();

            var result = set.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void HashSetExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var set = new HashSet<Int32>() { 1, 2, 4, 6 };

            var result = set.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void HashSetExtensions_Count_ReturnsCorrectSize()
        {
            var set = new HashSet<Int32>() { 1, 2, 3 };

            var result = set.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        public void HashSetExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var set = new HashSet<Int32>() { 1, 2, 3 };

            var result = set.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        public void HashSetExtensions_First_ReturnsFirstItemInHashSet()
        {
            var set = new HashSet<Int32>() { 1, 2, 3 };

            var result = set.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HashSetExtensions_First_ThrowsExceptionIfHashSetIsEmpty()
        {
            var set = new HashSet<Int32>();

            set.First();
        }

        [TestMethod]
        public void HashSetExtensions_Last_ReturnsLastItemInHashSet()
        {
            var set = new HashSet<Int32>() { 1, 2, 3 };

            var result = set.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HashSetExtensions_Last_ThrowsExceptionIfHashSetIsEmpty()
        {
            var set = new HashSet<Int32>();

            set.Last();
        }

        [TestMethod]
        public void HashSetExtensions_Single_ReturnsSingleItemInHashSet()
        {
            var set = new HashSet<Int32>() { 4 };

            var result = set.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HashSetExtensions_Single_ThrowsExceptionIfHashSetIsEmpty()
        {
            var set = new HashSet<Int32>();

            set.Single();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HashSetExtensions_Single_ThrowsExceptionIfHashSetHasMultipleItems()
        {
            var set = new HashSet<Int32>() { 1, 2 };

            set.Single();
        }

        [TestMethod]
        public void HashSetExtensions_SingleOrDefault_ReturnsSingleItemInHashSet()
        {
            var set = new HashSet<Int32>() { 4 };

            var result = set.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [TestMethod]
        public void HashSetExtensions_SingleOrDefault_ReturnsDefaultValueIfHashSetIsEmpty()
        {
            var set = new HashSet<Int32>();

            var result = set.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HashSetExtensions_SingleOrDefault_ThrowsExceptionIfHashSetHasMultipleItems()
        {
            var set = new HashSet<Int32>() { 1, 2 };

            set.SingleOrDefault();
        }

        [TestMethod]
        public void HashSetExtensions_Max_ReturnsMaxValue()
        {
            var set = new HashSet<Int32>() { 4, 5, 6, 99, 10, 1, 12, 45 };

            var result = set.Max();

            TheResultingValue(result).ShouldBe(99);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HashSetExtensions_Max_ThrowsExceptionIfHashSetIsEmpty()
        {
            var set = new HashSet<Int32>();

            set.Max();
        }

        [TestMethod]
        public void HashSetExtensions_Min_ReturnsMinValue()
        {
            var set = new HashSet<Int32>() { 4, 5, 6, 99, 10, 1, 12, 45 };

            var result = set.Min();

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HashSetExtensions_Min_ThrowsExceptionIfHashSetIsEmpty()
        {
            var set = new HashSet<Int32>();

            set.Min();
        }
    }
}

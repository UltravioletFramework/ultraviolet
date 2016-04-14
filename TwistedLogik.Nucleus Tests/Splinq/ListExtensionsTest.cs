using NUnit.Framework;
using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Splinq;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Splinq
{
    [TestFixture]
    public class ListExtensionsTest : NucleusTestFramework
    {
        [Test]
        public void ListExtensions_Any_ReturnsTrueIfListContainsItems()
        {
            var list = new List<Int32>() { 1 };

            var result = list.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ListExtensions_Any_ReturnsFalseIfListDoesNotContainItems()
        {
            var list = new List<Int32>();

            var result = list.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ListExtensions_AnyWithPredicate_ReturnsTrueIfListContainsMatchingItems()
        {
            var list = new List<Int32>() { 1, 2, 3 };

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ListExtensions_AnyWithPredicate_ReturnsFalseIfListDoesNotContainMatchingItems()
        {
            var list = new List<Int32>() { 1, 3 };

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ListExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var list = new List<Int32>() { 2, 4, 6 };

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ListExtensions_All_ReturnsTrueIfListIsEmpty()
        {
            var list = new List<Int32>();

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ListExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var list = new List<Int32>() { 1, 2, 4, 6 };

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ListExtensions_Count_ReturnsCorrectSize()
        {
            var list = new List<Int32>() { 1, 2, 3 };

            var result = list.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void ListExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var list = new List<Int32>() { 1, 2, 3 };

            var result = list.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void ListExtensions_First_ReturnsFirstItemInList()
        {
            var list = new List<Int32>() { 1, 2, 3 };

            var result = list.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void ListExtensions_First_ThrowsExceptionIfListIsEmpty()
        {
            var list = new List<Int32>();

            Assert.That(() => list.First(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ListExtensions_Last_ReturnsLastItemInList()
        {
            var list = new List<Int32>() { 1, 2, 3 };

            var result = list.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void ListExtensions_Last_ThrowsExceptionIfListIsEmpty()
        {
            var list = new List<Int32>();

            Assert.That(() => list.Last(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ListExtensions_Single_ReturnsSingleItemInList()
        {
            var list = new List<Int32>() { 4 };

            var result = list.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void ListExtensions_Single_ThrowsExceptionIfListIsEmpty()
        {
            var list = new List<Int32>();

            Assert.That(() => list.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ListExtensions_Single_ThrowsExceptionIfListHasMultipleItems()
        {
            var list = new List<Int32>() { 1, 2 };

            Assert.That(() => list.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ListExtensions_SingleOrDefault_ReturnsSingleItemInList()
        {
            var list = new List<Int32>() { 4 };

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void ListExtensions_SingleOrDefault_ReturnsDefaultValueIfListIsEmpty()
        {
            var list = new List<Int32>();

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [Test]
        public void ListExtensions_SingleOrDefault_ThrowsExceptionIfListHasMultipleItems()
        {
            var list = new List<Int32>() { 1, 2 };

            Assert.That(() => list.SingleOrDefault(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ListExtensions_Max_ReturnsMaxValue()
        {
            var list = new List<Int32>() { 4, 5, 6, 99, 10, 1, 12, 45 };

            var result = list.Max();

            TheResultingValue(result).ShouldBe(99);
        }

        [Test]
        public void ListExtensions_Max_ThrowsExceptionIfListIsEmpty()
        {
            var list = new List<Int32>();

            Assert.That(() => list.Max(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ListExtensions_Min_ReturnsMinValue()
        {
            var list = new List<Int32>() { 4, 5, 6, 99, 10, 1, 12, 45 };

            var result = list.Min();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void ListExtensions_Min_ThrowsExceptionIfListIsEmpty()
        {
            var list = new List<Int32>();

            Assert.That(() => list.Min(),
                Throws.TypeOf<InvalidOperationException>());
        }
    }
}

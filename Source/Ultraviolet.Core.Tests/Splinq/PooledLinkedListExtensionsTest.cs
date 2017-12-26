using System;
using NUnit.Framework;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.Splinq;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.CoreTests.Splinq
{
    [TestFixture]
    public class PooledLinkedListExtensionsTest : CoreTestFramework
    {
        [Test]
        public void PooledLinkedListExtensions_Any_ReturnsTrueIfPooledLinkedListContainsItems()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);

            var result = list.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void PooledLinkedListExtensions_Any_ReturnsFalseIfPooledLinkedListDoesNotContainItems()
        {
            var list = new PooledLinkedList<Int32>();

            var result = list.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void PooledLinkedListExtensions_AnyWithPredicate_ReturnsTrueIfPooledLinkedListContainsMatchingItems()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void PooledLinkedListExtensions_AnyWithPredicate_ReturnsFalseIfPooledLinkedListDoesNotContainMatchingItems()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(3);

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void PooledLinkedListExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(2);
            list.AddLast(4);
            list.AddLast(6);

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void PooledLinkedListExtensions_All_ReturnsTrueIfPooledLinkedListIsEmpty()
        {
            var list = new PooledLinkedList<Int32>();

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void PooledLinkedListExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(4);
            list.AddLast(6);

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void PooledLinkedListExtensions_Count_ReturnsCorrectSize()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void PooledLinkedListExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void PooledLinkedListExtensions_First_ReturnsFirstItemInPooledLinkedList()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void PooledLinkedListExtensions_First_ThrowsExceptionIfPooledLinkedListIsEmpty()
        {
            var list = new PooledLinkedList<Int32>();

            Assert.That(() => list.First(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void PooledLinkedListExtensions_Last_ReturnsLastItemInPooledLinkedList()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void PooledLinkedListExtensions_Last_ThrowsExceptionIfPooledLinkedListIsEmpty()
        {
            var list = new PooledLinkedList<Int32>();

            Assert.That(() => list.Last(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void PooledLinkedListExtensions_Single_ReturnsSingleItemInPooledLinkedList()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(4);

            var result = list.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void PooledLinkedListExtensions_Single_ThrowsExceptionIfPooledLinkedListIsEmpty()
        {
            var list = new PooledLinkedList<Int32>();

            Assert.That(() => list.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void PooledLinkedListExtensions_Single_ThrowsExceptionIfPooledLinkedListHasMultipleItems()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);

            Assert.That(() => list.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void PooledLinkedListExtensions_SingleOrDefault_ReturnsSingleItemInPooledLinkedList()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(4);

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void PooledLinkedListExtensions_SingleOrDefault_ReturnsDefaultValueIfPooledLinkedListIsEmpty()
        {
            var list = new PooledLinkedList<Int32>();

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [Test]
        public void PooledLinkedListExtensions_SingleOrDefault_ThrowsExceptionIfPooledLinkedListHasMultipleItems()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);

            Assert.That(() => list.SingleOrDefault(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void PooledLinkedListExtensions_Max_ReturnsMaxValue()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(4);
            list.AddLast(5);
            list.AddLast(6);
            list.AddLast(99);
            list.AddLast(10);
            list.AddLast(1);
            list.AddLast(12);
            list.AddLast(45);

            var result = list.Max();

            TheResultingValue(result).ShouldBe(99);
        }

        [Test]
        public void PooledLinkedListExtensions_Max_ThrowsExceptionIfPooledLinkedListIsEmpty()
        {
            var list = new PooledLinkedList<Int32>();

            Assert.That(() => list.Max(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void PooledLinkedListExtensions_Min_ReturnsMinValue()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(4);
            list.AddLast(5);
            list.AddLast(6);
            list.AddLast(99);
            list.AddLast(10);
            list.AddLast(1);
            list.AddLast(12);
            list.AddLast(45);

            var result = list.Min();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void PooledLinkedListExtensions_Min_ThrowsExceptionIfPooledLinkedListIsEmpty()
        {
            var list = new PooledLinkedList<Int32>();

            Assert.That(() => list.Min(),
                Throws.TypeOf<InvalidOperationException>());
        }
    }
}

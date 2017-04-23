using System;
using System.Collections.Generic;
using Ultraviolet.Core.Splinq;
using Ultraviolet.Core.TestFramework;
using NUnit.Framework;

namespace Ultraviolet.CoreTests.Splinq
{
    [TestFixture]
    public class LinkedListExtensionsTest : CoreTestFramework
    {
        [Test]
        public void LinkedListExtensions_Any_ReturnsTrueIfLinkedListContainsItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);

            var result = list.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void LinkedListExtensions_Any_ReturnsFalseIfLinkedListDoesNotContainItems()
        {
            var list = new LinkedList<Int32>();

            var result = list.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void LinkedListExtensions_AnyWithPredicate_ReturnsTrueIfLinkedListContainsMatchingItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void LinkedListExtensions_AnyWithPredicate_ReturnsFalseIfLinkedListDoesNotContainMatchingItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(3);

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void LinkedListExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(2);
            list.AddLast(4);
            list.AddLast(6);

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void LinkedListExtensions_All_ReturnsTrueIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void LinkedListExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(4);
            list.AddLast(6);

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void LinkedListExtensions_Count_ReturnsCorrectSize()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void LinkedListExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void LinkedListExtensions_First_ReturnsFirstItemInLinkedList()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void LinkedListExtensions_First_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            Assert.That(() => list.First(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void LinkedListExtensions_Last_ReturnsLastItemInLinkedList()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void LinkedListExtensions_Last_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            Assert.That(() => list.Last(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void LinkedListExtensions_Single_ReturnsSingleItemInLinkedList()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(4);

            var result = list.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void LinkedListExtensions_Single_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            Assert.That(() => list.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void LinkedListExtensions_Single_ThrowsExceptionIfLinkedListHasMultipleItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);

            Assert.That(() => list.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void LinkedListExtensions_SingleOrDefault_ReturnsSingleItemInLinkedList()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(4);

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void LinkedListExtensions_SingleOrDefault_ReturnsDefaultValueIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [Test]
        public void LinkedListExtensions_SingleOrDefault_ThrowsExceptionIfLinkedListHasMultipleItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);

            Assert.That(() => list.SingleOrDefault(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void LinkedListExtensions_Max_ReturnsMaxValue()
        {
            var list = new LinkedList<Int32>();
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
        public void LinkedListExtensions_Max_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            Assert.That(() => list.Max(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void LinkedListExtensions_Min_ReturnsMinValue()
        {
            var list = new LinkedList<Int32>();
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
        public void LinkedListExtensions_Min_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            Assert.That(() => list.Min(),
                Throws.TypeOf<InvalidOperationException>());
        }
    }
}

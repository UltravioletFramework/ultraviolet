using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Splinq;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Splinq
{
    [TestClass]
    public class LinkedListExtensionsTest : NucleusTestFramework
    {
        [TestMethod]
        public void LinkedListExtensions_Any_ReturnsTrueIfLinkedListContainsItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);

            var result = list.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void LinkedListExtensions_Any_ReturnsFalseIfLinkedListDoesNotContainItems()
        {
            var list = new LinkedList<Int32>();

            var result = list.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void LinkedListExtensions_AnyWithPredicate_ReturnsTrueIfLinkedListContainsMatchingItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void LinkedListExtensions_AnyWithPredicate_ReturnsFalseIfLinkedListDoesNotContainMatchingItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(3);

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void LinkedListExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(2);
            list.AddLast(4);
            list.AddLast(6);

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void LinkedListExtensions_All_ReturnsTrueIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
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

        [TestMethod]
        public void LinkedListExtensions_Count_ReturnsCorrectSize()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        public void LinkedListExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        public void LinkedListExtensions_First_ReturnsFirstItemInLinkedList()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListExtensions_First_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            list.First();
        }

        [TestMethod]
        public void LinkedListExtensions_Last_ReturnsLastItemInLinkedList()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            var result = list.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListExtensions_Last_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            list.Last();
        }

        [TestMethod]
        public void LinkedListExtensions_Single_ReturnsSingleItemInLinkedList()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(4);

            var result = list.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListExtensions_Single_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            list.Single();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListExtensions_Single_ThrowsExceptionIfLinkedListHasMultipleItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);

            list.Single();
        }

        [TestMethod]
        public void LinkedListExtensions_SingleOrDefault_ReturnsSingleItemInLinkedList()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(4);

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [TestMethod]
        public void LinkedListExtensions_SingleOrDefault_ReturnsDefaultValueIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListExtensions_SingleOrDefault_ThrowsExceptionIfLinkedListHasMultipleItems()
        {
            var list = new LinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);

            list.SingleOrDefault();
        }

        [TestMethod]
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListExtensions_Max_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            list.Max();
        }

        [TestMethod]
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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void LinkedListExtensions_Min_ThrowsExceptionIfLinkedListIsEmpty()
        {
            var list = new LinkedList<Int32>();

            list.Min();
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Splinq;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Splinq
{
    [TestClass]
    public class QueueExtensionsTest : NucleusTestFramework
    {
        [TestMethod]
        public void QueueExtensions_Any_ReturnsTrueIfQueueContainsItems()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);

            var result = queue.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void QueueExtensions_Any_ReturnsFalseIfQueueDoesNotContainItems()
        {
            var queue = new Queue<Int32>();

            var result = queue.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void QueueExtensions_AnyWithPredicate_ReturnsTrueIfQueueContainsMatchingItems()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var result = queue.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void QueueExtensions_AnyWithPredicate_ReturnsFalseIfQueueDoesNotContainMatchingItems()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(3);

            var result = queue.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void QueueExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(2);
            queue.Enqueue(4);
            queue.Enqueue(6);

            var result = queue.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void QueueExtensions_All_ReturnsTrueIfQueueIsEmpty()
        {
            var queue = new Queue<Int32>();

            var result = queue.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void QueueExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(4);
            queue.Enqueue(6);

            var result = queue.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void QueueExtensions_Count_ReturnsCorrectSize()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var result = queue.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        public void QueueExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var result = queue.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        public void QueueExtensions_First_ReturnsFirstItemInQueue()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var result = queue.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueueExtensions_First_ThrowsExceptionIfQueueIsEmpty()
        {
            var queue = new Queue<Int32>();

            queue.First();
        }

        [TestMethod]
        public void QueueExtensions_Last_ReturnsLastItemInQueue()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var result = queue.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueueExtensions_Last_ThrowsExceptionIfQueueIsEmpty()
        {
            var queue = new Queue<Int32>();

            queue.Last();
        }

        [TestMethod]
        public void QueueExtensions_Single_ReturnsSingleItemInQueue()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(4);

            var result = queue.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueueExtensions_Single_ThrowsExceptionIfQueueIsEmpty()
        {
            var queue = new Queue<Int32>();

            queue.Single();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueueExtensions_Single_ThrowsExceptionIfQueueHasMultipleItems()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            queue.Single();
        }

        [TestMethod]
        public void QueueExtensions_SingleOrDefault_ReturnsSingleItemInQueue()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(4);

            var result = queue.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [TestMethod]
        public void QueueExtensions_SingleOrDefault_ReturnsDefaultValueIfQueueIsEmpty()
        {
            var queue = new Queue<Int32>();

            var result = queue.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueueExtensions_SingleOrDefault_ThrowsExceptionIfQueueHasMultipleItems()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            queue.SingleOrDefault();
        }

        [TestMethod]
        public void QueueExtensions_Max_ReturnsMaxValue()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(4);
            queue.Enqueue(5);
            queue.Enqueue(6);
            queue.Enqueue(99);
            queue.Enqueue(10);
            queue.Enqueue(1);
            queue.Enqueue(12);
            queue.Enqueue(45);

            var result = queue.Max();

            TheResultingValue(result).ShouldBe(99);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueueExtensions_Max_ThrowsExceptionIfQueueIsEmpty()
        {
            var queue = new Queue<Int32>();

            queue.Max();
        }

        [TestMethod]
        public void QueueExtensions_Min_ReturnsMinValue()
        {
            var queue = new Queue<Int32>();
            queue.Enqueue(4);
            queue.Enqueue(5);
            queue.Enqueue(6);
            queue.Enqueue(99);
            queue.Enqueue(10);
            queue.Enqueue(1);
            queue.Enqueue(12);
            queue.Enqueue(45);

            var result = queue.Min();

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueueExtensions_Min_ThrowsExceptionIfQueueIsEmpty()
        {
            var queue = new Queue<Int32>();

            queue.Min();
        }
    }
}

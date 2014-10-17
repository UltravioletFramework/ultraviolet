using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests.IO
{
    [TestClass]
    public class PriorityQueueTest : NucleusTestFramework
    {
        [TestMethod]
        public void PriorityQueue_CanQueueAndDequeueItems()
        {
            var queue = new PriorityQueue<String>();

            queue.Enqueue(1, "world");
            queue.Enqueue(2, "this");
            queue.Enqueue(3, "is");
            queue.Enqueue(0, "Hello");
            queue.Enqueue(5, "test");
            queue.Enqueue(4, "a");

            TheResultingString(queue.Dequeue()).ShouldBe("Hello");
            TheResultingString(queue.Dequeue()).ShouldBe("world");
            TheResultingString(queue.Dequeue()).ShouldBe("this");
            TheResultingString(queue.Dequeue()).ShouldBe("is");
            TheResultingString(queue.Dequeue()).ShouldBe("a");
            TheResultingString(queue.Dequeue()).ShouldBe("test");
        }
    }
}

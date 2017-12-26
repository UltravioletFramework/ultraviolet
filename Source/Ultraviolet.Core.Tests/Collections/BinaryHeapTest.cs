using System;
using NUnit.Framework;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests.IO
{
    [TestFixture]
    public class BinaryHeapTest : CoreTestFramework
    {
        [Test]
        public void BinaryHeap_CanAddItems()
        {
            var heap = new BinaryHeap<Int32>();

            heap.Add(6);
            heap.Add(1);
            heap.Add(44);
            heap.Add(2);
            heap.Add(102);
            heap.Add(17);
            heap.Add(94);
            heap.Add(6);

            TheResultingCollection(heap)
                .ShouldBeExactly(1, 2, 6, 6, 17, 44, 94, 102);
        }

        [Test]
        public void BinaryHeap_CanRemoveItems()
        {
            var heap = new BinaryHeap<Int32>() { 6, 1, 44, 2, 102, 17, 94, 6 };

            heap.Remove(2);
            heap.Remove(44);

            TheResultingCollection(heap)
                .ShouldBeExactly(1, 6, 6, 17, 94, 102);
        }
    }
}

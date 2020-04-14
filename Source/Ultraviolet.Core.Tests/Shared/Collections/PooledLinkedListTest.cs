using System;
using NUnit.Framework;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests.IO
{
    [TestFixture]
    public class PooledLinkedListTest : CoreTestFramework
    {
        [Test]
        public void PooledLinkedList_CanAddItems()
        {
            var list = new PooledLinkedList<Int32>();

            list.AddLast(1);
            list.AddFirst(2);
            list.AddLast(3);

            TheResultingCollection(list)
                .ShouldBeExactly(2, 1, 3);
        }

        [Test]
        public void PooledLinkedList_CanRemoveItems()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);
            list.AddLast(2);
            list.AddLast(3);

            list.Remove(2);

            TheResultingCollection(list)
                .ShouldBeExactly(1, 3);
        }

        [Test]
        public void PooledLinkedList_ClearsNodesUponRemoval()
        {
            var list = new PooledLinkedList<Int32>();
            list.AddLast(1);

            TheResultingValue(list.First.Value)
                .ShouldBe(1);

            var node = list.Last;
            list.Remove(node);

            TheResultingValue(node.Value)
                .ShouldBe(default(Int32));
        }
    }
}

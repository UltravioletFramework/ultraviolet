using System;
using NUnit.Framework;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.CoreTests.Collections
{
    [TestFixture]
    public partial class ObservableListTest : CoreTestFramework
    {
        [Test]
        public void ObservableList_RaisesCollectionItemAdded()
        {
            var list  = new ObservableList<Int32>();
            var added = false;

            list.CollectionItemAdded += (source, index, value) =>
            {
                added = (value == 1234);
            };
            list.Add(1234);

            TheResultingValue(added).ShouldBe(true);
        }

        [Test]
        public void ObservableList_RaisesCollectionItemRemoved()
        {
            var list    = new ObservableList<Int32>();
            var removed = false;

            list.CollectionItemRemoved += (source, index, value) =>
            {
                removed = (value == 1234);
            };
            list.Add(1234);
            list.Remove(1234);

            TheResultingValue(removed).ShouldBe(true);
        }

        [Test]
        public void ObservableList_RaisesCollectionResetOnClear()
        {
            var list  = new ObservableList<Int32>();
            var reset = false;

            list.CollectionReset += (source) =>
            {
                reset = true;
            };
            list.Add(1234);
            list.Clear();

            TheResultingValue(reset).ShouldBe(true);
        }

        [Test]
        public void ObservableList_RaisesCollectionResetOnSort()
        {
            var list  = new ObservableList<Int32>();
            var reset = false;

            list.CollectionReset += (source) =>
            {
                reset = true;
            };
            list.Add(4);
            list.Add(6);
            list.Add(1);
            list.Add(3);
            list.Sort();

            TheResultingValue(reset).ShouldBe(true);
        }

        [Test]
        public void ObservableList_RaisesCollectionResetOnReverse()
        {
            var list  = new ObservableList<Int32>();
            var reset = false;

            list.CollectionReset += (source) =>
            {
                reset = true;
            };
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Reverse();

            TheResultingValue(reset).ShouldBe(true);
        }
    }
}

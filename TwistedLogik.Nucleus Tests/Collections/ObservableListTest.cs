using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Collections
{
    [TestClass]
    public partial class ObservableListTest : NucleusTestFramework
    {
        [TestMethod]
        public void ObservableList_RaisesItemAdded()
        {
            var list  = new ObservableList<Int32>();
            var added = false;

            list.ItemAdded += (source, value) =>
            {
                added = (value == 1234);
            };
            list.Add(1234);

            TheResultingValue(added).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableList_RaisesItemRemoved()
        {
            var list    = new ObservableList<Int32>();
            var removed = false;

            list.ItemRemoved += (source, value) =>
            {
                removed = (value == 1234);
            };
            list.Add(1234);
            list.Remove(1234);

            TheResultingValue(removed).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableList_RaisesItemPropertyChanged()
        {
            var list   = new ObservableList<ObservableItem>();
            var item   = new ObservableItem();
            var raisedA = false;
            var raisedB = false;

            list.ItemPropertyChanged += (s, i, pn) =>
            {
                if (s == list && i == item)
                {
                    switch (pn)
                    {
                        case "PropertyA":
                            raisedA = true;
                            break;

                        case "PropertyB":
                            raisedB = true;
                            break;
                    }
                }
            };
            list.Add(item);

            item.PropertyA = "Hello, world!";
            item.PropertyB = "Goodbye, world!";

            TheResultingValue(raisedA).ShouldBe(true);
            TheResultingValue(raisedB).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableList_RaisesCleared()
        {
            var list    = new ObservableList<Int32>();
            var cleared = false;

            list.Cleared += (source) =>
            {
                cleared = true;
            };
            list.Add(1234);
            list.Clear();

            TheResultingValue(cleared).ShouldBe(true);
        }
    }
}

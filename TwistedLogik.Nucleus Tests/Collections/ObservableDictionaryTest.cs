using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Collections
{
    [TestClass]
    public class ObservableDictionaryTest : NucleusTestFramework
    {
        [TestMethod]
        public void ObservableDictionary_RaisesItemAdded()
        {
            var dict  = new ObservableDictionary<String, Int32>();
            var added = false;

            dict.ItemAdded += (dictionary, key, value) =>
            {
                added = (key == "Testing" && value == 1234);
            };
            dict["Testing"] = 1234;

            TheResultingValue(added).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableDictionary_RaisesItemRemoved()
        {
            var dict    = new ObservableDictionary<String, Int32>();
            var removed = false;
            
            dict.ItemRemoved += (dictionary, key, value) =>
            {
                removed = (key == "Testing" && value == 1234);
            };
            dict["Testing"] = 1234;
            dict.Remove("Testing");

            TheResultingValue(removed).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableDictionary_RaisesItemPropertyChanged()
        {
            var list   = new ObservableDictionary<String, ObservableItem>();
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
            list.Add("test", item);

            item.PropertyA = "Hello, world!";
            item.PropertyB = "Goodbye, world!";

            TheResultingValue(raisedA).ShouldBe(true);
            TheResultingValue(raisedB).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableDictionary_RaisesCleared()
        {
            var dict    = new ObservableDictionary<String, Int32>();
            var cleared = false;
            
            dict.Cleared += (dictionary) =>
            {
                cleared = true;
            };
            dict["Testing"] = 1234;
            dict.Clear();

            TheResultingValue(cleared).ShouldBe(true);
        }
    }
}

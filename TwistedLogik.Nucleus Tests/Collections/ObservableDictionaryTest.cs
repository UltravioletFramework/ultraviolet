using NUnit.Framework;
using System;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Collections
{
    [TestFixture]
    public class ObservableDictionaryTest : NucleusTestFramework
    {
        [Test]
        public void ObservableDictionary_RaisesCollectionItemAdded()
        {
            var dict  = new ObservableDictionary<String, Int32>();
            var added = false;

            dict.CollectionItemAdded += (dictionary, key, value) =>
            {
                added = (key == "Testing" && value == 1234);
            };
            dict["Testing"] = 1234;

            TheResultingValue(added).ShouldBe(true);
        }

        [Test]
        public void ObservableDictionary_RaisesCollectionItemRemoved()
        {
            var dict    = new ObservableDictionary<String, Int32>();
            var removed = false;
            
            dict.CollectionItemRemoved += (dictionary, key, value) =>
            {
                removed = (key == "Testing" && value == 1234);
            };
            dict["Testing"] = 1234;
            dict.Remove("Testing");

            TheResultingValue(removed).ShouldBe(true);
        }

        [Test]
        public void ObservableDictionary_RaisesCollectionResetOnClear()
        {
            var dict  = new ObservableDictionary<String, Int32>();
            var reset = false;
            
            dict.CollectionReset += (dictionary) =>
            {
                reset = true;
            };
            dict["Testing"] = 1234;
            dict.Clear();

            TheResultingValue(reset).ShouldBe(true);
        }
    }
}

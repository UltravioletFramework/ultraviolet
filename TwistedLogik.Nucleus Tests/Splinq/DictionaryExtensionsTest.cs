using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Splinq;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Splinq
{
    [TestClass]
    public class DictionaryExtensionsTest : NucleusTestFramework
    {
        [TestMethod]
        public void DictionaryExtensions_Any_ReturnsTrueIfDictionaryContainsItems()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
            };

            var result = dictionary.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void DictionaryExtensions_Any_ReturnsFalseIfDictionaryDoesNotContainItems()
        {
            var dictionary = new Dictionary<Int32, String>();

            var result = dictionary.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void DictionaryExtensions_AnyWithPredicate_ReturnsTrueIfDictionaryContainsMatchingItems()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.Any(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void DictionaryExtensions_AnyWithPredicate_ReturnsFalseIfDictionaryDoesNotContainMatchingItems()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 3, "C" },
            };

            var result = dictionary.Any(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void DictionaryExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 2, "A" },
                { 4, "B" },
                { 6, "C" },
            };

            var result = dictionary.All(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void DictionaryExtensions_All_ReturnsTrueIfDictionaryIsEmpty()
        {
            var dictionary = new Dictionary<Int32, String>();

            var result = dictionary.All(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void DictionaryExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 4, "C" },
                { 6, "D" },
            };
            
            var result = dictionary.All(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void DictionaryExtensions_Count_ReturnsCorrectSize()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        public void DictionaryExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.Count(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        public void DictionaryExtensions_First_ReturnsFirstItemInDictionary()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.First();

            TheResultingValue(result).ShouldBe(new KeyValuePair<Int32, String>(1, "A"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DictionaryExtensions_First_ThrowsExceptionIfDictionaryIsEmpty()
        {
            var dictionary = new Dictionary<Int32, String>();

            dictionary.First();
        }

        [TestMethod]
        public void DictionaryExtensions_Last_ReturnsLastItemInDictionary()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.Last();

            TheResultingValue(result).ShouldBe(new KeyValuePair<Int32, String>(3, "C"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DictionaryExtensions_Last_ThrowsExceptionIfDictionaryIsEmpty()
        {
            var dictionary = new Dictionary<Int32, String>();

            dictionary.Last();
        }

        [TestMethod]
        public void DictionaryExtensions_Single_ReturnsSingleItemInDictionary()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 4, "A" },
            };

            var result = dictionary.Single();

            TheResultingValue(result).ShouldBe(new KeyValuePair<Int32, String>(4, "A"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DictionaryExtensions_Single_ThrowsExceptionIfDictionaryIsEmpty()
        {
            var dictionary = new Dictionary<Int32, String>();

            dictionary.Single();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DictionaryExtensions_Single_ThrowsExceptionIfDictionaryHasMultipleItems()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
            };

            dictionary.Single();
        }

        [TestMethod]
        public void DictionaryExtensions_SingleOrDefault_ReturnsSingleItemInDictionary()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 4, "A" },
            };

            var result = dictionary.SingleOrDefault();

            TheResultingValue(result).ShouldBe(new KeyValuePair<Int32, String>(4, "A"));
        }

        [TestMethod]
        public void DictionaryExtensions_SingleOrDefault_ReturnsDefaultValueIfDictionaryIsEmpty()
        {
            var dictionary = new Dictionary<Int32, String>();

            var result = dictionary.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(KeyValuePair<Int32, String>));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DictionaryExtensions_SingleOrDefault_ThrowsExceptionIfDictionaryHasMultipleItems()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
            };

            dictionary.SingleOrDefault();
        }

        [TestMethod]
        public void DictionaryExtensions_Max_ReturnsMaxValue()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                {  4, "A" },
                {  5, "B" },
                {  6, "C" },
                { 99, "D" },
                { 10, "E" },
                {  1, "F" },
                { 12, "G" },
                { 45, "H" },
            };

            var result = dictionary.Max(x => x.Key);

            TheResultingValue(result).ShouldBe(99);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DictionaryExtensions_Max_ThrowsExceptionIfDictionaryIsEmpty()
        {
            var dictionary = new Dictionary<Int32, String>();

            dictionary.Max(x => x.Key);
        }

        [TestMethod]
        public void DictionaryExtensions_Min_ReturnsMinValue()
        {
            var dictionary = new Dictionary<Int32, String>()
            {
                {  4, "A" },
                {  5, "B" },
                {  6, "C" },
                { 99, "D" },
                { 10, "E" },
                {  1, "F" },
                { 12, "G" },
                { 45, "H" },
            };

            var result = dictionary.Min(x => x.Key);

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DictionaryExtensions_Min_ThrowsExceptionIfDictionaryIsEmpty()
        {
            var dictionary = new Dictionary<Int32, String>();

            dictionary.Min(x => x.Key);
        }
    }
}

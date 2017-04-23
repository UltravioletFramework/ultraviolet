using NUnit.Framework;
using System;
using System.Collections.Generic;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.Splinq;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.CoreTests.Splinq
{
    [TestFixture]
    public class ObservableDictionaryExtensionsTest : CoreTestFramework
    {
        [Test]
        public void ObservableDictionaryExtensions_Any_ReturnsTrueIfObservableDictionaryContainsItems()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
            };

            var result = dictionary.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ObservableDictionaryExtensions_Any_ReturnsFalseIfObservableDictionaryDoesNotContainItems()
        {
            var dictionary = new ObservableDictionary<Int32, String>();

            var result = dictionary.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ObservableDictionaryExtensions_AnyWithPredicate_ReturnsTrueIfObservableDictionaryContainsMatchingItems()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.Any(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ObservableDictionaryExtensions_AnyWithPredicate_ReturnsFalseIfObservableDictionaryDoesNotContainMatchingItems()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 3, "C" },
            };

            var result = dictionary.Any(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ObservableDictionaryExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 2, "A" },
                { 4, "B" },
                { 6, "C" },
            };

            var result = dictionary.All(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ObservableDictionaryExtensions_All_ReturnsTrueIfObservableDictionaryIsEmpty()
        {
            var dictionary = new ObservableDictionary<Int32, String>();

            var result = dictionary.All(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ObservableDictionaryExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 4, "C" },
                { 6, "D" },
            };
            
            var result = dictionary.All(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ObservableDictionaryExtensions_Count_ReturnsCorrectSize()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void ObservableDictionaryExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.Count(x => x.Key % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void ObservableDictionaryExtensions_First_ReturnsFirstItemInObservableDictionary()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.First();

            TheResultingValue(result).ShouldBe(new KeyValuePair<Int32, String>(1, "A"));
        }

        [Test]
        public void ObservableDictionaryExtensions_First_ThrowsExceptionIfObservableDictionaryIsEmpty()
        {
            var dictionary = new ObservableDictionary<Int32, String>();

            Assert.That(() => dictionary.First(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableDictionaryExtensions_Last_ReturnsLastItemInObservableDictionary()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
                { 3, "C" },
            };

            var result = dictionary.Last();

            TheResultingValue(result).ShouldBe(new KeyValuePair<Int32, String>(3, "C"));
        }

        [Test]
        public void ObservableDictionaryExtensions_Last_ThrowsExceptionIfObservableDictionaryIsEmpty()
        {
            var dictionary = new ObservableDictionary<Int32, String>();

            Assert.That(() => dictionary.Last(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableDictionaryExtensions_Single_ReturnsSingleItemInObservableDictionary()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 4, "A" },
            };

            var result = dictionary.Single();

            TheResultingValue(result).ShouldBe(new KeyValuePair<Int32, String>(4, "A"));
        }

        [Test]
        public void ObservableDictionaryExtensions_Single_ThrowsExceptionIfObservableDictionaryIsEmpty()
        {
            var dictionary = new ObservableDictionary<Int32, String>();

            Assert.That(() => dictionary.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableDictionaryExtensions_Single_ThrowsExceptionIfObservableDictionaryHasMultipleItems()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
            };

            Assert.That(() => dictionary.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableDictionaryExtensions_SingleOrDefault_ReturnsSingleItemInObservableDictionary()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 4, "A" },
            };

            var result = dictionary.SingleOrDefault();

            TheResultingValue(result).ShouldBe(new KeyValuePair<Int32, String>(4, "A"));
        }

        [Test]
        public void ObservableDictionaryExtensions_SingleOrDefault_ReturnsDefaultValueIfObservableDictionaryIsEmpty()
        {
            var dictionary = new ObservableDictionary<Int32, String>();

            var result = dictionary.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(KeyValuePair<Int32, String>));
        }

        [Test]
        public void ObservableDictionaryExtensions_SingleOrDefault_ThrowsExceptionIfObservableDictionaryHasMultipleItems()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
            {
                { 1, "A" },
                { 2, "B" },
            };

            Assert.That(() => dictionary.SingleOrDefault(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableDictionaryExtensions_Max_ReturnsMaxValue()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
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

        [Test]
        public void ObservableDictionaryExtensions_Max_ThrowsExceptionIfObservableDictionaryIsEmpty()
        {
            var dictionary = new ObservableDictionary<Int32, String>();

            Assert.That(() => dictionary.Max(x => x.Key),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableDictionaryExtensions_Min_ReturnsMinValue()
        {
            var dictionary = new ObservableDictionary<Int32, String>()
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

        [Test]
        public void ObservableDictionaryExtensions_Min_ThrowsExceptionIfObservableDictionaryIsEmpty()
        {
            var dictionary = new ObservableDictionary<Int32, String>();

            Assert.That(() => dictionary.Min(x => x.Key),
                Throws.TypeOf<InvalidOperationException>());
        }
    }
}

using NUnit.Framework;
using System;
using Ultraviolet.Core.Collections;
using Ultraviolet.Core.Splinq;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.CoreTests.Splinq
{
    [TestFixture]
    public class ObservableListExtensionsTest : CoreTestFramework
    {
        [Test]
        public void ObservableListExtensions_Any_ReturnsTrueIfObservableListContainsItems()
        {
            var list = new ObservableList<Int32>() { 1 };

            var result = list.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ObservableListExtensions_Any_ReturnsFalseIfObservableListDoesNotContainItems()
        {
            var list = new ObservableList<Int32>();

            var result = list.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ObservableListExtensions_AnyWithPredicate_ReturnsTrueIfObservableListContainsMatchingItems()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ObservableListExtensions_AnyWithPredicate_ReturnsFalseIfObservableListDoesNotContainMatchingItems()
        {
            var list = new ObservableList<Int32>() { 1, 3 };

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ObservableListExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var list = new ObservableList<Int32>() { 2, 4, 6 };

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ObservableListExtensions_All_ReturnsTrueIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void ObservableListExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var list = new ObservableList<Int32>() { 1, 2, 4, 6 };

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void ObservableListExtensions_Count_ReturnsCorrectSize()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void ObservableListExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void ObservableListExtensions_First_ReturnsFirstItemInObservableList()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void ObservableListExtensions_First_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            Assert.That(() => list.First(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableListExtensions_Last_ReturnsLastItemInObservableList()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void ObservableListExtensions_Last_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            Assert.That(() => list.Last(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableListExtensions_Single_ReturnsSingleItemInObservableList()
        {
            var list = new ObservableList<Int32>() { 4 };

            var result = list.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void ObservableListExtensions_Single_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            Assert.That(() => list.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableListExtensions_Single_ThrowsExceptionIfObservableListHasMultipleItems()
        {
            var list = new ObservableList<Int32>() { 1, 2 };

            Assert.That(() => list.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableListExtensions_SingleOrDefault_ReturnsSingleItemInObservableList()
        {
            var list = new ObservableList<Int32>() { 4 };

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void ObservableListExtensions_SingleOrDefault_ReturnsDefaultValueIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [Test]
        public void ObservableListExtensions_SingleOrDefault_ThrowsExceptionIfObservableListHasMultipleItems()
        {
            var list = new ObservableList<Int32>() { 1, 2 };

            Assert.That(() => list.SingleOrDefault(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableListExtensions_Max_ReturnsMaxValue()
        {
            var list = new ObservableList<Int32>() { 4, 5, 6, 99, 10, 1, 12, 45 };

            var result = list.Max();

            TheResultingValue(result).ShouldBe(99);
        }

        [Test]
        public void ObservableListExtensions_Max_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            Assert.That(() => list.Max(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObservableListExtensions_Min_ReturnsMinValue()
        {
            var list = new ObservableList<Int32>() { 4, 5, 6, 99, 10, 1, 12, 45 };

            var result = list.Min();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void ObservableListExtensions_Min_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            Assert.That(() => list.Min(),
                Throws.TypeOf<InvalidOperationException>());
        }
    }
}

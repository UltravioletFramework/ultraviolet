using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Collections;
using TwistedLogik.Nucleus.Splinq;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Splinq
{
    [TestClass]
    public class ObservableListExtensionsTest : NucleusTestFramework
    {
        [TestMethod]
        public void ObservableListExtensions_Any_ReturnsTrueIfObservableListContainsItems()
        {
            var list = new ObservableList<Int32>() { 1 };

            var result = list.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableListExtensions_Any_ReturnsFalseIfObservableListDoesNotContainItems()
        {
            var list = new ObservableList<Int32>();

            var result = list.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void ObservableListExtensions_AnyWithPredicate_ReturnsTrueIfObservableListContainsMatchingItems()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableListExtensions_AnyWithPredicate_ReturnsFalseIfObservableListDoesNotContainMatchingItems()
        {
            var list = new ObservableList<Int32>() { 1, 3 };

            var result = list.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void ObservableListExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var list = new ObservableList<Int32>() { 2, 4, 6 };

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableListExtensions_All_ReturnsTrueIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [TestMethod]
        public void ObservableListExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var list = new ObservableList<Int32>() { 1, 2, 4, 6 };

            var result = list.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [TestMethod]
        public void ObservableListExtensions_Count_ReturnsCorrectSize()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        public void ObservableListExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        public void ObservableListExtensions_First_ReturnsFirstItemInObservableList()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObservableListExtensions_First_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            list.First();
        }

        [TestMethod]
        public void ObservableListExtensions_Last_ReturnsLastItemInObservableList()
        {
            var list = new ObservableList<Int32>() { 1, 2, 3 };

            var result = list.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObservableListExtensions_Last_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            list.Last();
        }

        [TestMethod]
        public void ObservableListExtensions_Single_ReturnsSingleItemInObservableList()
        {
            var list = new ObservableList<Int32>() { 4 };

            var result = list.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObservableListExtensions_Single_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            list.Single();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObservableListExtensions_Single_ThrowsExceptionIfObservableListHasMultipleItems()
        {
            var list = new ObservableList<Int32>() { 1, 2 };

            list.Single();
        }

        [TestMethod]
        public void ObservableListExtensions_SingleOrDefault_ReturnsSingleItemInObservableList()
        {
            var list = new ObservableList<Int32>() { 4 };

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [TestMethod]
        public void ObservableListExtensions_SingleOrDefault_ReturnsDefaultValueIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            var result = list.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObservableListExtensions_SingleOrDefault_ThrowsExceptionIfObservableListHasMultipleItems()
        {
            var list = new ObservableList<Int32>() { 1, 2 };

            list.SingleOrDefault();
        }

        [TestMethod]
        public void ObservableListExtensions_Max_ReturnsMaxValue()
        {
            var list = new ObservableList<Int32>() { 4, 5, 6, 99, 10, 1, 12, 45 };

            var result = list.Max();

            TheResultingValue(result).ShouldBe(99);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObservableListExtensions_Max_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            list.Max();
        }

        [TestMethod]
        public void ObservableListExtensions_Min_ReturnsMinValue()
        {
            var list = new ObservableList<Int32>() { 4, 5, 6, 99, 10, 1, 12, 45 };

            var result = list.Min();

            TheResultingValue(result).ShouldBe(1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObservableListExtensions_Min_ThrowsExceptionIfObservableListIsEmpty()
        {
            var list = new ObservableList<Int32>();

            list.Min();
        }
    }
}

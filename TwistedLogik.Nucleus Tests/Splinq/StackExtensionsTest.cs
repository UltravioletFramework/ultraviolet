using NUnit.Framework;
using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus.Splinq;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.NucleusTests.Splinq
{
    [TestFixture]
    public class StackExtensionsTest : NucleusTestFramework
    {
        [Test]
        public void StackExtensions_Any_ReturnsTrueIfStackContainsItems()
        {
            var stack = new Stack<Int32>();
            stack.Push(1);

            var result = stack.Any();

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void StackExtensions_Any_ReturnsFalseIfStackDoesNotContainItems()
        {
            var stack = new Stack<Int32>();

            var result = stack.Any();

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void StackExtensions_AnyWithPredicate_ReturnsTrueIfStackContainsMatchingItems()
        {
            var stack = new Stack<Int32>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            var result = stack.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void StackExtensions_AnyWithPredicate_ReturnsFalseIfStackDoesNotContainMatchingItems()
        {
            var stack = new Stack<Int32>();
            stack.Push(1);
            stack.Push(3);

            var result = stack.Any(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void StackExtensions_All_ReturnsTrueIfAllItemsMatchPredicate()
        {
            var stack = new Stack<Int32>();
            stack.Push(2);
            stack.Push(4);
            stack.Push(6);

            var result = stack.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void StackExtensions_All_ReturnsTrueIfStackIsEmpty()
        {
            var stack = new Stack<Int32>();

            var result = stack.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(true);
        }

        [Test]
        public void StackExtensions_All_ReturnsFalseIfOneItemDoesNotMatchPredicate()
        {
            var stack = new Stack<Int32>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(4);
            stack.Push(6);

            var result = stack.All(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(false);
        }

        [Test]
        public void StackExtensions_Count_ReturnsCorrectSize()
        {
            var stack = new Stack<Int32>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            var result = stack.Count();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void StackExtensions_CountWithPredicate_ReturnsCorrectSize()
        {
            var stack = new Stack<Int32>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            var result = stack.Count(x => x % 2 == 0);

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void StackExtensions_First_ReturnsFirstItemInStack()
        {
            var stack = new Stack<Int32>();
            stack.Push(3);
            stack.Push(2);
            stack.Push(1);

            var result = stack.First();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void StackExtensions_First_ThrowsExceptionIfStackIsEmpty()
        {
            var stack = new Stack<Int32>();

            Assert.That(() => stack.First(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void StackExtensions_Last_ReturnsLastItemInStack()
        {
            var stack = new Stack<Int32>();
            stack.Push(3);
            stack.Push(2);
            stack.Push(1);

            var result = stack.Last();

            TheResultingValue(result).ShouldBe(3);
        }

        [Test]
        public void StackExtensions_Last_ThrowsExceptionIfStackIsEmpty()
        {
            var stack = new Stack<Int32>();

            Assert.That(() => stack.Last(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void StackExtensions_Single_ReturnsSingleItemInStack()
        {
            var stack = new Stack<Int32>();
            stack.Push(4);

            var result = stack.Single();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void StackExtensions_Single_ThrowsExceptionIfStackIsEmpty()
        {
            var stack = new Stack<Int32>();

            Assert.That(() => stack.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void StackExtensions_Single_ThrowsExceptionIfStackHasMultipleItems()
        {
            var stack = new Stack<Int32>();
            stack.Push(1);
            stack.Push(2);

            Assert.That(() => stack.Single(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void StackExtensions_SingleOrDefault_ReturnsSingleItemInStack()
        {
            var stack = new Stack<Int32>();
            stack.Push(4);

            var result = stack.SingleOrDefault();

            TheResultingValue(result).ShouldBe(4);
        }

        [Test]
        public void StackExtensions_SingleOrDefault_ReturnsDefaultValueIfStackIsEmpty()
        {
            var stack = new Stack<Int32>();

            var result = stack.SingleOrDefault();

            TheResultingValue(result).ShouldBe(default(Int32));
        }

        [Test]
        public void StackExtensions_SingleOrDefault_ThrowsExceptionIfStackHasMultipleItems()
        {
            var stack = new Stack<Int32>();
            stack.Push(1);
            stack.Push(2);

            Assert.That(() => stack.SingleOrDefault(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void StackExtensions_Max_ReturnsMaxValue()
        {
            var stack = new Stack<Int32>();
            stack.Push(4);
            stack.Push(5);
            stack.Push(6);
            stack.Push(99);
            stack.Push(10);
            stack.Push(1);
            stack.Push(12);
            stack.Push(45);

            var result = stack.Max();

            TheResultingValue(result).ShouldBe(99);
        }

        [Test]
        public void StackExtensions_Max_ThrowsExceptionIfStackIsEmpty()
        {
            var stack = new Stack<Int32>();

            Assert.That(() => stack.Max(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void StackExtensions_Min_ReturnsMinValue()
        {
            var stack = new Stack<Int32>();
            stack.Push(4);
            stack.Push(5);
            stack.Push(6);
            stack.Push(99);
            stack.Push(10);
            stack.Push(1);
            stack.Push(12);
            stack.Push(45);

            var result = stack.Min();

            TheResultingValue(result).ShouldBe(1);
        }

        [Test]
        public void StackExtensions_Min_ThrowsExceptionIfStackIsEmpty()
        {
            var stack = new Stack<Int32>();

            Assert.That(() => stack.Min(),
                Throws.TypeOf<InvalidOperationException>());
        }
    }
}

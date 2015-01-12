using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class Point2Tests : UltravioletTestFramework
    {
        [TestMethod]
        public void Point2_IsConstructedProperly()
        {
            var result = new Point2(123, 456);

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [TestMethod]
        public void Point2_OpEquality()
        {
            var size1 = new Point2(123, 456);
            var size2 = new Point2(123, 456);
            var size3 = new Point2(123, 555);
            var size4 = new Point2(222, 456);

            TheResultingValue(size1 == size2).ShouldBe(true);
            TheResultingValue(size1 == size3).ShouldBe(false);
            TheResultingValue(size1 == size4).ShouldBe(false);
        }

        [TestMethod]
        public void Point2_OpInequality()
        {
            var size1 = new Point2(123, 456);
            var size2 = new Point2(123, 456);
            var size3 = new Point2(123, 555);
            var size4 = new Point2(222, 456);

            TheResultingValue(size1 != size2).ShouldBe(false);
            TheResultingValue(size1 != size3).ShouldBe(true);
            TheResultingValue(size1 != size4).ShouldBe(true);
        }

        [TestMethod]
        public void Point2_EqualsObject()
        {
            var size1 = new Point2(123, 456);
            var size2 = new Point2(123, 456);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Point2_EqualsPoint()
        {
            var size1 = new Point2(123, 456);
            var size2 = new Point2(123, 456);
            var size3 = new Point2(123, 555);
            var size4 = new Point2(222, 456);

            TheResultingValue(size1.Equals(size2)).ShouldBe(true);
            TheResultingValue(size1.Equals(size3)).ShouldBe(false);
            TheResultingValue(size1.Equals(size4)).ShouldBe(false);
        }

        [TestMethod]
        public void Point2_TryParse_SucceedsForValidStrings()
        {
            var str    = "123 456";
            var result = default(Point2);
            if (!Point2.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Point.");

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [TestMethod]
        public void Point2_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Point2);
            var succeeded = Point2.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void Point2_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456";
            var result = Point2.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Point2_Parse_FailsForInvalidStrings()
        {
            Point2.Parse("foo");
        }

        [TestMethod]
        public void Point2_Parse_CanRoundTrip()
        {
            var size1 = Point2.Parse("123 456");
            var size2 = Point2.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }
    }
}

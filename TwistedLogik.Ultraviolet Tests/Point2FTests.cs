using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class Point2FTests : UltravioletTestFramework
    {
        [TestMethod]
        public void Point2F_IsConstructedProperly()
        {
            var result = new Point2F(123.45f, 456.78f);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [TestMethod]
        public void Point2F_OpEquality()
        {
            var size1 = new Point2F(123.45f, 456.78f);
            var size2 = new Point2F(123.45f, 456.78f);
            var size3 = new Point2F(123.45f, 555.55f);
            var size4 = new Point2F(222.22f, 456.78f);

            TheResultingValue(size1 == size2).ShouldBe(true);
            TheResultingValue(size1 == size3).ShouldBe(false);
            TheResultingValue(size1 == size4).ShouldBe(false);
        }

        [TestMethod]
        public void Point2F_OpInequality()
        {
            var size1 = new Point2F(123.45f, 456.78f);
            var size2 = new Point2F(123.45f, 456.78f);
            var size3 = new Point2F(123.45f, 555.55f);
            var size4 = new Point2F(222.22f, 456.78f);

            TheResultingValue(size1 != size2).ShouldBe(false);
            TheResultingValue(size1 != size3).ShouldBe(true);
            TheResultingValue(size1 != size4).ShouldBe(true);
        }

        [TestMethod]
        public void Point2F_EqualsObject()
        {
            var size1 = new Point2F(123.45f, 456.78f);
            var size2 = new Point2F(123.45f, 456.78f);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Point2F_EqualsPoint2F()
        {
            var size1 = new Point2F(123.45f, 456.78f);
            var size2 = new Point2F(123.45f, 456.78f);
            var size3 = new Point2F(123.45f, 555.55f);
            var size4 = new Point2F(222.22f, 456.78f);

            TheResultingValue(size1.Equals(size2)).ShouldBe(true);
            TheResultingValue(size1.Equals(size3)).ShouldBe(false);
            TheResultingValue(size1.Equals(size4)).ShouldBe(false);
        }

        [TestMethod]
        public void Point2F_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = default(Point2F);
            if (!Point2F.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Point2F.");

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [TestMethod]
        public void Point2F_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Point2F);
            var succeeded = Point2F.TryParse("foo", out result);

            TheResultingValue(succeeded)
                .ShouldBe(false);
        }

        [TestMethod]
        public void Point2F_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = Point2F.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Point2F_Parse_FailsForInvalidStrings()
        {
            Point2F.Parse("foo");
        }

        [TestMethod]
        public void Point2F_Parse_CanRoundTrip()
        {
            var size1 = Point2F.Parse("123.4 456.7");
            var size2 = Point2F.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }
    }
}

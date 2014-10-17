using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class CircleTests : UltravioletTestFramework
    {
        [TestMethod]
        public void Circle_IsConstructedProperly()
        {
            var result = new Circle(123, 456, 100);

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveRadius(100);
        }

        [TestMethod]
        public void Circle_OpEquality()
        {
            var circle1 = new Circle(123, 456, 100);
            var circle2 = new Circle(123, 456, 100);
            var circle3 = new Circle(123, 555, 100);
            var circle4 = new Circle(222, 456, 100);
            var circle5 = new Circle(123, 456, 200);

            TheResultingValue(circle1 == circle2).ShouldBe(true);
            TheResultingValue(circle1 == circle3).ShouldBe(false);
            TheResultingValue(circle1 == circle4).ShouldBe(false);
            TheResultingValue(circle1 == circle5).ShouldBe(false);
        }

        [TestMethod]
        public void Circle_OpInequality()
        {
            var circle1 = new Circle(123, 456, 100);
            var circle2 = new Circle(123, 456, 100);
            var circle3 = new Circle(123, 555, 100);
            var circle4 = new Circle(222, 456, 100);
            var circle5 = new Circle(123, 456, 200);

            TheResultingValue(circle1 != circle2).ShouldBe(false);
            TheResultingValue(circle1 != circle3).ShouldBe(true);
            TheResultingValue(circle1 != circle4).ShouldBe(true);
            TheResultingValue(circle1 != circle5).ShouldBe(true);
        }

        [TestMethod]
        public void Circle_EqualsObject()
        {
            var circle1 = new Circle(123, 456, 100);
            var circle2 = new Circle(123, 456, 100);

            TheResultingValue(circle1.Equals((Object)circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Circle_EqualsCircle()
        {
            var circle1 = new Circle(123, 456, 100);
            var circle2 = new Circle(123, 456, 100);
            var circle3 = new Circle(123, 555, 100);
            var circle4 = new Circle(222, 456, 100);

            TheResultingValue(circle1.Equals(circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals(circle3)).ShouldBe(false);
            TheResultingValue(circle1.Equals(circle4)).ShouldBe(false);
        }

        [TestMethod]
        public void Circle_TryParse_SucceedsForValidStrings()
        {
            var str    = "123 456 100";
            var result = default(Circle);
            if (!Circle.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Circle.");

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveRadius(100);
        }

        [TestMethod]
        public void Circle_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Circle);
            var succeeded = Circle.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void Circle_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456 100";
            var result = Circle.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveRadius(100);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Circle_Parse_FailsForInvalidStrings()
        {
            Circle.Parse("foo");
        }

        [TestMethod]
        public void Circle_Parse_CanRoundTrip()
        {
            var circle1 = CircleF.Parse("123 456 100");
            var circle2 = CircleF.Parse(circle1.ToString());

            TheResultingValue(circle1 == circle2).ShouldBe(true);
        }
    }
}

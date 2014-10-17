using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class RectangleTests : UltravioletTestFramework
    {
        [TestMethod]
        public void Rectangle_IsConstructedProperly()
        {
            var result = new Rectangle(123, 456, 789, 999);

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveDimensions(789, 999);
        }

        [TestMethod]
        public void Rectangle_OpEquality()
        {
            var rectangle1 = new Rectangle(123, 456, 789, 999);
            var rectangle2 = new Rectangle(123, 456, 789, 999);
            var rectangle3 = new Rectangle(222, 456, 789, 999);
            var rectangle4 = new Rectangle(123, 333, 789, 999);
            var rectangle5 = new Rectangle(123, 456, 444, 999);
            var rectangle6 = new Rectangle(123, 456, 789, 555);

            TheResultingValue(rectangle1 == rectangle2).ShouldBe(true);
            TheResultingValue(rectangle1 == rectangle3).ShouldBe(false);
            TheResultingValue(rectangle1 == rectangle4).ShouldBe(false);
            TheResultingValue(rectangle1 == rectangle5).ShouldBe(false);
            TheResultingValue(rectangle1 == rectangle6).ShouldBe(false);
        }

        [TestMethod]
        public void Rectangle_OpInequality()
        {
            var rectangle1 = new Rectangle(123, 456, 789, 999);
            var rectangle2 = new Rectangle(123, 456, 789, 999);
            var rectangle3 = new Rectangle(222, 456, 789, 999);
            var rectangle4 = new Rectangle(123, 333, 789, 999);
            var rectangle5 = new Rectangle(123, 456, 444, 999);
            var rectangle6 = new Rectangle(123, 456, 789, 555);

            TheResultingValue(rectangle1 != rectangle2).ShouldBe(false);
            TheResultingValue(rectangle1 != rectangle3).ShouldBe(true);
            TheResultingValue(rectangle1 != rectangle4).ShouldBe(true);
            TheResultingValue(rectangle1 != rectangle5).ShouldBe(true);
            TheResultingValue(rectangle1 != rectangle6).ShouldBe(true);
        }

        [TestMethod]
        public void Rectangle_EqualsObject()
        {
            var rectangle1 = new Rectangle(123, 456, 789, 999);
            var rectangle2 = new Rectangle(123, 456, 789, 999);

            TheResultingValue(rectangle1.Equals((Object)rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Rectangle_EqualsRectangle()
        {
            var rectangle1 = new Rectangle(123, 456, 789, 999);
            var rectangle2 = new Rectangle(123, 456, 789, 999);
            var rectangle3 = new Rectangle(222, 456, 789, 999);
            var rectangle4 = new Rectangle(123, 333, 789, 999);
            var rectangle5 = new Rectangle(123, 456, 444, 999);
            var rectangle6 = new Rectangle(123, 456, 789, 555);

            TheResultingValue(rectangle1.Equals(rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals(rectangle3)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle4)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle5)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle6)).ShouldBe(false);
        }

        [TestMethod]
        public void Rectangle_TryParse_SucceedsForValidStrings()
        {
            var str    = "123 456 789 999";
            var result = default(Rectangle);
            if (!Rectangle.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Rectangle.");

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveDimensions(789, 999);
        }

        [TestMethod]
        public void Rectangle_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Rectangle);
            var succeeded = Rectangle.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void Rectangle_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456 789 999";
            var result = Rectangle.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveDimensions(789, 999);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Rectangle_Parse_FailsForInvalidStrings()
        {
            Rectangle.Parse("foo");
        }

        [TestMethod]
        public void Rectangle_Parse_CanRoundTrip()
        {
            var rect1 = Rectangle.Parse("123 456 789 234");
            var rect2 = Rectangle.Parse(rect1.ToString());

            TheResultingValue(rect1 == rect2).ShouldBe(true);
        }
    }
}

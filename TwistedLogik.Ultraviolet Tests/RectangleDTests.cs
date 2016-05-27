using System;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestFixture]
    public class RectangleDTests : UltravioletTestFramework
    {
        [Test]
        public void RectangleD_IsConstructedProperly()
        {
            var result = new RectangleD(123.45, 456.78, 789.99, 999.99);

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveDimensions(789.99, 999.99);
        }

        [Test]
        public void RectangleD_OpEquality()
        {
            var rectangle1 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle2 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle3 = new RectangleD(222, 456.78, 789.99, 999.99);
            var rectangle4 = new RectangleD(123.45, 333, 789.99, 999.99);
            var rectangle5 = new RectangleD(123.45, 456.78, 444, 999.99);
            var rectangle6 = new RectangleD(123.45, 456.78, 789.99, 555);

            Assert.AreEqual(true, rectangle1 == rectangle2);
            Assert.AreEqual(false, rectangle1 == rectangle3);
            Assert.AreEqual(false, rectangle1 == rectangle4);
            Assert.AreEqual(false, rectangle1 == rectangle5);
            Assert.AreEqual(false, rectangle1 == rectangle6);
        }

        [Test]
        public void RectangleD_OpInequality()
        {
            var rectangle1 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle2 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle3 = new RectangleD(222, 456.78, 789.99, 999.99);
            var rectangle4 = new RectangleD(123.45, 333, 789.99, 999.99);
            var rectangle5 = new RectangleD(123.45, 456.78, 444, 999.99);
            var rectangle6 = new RectangleD(123.45, 456.78, 789.99, 555);

            Assert.AreEqual(false, rectangle1 != rectangle2);
            Assert.AreEqual(true, rectangle1 != rectangle3);
            Assert.AreEqual(true, rectangle1 != rectangle4);
            Assert.AreEqual(true, rectangle1 != rectangle5);
            Assert.AreEqual(true, rectangle1 != rectangle6);
        }

        [Test]
        public void RectangleD_EqualsObject()
        {
            var rectangle1 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle2 = new RectangleD(123.45, 456.78, 789.99, 999.99);

            TheResultingValue(rectangle1.Equals((Object)rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void RectangleD_EqualsRectangleD()
        {
            var rectangle1 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle2 = new RectangleD(123.45, 456.78, 789.99, 999.99);
            var rectangle3 = new RectangleD(222, 456.78, 789.99, 999.99);
            var rectangle4 = new RectangleD(123.45, 333, 789.99, 999.99);
            var rectangle5 = new RectangleD(123.45, 456.78, 444, 999.99);
            var rectangle6 = new RectangleD(123.45, 456.78, 789.99, 555);

            TheResultingValue(rectangle1.Equals(rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals(rectangle3)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle4)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle5)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle6)).ShouldBe(false);
        }

        [Test]
        public void RectangleD_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99 999.99";
            var result = default(RectangleD);
            if (!RectangleD.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to RectangleD.");

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveDimensions(789.99, 999.99);
        }

        [Test]
        public void RectangleD_TryParse_FailsForInvalidStrings()
        {
            var result    = default(RectangleD);
            var succeeded = RectangleD.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void RectangleD_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99 999.99";
            var result = RectangleD.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveDimensions(789.99, 999.99);
        }

        [Test]
        public void RectangleD_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => RectangleD.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void RectangleD_Parse_CanRoundTrip()
        {
            var rect1 = RectangleD.Parse("123.4 456.7 789.1 234.5");
            var rect2 = RectangleD.Parse(rect1.ToString());

            TheResultingValue(rect1 == rect2).ShouldBe(true);
        }

        [Test]
        public void RectangleD_SerializesToJson()
        {
            var converter = new UltravioletJsonConverter();
            var rect = new RectangleD(1.2, 2.3, 3.4, 4.5);
            var json = JsonConvert.SerializeObject(rect, converter);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}");
        }

        [Test]
        public void RectangleD_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}";

            var converter = new UltravioletJsonConverter();
            var rect = JsonConvert.DeserializeObject<RectangleD>(json, converter);

            TheResultingValue(rect)
                .ShouldHavePosition(1.2, 2.3)
                .ShouldHaveDimensions(3.4, 4.5);
        }
    }
}

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
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
        public void RectangleD_SerializesToJson()
        {
            var rect = new RectangleD(1.2, 2.3, 3.4, 4.5);
            var json = JsonConvert.SerializeObject(rect,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}");
        }

        [Test]
        public void RectangleD_SerializesToJson_WhenNullable()
        {
            var rect = new RectangleD(1.2, 2.3, 3.4, 4.5);
            var json = JsonConvert.SerializeObject((RectangleD?)rect,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}");
        }

        [Test]
        public void RectangleD_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}";
            
            var rect = JsonConvert.DeserializeObject<RectangleD>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect)
                .ShouldHavePosition(1.2, 2.3)
                .ShouldHaveDimensions(3.4, 4.5);
        }

        [Test]
        public void RectangleD_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}";

            var rect1 = JsonConvert.DeserializeObject<RectangleD?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect1.Value)
                .ShouldHavePosition(1.2, 2.3)
                .ShouldHaveDimensions(3.4, 4.5);

            const String json2 = @"null";

            var rect2 = JsonConvert.DeserializeObject<RectangleD?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect2.HasValue)
                .ShouldBe(false);
        }
    }
}

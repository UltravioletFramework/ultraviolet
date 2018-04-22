using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class RectangleTests : UltravioletTestFramework
    {
        [Test]
        public void Rectangle_IsConstructedProperly()
        {
            var result = new Rectangle(123, 456, 789, 999);

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveDimensions(789, 999);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Rectangle_EqualsObject()
        {
            var rectangle1 = new Rectangle(123, 456, 789, 999);
            var rectangle2 = new Rectangle(123, 456, 789, 999);

            TheResultingValue(rectangle1.Equals((Object)rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Rectangle_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Rectangle);
            var succeeded = Rectangle.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Rectangle_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456 789 999";
            var result = Rectangle.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveDimensions(789, 999);
        }

        [Test]
        public void Rectangle_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Rectangle.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }
        
        [Test]
        public void Rectangle_SerializesToJson()
        {
            var rect = new Rectangle(1, 2, 3, 4);
            var json = JsonConvert.SerializeObject(rect,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1,""y"":2,""width"":3,""height"":4}");
        }

        [Test]
        public void Rectangle_SerializesToJson_WhenNullable()
        {
            var rect = new Rectangle(1, 2, 3, 4);
            var json = JsonConvert.SerializeObject((Rectangle?)rect,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1,""y"":2,""width"":3,""height"":4}");
        }

        [Test]
        public void Rectangle_DeserializesFromJson()
        {
            const String json = @"{""x"":1,""y"":2,""width"":3,""height"":4}";
            
            var rect = JsonConvert.DeserializeObject<Rectangle>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect)
                .ShouldHavePosition(1, 2)
                .ShouldHaveDimensions(3, 4);
        }

        [Test]
        public void Rectangle_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1,""y"":2,""width"":3,""height"":4}";

            var rect1 = JsonConvert.DeserializeObject<Rectangle?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect1.Value)
                .ShouldHavePosition(1, 2)
                .ShouldHaveDimensions(3, 4);

            const String json2 = @"null";

            var rect2 = JsonConvert.DeserializeObject<Rectangle?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect2.HasValue)
                .ShouldBe(false);
        }
    }
}

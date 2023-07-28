using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class RectangleFTests : UltravioletTestFramework
    {
        [Test]
        public void RectangleF_IsConstructedProperly()
        {
            var result = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveDimensions(789.99f, 999.99f);
        }

        [Test]
        public void RectangleF_OpEquality()
        {
            var rectangle1 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle2 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle3 = new RectangleF(222f, 456.78f, 789.99f, 999.99f);
            var rectangle4 = new RectangleF(123.45f, 333f, 789.99f, 999.99f);
            var rectangle5 = new RectangleF(123.45f, 456.78f, 444f, 999.99f);
            var rectangle6 = new RectangleF(123.45f, 456.78f, 789.99f, 555f);

            Assert.AreEqual(true, rectangle1 == rectangle2);
            Assert.AreEqual(false, rectangle1 == rectangle3);
            Assert.AreEqual(false, rectangle1 == rectangle4);
            Assert.AreEqual(false, rectangle1 == rectangle5);
            Assert.AreEqual(false, rectangle1 == rectangle6);
        }

        [Test]
        public void RectangleF_OpInequality()
        {
            var rectangle1 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle2 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle3 = new RectangleF(222f, 456.78f, 789.99f, 999.99f);
            var rectangle4 = new RectangleF(123.45f, 333f, 789.99f, 999.99f);
            var rectangle5 = new RectangleF(123.45f, 456.78f, 444f, 999.99f);
            var rectangle6 = new RectangleF(123.45f, 456.78f, 789.99f, 555f);

            Assert.AreEqual(false, rectangle1 != rectangle2);
            Assert.AreEqual(true, rectangle1 != rectangle3);
            Assert.AreEqual(true, rectangle1 != rectangle4);
            Assert.AreEqual(true, rectangle1 != rectangle5);
            Assert.AreEqual(true, rectangle1 != rectangle6);
        }

        [Test]
        public void RectangleF_EqualsObject()
        {
            var rectangle1 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle2 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);

            TheResultingValue(rectangle1.Equals((Object)rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void RectangleF_EqualsRectangleF()
        {
            var rectangle1 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle2 = new RectangleF(123.45f, 456.78f, 789.99f, 999.99f);
            var rectangle3 = new RectangleF(222f, 456.78f, 789.99f, 999.99f);
            var rectangle4 = new RectangleF(123.45f, 333f, 789.99f, 999.99f);
            var rectangle5 = new RectangleF(123.45f, 456.78f, 444f, 999.99f);
            var rectangle6 = new RectangleF(123.45f, 456.78f, 789.99f, 555f);

            TheResultingValue(rectangle1.Equals(rectangle2)).ShouldBe(true);
            TheResultingValue(rectangle1.Equals(rectangle3)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle4)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle5)).ShouldBe(false);
            TheResultingValue(rectangle1.Equals(rectangle6)).ShouldBe(false);
        }

        [Test]
        public void RectangleF_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99 999.99";
            var result = default(RectangleF);
            if (!RectangleF.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to RectangleF.");

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveDimensions(789.99f, 999.99f);
        }

        [Test]
        public void RectangleF_TryParse_FailsForInvalidStrings()
        {
            var result    = default(RectangleF);
            var succeeded = RectangleF.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void RectangleF_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99 999.99";
            var result = RectangleF.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveDimensions(789.99f, 999.99f);
        }

        [Test]
        public void RectangleF_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => RectangleF.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }
        
        [Test]
        public void RectangleF_SerializesToJson()
        {
            var rect = new RectangleF(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject(rect,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}");
        }

        [Test]
        public void RectangleF_SerializesToJson_WhenNullable()
        {
            var rect = new RectangleF(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject((RectangleF?)rect,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}");
        }

        [Test]
        public void RectangleF_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}";
            
            var rect = JsonConvert.DeserializeObject<RectangleF>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect)
                .ShouldHavePosition(1.2f, 2.3f)
                .ShouldHaveDimensions(3.4f, 4.5f);
        }

        [Test]
        public void RectangleF_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1.2,""y"":2.3,""width"":3.4,""height"":4.5}";

            var rect1 = JsonConvert.DeserializeObject<RectangleF?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect1.Value)
                .ShouldHavePosition(1.2f, 2.3f)
                .ShouldHaveDimensions(3.4f, 4.5f);

            const String json2 = @"null";

            var rect2 = JsonConvert.DeserializeObject<RectangleF?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(rect2.HasValue)
                .ShouldBe(false);
        }
    }
}

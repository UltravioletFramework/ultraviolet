using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class CircleDTests : UltravioletTestFramework
    {
        [Test]
        public void CircleD_IsConstructedProperly()
        {
            var result = new CircleD(123.45, 456.78, 100.10);

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveRadius(100.10);
        }

        [Test]
        public void CircleD_OpEquality()
        {
            var circle1 = new CircleD(123.45, 456.78, 100.10);
            var circle2 = new CircleD(123.45, 456.78, 100.10);
            var circle3 = new CircleD(123.45, 555, 100.10);
            var circle4 = new CircleD(222, 456.78, 100.10);
            var circle5 = new CircleD(123.45, 456.78, 200);

            TheResultingValue(circle1 == circle2).ShouldBe(true);
            TheResultingValue(circle1 == circle3).ShouldBe(false);
            TheResultingValue(circle1 == circle4).ShouldBe(false);
            TheResultingValue(circle1 == circle5).ShouldBe(false);
        }

        [Test]
        public void CircleD_OpInequality()
        {
            var circle1 = new CircleD(123.45, 456.78, 100.10);
            var circle2 = new CircleD(123.45, 456.78, 100.10);
            var circle3 = new CircleD(123.45, 555, 100.10);
            var circle4 = new CircleD(222, 456.78, 100.10);
            var circle5 = new CircleD(123.45, 456.78, 200);

            TheResultingValue(circle1 != circle2).ShouldBe(false);
            TheResultingValue(circle1 != circle3).ShouldBe(true);
            TheResultingValue(circle1 != circle4).ShouldBe(true);
            TheResultingValue(circle1 != circle5).ShouldBe(true);
        }

        [Test]
        public void CircleD_EqualsObject()
        {
            var circle1 = new CircleD(123.45, 456.78, 100.10);
            var circle2 = new CircleD(123.45, 456.78, 100.10);

            TheResultingValue(circle1.Equals((Object)circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void CircleD_EqualsCircleD()
        {
            var circle1 = new CircleD(123.45, 456.78, 100.10);
            var circle2 = new CircleD(123.45, 456.78, 100.10);
            var circle3 = new CircleD(123.45, 555.55, 100.10);
            var circle4 = new CircleD(222.22, 456.78, 100.10);

            TheResultingValue(circle1.Equals(circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals(circle3)).ShouldBe(false);
            TheResultingValue(circle1.Equals(circle4)).ShouldBe(false);
        }

        [Test]
        public void CircleD_TryParse_SucceedsForValidStrings()
        {
            var str = "123.45 456.78 100.10";
            var result = default(CircleD);
            if (!CircleD.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to CircleD.");

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveRadius(100.10);
        }

        [Test]
        public void CircleD_TryParse_FailsForInvalidStrings()
        {
            var result    = default(CircleD);
            var succeeded = CircleD.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void CircleD_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 100.10";
            var result = CircleD.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123.45, 456.78)
                .ShouldHaveRadius(100.10);
        }

        [Test]
        public void CircleD_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => CircleD.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void CircleD_SerializesToJson()
        {
            var circle = new CircleD(1.2, 2.3, 3.4);
            var json = JsonConvert.SerializeObject(circle, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""radius"":3.4}");
        }

        [Test]
        public void CircleD_SerializesToJson_WhenNullable()
        {
            var circle = new CircleD(1.2, 2.3, 3.4);
            var json = JsonConvert.SerializeObject((CircleD?)circle, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""radius"":3.4}");
        }

        [Test]
        public void CircleD_DeserializesFromJson()
        {
            const String json = @"{ ""x"":1.2,""y"":2.3,""radius"":3.4 }";
            
            var circle = JsonConvert.DeserializeObject<CircleD>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle)
                .ShouldHavePosition(1.2, 2.3)
                .ShouldHaveRadius(3.4);
        }

        [Test]
        public void CircleD_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{ ""x"":1.2,""y"":2.3,""radius"":3.4 }";

            var circle1 = JsonConvert.DeserializeObject<CircleD?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle1.Value)
                .ShouldHavePosition(1.2, 2.3)
                .ShouldHaveRadius(3.4);

            const String json2 = @"null";

            var circle2 = JsonConvert.DeserializeObject<CircleD?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle2.HasValue)
                .ShouldBe(false);
        }
    }
}

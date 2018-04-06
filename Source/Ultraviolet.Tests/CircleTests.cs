using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class CircleTests : UltravioletTestFramework
    {
        [Test]
        public void Circle_IsConstructedProperly()
        {
            var result = new Circle(123, 456, 100);

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveRadius(100);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Circle_EqualsObject()
        {
            var circle1 = new Circle(123, 456, 100);
            var circle2 = new Circle(123, 456, 100);

            TheResultingValue(circle1.Equals((Object)circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Circle_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Circle);
            var succeeded = Circle.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Circle_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456 100";
            var result = Circle.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123, 456)
                .ShouldHaveRadius(100);
        }

        [Test]
        public void Circle_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Circle.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Circle_SerializesToJson()
        {
            var circle = new Circle(1, 2, 3);
            var json = JsonConvert.SerializeObject(circle,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1,""y"":2,""radius"":3}");
        }

        [Test]
        public void Circle_SerializesToJson_WhenNullable()
        {
            var circle = new Circle(1, 2, 3);
            var json = JsonConvert.SerializeObject((Circle?)circle, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1,""y"":2,""radius"":3}");
        }

        [Test]
        public void CircleF_DeserializesFromJson()
        {
            const String json = @"{ ""x"":1,""y"":2,""radius"":3 }";
            
            var circle = JsonConvert.DeserializeObject<Circle>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle)
                .ShouldHavePosition(1, 2)
                .ShouldHaveRadius(3);
        }

        [Test]
        public void CircleF_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{ ""x"":1,""y"":2,""radius"":3 }";

            var circle1 = JsonConvert.DeserializeObject<Circle?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle1.Value)
                .ShouldHavePosition(1, 2)
                .ShouldHaveRadius(3);

            const String json2 = @"null";

            var circle2 = JsonConvert.DeserializeObject<Circle?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle2.HasValue)
                .ShouldBe(false);
        }
    }
}

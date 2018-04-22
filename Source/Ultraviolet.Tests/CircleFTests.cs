using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class CircleFTests : UltravioletTestFramework
    {
        [Test]
        public void CircleF_IsConstructedProperly()
        {
            var result = new CircleF(123.45f, 456.78f, 100.10f);

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveRadius(100.10f);
        }

        [Test]
        public void CircleF_OpEquality()
        {
            var circle1 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle2 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle3 = new CircleF(123.45f, 555, 100.10f);
            var circle4 = new CircleF(222, 456.78f, 100.10f);
            var circle5 = new CircleF(123.45f, 456.78f, 200);

            TheResultingValue(circle1 == circle2).ShouldBe(true);
            TheResultingValue(circle1 == circle3).ShouldBe(false);
            TheResultingValue(circle1 == circle4).ShouldBe(false);
            TheResultingValue(circle1 == circle5).ShouldBe(false);
        }

        [Test]
        public void CircleF_OpInequality()
        {
            var circle1 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle2 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle3 = new CircleF(123.45f, 555, 100.10f);
            var circle4 = new CircleF(222, 456.78f, 100.10f);
            var circle5 = new CircleF(123.45f, 456.78f, 200);

            TheResultingValue(circle1 != circle2).ShouldBe(false);
            TheResultingValue(circle1 != circle3).ShouldBe(true);
            TheResultingValue(circle1 != circle4).ShouldBe(true);
            TheResultingValue(circle1 != circle5).ShouldBe(true);
        }

        [Test]
        public void CircleF_EqualsObject()
        {
            var circle1 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle2 = new CircleF(123.45f, 456.78f, 100.10f);

            TheResultingValue(circle1.Equals((Object)circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void CircleF_EqualsCircleF()
        {
            var circle1 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle2 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle3 = new CircleF(123.45f, 555.55f, 100.10f);
            var circle4 = new CircleF(222.22f, 456.78f, 100.10f);

            TheResultingValue(circle1.Equals(circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals(circle3)).ShouldBe(false);
            TheResultingValue(circle1.Equals(circle4)).ShouldBe(false);
        }

        [Test]
        public void CircleF_TryParse_SucceedsForValidStrings()
        {
            var str = "123.45 456.78 100.10";
            var result = default(CircleF);
            if (!CircleF.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to CircleF.");

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveRadius(100.10f);
        }

        [Test]
        public void CircleF_TryParse_FailsForInvalidStrings()
        {
            var result    = default(CircleF);
            var succeeded = CircleF.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void CircleF_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 100.10";
            var result = CircleF.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveRadius(100.10f);
        }

        [Test]
        public void CircleF_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => CircleF.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void CircleF_SerializesToJson()
        {
            var circle = new CircleF(1.2f, 2.3f, 3.4f);
            var json = JsonConvert.SerializeObject(circle,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""radius"":3.4}");
        }

        [Test]
        public void CircleF_SerializesToJson_WhenNullable()
        {
            var circle = new CircleF(1.2f, 2.3f, 3.4f);
            var json = JsonConvert.SerializeObject((CircleF?)circle,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3,""radius"":3.4}");
        }

        [Test]
        public void CircleF_DeserializesFromJson()
        {
            const String json = @"{ ""x"":1.2,""y"":2.3,""radius"":3.4 }";
            
            var circle = JsonConvert.DeserializeObject<CircleF>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle)
                .ShouldHavePosition(1.2f, 2.3f)
                .ShouldHaveRadius(3.4f);
        }

        [Test]
        public void CircleF_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{ ""x"":1.2,""y"":2.3,""radius"":3.4 }";

            var circle1 = JsonConvert.DeserializeObject<CircleF?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle1.Value)
                .ShouldHavePosition(1.2f, 2.3f)
                .ShouldHaveRadius(3.4f);

            const String json2 = @"null";

            var circle2 = JsonConvert.DeserializeObject<CircleF?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(circle2.HasValue)
                .ShouldBe(false);
        }
    }
}

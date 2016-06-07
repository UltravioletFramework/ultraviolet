using System;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestFixture]
    public class Point2FTests : UltravioletTestFramework
    {
        [Test]
        public void Point2F_IsConstructedProperly()
        {
            var result = new Point2F(123.45f, 456.78f);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Point2F_EqualsObject()
        {
            var size1 = new Point2F(123.45f, 456.78f);
            var size2 = new Point2F(123.45f, 456.78f);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
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

        [Test]
        public void Point2F_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = default(Point2F);
            if (!Point2F.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Point2F.");

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [Test]
        public void Point2F_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Point2F);
            var succeeded = Point2F.TryParse("foo", out result);

            TheResultingValue(succeeded)
                .ShouldBe(false);
        }

        [Test]
        public void Point2F_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = Point2F.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [Test]
        public void Point2F_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Point2F.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Point2F_Parse_CanRoundTrip()
        {
            var size1 = Point2F.Parse("123.4 456.7");
            var size2 = Point2F.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }

        [Test]
        public void Point2F_SerializesToJson()
        {
            var point = new Point2F(1.2f, 2.3f);
            var json = JsonConvert.SerializeObject(point);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Point2F_SerializesToJson_WhenNullable()
        {
            var point = new Point2F(1.2f, 2.3f);
            var json = JsonConvert.SerializeObject((Point2F?)point);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Point2F_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3}";
            
            var point = JsonConvert.DeserializeObject<Point2F>(json);

            TheResultingValue(point)
                .ShouldBe(1.2f, 2.3f);
        }

        [Test]
        public void Point2F_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1.2,""y"":2.3}";

            var point1 = JsonConvert.DeserializeObject<Point2F?>(json1);

            TheResultingValue(point1.Value)
                .ShouldBe(1.2f, 2.3f);

            const String json2 = @"null";

            var point2 = JsonConvert.DeserializeObject<Point2F?>(json2);

            TheResultingValue(point2.HasValue)
                .ShouldBe(false);
        }
    }
}

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestFixture]
    public class Point2DTests : UltravioletTestFramework
    {
        [Test]
        public void Point2D_IsConstructedProperly()
        {
            var result = new Point2D(123.45, 456.78);

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78);
        }

        [Test]
        public void Point2D_OpEquality()
        {
            var size1 = new Point2D(123.45, 456.78);
            var size2 = new Point2D(123.45, 456.78);
            var size3 = new Point2D(123.45, 555.55);
            var size4 = new Point2D(222.22, 456.78);

            TheResultingValue(size1 == size2).ShouldBe(true);
            TheResultingValue(size1 == size3).ShouldBe(false);
            TheResultingValue(size1 == size4).ShouldBe(false);
        }

        [Test]
        public void Point2D_OpInequality()
        {
            var size1 = new Point2D(123.45, 456.78);
            var size2 = new Point2D(123.45, 456.78);
            var size3 = new Point2D(123.45, 555.55);
            var size4 = new Point2D(222.22, 456.78);

            TheResultingValue(size1 != size2).ShouldBe(false);
            TheResultingValue(size1 != size3).ShouldBe(true);
            TheResultingValue(size1 != size4).ShouldBe(true);
        }

        [Test]
        public void Point2D_EqualsObject()
        {
            var size1 = new Point2D(123.45, 456.78);
            var size2 = new Point2D(123.45, 456.78);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void Point2D_EqualsPoint2D()
        {
            var size1 = new Point2D(123.45, 456.78);
            var size2 = new Point2D(123.45, 456.78);
            var size3 = new Point2D(123.45, 555.55);
            var size4 = new Point2D(222.22, 456.78);

            TheResultingValue(size1.Equals(size2)).ShouldBe(true);
            TheResultingValue(size1.Equals(size3)).ShouldBe(false);
            TheResultingValue(size1.Equals(size4)).ShouldBe(false);
        }

        [Test]
        public void Point2D_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = default(Point2D);
            if (!Point2D.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Point2D.");

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78);
        }

        [Test]
        public void Point2D_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Point2D);
            var succeeded = Point2D.TryParse("foo", out result);

            TheResultingValue(succeeded)
                .ShouldBe(false);
        }

        [Test]
        public void Point2D_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = Point2D.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78);
        }

        [Test]
        public void Point2D_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Point2D.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Point2D_Parse_CanRoundTrip()
        {
            var size1 = Point2D.Parse("123.4 456.7");
            var size2 = Point2D.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }

        [Test]
        public void Point2D_SerializesToJson()
        {
            var point = new Point2D(1.2, 2.3);
            var json = JsonConvert.SerializeObject(point);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Point2D_SerializesToJson_WhenNullable()
        {
            var point = new Point2D(1.2, 2.3);
            var json = JsonConvert.SerializeObject((Point2D?)point);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Point2D_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3}";
            
            var point = JsonConvert.DeserializeObject<Point2D>(json);

            TheResultingValue(point)
                .ShouldBe(1.2, 2.3);
        }

        [Test]
        public void Point2D_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1.2,""y"":2.3}";

            var point1 = JsonConvert.DeserializeObject<Point2D?>(json1);

            TheResultingValue(point1.Value)
                .ShouldBe(1.2, 2.3);

            const String json2 = @"null";

            var point2 = JsonConvert.DeserializeObject<Point2D?>(json2);

            TheResultingValue(point2.HasValue)
                .ShouldBe(false);
        }
    }
}

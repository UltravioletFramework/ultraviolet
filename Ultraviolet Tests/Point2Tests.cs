using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.Testing;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class Point2Tests : UltravioletTestFramework
    {
        [Test]
        public void Point2_IsConstructedProperly()
        {
            var result = new Point2(123, 456);

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [Test]
        public void Point2_OpEquality()
        {
            var size1 = new Point2(123, 456);
            var size2 = new Point2(123, 456);
            var size3 = new Point2(123, 555);
            var size4 = new Point2(222, 456);

            TheResultingValue(size1 == size2).ShouldBe(true);
            TheResultingValue(size1 == size3).ShouldBe(false);
            TheResultingValue(size1 == size4).ShouldBe(false);
        }

        [Test]
        public void Point2_OpInequality()
        {
            var size1 = new Point2(123, 456);
            var size2 = new Point2(123, 456);
            var size3 = new Point2(123, 555);
            var size4 = new Point2(222, 456);

            TheResultingValue(size1 != size2).ShouldBe(false);
            TheResultingValue(size1 != size3).ShouldBe(true);
            TheResultingValue(size1 != size4).ShouldBe(true);
        }

        [Test]
        public void Point2_EqualsObject()
        {
            var size1 = new Point2(123, 456);
            var size2 = new Point2(123, 456);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void Point2_EqualsPoint()
        {
            var size1 = new Point2(123, 456);
            var size2 = new Point2(123, 456);
            var size3 = new Point2(123, 555);
            var size4 = new Point2(222, 456);

            TheResultingValue(size1.Equals(size2)).ShouldBe(true);
            TheResultingValue(size1.Equals(size3)).ShouldBe(false);
            TheResultingValue(size1.Equals(size4)).ShouldBe(false);
        }

        [Test]
        public void Point2_TryParse_SucceedsForValidStrings()
        {
            var str    = "123 456";
            var result = default(Point2);
            if (!Point2.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Point.");

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [Test]
        public void Point2_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Point2);
            var succeeded = Point2.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Point2_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456";
            var result = Point2.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [Test]
        public void Point2_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Point2.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Point2_Parse_CanRoundTrip()
        {
            var size1 = Point2.Parse("123 456");
            var size2 = Point2.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }

        [Test]
        public void Point2_SerializesToJson()
        {
            var point = new Point2(1, 2);
            var json = JsonConvert.SerializeObject(point);

            TheResultingString(json).ShouldBe(@"{""x"":1,""y"":2}");
        }

        [Test]
        public void Point2_SerializesToJson_WhenNullable()
        {
            var point = new Point2(1, 2);
            var json = JsonConvert.SerializeObject((Point2?)point);

            TheResultingString(json).ShouldBe(@"{""x"":1,""y"":2}");
        }

        [Test]
        public void Point2_DeserializesFromJson()
        {
            const String json = @"{""x"":1,""y"":2}";
            
            var point = JsonConvert.DeserializeObject<Point2>(json);

            TheResultingValue(point)
                .ShouldBe(1, 2);
        }

        [Test]
        public void Point2_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1,""y"":2}";

            var point1 = JsonConvert.DeserializeObject<Point2?>(json1);

            TheResultingValue(point1.Value)
                .ShouldBe(1, 2);

            const String Json2 = @"null";

            var point2 = JsonConvert.DeserializeObject<Point2?>(Json2);

            TheResultingValue(point2.HasValue)
                .ShouldBe(false);
        }
    }
}

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
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
            var point1 = new Point2D(123.45, 456.78);
            var point2 = new Point2D(123.45, 456.78);
            var point3 = new Point2D(123.45, 555.55);
            var point4 = new Point2D(222.22, 456.78);

            TheResultingValue(point1 == point2).ShouldBe(true);
            TheResultingValue(point1 == point3).ShouldBe(false);
            TheResultingValue(point1 == point4).ShouldBe(false);
        }

        [Test]
        public void Point2D_OpInequality()
        {
            var point1 = new Point2D(123.45, 456.78);
            var point2 = new Point2D(123.45, 456.78);
            var point3 = new Point2D(123.45, 555.55);
            var point4 = new Point2D(222.22, 456.78);

            TheResultingValue(point1 != point2).ShouldBe(false);
            TheResultingValue(point1 != point3).ShouldBe(true);
            TheResultingValue(point1 != point4).ShouldBe(true);
        }

        [Test]
        public void Point2D_EqualsObject()
        {
            var point1 = new Point2D(123.45, 456.78);
            var point2 = new Point2D(123.45, 456.78);

            TheResultingValue(point1.Equals((Object)point2)).ShouldBe(true);
            TheResultingValue(point1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void Point2D_EqualsPoint2D()
        {
            var point1 = new Point2D(123.45, 456.78);
            var point2 = new Point2D(123.45, 456.78);
            var point3 = new Point2D(123.45, 555.55);
            var point4 = new Point2D(222.22, 456.78);

            TheResultingValue(point1.Equals(point2)).ShouldBe(true);
            TheResultingValue(point1.Equals(point3)).ShouldBe(false);
            TheResultingValue(point1.Equals(point4)).ShouldBe(false);
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
        public void Point2D_TransformsCorrectly_WithMatrix()
        {
            var point1 = new Point2D(123, 456);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            var result = Point2D.Transform(point1, transform);

            TheResultingValue(result).WithinDelta(0.1)
                .ShouldBe(-123.0, -456.0);
        }

        [Test]
        public void Point2D_TransformsCorrectly_WithMatrix_WithOutParam()
        {
            var point1 = new Point2D(123, 456);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            var result = Point2D.Zero;
            Point2D.Transform(ref point1, ref transform, out result);

            TheResultingValue(result).WithinDelta(0.1)
                .ShouldBe(-123.0, -456.0);
        }

        [Test]
        public void Point2D_TransformsCorrectly_WithQuaternion()
        {
            var point1 = new Point2D(123, 456);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            var result = Point2D.Transform(point1, transform);

            TheResultingValue(result).WithinDelta(0.1)
                .ShouldBe(-123.0, -456.0);
        }

        [Test]
        public void Point2D_TransformsForrectly_WithQuaternion_WithOutParam()
        {
            var point1 = new Point2D(123, 456);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            Point2D.Transform(ref point1, ref transform, out Point2D result);

            TheResultingValue(result).WithinDelta(0.1)
                .ShouldBe(-123.0, -456.0);
        }

        [Test]
        public void Point2D_SerializesToJson()
        {
            var point = new Point2D(1.2, 2.3);
            var json = JsonConvert.SerializeObject(point,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Point2D_SerializesToJson_WhenNullable()
        {
            var point = new Point2D(1.2, 2.3);
            var json = JsonConvert.SerializeObject((Point2D?)point, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Point2D_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3}";
            
            var point = JsonConvert.DeserializeObject<Point2D>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(point)
                .ShouldBe(1.2, 2.3);
        }

        [Test]
        public void Point2D_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1.2,""y"":2.3}";

            var point1 = JsonConvert.DeserializeObject<Point2D?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(point1.Value)
                .ShouldBe(1.2, 2.3);

            const String json2 = @"null";

            var point2 = JsonConvert.DeserializeObject<Point2D?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(point2.HasValue)
                .ShouldBe(false);
        }
    }
}

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
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
            var point1 = new Point2F(123.45f, 456.78f);
            var point2 = new Point2F(123.45f, 456.78f);
            var point3 = new Point2F(123.45f, 555.55f);
            var point4 = new Point2F(222.22f, 456.78f);

            TheResultingValue(point1 == point2).ShouldBe(true);
            TheResultingValue(point1 == point3).ShouldBe(false);
            TheResultingValue(point1 == point4).ShouldBe(false);
        }

        [Test]
        public void Point2F_OpInequality()
        {
            var point1 = new Point2F(123.45f, 456.78f);
            var point2 = new Point2F(123.45f, 456.78f);
            var point3 = new Point2F(123.45f, 555.55f);
            var point4 = new Point2F(222.22f, 456.78f);

            TheResultingValue(point1 != point2).ShouldBe(false);
            TheResultingValue(point1 != point3).ShouldBe(true);
            TheResultingValue(point1 != point4).ShouldBe(true);
        }

        [Test]
        public void Point2F_EqualsObject()
        {
            var point1 = new Point2F(123.45f, 456.78f);
            var point2 = new Point2F(123.45f, 456.78f);

            TheResultingValue(point1.Equals((Object)point2)).ShouldBe(true);
            TheResultingValue(point1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void Point2F_EqualsPoint2F()
        {
            var point1 = new Point2F(123.45f, 456.78f);
            var point2 = new Point2F(123.45f, 456.78f);
            var point3 = new Point2F(123.45f, 555.55f);
            var point4 = new Point2F(222.22f, 456.78f);

            TheResultingValue(point1.Equals(point2)).ShouldBe(true);
            TheResultingValue(point1.Equals(point3)).ShouldBe(false);
            TheResultingValue(point1.Equals(point4)).ShouldBe(false);
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
        public void Point2F_TransformsCorrectly_WithMatrix()
        {
            var point1 = new Point2F(123, 456);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            var result = Point2F.Transform(point1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f);
        }

        [Test]
        public void Point2F_TransformsCorrectly_WithMatrix_WithOutParam()
        {
            var point1 = new Point2F(123, 456);
            var transform = Matrix.CreateRotationZ((float)Math.PI);

            var result = Point2F.Zero;
            Point2F.Transform(ref point1, ref transform, out result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f);
        }

        [Test]
        public void Point2F_TransformsCorrectly_WithQuaternion()
        {
            var point1 = new Point2F(123, 456);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            var result = Point2F.Transform(point1, transform);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f);
        }

        [Test]
        public void Point2F_TransformsForrectly_WithQuaternion_WithOutParam()
        {
            var point1 = new Point2F(123, 456);
            var matrix = Matrix.CreateRotationZ((float)Math.PI);
            var transform = Quaternion.CreateFromRotationMatrix(matrix);

            Point2F.Transform(ref point1, ref transform, out Point2F result);

            TheResultingValue(result).WithinDelta(0.1f)
                .ShouldBe(-123.0f, -456.0f);
        }

        [Test]
        public void Point2F_SerializesToJson()
        {
            var point = new Point2F(1.2f, 2.3f);
            var json = JsonConvert.SerializeObject(point,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Point2F_SerializesToJson_WhenNullable()
        {
            var point = new Point2F(1.2f, 2.3f);
            var json = JsonConvert.SerializeObject((Point2F?)point,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""x"":1.2,""y"":2.3}");
        }

        [Test]
        public void Point2F_DeserializesFromJson()
        {
            const String json = @"{""x"":1.2,""y"":2.3}";
            
            var point = JsonConvert.DeserializeObject<Point2F>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(point)
                .ShouldBe(1.2f, 2.3f);
        }

        [Test]
        public void Point2F_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""x"":1.2,""y"":2.3}";

            var point1 = JsonConvert.DeserializeObject<Point2F?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(point1.Value)
                .ShouldBe(1.2f, 2.3f);

            const String json2 = @"null";

            var point2 = JsonConvert.DeserializeObject<Point2F?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(point2.HasValue)
                .ShouldBe(false);
        }
    }
}

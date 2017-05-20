using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class PlaneTests : UltravioletTestFramework
    {
        [Test]
        public void Plane_IsConstructedProperly_FromComponents()
        {
            var result = new Plane(1.1f, 2.2f, 3.3f, 4.4f);

            TheResultingValue(result)
                .ShouldHaveNormal(1.1f, 2.2f, 3.3f)
                .ShouldHaveDistance(4.4f);
        }

        [Test]
        public void Plane_IsConstructedProperly_FromVector3AndDistance()
        {
            var result = new Plane(new Vector3(1.1f, 2.2f, 3.3f), 4.4f);

            TheResultingValue(result)
                .ShouldHaveNormal(1.1f, 2.2f, 3.3f)
                .ShouldHaveDistance(4.4f);
        }

        [Test]
        public void Plane_IsConstructedProperly_FromVector4()
        {
            var result = new Plane(new Vector4(1.1f, 2.2f, 3.3f, 4.4f));

            TheResultingValue(result)
                .ShouldHaveNormal(1.1f, 2.2f, 3.3f)
                .ShouldHaveDistance(4.4f);
        }

        [Test]
        public void Plane_OpEquality()
        {
            var plane1 = new Plane(123.45f, 456.78f, 100.10f, 345.6f);
            var plane2 = new Plane(123.45f, 456.78f, 100.10f, 345.6f);
            var plane3 = new Plane(123.45f, 555f, 100.10f, 999.9f);
            var plane4 = new Plane(222f, 456.78f, 100.10f, 888.8f);
            var plane5 = new Plane(123.45f, 456.78f, 200f, 777.7f);

            TheResultingValue(plane1 == plane2).ShouldBe(true);
            TheResultingValue(plane1 == plane3).ShouldBe(false);
            TheResultingValue(plane1 == plane4).ShouldBe(false);
            TheResultingValue(plane1 == plane5).ShouldBe(false);
        }

        [Test]
        public void Plane_OpInequality()
        {
            var plane1 = new Plane(123.45f, 456.78f, 100.10f, 345.6f);
            var plane2 = new Plane(123.45f, 456.78f, 100.10f, 345.6f);
            var plane3 = new Plane(123.45f, 555f, 100.10f, 999.9f);
            var plane4 = new Plane(222f, 456.78f, 100.10f, 888.8f);
            var plane5 = new Plane(123.45f, 456.78f, 200f, 777.7f);

            TheResultingValue(plane1 != plane2).ShouldBe(false);
            TheResultingValue(plane1 != plane3).ShouldBe(true);
            TheResultingValue(plane1 != plane4).ShouldBe(true);
            TheResultingValue(plane1 != plane5).ShouldBe(true);
        }

        [Test]
        public void Plane_EqualsObject()
        {
            var plane1 = new Plane(123.45f, 456.78f, 100.10f, 345.6f);
            var plane2 = new Plane(123.45f, 456.78f, 100.10f, 345.6f);

            TheResultingValue(plane1.Equals((Object)plane2)).ShouldBe(true);
            TheResultingValue(plane1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void Plane_EqualsPlane()
        {
            var plane1 = new Plane(123.45f, 456.78f, 100.10f, 345.6f);
            var plane2 = new Plane(123.45f, 456.78f, 100.10f, 345.6f);
            var plane3 = new Plane(123.45f, 555f, 100.10f, 999.9f);
            var plane4 = new Plane(222f, 456.78f, 100.10f, 888.8f);

            TheResultingValue(plane1.Equals(plane2)).ShouldBe(true);
            TheResultingValue(plane1.Equals(plane3)).ShouldBe(false);
            TheResultingValue(plane1.Equals(plane4)).ShouldBe(false);
        }

        [Test]
        public void Plane_TryParse_SucceedsForValidStrings()
        {
            var str = "123.45 456.78 100.10 345.6";
            if (!Plane.TryParse(str, out Plane result))
                throw new InvalidOperationException("Unable to parse string to Plane.");

            TheResultingValue(result)
                .ShouldHaveNormal(123.45f, 456.78f, 100.10f)
                .ShouldHaveDistance(345.6f);
        }

        [Test]
        public void Plane_TryParse_FailsForInvalidStrings()
        {
            var succeeded = Plane.TryParse("foo", out Plane result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Plane_Parse_SucceedsForValidStrings()
        {
            var str = "123.45 456.78 100.10 345.6";
            var result = Plane.Parse(str);

            TheResultingValue(result)
                .ShouldHaveNormal(123.45f, 456.78f, 100.10f)
                .ShouldHaveDistance(345.6f);
        }

        [Test]
        public void Plane_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Plane.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Plane_Parse_CanRoundTrip()
        {
            var plane1 = Plane.Parse("123.4 456.7 100 345.6");
            var plane2 = Plane.Parse(plane1.ToString());

            TheResultingValue(plane1 == plane2).ShouldBe(true);
        }

        [Test]
        public void Plane_SerializesToJson()
        {
            var plane = new Plane(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject(plane);

            TheResultingString(json).ShouldBe(@"{""normal"":{""x"":1.2,""y"":2.3,""z"":3.4},""d"":4.5}");
        }

        [Test]
        public void Plane_SerializesToJson_WhenNullable()
        {
            var plane = new Plane(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject((Plane?)plane);

            TheResultingString(json).ShouldBe(@"{""normal"":{""x"":1.2,""y"":2.3,""z"":3.4},""d"":4.5}");
        }

        [Test]
        public void Plane_DeserializesFromJson()
        {
            const String json = @"{ ""normal"": { ""x"":1.2, ""y"":2.3, ""z"":3.4 },""d"":4.5 }";

            var plane = JsonConvert.DeserializeObject<Plane>(json);

            TheResultingValue(plane)
                .ShouldHaveNormal(1.2f, 2.3f, 3.4f)
                .ShouldHaveDistance(4.5f);
        }

        [Test]
        public void Plane_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{ ""normal"": { ""x"":1.2, ""y"":2.3, ""z"":3.4 },""d"":4.5 }";

            var plane1 = JsonConvert.DeserializeObject<Plane?>(json1);

            TheResultingValue(plane1.Value)
                .ShouldHaveNormal(1.2f, 2.3f, 3.4f)
                .ShouldHaveDistance(4.5f);

            const String json2 = @"null";

            var plane2 = JsonConvert.DeserializeObject<Plane?>(json2);

            TheResultingValue(plane2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Plane_CalculatesDotCorrectly()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var vector = new Vector4(56.7f, 67.8f, 78.9f, 89.0f);

            var result = Plane.Dot(plane, vector);

            TheResultingValue(result)
                .WithinDelta(0.0001f).ShouldBe(9064.38f);
        }

        [Test]
        public void Plane_CalculatesDotCorrectly_WithOutParam()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var vector = new Vector4(56.7f, 67.8f, 78.9f, 89.0f);

            Plane.Dot(ref plane, ref vector, out Single result);

            TheResultingValue(result)
                .WithinDelta(0.0001f).ShouldBe(9064.38f);
        }

        [Test]
        public void Plane_CalculatesDotCoordinateCorrectly()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var vector = new Vector3(56.7f, 67.8f, 78.9f);

            var result = Plane.DotCoordinate(plane, vector);

            TheResultingValue(result)
                .WithinDelta(0.0001f).ShouldBe(5051.58f);
        }

        [Test]
        public void Plane_CalculatesDotCoordinateCorrectly_WithOutParam()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var vector = new Vector3(56.7f, 67.8f, 78.9f);

            Plane.DotCoordinate(ref plane, ref vector, out Single result);

            TheResultingValue(result)
                .WithinDelta(0.0001f).ShouldBe(5051.58f);
        }

        [Test]
        public void Plane_NormalizesCorrectly()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);

            var result = Plane.Normalize(plane);

            TheResultingValue(result)
                .WithinDelta(0.0001f)
                .ShouldHaveNormal(0.2830f, 0.5384f, 0.7938f)
                .ShouldHaveDistance(1.0491f);
        }

        [Test]
        public void Plane_NormalizesCorrectly_WithOutParam()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);

            Plane.Normalize(ref plane, out Plane result);

            TheResultingValue(result)
                .WithinDelta(0.0001f)
                .ShouldHaveNormal(0.2830f, 0.5384f, 0.7938f)
                .ShouldHaveDistance(1.0491f);
        }

        [Test]
        public void Plane_TransformsCorrectly_WithMatrix()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var matrix = Matrix.CreateRotationZ(2.0f);

            var result = Plane.Transform(plane, matrix);

            TheResultingValue(result)
                .WithinDelta(0.0001f)
                .ShouldHaveNormal(-26.3962f, 1.4465f, 34.5f)
                .ShouldHaveDistance(45.6f);
        }

        [Test]
        public void Plane_TransformsCorrectly_WithMatrix_WithOutParam()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var matrix = Matrix.CreateRotationZ(2.0f);

            Plane.Transform(ref plane, ref matrix, out Plane result);

            TheResultingValue(result)
                .WithinDelta(0.0001f)
                .ShouldHaveNormal(-26.3962f, 1.4465f, 34.5f)
                .ShouldHaveDistance(45.6f);
        }
    }
}

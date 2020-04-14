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
        public void Plane_SerializesToJson()
        {
            var plane = new Plane(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject(plane, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""normal"":{""x"":1.2,""y"":2.3,""z"":3.4},""d"":4.5}");
        }

        [Test]
        public void Plane_SerializesToJson_WhenNullable()
        {
            var plane = new Plane(1.2f, 2.3f, 3.4f, 4.5f);
            var json = JsonConvert.SerializeObject((Plane?)plane, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""normal"":{""x"":1.2,""y"":2.3,""z"":3.4},""d"":4.5}");
        }

        [Test]
        public void Plane_DeserializesFromJson()
        {
            const String json = @"{ ""normal"": { ""x"":1.2, ""y"":2.3, ""z"":3.4 },""d"":4.5 }";

            var plane = JsonConvert.DeserializeObject<Plane>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(plane)
                .ShouldHaveNormal(1.2f, 2.3f, 3.4f)
                .ShouldHaveDistance(4.5f);
        }

        [Test]
        public void Plane_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{ ""normal"": { ""x"":1.2, ""y"":2.3, ""z"":3.4 },""d"":4.5 }";

            var plane1 = JsonConvert.DeserializeObject<Plane?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(plane1.Value)
                .ShouldHaveNormal(1.2f, 2.3f, 3.4f)
                .ShouldHaveDistance(4.5f);

            const String json2 = @"null";

            var plane2 = JsonConvert.DeserializeObject<Plane?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(plane2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Plane_CalculatesDotCorrectly()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var vector = new Vector4(56.7f, 67.8f, 78.9f, 89.0f);

            var result = Plane.Dot(plane, vector);

            if (Environment.Is64BitProcess)
            {
                TheResultingValue(result)
                    .WithinDelta(0.001f).ShouldBe(9064.3808f);
            }
            else
            {
                TheResultingValue(result)
                    .WithinDelta(0.001f).ShouldBe(9064.3798f);
            }
        }

        [Test]
        public void Plane_CalculatesDotCorrectly_WithOutParam()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var vector = new Vector4(56.7f, 67.8f, 78.9f, 89.0f);

            Plane.Dot(ref plane, ref vector, out Single result);

            if (Environment.Is64BitProcess)
            {
                TheResultingValue(result)
                    .WithinDelta(0.001f).ShouldBe(9064.3808f);
            }
            else
            {
                TheResultingValue(result)
                    .WithinDelta(0.001f).ShouldBe(9064.3798f);
            }
        }

        [Test]
        public void Plane_CalculatesDotCoordinateCorrectly()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var vector = new Vector3(56.7f, 67.8f, 78.9f);

            var result = Plane.DotCoordinate(plane, vector);

            if (Environment.Is64BitProcess)
            {
                TheResultingValue(result)
                    .WithinDelta(0.001f).ShouldBe(5051.5805f);
            }
            else
            {
                TheResultingValue(result)
                    .WithinDelta(0.001f).ShouldBe(5051.5800f);
            }
        }

        [Test]
        public void Plane_CalculatesDotCoordinateCorrectly_WithOutParam()
        {
            var plane = new Plane(12.3f, 23.4f, 34.5f, 45.6f);
            var vector = new Vector3(56.7f, 67.8f, 78.9f);

            Plane.DotCoordinate(ref plane, ref vector, out Single result);

            if (Environment.Is64BitProcess)
            {
                TheResultingValue(result)
                    .WithinDelta(0.001f).ShouldBe(5051.5805f);
            }
            else
            {
                TheResultingValue(result)
                    .WithinDelta(0.001f).ShouldBe(5051.5800f);
            }
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

        [Test]
        public void Plane_TransformsCorrectly_WithQuaternion()
        {
            var plane = Plane.Normalize(new Plane(12.3f, 23.4f, 34.5f, 45.6f));
            var quaternion = Quaternion.CreateFromAxisAngle(Vector3.Normalize(new Vector3(56.7f, 67.8f, 78.9f)), 89.0f);

            var result = Plane.Transform(plane, quaternion);
            
            TheResultingValue(result)
                .WithinDelta(0.0001f)
                .ShouldHaveNormal(0.4545f, 0.3825f, 0.8043f)
                .ShouldHaveDistance(1.0491f);
        }

        [Test]
        public void Plane_TransformsCorrectly_WithQuaternion_WithOutParam()
        {
            var plane = Plane.Normalize(new Plane(12.3f, 23.4f, 34.5f, 45.6f));
            var quaternion = Quaternion.CreateFromAxisAngle(Vector3.Normalize(new Vector3(56.7f, 67.8f, 78.9f)), 89.0f);

            Plane.Transform(ref plane, ref quaternion, out Plane result);

            TheResultingValue(result)
                .WithinDelta(0.0001f)
                .ShouldHaveNormal(0.4545f, 0.3825f, 0.8043f)
                .ShouldHaveDistance(1.0491f);
        }

        [Test]
        public void Plane_CalculatesIntersectsBoundingFrustumCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var plane1 = new Plane(frustum.Near.Normal, frustum.Near.D - 1000f);
            var plane2 = new Plane(frustum.Near.Normal, frustum.Near.D + 1000f);
            var plane3 = new Plane(frustum.Near.Normal, frustum.Near.D + 500f);

            var result1 = plane1.Intersects(frustum);
            var result2 = plane2.Intersects(frustum);
            var result3 = plane3.Intersects(frustum);

            TheResultingValue(result1).ShouldBe(PlaneIntersectionType.Back);
            TheResultingValue(result2).ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3).ShouldBe(PlaneIntersectionType.Intersecting);
        }

        [Test]
        public void Plane_CalculatesIntersectsBoundingFrustumCorrectly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var plane1 = new Plane(frustum.Near.Normal, frustum.Near.D - 1000f);
            var plane2 = new Plane(frustum.Near.Normal, frustum.Near.D + 1000f);
            var plane3 = new Plane(frustum.Near.Normal, frustum.Near.D + 500f);

            plane1.Intersects(frustum, out PlaneIntersectionType result1);
            plane2.Intersects(frustum, out PlaneIntersectionType result2);
            plane3.Intersects(frustum, out PlaneIntersectionType result3);

            TheResultingValue(result1).ShouldBe(PlaneIntersectionType.Back);
            TheResultingValue(result2).ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3).ShouldBe(PlaneIntersectionType.Intersecting);
        }
        
        [Test]
        public void Plane_CalculatesIntersectsBoundingSphereCorrectly()
        {
            var sphere = new BoundingSphere(Vector3.Zero, 10f);
            var plane1 = new Plane(new Vector3(1f, 0f, 0f), 0f);
            var plane2 = new Plane(new Vector3(1f, 0f, 0f), 100f);

            var result1 = plane1.Intersects(sphere);
            var result2 = plane2.Intersects(sphere);

            TheResultingValue(result1)
                .ShouldBe(PlaneIntersectionType.Intersecting);
            TheResultingValue(result2)
                .ShouldBe(PlaneIntersectionType.Front);
        }

        [Test]
        public void Plane_CalculatesIntersectsBoundingSphereCorrectly_WithOutParam()
        {
            var sphere = new BoundingSphere(Vector3.Zero, 10f);
            var plane1 = new Plane(new Vector3(1f, 0f, 0f), 0f);
            var plane2 = new Plane(new Vector3(1f, 0f, 0f), 100f);

            plane1.Intersects(ref sphere, out PlaneIntersectionType result1);
            plane2.Intersects(ref sphere, out PlaneIntersectionType result2);

            TheResultingValue(result1)
                .ShouldBe(PlaneIntersectionType.Intersecting);
            TheResultingValue(result2)
                .ShouldBe(PlaneIntersectionType.Front);
        }

        [Test]
        public void Plane_CalculatesIntersectsBoundingBoxCorrectly()
        {
            var box = new BoundingBox(new Vector3(-10f), new Vector3(10f));
            var plane1 = new Plane(new Vector3(1f, 0f, 0f), 0f);
            var plane2 = new Plane(new Vector3(1f, 0f, 0f), 100f);
            var plane3 = new Plane(new Vector3(1f, 0f, 0f), -100f);

            var result1 = plane1.Intersects(box);
            var result2 = plane2.Intersects(box);
            var result3 = plane3.Intersects(box);

            TheResultingValue(result1)
                .ShouldBe(PlaneIntersectionType.Intersecting);
            TheResultingValue(result2)
                .ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3)
                .ShouldBe(PlaneIntersectionType.Back);
        }

        [Test]
        public void Plane_CalculatesIntersectsBoundingBoxCorrectly_WithOutParam()
        {
            var box = new BoundingBox(new Vector3(-10f), new Vector3(10f));
            var plane1 = new Plane(new Vector3(1f, 0f, 0f), 0f);
            var plane2 = new Plane(new Vector3(1f, 0f, 0f), 100f);
            var plane3 = new Plane(new Vector3(1f, 0f, 0f), -100f);

            plane1.Intersects(ref box, out var result1);
            plane2.Intersects(ref box, out var result2);
            plane3.Intersects(ref box, out var result3);

            TheResultingValue(result1)
                .ShouldBe(PlaneIntersectionType.Intersecting);
            TheResultingValue(result2)
                .ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3)
                .ShouldBe(PlaneIntersectionType.Back);
        }

    }
}

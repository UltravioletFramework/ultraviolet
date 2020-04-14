using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class BoundingSphereTests : UltravioletTestFramework
    {
        [Test]
        public void BoundingSphere_ConstructorSetsValues()
        {
            var sphere = new BoundingSphere(new Vector3(1.2f, 2.3f, 3.4f), 4.5f);

            TheResultingValue(sphere.Center)
                .ShouldBe(1.2f, 2.3f, 3.4f);
            TheResultingValue(sphere.Radius)
                .ShouldBe(4.5f);
        }

        [Test]
        public void BoundingSphere_CreateMergedWorksCorrectly()
        {
            var original = new BoundingSphere(new Vector3(0, 0, 0), 2f);
            var additional = new BoundingSphere(new Vector3(1.2f, 2.3f, 3.4f), 4.5f);

            var merged = BoundingSphere.CreateMerged(original, additional);

            TheResultingValue(merged.Center).WithinDelta(0.0001f)
                .ShouldBe(0.9507f, 1.8222f, 2.6937f);
            TheResultingValue(merged.Radius).WithinDelta(0.0001f)
                .ShouldBe(5.3883f);
        }

        [Test]
        public void BoundingSphere_CreateMergedWorksCorrectly_WithOutParam()
        {
            var original = new BoundingSphere(new Vector3(0, 0, 0), 2f);
            var additional = new BoundingSphere(new Vector3(1.2f, 2.3f, 3.4f), 4.5f);

            BoundingSphere.CreateMerged(ref original, ref additional, out BoundingSphere merged);

            TheResultingValue(merged.Center).WithinDelta(0.0001f)
                .ShouldBe(0.9507f, 1.8222f, 2.6937f);
            TheResultingValue(merged.Radius).WithinDelta(0.0001f)
                .ShouldBe(5.3883f);
        }
        
        [Test]
        public void BoundingSphere_CreateFromPointsWorksCorrectly()
        {
            var sphere = BoundingSphere.CreateFromPoints((IEnumerable<Vector3>)new[] {
                new Vector3(1.2f, 3.4f, 4.5f),
                new Vector3(4.5f, 3.4f, 1.2f),
                new Vector3(1.1f, 2.2f, 3.3f),
                new Vector3(2.3f, 4.5f, 6.7f),
            });

            TheResultingValue(sphere.Center).WithinDelta(0.0001f)
                .ShouldBe(3.4f, 3.95f, 3.95f);
            TheResultingValue(sphere.Radius).WithinDelta(0.0001f)
                .ShouldBe(3.0124f);
        }

        [Test]
        public void BoundingSphere_CreateFromPointsWorksCorrectly_WithOutParam()
        {
            BoundingSphere.CreateFromPoints((IEnumerable<Vector3>)new[] {
                new Vector3(1.2f, 3.4f, 4.5f),
                new Vector3(4.5f, 3.4f, 1.2f),
                new Vector3(1.1f, 2.2f, 3.3f),
                new Vector3(2.3f, 4.5f, 6.7f),
            }, out BoundingSphere sphere);

            TheResultingValue(sphere.Center).WithinDelta(0.0001f)
                .ShouldBe(3.4f, 3.95f, 3.95f);
            TheResultingValue(sphere.Radius).WithinDelta(0.0001f)
                .ShouldBe(3.0124f);
        }

        [Test]
        public void BoundingSphere_CreateFromPointsWorksCorrectly_FromIList()
        {
            var sphere = BoundingSphere.CreateFromPoints(new[] {
                new Vector3(1.2f, 3.4f, 4.5f),
                new Vector3(4.5f, 3.4f, 1.2f),
                new Vector3(1.1f, 2.2f, 3.3f),
                new Vector3(2.3f, 4.5f, 6.7f),
            });

            TheResultingValue(sphere.Center).WithinDelta(0.0001f)
                .ShouldBe(3.4f, 3.95f, 3.95f);
            TheResultingValue(sphere.Radius).WithinDelta(0.0001f)
                .ShouldBe(3.0124f);
        }

        [Test]
        public void BoundingSphere_CreateFromPointsWorksCorrectly_FromIList_WithOutParam()
        {
            BoundingSphere.CreateFromPoints(new[] {
                new Vector3(1.2f, 3.4f, 4.5f),
                new Vector3(4.5f, 3.4f, 1.2f),
                new Vector3(1.1f, 2.2f, 3.3f),
                new Vector3(2.3f, 4.5f, 6.7f),
            }, out BoundingSphere sphere);

            TheResultingValue(sphere.Center).WithinDelta(0.0001f)
                .ShouldBe(3.4f, 3.95f, 3.95f);
            TheResultingValue(sphere.Radius).WithinDelta(0.0001f)
                .ShouldBe(3.0124f);
        }

        [Test]
        public void BoundingSphere_CreateFromFrustumWorksCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var result = BoundingSphere.CreateFromFrustum(frustum);

            if (Environment.Is64BitProcess)
            {
                TheResultingValue(result.Center).WithinDelta(0.001f)
                    .ShouldBe(-0.0853f, 0.06398f, -840.6783f);
                TheResultingValue(result.Radius).WithinDelta(0.001f)
                    .ShouldBe(844.6787f);
            }
            else
            {
                TheResultingValue(result.Center).WithinDelta(0.001f)
                    .ShouldBe(-0.0853f, 0.06398f, -840.6783f);
                TheResultingValue(result.Radius).WithinDelta(0.001f)
                    .ShouldBe(844.6785f);
            }
        }

        [Test]
        public void BoundingSphere_CreateFromFrustumWorksCorrectly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            BoundingSphere.CreateFromFrustum(frustum, out BoundingSphere result);

            if (Environment.Is64BitProcess)
            {
                TheResultingValue(result.Center).WithinDelta(0.001f)
                    .ShouldBe(-0.0853f, 0.06398f, -840.6783f);
                TheResultingValue(result.Radius).WithinDelta(0.001f)
                    .ShouldBe(844.6787f);
            }
            else
            {
                TheResultingValue(result.Center).WithinDelta(0.001f)
                    .ShouldBe(-0.0853f, 0.06398f, -840.6783f);
                TheResultingValue(result.Radius).WithinDelta(0.001f)
                    .ShouldBe(844.6785f);
            }
        }

        [Test]
        public void BoundingSphere_CalculatesContainsVector3Correctly()
        {
            var sphere = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var point1 = new Vector3(11f, 9f, 10f);
            var result1 = sphere.Contains(point1);
            var point2 = new Vector3(1000f, 1000f, 1000f);
            var result2 = sphere.Contains(point2);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingSphere_CalculatesContainsVector3Correctly_WithOutParam()
        {
            var sphere = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var point1 = new Vector3(11f, 9f, 10f);
            sphere.Contains(ref point1, out ContainmentType result1);
            var point2 = new Vector3(1000f, 1000f, 1000f);
            sphere.Contains(ref point2, out ContainmentType result2);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingSphere_CalculatesContainsBoundingFrustumCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);
            var result1 = sphere1.Contains(frustum);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 10f);
            var result2 = sphere2.Contains(frustum);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Disjoint);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
        }

        [Test]
        public void BoundingSphere_CalculatesContainsBoundingFrustumCorrectly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);
            sphere1.Contains(frustum, out var result1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 10f);
            sphere2.Contains(frustum, out var result2);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Disjoint);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
        }

        [Test]
        public void BoundingSphere_CalculatesContainsBoundingSphereCorrectly()
        {
            var sphere0 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 5f);
            var result1 = sphere0.Contains(sphere1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 8f);
            var result2 = sphere0.Contains(sphere2);
            var sphere3 = new BoundingSphere(new Vector3(0, 0, 0), 1f);
            var result3 = sphere0.Contains(sphere3);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result3)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingSphere_CalculatesContainsBoundingSphereCorrectly_WithOutParam()
        {
            var sphere0 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 5f);
            sphere0.Contains(ref sphere1, out var result1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 8f);
            sphere0.Contains(ref sphere2, out var result2);
            var sphere3 = new BoundingSphere(new Vector3(0, 0, 0), 1f);
            sphere0.Contains(ref sphere3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result3)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingSphere_CalculatesContainsBoundingBoxCorrectly()
        {
            var sphere = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var box1 = new BoundingBox(new Vector3(5f, 5f, 5f), new Vector3(15f, 15f, 15f));
            var result1 = sphere.Contains(box1);
            var box2 = new BoundingBox(new Vector3(-8f, -8f, -8f), new Vector3(8f, 8f, 8f));
            var result2 = sphere.Contains(box2);
            var box3 = new BoundingBox(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, 1f));
            var result3 = sphere.Contains(box3);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result3)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingSphere_CalculatesContainsBoundingBoxCorrectly_WithOutParam()
        {
            var sphere = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var box1 = new BoundingBox(new Vector3(5f, 5f, 5f), new Vector3(15f, 15f, 15f));
            sphere.Contains(ref box1, out var result1);
            var box2 = new BoundingBox(new Vector3(-8f, -8f, -8f), new Vector3(8f, 8f, 8f));
            sphere.Contains(ref box2, out var result2);
            var box3 = new BoundingBox(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, 1f));
            sphere.Contains(ref box3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result3)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsBoundingFrustumCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);
            var result1 = sphere1.Intersects(frustum);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 10f);
            var result2 = sphere2.Intersects(frustum);

            TheResultingValue(result1)
                .ShouldBe(false);
            TheResultingValue(result2)
                .ShouldBe(true);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsBoundingFrustumCorrectly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);
            sphere1.Intersects(frustum, out var result1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 10f);
            sphere2.Intersects(frustum, out var result2);

            TheResultingValue(result1)
                .ShouldBe(false);
            TheResultingValue(result2)
                .ShouldBe(true);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsBoundingSphereCorrectly()
        {
            var sphere0 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 5f);
            var result1 = sphere0.Intersects(sphere1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 8f);
            var result2 = sphere0.Intersects(sphere2);
            var sphere3 = new BoundingSphere(new Vector3(0, 0, 0), 1f);
            var result3 = sphere0.Intersects(sphere3);

            TheResultingValue(result1)
                .ShouldBe(true);
            TheResultingValue(result2)
                .ShouldBe(true);
            TheResultingValue(result3)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsBoundingSphereCorrectly_WithOutParam()
        {
            var sphere0 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 5f);
            sphere0.Intersects(ref sphere1, out var result1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 8f);
            sphere0.Intersects(ref sphere2, out var result2);
            var sphere3 = new BoundingSphere(new Vector3(0, 0, 0), 1f);
            sphere0.Intersects(ref sphere3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(true);
            TheResultingValue(result2)
                .ShouldBe(true);
            TheResultingValue(result3)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsBoundingBoxCorrectly()
        {
            var sphere = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var box1 = new BoundingBox(new Vector3(5f, 5f, 5f), new Vector3(15f, 15f, 15f));
            var result1 = sphere.Intersects(box1);
            var box2 = new BoundingBox(new Vector3(-8f, -8f, -8f), new Vector3(8f, 8f, 8f));
            var result2 = sphere.Intersects(box2);
            var box3 = new BoundingBox(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, 1f));
            var result3 = sphere.Intersects(box3);

            TheResultingValue(result1)
                .ShouldBe(true);
            TheResultingValue(result2)
                .ShouldBe(true);
            TheResultingValue(result3)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsBoundingBoxCorrectly_WithOutParam()
        {
            var sphere = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);

            var box1 = new BoundingBox(new Vector3(5f, 5f, 5f), new Vector3(15f, 15f, 15f));
            sphere.Intersects(ref box1, out var result1);
            var box2 = new BoundingBox(new Vector3(-8f, -8f, -8f), new Vector3(8f, 8f, 8f));
            sphere.Intersects(ref box2, out var result2);
            var box3 = new BoundingBox(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, 1f));
            sphere.Intersects(ref box3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(true);
            TheResultingValue(result2)
                .ShouldBe(true);
            TheResultingValue(result3)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsPlaneCorrectly()
        {
            var plane = new Plane(Vector3.Forward, 100f);

            var sphere1 = new BoundingSphere(new Vector3(100f, 100f, 200f), 10f);
            var result1 = sphere1.Intersects(plane);
            var sphere2 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);
            var result2 = sphere2.Intersects(plane);
            var sphere3 = new BoundingSphere(new Vector3(0, 0, 100f), 10f);
            var result3 = sphere3.Intersects(plane);

            TheResultingValue(result1)
                .ShouldBe(PlaneIntersectionType.Back);
            TheResultingValue(result2)
                .ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3)
                .ShouldBe(PlaneIntersectionType.Intersecting);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsPlaneCorrectly_WithOutParam()
        {
            var plane = new Plane(Vector3.Forward, 100f);

            var sphere1 = new BoundingSphere(new Vector3(100f, 100f, 200f), 10f);
            sphere1.Intersects(ref plane, out var result1);
            var sphere2 = new BoundingSphere(new Vector3(10f, 10f, 10f), 10f);
            sphere2.Intersects(ref plane, out var result2);
            var sphere3 = new BoundingSphere(new Vector3(0, 0, 100f), 10f);
            sphere3.Intersects(ref plane, out var result3);

            TheResultingValue(result1)
                .ShouldBe(PlaneIntersectionType.Back);
            TheResultingValue(result2)
                .ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3)
                .ShouldBe(PlaneIntersectionType.Intersecting);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsRayCorrectly()
        {
            var sphere = new BoundingSphere(Vector3.Zero, 10f);

            var ray1 = new Ray(new Vector3(0f, 0f, 100f), new Vector3(0f, 0f, -1f));
            var result1 = sphere.Intersects(ray1);
            var ray2 = new Ray(new Vector3(0f, 0f, 100f), new Vector3(0f, 1f, 0f));
            var result2 = sphere.Intersects(ray2);

            TheResultingValue(result1.Value)
                .ShouldBe(90f);
            TheResultingValue(result2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingSphere_CalculatesIntersectsRayCorrectly_WithOutParam()
        {
            var sphere = new BoundingSphere(Vector3.Zero, 10f);

            var ray1 = new Ray(new Vector3(0f, 0f, 100f), new Vector3(0f, 0f, -1f));
            sphere.Intersects(ref ray1, out var result1);
            var ray2 = new Ray(new Vector3(0f, 0f, 100f), new Vector3(0f, 1f, 0f));
            sphere.Intersects(ref ray2, out var result2);

            TheResultingValue(result1.Value)
                .ShouldBe(90f);
            TheResultingValue(result2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingSphere_IsTransformedCorrectly()
        {
            var sphere = new BoundingSphere(Vector3.Zero, 10f);

            var matrix = Matrix.CreateScale(2f) * Matrix.CreateTranslation(1f, 2f, 3f);
            var result = sphere.Transform(matrix);

            TheResultingValue(result)
                .ShouldHaveCenter(1f, 2f, 3f)
                .ShouldHaveRadius(20f);
        }

        [Test]
        public void BoundingSphere_IsTransformedCorrectly_WithOutParam()
        {
            var sphere = new BoundingSphere(Vector3.Zero, 10f);

            var matrix = Matrix.CreateScale(2f) * Matrix.CreateTranslation(1f, 2f, 3f);
            sphere.Transform(ref matrix, out var result);

            TheResultingValue(result)
                .ShouldHaveCenter(1f, 2f, 3f)
                .ShouldHaveRadius(20f);
        }
        
        [Test]
        public void BoundingSphere_TryParse_SucceedsForValidStrings()
        {
            var str = "12.3 45.6 78.9 10.11";
            if (!BoundingSphere.TryParse(str, out var result))
                throw new InvalidOperationException("Unable to parse string to BoundingSphere.");

            TheResultingValue(result)
                .ShouldHaveCenter(12.3f, 45.6f, 78.9f)
                .ShouldHaveRadius(10.11f);
        }

        [Test]
        public void BoundingSphere_TryParse_FailsForInvalidStrings()
        {
            var succeeded = BoundingSphere.TryParse("foo", out var result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void BoundingSphere_Parse_SucceedsForValidStrings()
        {
            var str = "12.3 45.6 78.9 10.11";
            var result = BoundingSphere.Parse(str);

            TheResultingValue(result)
                .ShouldHaveCenter(12.3f, 45.6f, 78.9f)
                .ShouldHaveRadius(10.11f);
        }

        [Test]
        public void BoundingSphere_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => BoundingSphere.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void BoundingSphere_SerializesToJson()
        {
            var sphere = new BoundingSphere(new Vector3(1.2f, 2.3f, 3.4f), 4.5f);
            var json = JsonConvert.SerializeObject(sphere, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""center"":{""x"":1.2,""y"":2.3,""z"":3.4},""radius"":4.5}");
        }

        [Test]
        public void BoundingSphere_SerializesToJson_WhenNullable()
        {
            var sphere = new BoundingSphere(new Vector3(1.2f, 2.3f, 3.4f), 4.5f);
            var json = JsonConvert.SerializeObject((BoundingSphere?)sphere, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""center"":{""x"":1.2,""y"":2.3,""z"":3.4},""radius"":4.5}");
        }

        [Test]
        public void BoundingSphere_DeserializesFromJson()
        {
            const String json = @"{""center"":{""x"":1.2,""y"":2.3,""z"":3.4},""radius"":4.5}";

            var sphere = JsonConvert.DeserializeObject<BoundingSphere>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(sphere)
                .ShouldHaveCenter(1.2f, 2.3f, 3.4f)
                .ShouldHaveRadius(4.5f);
        }

        [Test]
        public void BoundingSphere_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""center"":{""x"":1.2,""y"":2.3,""z"":3.4},""radius"":4.5}";

            var sphere1 = JsonConvert.DeserializeObject<BoundingSphere?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(sphere1.Value)
                .ShouldHaveCenter(1.2f, 2.3f, 3.4f)
                .ShouldHaveRadius(4.5f);

            const String json2 = @"null";

            var sphere2 = JsonConvert.DeserializeObject<BoundingSphere?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(sphere2.HasValue)
                .ShouldBe(false);
        }
    }
}
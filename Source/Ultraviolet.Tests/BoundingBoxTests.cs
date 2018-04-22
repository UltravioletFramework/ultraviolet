using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class BoundingBoxTests : UltravioletTestFramework
    {
        [Test]
        public void BoundingBox_ConstructorSetsValues()
        {
            var box = new BoundingBox(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));

            TheResultingValue(box)
                .ShouldHaveMin(1.2f, 2.3f, 3.4f)
                .ShouldHaveMax(4.5f, 5.6f, 6.7f);
        }
        
        [Test]
        public void BoundingBox_CreateFromPointsWorksCorrectly()
        {
            var box = BoundingBox.CreateFromPoints((IEnumerable<Vector3>)new[] {
                new Vector3(1.2f, 3.4f, 4.5f),
                new Vector3(4.5f, 3.4f, 1.2f),
                new Vector3(1.1f, 2.2f, 3.3f),
                new Vector3(2.3f, 4.5f, 6.7f),
            });

            TheResultingValue(box)
                .ShouldHaveMin(1.1f, 2.2f, 1.2f)
                .ShouldHaveMax(4.5f, 4.5f, 6.7f);
        }

        [Test]
        public void BoundingBox_CreateFromPointsWorksCorrectly_WithOutParam()
        {
            BoundingBox.CreateFromPoints((IEnumerable<Vector3>)new[] {
                new Vector3(1.2f, 3.4f, 4.5f),
                new Vector3(4.5f, 3.4f, 1.2f),
                new Vector3(1.1f, 2.2f, 3.3f),
                new Vector3(2.3f, 4.5f, 6.7f),
            }, out var box);

            TheResultingValue(box)
                .ShouldHaveMin(1.1f, 2.2f, 1.2f)
                .ShouldHaveMax(4.5f, 4.5f, 6.7f);
        }

        [Test]
        public void BoundingBox_CreateFromPointsWorksCorrectly_FromIList()
        {
            var box = BoundingBox.CreateFromPoints(new[] {
                new Vector3(1.2f, 3.4f, 4.5f),
                new Vector3(4.5f, 3.4f, 1.2f),
                new Vector3(1.1f, 2.2f, 3.3f),
                new Vector3(2.3f, 4.5f, 6.7f),
            });

            TheResultingValue(box)
                .ShouldHaveMin(1.1f, 2.2f, 1.2f)
                .ShouldHaveMax(4.5f, 4.5f, 6.7f);
        }

        [Test]
        public void BoundingBox_CreateFromPointsWorksCorrectly_FromIList_WithOutParam()
        {
            BoundingBox.CreateFromPoints(new[] {
                new Vector3(1.2f, 3.4f, 4.5f),
                new Vector3(4.5f, 3.4f, 1.2f),
                new Vector3(1.1f, 2.2f, 3.3f),
                new Vector3(2.3f, 4.5f, 6.7f),
            }, out var box);

            TheResultingValue(box)
                .ShouldHaveMin(1.1f, 2.2f, 1.2f)
                .ShouldHaveMax(4.5f, 4.5f, 6.7f);
        }

        [Test]
        public void BoundingBox_CreateFromSphereWorksCorrectly()
        {
            var sphere = new BoundingSphere(new Vector3(1.2f, 2.3f, 3.4f), 4.5f);

            var box = BoundingBox.CreateFromSphere(sphere);

            TheResultingValue(box)
                .ShouldHaveMin(-3.3f, -2.2f, -1.1f)
                .ShouldHaveMax(5.7f, 6.8f, 7.9f);
        }

        [Test]
        public void BoundingBox_CreateFromSphereWorksCorrectly_WithOutParam()
        {
            var sphere = new BoundingSphere(new Vector3(1.2f, 2.3f, 3.4f), 4.5f);

            BoundingBox.CreateFromSphere(ref sphere, out var box);

            TheResultingValue(box)
                .ShouldHaveMin(-3.3f, -2.2f, -1.1f)
                .ShouldHaveMax(5.7f, 6.8f, 7.9f);
        }

        [Test]
        public void BoundingBox_CreateMergedWorksCorrectly()
        {
            var box1 = new BoundingBox(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var box2 = new BoundingBox(new Vector3(-1.0f, 2.0f, -3.0f), new Vector3(4.0f, -5.0f, 6.0f));

            var result = BoundingBox.CreateMerged(box1, box2);

            TheResultingValue(result)
                .ShouldHaveMin(-1.0f, 2.0f, -3.0f)
                .ShouldHaveMax(4.5f, 5.6f, 6.7f);
        }

        [Test]
        public void BoundingBox_CreateMergedWorksCorrectly_WithOutParam()
        {
            var box1 = new BoundingBox(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var box2 = new BoundingBox(new Vector3(-1.0f, 2.0f, -3.0f), new Vector3(4.0f, -5.0f, 6.0f));

            BoundingBox.CreateMerged(ref box1, ref box2, out var result);

            TheResultingValue(result)
                .ShouldHaveMin(-1.0f, 2.0f, -3.0f)
                .ShouldHaveMax(4.5f, 5.6f, 6.7f);
        }

        [Test]
        public void BoundingBox_GetCornerWorksCorrectly()
        {
            var box = new BoundingBox(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));

            var corner0 = box.GetCorner(0);
            TheResultingValue(corner0)
                .ShouldBe(1.2f, 5.6f, 6.7f);

            var corner1 = box.GetCorner(1);
            TheResultingValue(corner1)
                .ShouldBe(4.5f, 5.6f, 6.7f);

            var corner2 = box.GetCorner(2);
            TheResultingValue(corner2)
                .ShouldBe(4.5f, 2.3f, 6.7f);

            var corner3 = box.GetCorner(3);
            TheResultingValue(corner3)
                .ShouldBe(1.2f, 2.3f, 6.7f);

            var corner4 = box.GetCorner(4);
            TheResultingValue(corner4)
                .ShouldBe(1.2f, 5.6f, 3.4f);

            var corner5 = box.GetCorner(5);
            TheResultingValue(corner5)
                .ShouldBe(4.5f, 5.6f, 3.4f);

            var corner6 = box.GetCorner(6);
            TheResultingValue(corner6)
                .ShouldBe(4.5f, 2.3f, 3.4f);

            var corner7 = box.GetCorner(7);
            TheResultingValue(corner7)
                .ShouldBe(1.2f, 2.3f, 3.4f);
        }

        [Test]
        public void BoundingBox_GetCornerWorksCorrectly_WithOutParam()
        {
            var box = new BoundingBox(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));

            box.GetCorner(0, out var corner0);
            TheResultingValue(corner0)
                .ShouldBe(1.2f, 5.6f, 6.7f);

            box.GetCorner(1, out var corner1);
            TheResultingValue(corner1)
                .ShouldBe(4.5f, 5.6f, 6.7f);

            box.GetCorner(2, out var corner2);
            TheResultingValue(corner2)
                .ShouldBe(4.5f, 2.3f, 6.7f);

            box.GetCorner(3, out var corner3);
            TheResultingValue(corner3)
                .ShouldBe(1.2f, 2.3f, 6.7f);

            box.GetCorner(4, out var corner4);
            TheResultingValue(corner4)
                .ShouldBe(1.2f, 5.6f, 3.4f);

            box.GetCorner(5, out var corner5);
            TheResultingValue(corner5)
                .ShouldBe(4.5f, 5.6f, 3.4f);

            box.GetCorner(6, out var corner6);
            TheResultingValue(corner6)
                .ShouldBe(4.5f, 2.3f, 3.4f);

            box.GetCorner(7, out var corner7);
            TheResultingValue(corner7)
                .ShouldBe(1.2f, 2.3f, 3.4f);
        }

        [Test]
        public void BoundingBox_GetCornersWorksCorrectly()
        {
            var box = new BoundingBox(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));

            var corners = new Vector3[BoundingBox.CornerCount];
            box.GetCorners(corners);

            TheResultingValue(corners[0])
                .ShouldBe(1.2f, 5.6f, 6.7f);

            TheResultingValue(corners[1])
                .ShouldBe(4.5f, 5.6f, 6.7f);

            TheResultingValue(corners[2])
                .ShouldBe(4.5f, 2.3f, 6.7f);

            TheResultingValue(corners[3])
                .ShouldBe(1.2f, 2.3f, 6.7f);

            TheResultingValue(corners[4])
                .ShouldBe(1.2f, 5.6f, 3.4f);

            TheResultingValue(corners[5])
                .ShouldBe(4.5f, 5.6f, 3.4f);

            TheResultingValue(corners[6])
                .ShouldBe(4.5f, 2.3f, 3.4f);

            TheResultingValue(corners[7])
                .ShouldBe(1.2f, 2.3f, 3.4f);
        }

        [Test]
        public void BoundingBox_CalculatesContainsVector3Correctly()
        {
            var box = new BoundingBox(new Vector3(-10f, -10f, -10f), new Vector3(20f, 20f, 20f));

            var point1 = new Vector3(11f, 9f, 10f);
            var result1 = box.Contains(point1);
            var point2 = new Vector3(1000f, 1000f, 1000f);
            var result2 = box.Contains(point2);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingBox_CalculatesContainsVector3Correctly_WithOutParam()
        {
            var box = new BoundingBox(new Vector3(-10f, -10f, -10f), new Vector3(20f, 20f, 20f));

            var point1 = new Vector3(11f, 9f, 10f);
            box.Contains(ref point1, out ContainmentType result1);
            var point2 = new Vector3(1000f, 1000f, 1000f);
            box.Contains(ref point2, out ContainmentType result2);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Disjoint);
        }
        
        [Test]
        public void BoundingBox_CalculatesContainsBoundingSphereCorrectly()
        {
            var box = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 5f);
            var result1 = box.Contains(sphere1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 8f);
            var result2 = box.Contains(sphere2);
            var sphere3 = new BoundingSphere(new Vector3(300f, 0, 0), 1f);
            var result3 = box.Contains(sphere3);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result3)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingBox_CalculatesContainsBoundingSphereCorrectly_WithOutParam()
        {
            var box = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));

            var sphere1 = new BoundingSphere(new Vector3(10f, 10f, 10f), 5f);
            box.Contains(ref sphere1, out var result1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 8f);
            box.Contains(ref sphere2, out var result2);
            var sphere3 = new BoundingSphere(new Vector3(300f, 0, 0), 1f);
            box.Contains(ref sphere3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result3)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingBox_CalculatesContainsBoundingBoxCorrectly()
        {
            var box0 = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));

            var box1 = new BoundingBox(new Vector3(5f, 5f, 5f), new Vector3(15f, 15f, 15f));
            var result1 = box0.Contains(box1);
            var box2 = new BoundingBox(new Vector3(-8f, -8f, -8f), new Vector3(8f, 8f, 8f));
            var result2 = box0.Contains(box2);
            var box3 = new BoundingBox(new Vector3(300f, -1f, -1f), new Vector3(301f, 1f, 1f));
            var result3 = box0.Contains(box3);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result3)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingBox_CalculatesContainsBoundingBoxCorrectly_WithOutParam()
        {
            var box0 = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));

            var box1 = new BoundingBox(new Vector3(5f, 5f, 5f), new Vector3(15f, 15f, 15f));
            box0.Contains(ref box1, out var result1);
            var box2 = new BoundingBox(new Vector3(-8f, -8f, -8f), new Vector3(8f, 8f, 8f));
            box0.Contains(ref box2, out var result2);
            var box3 = new BoundingBox(new Vector3(300f, -1f, -1f), new Vector3(301f, 1f, 1f));
            box0.Contains(ref box3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Contains);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result3)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingBox_CalculatesContainsBoundingFrustumCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var box1 = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));
            var result1 = box1.Contains(frustum);
            var box2 = new BoundingBox(new Vector3(-10000f, -10000f, -10000f), new Vector3(-4700f, -2003f, -3002f));
            var result2 = box2.Contains(frustum);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingBox_CalculatesContainsBoundingFrustumCorrectly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var box1 = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));
            box1.Contains(frustum, out var result1);
            var box2 = new BoundingBox(new Vector3(-10000f, -10000f, -10000f), new Vector3(-4700f, -2003f, -3002f));
            box2.Contains(frustum, out var result2);

            TheResultingValue(result1)
                .ShouldBe(ContainmentType.Intersects);
            TheResultingValue(result2)
                .ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingBox_CalculatesIntersectsBoundingSphereCorrectly()
        {
            var box = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));

            var sphere1 = new BoundingSphere(new Vector3(25f, 25f, 25f), 5f);
            var result1 = box.Intersects(sphere1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 8f);
            var result2 = box.Intersects(sphere2);
            var sphere3 = new BoundingSphere(new Vector3(0, 0, 0), 1f);
            var result3 = box.Intersects(sphere3);

            TheResultingValue(result1)
                .ShouldBe(false);
            TheResultingValue(result2)
                .ShouldBe(true);
            TheResultingValue(result3)
                .ShouldBe(true);
        }

        [Test]
        public void BoundingBox_CalculatesIntersectsBoundingSphereCorrectly_WithOutParam()
        {
            var box = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));

            var sphere1 = new BoundingSphere(new Vector3(25f, 25f, 25f), 5f);
            box.Intersects(ref sphere1, out var result1);
            var sphere2 = new BoundingSphere(new Vector3(0, 0, 0), 8f);
            box.Intersects(ref sphere2, out var result2);
            var sphere3 = new BoundingSphere(new Vector3(0, 0, 0), 1f);
            box.Intersects(ref sphere3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(false);
            TheResultingValue(result2)
                .ShouldBe(true);
            TheResultingValue(result3)
                .ShouldBe(true);
        }

        [Test]
        public void BoundingBox_CalculatesIntersectsBoundingBoxCorrectly()
        {
            var box0 = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));

            var box1 = new BoundingBox(new Vector3(25f, 25f, 25f), new Vector3(35f, 35f, 35f));
            var result1 = box0.Intersects(box1);
            var box2 = new BoundingBox(new Vector3(-8f, -8f, -8f), new Vector3(8f, 8f, 8f));
            var result2 = box0.Intersects(box2);
            var box3 = new BoundingBox(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, 1f));
            var result3 = box0.Intersects(box3);

            TheResultingValue(result1)
                .ShouldBe(false);
            TheResultingValue(result2)
                .ShouldBe(true);
            TheResultingValue(result3)
                .ShouldBe(true);
        }

        [Test]
        public void BoundingBox_CalculatesIntersectsBoundingBoxCorrectly_WithOutParam()
        {
            var box0 = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));

            var box1 = new BoundingBox(new Vector3(25f, 25f, 25f), new Vector3(35f, 35f, 35f));
            box0.Intersects(ref box1, out var result1);
            var box2 = new BoundingBox(new Vector3(-8f, -8f, -8f), new Vector3(8f, 8f, 8f));
            box0.Intersects(ref box2, out var result2);
            var box3 = new BoundingBox(new Vector3(-1f, -1f, -1f), new Vector3(1f, 1f, 1f));
            box0.Intersects(ref box3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(false);
            TheResultingValue(result2)
                .ShouldBe(true);
            TheResultingValue(result3)
                .ShouldBe(true);
        }

        [Test]
        public void BoundingBox_CalculatesIntersectsBoundingFrustumCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var box1 = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));
            var result1 = box1.Intersects(frustum);
            var box2 = new BoundingBox(new Vector3(-50f, -10f, -10f), new Vector3(-20f, 10f, 10f));
            var result2 = box2.Intersects(frustum);

            TheResultingValue(result1)
                .ShouldBe(true);
            TheResultingValue(result2)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingBox_CalculatesIntersectsBoundingFrustumCorrectly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var box1 = new BoundingBox(new Vector3(0f, 0f, 0f), new Vector3(20f, 20f, 20f));
            box1.Intersects(frustum, out var result1);
            var box2 = new BoundingBox(new Vector3(-50f, -10f, -10f), new Vector3(-20f, 10f, 10f));
            box2.Intersects(frustum, out var result2);

            TheResultingValue(result1)
                .ShouldBe(true);
            TheResultingValue(result2)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingBox_CalculatesIntersectsPlaneCorrectly()
        {
            var box = new BoundingBox(new Vector3(-10f), new Vector3(10f));
            var plane1 = new Plane(new Vector3(1f, 0f, 0f), 0f);
            var plane2 = new Plane(new Vector3(1f, 0f, 0f), 100f);
            var plane3 = new Plane(new Vector3(1f, 0f, 0f), -100f);

            var result1 = box.Intersects(plane1);
            var result2 = box.Intersects(plane2);
            var result3 = box.Intersects(plane3);

            TheResultingValue(result1)
                .ShouldBe(PlaneIntersectionType.Intersecting);
            TheResultingValue(result2)
                .ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3)
                .ShouldBe(PlaneIntersectionType.Back);
        }

        [Test]
        public void BoundingBox_CalculatesIntersectsPlaneCorrectly_WithOutParam()
        {
            var box = new BoundingBox(new Vector3(-10f), new Vector3(10f));
            var plane1 = new Plane(new Vector3(1f, 0f, 0f), 0f);
            var plane2 = new Plane(new Vector3(1f, 0f, 0f), 100f);
            var plane3 = new Plane(new Vector3(1f, 0f, 0f), -100f);

            box.Intersects(ref plane1, out var result1);
            box.Intersects(ref plane2, out var result2);
            box.Intersects(ref plane3, out var result3);

            TheResultingValue(result1)
                .ShouldBe(PlaneIntersectionType.Intersecting);
            TheResultingValue(result2)
                .ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3)
                .ShouldBe(PlaneIntersectionType.Back);
        }

        [Test]
        public void BoundingBox_CalculatesRayIntersectionCorrectly_WhenIntersectionExists()
        {
            var ray = new Ray(new Vector3(0, 0, 100f), new Vector3(0, 0, -1f));
            var box = new BoundingBox(new Vector3(-10f), new Vector3(10f));

            var result = box.Intersects(ray);

            TheResultingValue(result.Value)
                .ShouldBe(90f);
        }

        [Test]
        public void BoundingBox_CalculatesRayIntersectionCorrectly_WhenIntersectionExists_WithOutParam()
        {
            var ray = new Ray(new Vector3(0, 0, 100f), new Vector3(0, 0, -1f));
            var box = new BoundingBox(new Vector3(-10f), new Vector3(10f));

            box.Intersects(ref ray, out Single? result);

            TheResultingValue(result.Value)
                .ShouldBe(90f);
        }

        [Test]
        public void BoundingBox_CalculatesRayIntersectionCorrectly_WhenNoIntersectionExists()
        {
            var ray = new Ray(new Vector3(0, 0, 100f), new Vector3(0, 0, 100f));
            var box = new BoundingBox(new Vector3(-10f), new Vector3(10f));

            var result = box.Intersects(ray);

            TheResultingValue(result.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingBox_CalculatesRayIntersectionCorrectly_WhenNoIntersectionExists_WithOutParam()
        {
            var ray = new Ray(new Vector3(0, 0, 100f), new Vector3(0, 0, 100f));
            var box = new BoundingBox(new Vector3(-10f), new Vector3(10f));

            box.Intersects(ref ray, out Single? result);

            TheResultingValue(result.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void BoundingBox_TryParse_SucceedsForValidStrings()
        {
            var str = "12.3 45.6 78.9 10.11 11.12 13.14";
            if (!BoundingBox.TryParse(str, out var result))
                throw new InvalidOperationException("Unable to parse string to BoundingBox.");

            TheResultingValue(result)
                .ShouldHaveMin(12.3f, 45.6f, 78.9f)
                .ShouldHaveMax(10.11f, 11.12f, 13.14f);
        }

        [Test]
        public void BoundingBox_TryParse_FailsForInvalidStrings()
        {
            var succeeded = BoundingBox.TryParse("foo", out var result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void BoundingBox_Parse_SucceedsForValidStrings()
        {
            var str = "12.3 45.6 78.9 10.11 11.12 13.14";
            var result = BoundingBox.Parse(str);

            TheResultingValue(result)
                .ShouldHaveMin(12.3f, 45.6f, 78.9f)
                .ShouldHaveMax(10.11f, 11.12f, 13.14f);
        }

        [Test]
        public void BoundingBox_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => BoundingSphere.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void BoundingBox_SerializesToJson()
        {
            var box = new BoundingBox(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var json = JsonConvert.SerializeObject(box,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""min"":{""x"":1.2,""y"":2.3,""z"":3.4},""max"":{""x"":4.5,""y"":5.6,""z"":6.7}}");
        }

        [Test]
        public void BoundingBox_SerializesToJson_WhenNullable()
        {
            var box = new BoundingBox(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var json = JsonConvert.SerializeObject((BoundingBox?)box, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""min"":{""x"":1.2,""y"":2.3,""z"":3.4},""max"":{""x"":4.5,""y"":5.6,""z"":6.7}}");
        }

        [Test]
        public void BoundingBox_DeserializesFromJson()
        {
            const String json = @"{""min"":{""x"":1.2,""y"":2.3,""z"":3.4},""max"":{""x"":4.5,""y"":5.6,""z"":6.7}}";

            var box = JsonConvert.DeserializeObject<BoundingBox>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(box)
                .ShouldHaveMin(1.2f, 2.3f, 3.4f)
                .ShouldHaveMax(4.5f, 5.6f, 6.7f);
        }

        [Test]
        public void BoundingBox_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""min"":{""x"":1.2,""y"":2.3,""z"":3.4},""max"":{""x"":4.5,""y"":5.6,""z"":6.7}}";

            var box1 = JsonConvert.DeserializeObject<BoundingBox?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(box1.Value)
                .ShouldHaveMin(1.2f, 2.3f, 3.4f)
                .ShouldHaveMax(4.5f, 5.6f, 6.7f);

            const String json2 = @"null";

            var box2 = JsonConvert.DeserializeObject<BoundingBox?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(box2.HasValue)
                .ShouldBe(false);
        }
    }
}
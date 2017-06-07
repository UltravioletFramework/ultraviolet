using System;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class BoundingFrustumTests : UltravioletTestFramework
    {
        [Test]
        public void BoundingFrustum_ConstructorSetsValues()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            TheResultingValue(frustum.Near).WithinDelta(0.0001f)
                .ShouldHaveNormal(0f, 0f, 1f).ShouldHaveDistance(-4f);
            TheResultingValue(frustum.Far).WithinDelta(0.0001f)
                .ShouldHaveNormal(0f, 0f, -1f).ShouldHaveDistance(-995.0006f);
            TheResultingValue(frustum.Left).WithinDelta(0.0001f)
                .ShouldHaveNormal(-0.8753f, 0f, 0.4834f).ShouldHaveDistance(-2.4172f);
            TheResultingValue(frustum.Right).WithinDelta(0.0001f)
                .ShouldHaveNormal(0.8753f, 0f, 0.4834f).ShouldHaveDistance(-2.4172f);
            TheResultingValue(frustum.Top).WithinDelta(0.0001f)
                .ShouldHaveNormal(0f, 0.9238f, 0.3826f).ShouldHaveDistance(-1.9134f);
            TheResultingValue(frustum.Bottom).WithinDelta(0.0001f)
                .ShouldHaveNormal(0f, -0.9238f, 0.3826f).ShouldHaveDistance(-1.9134f);
        }

        [Test]
        public void BoundingFrustum_CalculatesCornersCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var corners = new Vector3[BoundingFrustum.CornerCount];
            frustum.GetCorners(corners);

            TheResultingValue(corners[0]).WithinDelta(0.0001f)
                .ShouldBe(-0.5522f, 0.4142f, 4f);
            TheResultingValue(corners[1]).WithinDelta(0.0001f)
                .ShouldBe(0.5522f, 0.4142f, 4f);
            TheResultingValue(corners[2]).WithinDelta(0.0001f)
                .ShouldBe(0.5522f, -0.4142f, 4f);
            TheResultingValue(corners[3]).WithinDelta(0.0001f)
                .ShouldBe(-0.5522f, -0.4142f, 4f);
            TheResultingValue(corners[4]).WithinDelta(0.0001f)
                .ShouldBe(-552.2851f, 414.2138f, -995.0007f);
            TheResultingValue(corners[5]).WithinDelta(0.0001f)
                .ShouldBe(552.2851f, 414.2138f, -995.0007f);
            TheResultingValue(corners[6]).WithinDelta(0.0001f)
                .ShouldBe(552.2851f, -414.2138f, -995.0007f);
            TheResultingValue(corners[7]).WithinDelta(0.0001f)
                .ShouldBe(-552.2851f, -414.2138f, -995.0007f);
        }

        [Test]
        public void BoundingFrustum_CalculatesContainsVector3Correctly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var c1 = frustum.Contains(new Vector3(0, 0, 0));
            var c2 = frustum.Contains(new Vector3(10000, 0, 0));

            TheResultingValue(c1).ShouldBe(ContainmentType.Contains);
            TheResultingValue(c2).ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingFrustum_CalculatesContainsVector3Correctly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var pt1 = new Vector3(0, 0, 0);
            frustum.Contains(ref pt1, out ContainmentType c1);

            var pt2 = new Vector3(10000, 0, 0);
            frustum.Contains(ref pt2, out ContainmentType c2);

            TheResultingValue(c1).ShouldBe(ContainmentType.Contains);
            TheResultingValue(c2).ShouldBe(ContainmentType.Disjoint);
        }

        [Test]
        public void BoundingFrustum_CalculatesContainsBoundingFrustumCorrectly()
        {
            var view1 = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj1 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum1 = new BoundingFrustum(view1 * proj1);

            var view2 = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(0, 0, 10), Vector3.Up);
            var proj2 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum2 = new BoundingFrustum(view2 * proj2);

            var view3 = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(2, 0, 0), Vector3.Up);
            var proj3 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum3 = new BoundingFrustum(view3 * proj3);

            var r1 = frustum1.Contains(frustum2);
            var r2 = frustum1.Contains(frustum3);
            var r3 = frustum1.Contains(frustum1);

            TheResultingValue(r1).ShouldBe(ContainmentType.Disjoint);
            TheResultingValue(r2).ShouldBe(ContainmentType.Intersects);
            TheResultingValue(r3).ShouldBe(ContainmentType.Contains);
        }

        [Test]
        public void BoundingFrustum_CalculatesContainsBoundingFrustumCorrectly_WithOutParam()
        {
            var view1 = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj1 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum1 = new BoundingFrustum(view1 * proj1);

            var view2 = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(0, 0, 10), Vector3.Up);
            var proj2 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum2 = new BoundingFrustum(view2 * proj2);

            var view3 = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(2, 0, 0), Vector3.Up);
            var proj3 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum3 = new BoundingFrustum(view3 * proj3);

            frustum1.Contains(frustum2, out ContainmentType r1);
            frustum1.Contains(frustum3, out ContainmentType r2);
            frustum1.Contains(frustum1, out ContainmentType r3);

            TheResultingValue(r1).ShouldBe(ContainmentType.Disjoint);
            TheResultingValue(r2).ShouldBe(ContainmentType.Intersects);
            TheResultingValue(r3).ShouldBe(ContainmentType.Contains);
        }

        [Test]
        public void BoundingFrustum_CalculatesIntersectsRayCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var ray1 = new Ray(new Vector3(0, 0, 10), Vector3.Zero - new Vector3(0, 0, 10));
            var ray2 = new Ray(new Vector3(0, 0, 10), -ray1.Direction);

            var result1 = frustum.Intersects(ray1);
            var result2 = frustum.Intersects(ray2);

            TheResultingValue(result1.Value).ShouldBe(0.6f);
            TheResultingValue(result2.HasValue).ShouldBe(false);
        }

        [Test]
        public void BoundingFrustum_CalculatesIntersectsRayCorrectly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var ray1 = new Ray(new Vector3(0, 0, 10), Vector3.Zero - new Vector3(0, 0, 10));
            var ray2 = new Ray(new Vector3(0, 0, 10), -ray1.Direction);

            frustum.Intersects(ref ray1, out Single? result1);
            frustum.Intersects(ref ray2, out Single? result2);

            TheResultingValue(result1.Value).ShouldBe(0.6f);
            TheResultingValue(result2.HasValue).ShouldBe(false);
        }

        [Test]
        public void BoundingFrustum_CalculatesIntersectsPlaneCorrectly()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var plane1 = new Plane(frustum.Near.Normal, frustum.Near.D - 1000f);
            var plane2 = new Plane(frustum.Near.Normal, frustum.Near.D + 1000f);
            var plane3 = new Plane(frustum.Near.Normal, frustum.Near.D + 500f);

            var result1 = frustum.Intersects(plane1);
            var result2 = frustum.Intersects(plane2);
            var result3 = frustum.Intersects(plane3);

            TheResultingValue(result1).ShouldBe(PlaneIntersectionType.Back);
            TheResultingValue(result2).ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3).ShouldBe(PlaneIntersectionType.Intersecting);
        }

        [Test]
        public void BoundingFrustum_CalculatesIntersectsPlaneCorrectly_WithOutParam()
        {
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum = new BoundingFrustum(view * proj);

            var plane1 = new Plane(frustum.Near.Normal, frustum.Near.D - 1000f);
            var plane2 = new Plane(frustum.Near.Normal, frustum.Near.D + 1000f);
            var plane3 = new Plane(frustum.Near.Normal, frustum.Near.D + 500f);

            frustum.Intersects(ref plane1, out PlaneIntersectionType result1);
            frustum.Intersects(ref plane2, out PlaneIntersectionType result2);
            frustum.Intersects(ref plane3, out PlaneIntersectionType result3);

            TheResultingValue(result1).ShouldBe(PlaneIntersectionType.Back);
            TheResultingValue(result2).ShouldBe(PlaneIntersectionType.Front);
            TheResultingValue(result3).ShouldBe(PlaneIntersectionType.Intersecting);
        }

        [Test]
        public void BoundingFrustum_CalculatesIntersectsBoundingFrustumCorrectly()
        {
            var view1 = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj1 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum1 = new BoundingFrustum(view1 * proj1);

            var view2 = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(0, 0, 10), Vector3.Up);
            var proj2 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum2 = new BoundingFrustum(view2 * proj2);

            var view3 = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(2, 0, 0), Vector3.Up);
            var proj3 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum3 = new BoundingFrustum(view3 * proj3);

            var r1 = frustum1.Intersects(frustum2);
            var r2 = frustum1.Intersects(frustum3);
            var r3 = frustum1.Intersects(frustum1);

            TheResultingValue(r1).ShouldBe(false);
            TheResultingValue(r2).ShouldBe(true);
            TheResultingValue(r3).ShouldBe(true);
        }

        [Test]
        public void BoundingFrustum_CalculatesIntersectsBoundingFrustumCorrectly_WithOutParam()
        {
            var view1 = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            var proj1 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum1 = new BoundingFrustum(view1 * proj1);

            var view2 = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(0, 0, 10), Vector3.Up);
            var proj2 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum2 = new BoundingFrustum(view2 * proj2);

            var view3 = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(2, 0, 0), Vector3.Up);
            var proj3 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, 4f / 3f, 1f, 1000f);
            var frustum3 = new BoundingFrustum(view3 * proj3);

            frustum1.Intersects(frustum2, out Boolean r1);
            frustum1.Intersects(frustum3, out Boolean r2);
            frustum1.Intersects(frustum1, out Boolean r3);

            TheResultingValue(r1).ShouldBe(false);
            TheResultingValue(r2).ShouldBe(true);
            TheResultingValue(r3).ShouldBe(true);
        }
    }
}

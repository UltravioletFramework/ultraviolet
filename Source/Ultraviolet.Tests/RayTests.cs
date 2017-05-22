using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class RayTests : UltravioletTestFramework
    {
        [Test]
        public void Ray_IsConstructedProperly()
        {
            var result = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));

            TheResultingValue(result)
                .ShouldHavePosition(1.2f, 2.3f, 3.4f)
                .ShouldHaveDirection(4.5f, 5.6f, 6.7f);
        }

        [Test]
        public void Ray_OpEquality()
        {
            var ray1 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var ray2 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var ray3 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(5.5f, 5.5f, 5.5f));
            var ray4 = new Ray(new Vector3(2.2f, 2.2f, 2.2f), new Vector3(4.5f, 5.6f, 6.7f));

            TheResultingValue(ray1 == ray2).ShouldBe(true);
            TheResultingValue(ray1 == ray3).ShouldBe(false);
            TheResultingValue(ray1 == ray4).ShouldBe(false);
        }

        [Test]
        public void Ray_OpInequality()
        {
            var ray1 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var ray2 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var ray3 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(5.5f, 5.5f, 5.5f));
            var ray4 = new Ray(new Vector3(2.2f, 2.2f, 2.2f), new Vector3(4.5f, 5.6f, 6.7f));

            TheResultingValue(ray1 != ray2).ShouldBe(false);
            TheResultingValue(ray1 != ray3).ShouldBe(true);
            TheResultingValue(ray1 != ray4).ShouldBe(true);
        }

        [Test]
        public void Ray_EqualsObject()
        {
            var ray1 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var ray2 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));

            TheResultingValue(ray1.Equals((Object)ray2)).ShouldBe(true);
            TheResultingValue(ray1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void Ray_EqualsRay()
        {
            var ray1 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var ray2 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var ray3 = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(5.5f, 5.5f, 5.5f));
            var ray4 = new Ray(new Vector3(2.2f, 2.2f, 2.2f), new Vector3(4.5f, 5.6f, 6.7f));

            TheResultingValue(ray1.Equals(ray2)).ShouldBe(true);
            TheResultingValue(ray1.Equals(ray3)).ShouldBe(false);
            TheResultingValue(ray1.Equals(ray4)).ShouldBe(false);
        }

        [Test]
        public void Ray_CalculatesPlaneIntersectionCorrectly_WhenIntersectionExists()
        {
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(0, 0, 1));

            var planeFacingAwayFromRay = new Plane(0, 0, 1, -100);
            var planeFacingAwayFromRayResult = ray.Intersects(planeFacingAwayFromRay);

            TheResultingValue(planeFacingAwayFromRayResult.Value)
                .ShouldBe(100f);

            var planeFacingTowardsRay = new Plane(0, 0, -1, 100);
            var planeFacingTowardsRayResult = ray.Intersects(planeFacingTowardsRay);

            TheResultingValue(planeFacingTowardsRayResult.Value)
                .ShouldBe(100f);
        }

        [Test]
        public void Ray_CalculatesPlaneIntersectionCorrectly_WhenIntersectionExists_WithOutParam()
        {
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(0, 0, 1));

            var planeFacingAwayFromRay = new Plane(0, 0, 1, -100);
            ray.Intersects(ref planeFacingAwayFromRay, out Single? planeFacingAwayFromRayResult);

            TheResultingValue(planeFacingAwayFromRayResult.Value)
                .ShouldBe(100f);

            var planeFacingTowardsRay = new Plane(0, 0, -1, 100);
            ray.Intersects(ref planeFacingTowardsRay, out Single? planeFacingTowardsRayResult);

            TheResultingValue(planeFacingTowardsRayResult.Value)
                .ShouldBe(100f);
        }

        [Test]
        public void Ray_CalculatesPlaneIntersectionCorrectly_WhenNoIntersectionExists()
        {
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
            var plane = new Plane(0, 1, 0, -100);

            var result = ray.Intersects(plane);

            TheResultingValue(result.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Ray_CalculatesPlaneIntersectionCorrectly_WhenNoIntersectionExists_WithOutParam()
        {
            var ray = new Ray(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
            var plane = new Plane(0, 1, 0, -100);

            ray.Intersects(ref plane, out Single? result);

            TheResultingValue(result.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void Ray_TryParse_SucceedsForValidStrings()
        {
            if (!Ray.TryParse("1.2 2.3 3.4 4.5 5.6 6.7", out Ray result))
                throw new InvalidOperationException("Unable to parse string to Point.");

            TheResultingValue(result)
                .ShouldHavePosition(1.2f, 2.3f, 3.4f)
                .ShouldHaveDirection(4.5f, 5.6f, 6.7f);
        }

        [Test]
        public void Ray_TryParse_FailsForInvalidStrings()
        {
            var succeeded = Ray.TryParse("foo", out Ray result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Ray_Parse_SucceedsForValidStrings()
        {
            var result = Ray.Parse("1.2 2.3 3.4 4.5 5.6 6.7");

            TheResultingValue(result)
                .ShouldHavePosition(1.2f, 2.3f, 3.4f)
                .ShouldHaveDirection(4.5f, 5.6f, 6.7f);
        }

        [Test]
        public void Ray_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Ray.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Ray_Parse_CanRoundTrip()
        {
            var size1 = Ray.Parse("1.2 2.3 3.4 4.5 5.6 6.7");
            var size2 = Ray.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }
        
        [Test]
        public void Ray_SerializesToJson()
        {
            var ray = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var json = JsonConvert.SerializeObject(ray);

            TheResultingString(json).ShouldBe(@"{""position"":{""x"":1.2,""y"":2.3,""z"":3.4},""direction"":{""x"":4.5,""y"":5.6,""z"":6.7}}");
        }

        [Test]
        public void Ray_SerializesToJson_WhenNullable()
        {
            var ray = new Ray(new Vector3(1.2f, 2.3f, 3.4f), new Vector3(4.5f, 5.6f, 6.7f));
            var json = JsonConvert.SerializeObject((Ray?)ray);

            TheResultingString(json).ShouldBe(@"{""position"":{""x"":1.2,""y"":2.3,""z"":3.4},""direction"":{""x"":4.5,""y"":5.6,""z"":6.7}}");
        }

        [Test]
        public void Ray_DeserializesFromJson()
        {
            const String json = (@"{""position"":{""x"":1.2,""y"":2.3,""z"":3.4},""direction"":{""x"":4.5,""y"":5.6,""z"":6.7}}");

            var ray = JsonConvert.DeserializeObject<Ray>(json);

            TheResultingValue(ray)
                .ShouldHavePosition(1.2f, 2.3f, 3.4f)
                .ShouldHaveDirection(4.5f, 5.6f, 6.7f);
        }

        [Test]
        public void Ray_DeserializesFromJson_WhenNullable()
        {
            const String json1 = (@"{""position"":{""x"":1.2,""y"":2.3,""z"":3.4},""direction"":{""x"":4.5,""y"":5.6,""z"":6.7}}");

            var ray1 = JsonConvert.DeserializeObject<Ray?>(json1);

            TheResultingValue(ray1.Value)
                .ShouldHavePosition(1.2f, 2.3f, 3.4f)
                .ShouldHaveDirection(4.5f, 5.6f, 6.7f);

            const String json2 = @"null";

            var ray2 = JsonConvert.DeserializeObject<Ray?>(json2);

            TheResultingValue(ray2.HasValue)
                .ShouldBe(false);
        }
    }
}

using System;
using NUnit.Framework;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestFixture]
    public class CircleFTests : UltravioletTestFramework
    {
        [Test]
        public void CircleF_IsConstructedProperly()
        {
            var result = new CircleF(123.45f, 456.78f, 100.10f);

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveRadius(100.10f);
        }

        [Test]
        public void CircleF_OpEquality()
        {
            var circle1 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle2 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle3 = new CircleF(123.45f, 555, 100.10f);
            var circle4 = new CircleF(222, 456.78f, 100.10f);
            var circle5 = new CircleF(123.45f, 456.78f, 200);

            TheResultingValue(circle1 == circle2).ShouldBe(true);
            TheResultingValue(circle1 == circle3).ShouldBe(false);
            TheResultingValue(circle1 == circle4).ShouldBe(false);
            TheResultingValue(circle1 == circle5).ShouldBe(false);
        }

        [Test]
        public void CircleF_OpInequality()
        {
            var circle1 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle2 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle3 = new CircleF(123.45f, 555, 100.10f);
            var circle4 = new CircleF(222, 456.78f, 100.10f);
            var circle5 = new CircleF(123.45f, 456.78f, 200);

            TheResultingValue(circle1 != circle2).ShouldBe(false);
            TheResultingValue(circle1 != circle3).ShouldBe(true);
            TheResultingValue(circle1 != circle4).ShouldBe(true);
            TheResultingValue(circle1 != circle5).ShouldBe(true);
        }

        [Test]
        public void CircleF_EqualsObject()
        {
            var circle1 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle2 = new CircleF(123.45f, 456.78f, 100.10f);

            TheResultingValue(circle1.Equals((Object)circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void CircleF_EqualsCircleF()
        {
            var circle1 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle2 = new CircleF(123.45f, 456.78f, 100.10f);
            var circle3 = new CircleF(123.45f, 555.55f, 100.10f);
            var circle4 = new CircleF(222.22f, 456.78f, 100.10f);

            TheResultingValue(circle1.Equals(circle2)).ShouldBe(true);
            TheResultingValue(circle1.Equals(circle3)).ShouldBe(false);
            TheResultingValue(circle1.Equals(circle4)).ShouldBe(false);
        }

        [Test]
        public void CircleF_TryParse_SucceedsForValidStrings()
        {
            var str = "123.45 456.78 100.10";
            var result = default(CircleF);
            if (!CircleF.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to CircleF.");

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveRadius(100.10f);
        }

        [Test]
        public void CircleF_TryParse_FailsForInvalidStrings()
        {
            var result    = default(CircleF);
            var succeeded = CircleF.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void CircleF_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 100.10";
            var result = CircleF.Parse(str);

            TheResultingValue(result)
                .ShouldHavePosition(123.45f, 456.78f)
                .ShouldHaveRadius(100.10f);
        }

        [Test]
        public void CircleF_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => CircleF.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void CircleF_Parse_CanRoundTrip()
        {
            var circle1 = CircleF.Parse("123.4 456.7 100");
            var circle2 = CircleF.Parse(circle1.ToString());

            TheResultingValue(circle1 == circle2).ShouldBe(true);
        }
    }
}

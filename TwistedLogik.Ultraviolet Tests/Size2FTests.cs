using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class Size2FTests : UltravioletTestFramework
    {
        [TestMethod]
        public void Size2F_IsConstructedProperly()
        {
            var result = new Size2F(123.45f, 456.78f);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [TestMethod]
        public void Size2F_OpEquality()
        {
            var size1 = new Size2F(123.45f, 456.78f);
            var size2 = new Size2F(123.45f, 456.78f);
            var size3 = new Size2F(123.45f, 555.55f);
            var size4 = new Size2F(222.22f, 456.78f);

            TheResultingValue(size1 == size2).ShouldBe(true);
            TheResultingValue(size1 == size3).ShouldBe(false);
            TheResultingValue(size1 == size4).ShouldBe(false);
        }

        [TestMethod]
        public void Size2F_OpInequality()
        {
            var size1 = new Size2F(123.45f, 456.78f);
            var size2 = new Size2F(123.45f, 456.78f);
            var size3 = new Size2F(123.45f, 555.55f);
            var size4 = new Size2F(222.22f, 456.78f);

            TheResultingValue(size1 != size2).ShouldBe(false);
            TheResultingValue(size1 != size3).ShouldBe(true);
            TheResultingValue(size1 != size4).ShouldBe(true);
        }

        [TestMethod]
        public void Size2F_EqualsObject()
        {
            var size1 = new Size2F(123.45f, 456.78f);
            var size2 = new Size2F(123.45f, 456.78f);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Size2F_EqualsSize2F()
        {
            var size1 = new Size2F(123.45f, 456.78f);
            var size2 = new Size2F(123.45f, 456.78f);
            var size3 = new Size2F(123.45f, 555.55f);
            var size4 = new Size2F(222.22f, 456.78f);

            TheResultingValue(size1.Equals(size2)).ShouldBe(true);
            TheResultingValue(size1.Equals(size3)).ShouldBe(false);
            TheResultingValue(size1.Equals(size4)).ShouldBe(false);
        }

        [TestMethod]
        public void Size2F_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = default(Size2F);
            if (!Size2F.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Size2F.");

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [TestMethod]
        public void Size2F_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Size2F);
            var succeeded = Size2F.TryParse("foo", out result);

            TheResultingValue(succeeded)
                .ShouldBe(false);
        }

        [TestMethod]
        public void Size2F_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = Size2F.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Size2F_Parse_FailsForInvalidStrings()
        {
            Size2F.Parse("foo");
        }

        [TestMethod]
        public void Size2F_Parse_CanRoundTrip()
        {
            var size1 = Size2F.Parse("123.4 456.7");
            var size2 = Size2F.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }

        [TestMethod]
        public void Size2F_Area_IsCalculatedCorrectly()
        {
            var size1 = new Size2F(123.45f, 456.78f);
            TheResultingValue(size1.Area).ShouldBe(123.45f * 456.78f);

            var size2 = new Size2F(222.22f, 55555.55f);
            TheResultingValue(size2.Area).ShouldBe(222.22f * 55555.55f);
        }
    }
}

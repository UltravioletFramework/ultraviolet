using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class Size2DTests : UltravioletTestFramework
    {
        [TestMethod]
        public void Size2D_IsConstructedProperly()
        {
            var result = new Size2D(123.45, 456.78);

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78);
        }

        [TestMethod]
        public void Size2D_OpEquality()
        {
            var size1 = new Size2D(123.45, 456.78);
            var size2 = new Size2D(123.45, 456.78);
            var size3 = new Size2D(123.45, 555.55);
            var size4 = new Size2D(222.22, 456.78);

            TheResultingValue(size1 == size2).ShouldBe(true);
            TheResultingValue(size1 == size3).ShouldBe(false);
            TheResultingValue(size1 == size4).ShouldBe(false);
        }

        [TestMethod]
        public void Size2D_OpInequality()
        {
            var size1 = new Size2D(123.45, 456.78);
            var size2 = new Size2D(123.45, 456.78);
            var size3 = new Size2D(123.45, 555.55);
            var size4 = new Size2D(222.22, 456.78);

            TheResultingValue(size1 != size2).ShouldBe(false);
            TheResultingValue(size1 != size3).ShouldBe(true);
            TheResultingValue(size1 != size4).ShouldBe(true);
        }

        [TestMethod]
        public void Size2D_EqualsObject()
        {
            var size1 = new Size2D(123.45, 456.78);
            var size2 = new Size2D(123.45, 456.78);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Size2D_EqualsSize2D()
        {
            var size1 = new Size2D(123.45, 456.78);
            var size2 = new Size2D(123.45, 456.78);
            var size3 = new Size2D(123.45, 555.55);
            var size4 = new Size2D(222.22, 456.78);

            TheResultingValue(size1.Equals(size2)).ShouldBe(true);
            TheResultingValue(size1.Equals(size3)).ShouldBe(false);
            TheResultingValue(size1.Equals(size4)).ShouldBe(false);
        }

        [TestMethod]
        public void Size2D_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = default(Size2D);
            if (!Size2D.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Size2D.");

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78);
        }

        [TestMethod]
        public void Size2D_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Size2D);
            var succeeded = Size2D.TryParse("foo", out result);

            TheResultingValue(succeeded)
                .ShouldBe(false);
        }

        [TestMethod]
        public void Size2D_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = Size2D.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123.45, 456.78);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Size2D_Parse_FailsForInvalidStrings()
        {
            Size2D.Parse("foo");
        }

        [TestMethod]
        public void Size2D_Parse_CanRoundTrip()
        {
            var size1 = Size2D.Parse("123.4 456.7");
            var size2 = Size2D.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }

        [TestMethod]
        public void Size2D_Area_IsCalculatedCorrectly()
        {
            var size1 = new Size2D(123.45, 456.78);
            TheResultingValue(size1.Area).ShouldBe(123.45 * 456.78);

            var size2 = new Size2D(222.22, 55555.55);
            TheResultingValue(size2.Area).ShouldBe(222.22 * 55555.55);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public class Size2Tests : UltravioletTestFramework
    {
        [TestMethod]
        public void Size2_IsConstructedProperly()
        {
            var result = new Size2(123, 456);

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [TestMethod]
        public void Size2_OpEquality()
        {
            var size1 = new Size2(123, 456);
            var size2 = new Size2(123, 456);
            var size3 = new Size2(123, 555);
            var size4 = new Size2(222, 456);

            TheResultingValue(size1 == size2).ShouldBe(true);
            TheResultingValue(size1 == size3).ShouldBe(false);
            TheResultingValue(size1 == size4).ShouldBe(false);
        }

        [TestMethod]
        public void Size2_OpInequality()
        {
            var size1 = new Size2(123, 456);
            var size2 = new Size2(123, 456);
            var size3 = new Size2(123, 555);
            var size4 = new Size2(222, 456);

            TheResultingValue(size1 != size2).ShouldBe(false);
            TheResultingValue(size1 != size3).ShouldBe(true);
            TheResultingValue(size1 != size4).ShouldBe(true);
        }

        [TestMethod]
        public void Size2_EqualsObject()
        {
            var size1 = new Size2(123, 456);
            var size2 = new Size2(123, 456);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [TestMethod]
        public void Size2_EqualsSize2()
        {
            var size1 = new Size2(123, 456);
            var size2 = new Size2(123, 456);
            var size3 = new Size2(123, 555);
            var size4 = new Size2(222, 456);

            TheResultingValue(size1.Equals(size2)).ShouldBe(true);
            TheResultingValue(size1.Equals(size3)).ShouldBe(false);
            TheResultingValue(size1.Equals(size4)).ShouldBe(false);
        }

        [TestMethod]
        public void Size2_TryParse_SucceedsForValidStrings()
        {
            var str    = "123 456";
            var result = default(Size2);
            if (!Size2.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Size2.");

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [TestMethod]
        public void Size2_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Size2);
            var succeeded = Size2.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [TestMethod]
        public void Size2_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456";
            var result = Size2.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Size2_Parse_FailsForInvalidStrings()
        {
            Size2.Parse("foo");
        }

        [TestMethod]
        public void Size2_Parse_CanRoundTrip()
        {
            var size1 = Size2.Parse("123 456");
            var size2 = Size2.Parse(size1.ToString());

            TheResultingValue(size1 == size2).ShouldBe(true);
        }

        [TestMethod]
        public void Size2_Area_IsCalculatedCorrectly()
        {
            var size1 = new Size2(123, 456);
            TheResultingValue(size1.Area).ShouldBe(123 * 456);

            var size2 = new Size2(222, 55555);
            TheResultingValue(size2.Area).ShouldBe(222 * 55555);
        }
    }
}

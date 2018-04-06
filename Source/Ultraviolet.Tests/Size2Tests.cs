using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class Size2Tests : UltravioletTestFramework
    {
        [Test]
        public void Size2_IsConstructedProperly()
        {
            var result = new Size2(123, 456);

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Size2_EqualsObject()
        {
            var size1 = new Size2(123, 456);
            var size2 = new Size2(123, 456);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
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

        [Test]
        public void Size2_TryParse_SucceedsForValidStrings()
        {
            var str    = "123 456";
            var result = default(Size2);
            if (!Size2.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Size2.");

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [Test]
        public void Size2_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Size2);
            var succeeded = Size2.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Size2_Parse_SucceedsForValidStrings()
        {
            var str    = "123 456";
            var result = Size2.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123, 456);
        }

        [Test]
        public void Size2_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Size2.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }
        
        [Test]
        public void Size2_Area_IsCalculatedCorrectly()
        {
            var size1 = new Size2(123, 456);
            TheResultingValue(size1.Area).ShouldBe(123 * 456);

            var size2 = new Size2(222, 55555);
            TheResultingValue(size2.Area).ShouldBe(222 * 55555);
        }

        [Test]
        public void Size2_SerializesToJson()
        {
            var size = new Size2(1, 2);
            var json = JsonConvert.SerializeObject(size,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""width"":1,""height"":2}");
        }

        [Test]
        public void Size2_SerializesToJson_WhenNullable()
        {
            var size = new Size2(1, 2);
            var json = JsonConvert.SerializeObject((Size2?)size,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""width"":1,""height"":2}");
        }

        [Test]
        public void Size2_DeserializesFromJson()
        {
            const String json = @"{""width"":1,""height"":2}";
            
            var size = JsonConvert.DeserializeObject<Size2>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size)
                .ShouldBe(1, 2);
        }

        [Test]
        public void Size2_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""width"":1,""height"":2}";

            var size1 = JsonConvert.DeserializeObject<Size2?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size1.Value)
                .ShouldBe(1, 2);

            const String json2 = @"null";

            var size2 = JsonConvert.DeserializeObject<Size2?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size2.HasValue)
                .ShouldBe(false);
        }
    }
}

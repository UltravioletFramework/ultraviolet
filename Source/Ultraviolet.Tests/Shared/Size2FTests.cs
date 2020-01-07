using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class Size2FTests : UltravioletTestFramework
    {
        [Test]
        public void Size2F_IsConstructedProperly()
        {
            var result = new Size2F(123.45f, 456.78f);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Size2F_EqualsObject()
        {
            var size1 = new Size2F(123.45f, 456.78f);
            var size2 = new Size2F(123.45f, 456.78f);

            TheResultingValue(size1.Equals((Object)size2)).ShouldBe(true);
            TheResultingValue(size1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
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

        [Test]
        public void Size2F_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = default(Size2F);
            if (!Size2F.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Size2F.");

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [Test]
        public void Size2F_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Size2F);
            var succeeded = Size2F.TryParse("foo", out result);

            TheResultingValue(succeeded)
                .ShouldBe(false);
        }

        [Test]
        public void Size2F_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78";
            var result = Size2F.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f);
        }

        [Test]
        public void Size2F_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Size2F.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }
        
        [Test]
        public void Size2F_Area_IsCalculatedCorrectly()
        {
            var size1 = new Size2F(123.45f, 456.78f);
            TheResultingValue(size1.Area).ShouldBe(123.45f * 456.78f);

            var size2 = new Size2F(222.22f, 55555.55f);
            TheResultingValue(size2.Area).ShouldBe(222.22f * 55555.55f);
        }

        [Test]
        public void Size2F_SerializesToJson()
        {
            var size = new Size2F(1.2f, 2.3f);
            var json = JsonConvert.SerializeObject(size,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""width"":1.2,""height"":2.3}");
        }

        [Test]
        public void Size2F_SerializesToJson_WhenNullable()
        {
            var size = new Size2F(1.2f, 2.3f);
            var json = JsonConvert.SerializeObject((Size2F?)size,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""width"":1.2,""height"":2.3}");
        }

        [Test]
        public void Size2F_DeserializesFromJson()
        {
            const String json = @"{""width"":1.2,""height"":2.3}";
            
            var size = JsonConvert.DeserializeObject<Size2F>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size)
                .ShouldBe(1.2f, 2.3f);
        }

        [Test]
        public void Size2F_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""width"":1.2,""height"":2.3}";

            var size1 = JsonConvert.DeserializeObject<Size2F?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size1.Value)
                .ShouldBe(1.2f, 2.3f);

            const String json2 = @"null";

            var size2 = JsonConvert.DeserializeObject<Size2F?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size2.HasValue)
                .ShouldBe(false);
        }
    }
}

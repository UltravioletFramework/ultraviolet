using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
{
    [TestFixture]
    public class Size3FTests : UltravioletTestFramework
    {
        [Test]
        public void Size3F_IsConstructedProperly()
        {
            var result = new Size3F(123.45f, 456.78f, 789.99f);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f, 789.99f);
        }

        [Test]
        public void Size3F_OpEquality()
        {
            var volume1 = new Size3F(123.45f, 456.78f, 789.99f);
            var volume2 = new Size3F(123.45f, 456.78f, 789.99f);
            var volume3 = new Size3F(123.45f, 555, 789.99f);
            var volume4 = new Size3F(222, 456.78f, 789.99f);
            var volume5 = new Size3F(123.45f, 456.78f, 999);

            TheResultingValue(volume1 == volume2).ShouldBe(true);
            TheResultingValue(volume1 == volume3).ShouldBe(false);
            TheResultingValue(volume1 == volume4).ShouldBe(false);
            TheResultingValue(volume1 == volume5).ShouldBe(false);
        }

        [Test]
        public void Size3F_OpInequality()
        {
            var volume1 = new Size3F(123.45f, 456.78f, 789.99f);
            var volume2 = new Size3F(123.45f, 456.78f, 789.99f);
            var volume3 = new Size3F(123.45f, 555, 789.99f);
            var volume4 = new Size3F(222, 456.78f, 789.99f);
            var volume5 = new Size3F(123.45f, 456.78f, 999);

            TheResultingValue(volume1 != volume2).ShouldBe(false);
            TheResultingValue(volume1 != volume3).ShouldBe(true);
            TheResultingValue(volume1 != volume4).ShouldBe(true);
            TheResultingValue(volume1 != volume5).ShouldBe(true);
        }

        [Test]
        public void Size3F_EqualsObject()
        {
            var volume1 = new Size3F(123.45f, 456.78f, 789.99f);
            var volume2 = new Size3F(123.45f, 456.78f, 789.99f);

            TheResultingValue(volume1.Equals((Object)volume2)).ShouldBe(true);
            TheResultingValue(volume1.Equals("This is a test")).ShouldBe(false);
        }

        [Test]
        public void Size3F_EqualsSize3F()
        {
            var volume1 = new Size3F(123.45f, 456.78f, 789.99f);
            var volume2 = new Size3F(123.45f, 456.78f, 789.99f);
            var volume3 = new Size3F(123.45f, 555, 789.99f);
            var volume4 = new Size3F(222, 456.78f, 789.99f);
            var volume5 = new Size3F(123.45f, 456.78f, 999);

            TheResultingValue(volume1.Equals(volume2)).ShouldBe(true);
            TheResultingValue(volume1.Equals(volume3)).ShouldBe(false);
            TheResultingValue(volume1.Equals(volume4)).ShouldBe(false);
            TheResultingValue(volume1.Equals(volume5)).ShouldBe(false);
        }

        [Test]
        public void Size3F_TryParse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99";
            var result = default(Size3F);
            if (!Size3F.TryParse(str, out result))
                throw new InvalidOperationException("Unable to parse string to Size3F.");

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f, 789.99f);
        }

        [Test]
        public void Size3F_TryParse_FailsForInvalidStrings()
        {
            var result    = default(Size3F);
            var succeeded = Size3F.TryParse("foo", out result);

            TheResultingValue(succeeded).ShouldBe(false);
        }

        [Test]
        public void Size3F_Parse_SucceedsForValidStrings()
        {
            var str    = "123.45 456.78 789.99";
            var result = Size3F.Parse(str);

            TheResultingValue(result)
                .ShouldBe(123.45f, 456.78f, 789.99f);
        }

        [Test]
        public void Size3F_Parse_FailsForInvalidStrings()
        {
            Assert.That(() => Size3F.Parse("foo"),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Size3F_Volume_IsCalculatedCorrectly()
        {
            var volume1width  = 123.45f;
            var volume1height = 456.78f;
            var volume1depth  = 789.99f;
            var volume1 = new Size3F(volume1width, volume1height, volume1depth);
            TheResultingValue(volume1.Volume).ShouldBe(volume1width * volume1height * volume1depth);

            var volume2width  = 222.22f;
            var volume2height = 555.55f;
            var volume2depth  = 999.99f;
            var volume2       = new Size3F(volume2width, volume2height, volume2depth);
            TheResultingValue(volume2.Volume).ShouldBe(volume2width * volume2height * volume2depth);
        }

        [Test]
        public void Size3F_SerializesToJson()
        {
            var size = new Size3F(1.2f, 2.3f, 3.4f);
            var json = JsonConvert.SerializeObject(size,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""width"":1.2,""height"":2.3,""depth"":3.4}");
        }

        [Test]
        public void Size3F_SerializesToJson_WhenNullable()
        {
            var size = new Size3F(1.2f, 2.3f, 3.4f);
            var json = JsonConvert.SerializeObject((Size3F?)size,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"{""width"":1.2,""height"":2.3,""depth"":3.4}");
        }

        [Test]
        public void Size3F_DeserializesFromJson()
        {
            const String json = @"{""width"":1.2,""height"":2.3,""depth"":3.4}";
            
            var size = JsonConvert.DeserializeObject<Size3F>(json,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size)
                .ShouldBe(1.2f, 2.3f, 3.4f);
        }

        [Test]
        public void Size3F_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"{""width"":1.2,""height"":2.3,""depth"":3.4}";

            var size1 = JsonConvert.DeserializeObject<Size3F?>(json1,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size1.Value)
                .ShouldBe(1.2f, 2.3f, 3.4f);

            const String json2 = @"null";

            var size2 = JsonConvert.DeserializeObject<Size3F?>(json2,
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(size2.HasValue)
                .ShouldBe(false);
        }
    }
}

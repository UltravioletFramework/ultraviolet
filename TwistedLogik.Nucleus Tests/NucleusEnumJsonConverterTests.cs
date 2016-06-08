using System;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests
{
    [TestFixture]
    public sealed class NucleusEnumJsonConverterTests : NucleusTestFramework
    {
        [Flags]
        [JsonConverter(typeof(NucleusEnumJsonConverter))]
        private enum FlagsEnum
        {
            Foo = 1,
            Bar = 2,
            Baz = 4,
            FooAndBaz = Foo | Baz,
        }

        [Test]
        public void NucleusEnumJsonConverter_Deserializes_ArrayOfFlags()
        {
            const String json = @"[ ""Foo"", ""Bar"" ]";

            var value = JsonConvert.DeserializeObject<FlagsEnum>(json);

            TheResultingValue(value)
                .ShouldBe(FlagsEnum.Foo | FlagsEnum.Bar);
        }

        [Test]
        public void NucleusEnumJsonConverter_Deserializes_ArrayOfFlags_WhenNullable()
        {
            const String json1 = @"[ ""Foo"", ""Bar"" ]";

            var value1 = JsonConvert.DeserializeObject<FlagsEnum?>(json1);

            TheResultingValue(value1.Value)
                .ShouldBe(FlagsEnum.Foo | FlagsEnum.Bar);

            const String json2 = @"null";

            var value2 = JsonConvert.DeserializeObject<FlagsEnum?>(json2);

            TheResultingValue(value2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void NucleusEnumJsonConverter_Deserializes_SingleValue()
        {
            const String json = @"""Baz""";

            var value = JsonConvert.DeserializeObject<FlagsEnum>(json);

            TheResultingValue(value)
                .ShouldBe(FlagsEnum.Baz);
        }

        [Test]
        public void NucleusEnumJsonConverter_Deserializes_SingleValue_WhenNullable()
        {
            const String json1 = @"""Baz""";

            var value1 = JsonConvert.DeserializeObject<FlagsEnum?>(json1);

            TheResultingValue(value1.Value)
                .ShouldBe(FlagsEnum.Baz);

            const String json2 = @"null";

            var value2 = JsonConvert.DeserializeObject<FlagsEnum?>(json2);

            TheResultingValue(value2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void NucleusEnumJsonConverter_Serializes_ArrayOfFlags()
        {
            const FlagsEnum value = FlagsEnum.Foo | FlagsEnum.Bar;

            var json = JsonConvert.SerializeObject(value);

            TheResultingString(json)
                .ShouldBe(@"[""Bar"",""Foo""]");
        }

        [Test]
        public void NucleusEnumJsonConverter_Serializes_ArrayOfFlags_WhenNullable()
        {
            const FlagsEnum value = FlagsEnum.Foo | FlagsEnum.Bar;

            var json = JsonConvert.SerializeObject((FlagsEnum?)value);

            TheResultingString(json)
                .ShouldBe(@"[""Bar"",""Foo""]");
        }

        [Test]
        public void NucleusEnumJsonConverter_Serializes_ArrayOfFlags_AsLargestCombinedValue()
        {
            const FlagsEnum value = FlagsEnum.Foo | FlagsEnum.Bar | FlagsEnum.Baz;

            var json = JsonConvert.SerializeObject(value);

            TheResultingString(json)
                .ShouldBe(@"[""FooAndBaz"",""Bar""]");
        }

        [Test]
        public void NucleusEnumJsonConverter_Serializes_ArrayOfFlags_AsLargestCombinedValue_WhenNullable()
        {
            const FlagsEnum value = FlagsEnum.Foo | FlagsEnum.Bar | FlagsEnum.Baz;

            var json = JsonConvert.SerializeObject((FlagsEnum?)value);

            TheResultingString(json)
                .ShouldBe(@"[""FooAndBaz"",""Bar""]");
        }

        [Test]
        public void NucleusEnumJsonConverter_Serializes_SingleValue()
        {
            const FlagsEnum value = FlagsEnum.Foo;

            var json = JsonConvert.SerializeObject(value);

            TheResultingString(json)
                .ShouldBe(@"""Foo""");
        }

        [Test]
        public void NucleusEnumJsonConverter_Serializes_SingleValue_WhenNullable()
        {
            const FlagsEnum value = FlagsEnum.Foo;

            var json = JsonConvert.SerializeObject((FlagsEnum?)value);

            TheResultingString(json)
                .ShouldBe(@"""Foo""");
        }
    }
}

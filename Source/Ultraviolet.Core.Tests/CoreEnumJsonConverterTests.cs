using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests
{
    [TestFixture]
    public sealed class CoreEnumJsonConverterTests : CoreTestFramework
    {
        [Flags]
        [JsonConverter(typeof(CoreEnumJsonConverter))]
        private enum FlagsEnum
        {
            Foo = 1,
            Bar = 2,
            Baz = 4,
            FooAndBaz = Foo | Baz,
        }

        [Test]
        public void CoreEnumJsonConverter_Deserializes_ArrayOfFlags()
        {
            const String json = @"[ ""Foo"", ""Bar"" ]";

            var value = JsonConvert.DeserializeObject<FlagsEnum>(json, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value)
                .ShouldBe(FlagsEnum.Foo | FlagsEnum.Bar);
        }

        [Test]
        public void CoreEnumJsonConverter_Deserializes_ArrayOfFlags_WhenNullable()
        {
            const String json1 = @"[ ""Foo"", ""Bar"" ]";

            var value1 = JsonConvert.DeserializeObject<FlagsEnum?>(json1, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value1.Value)
                .ShouldBe(FlagsEnum.Foo | FlagsEnum.Bar);

            const String json2 = @"null";

            var value2 = JsonConvert.DeserializeObject<FlagsEnum?>(json2, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void CoreEnumJsonConverter_Deserializes_SingleValue()
        {
            const String json = @"""Baz""";

            var value = JsonConvert.DeserializeObject<FlagsEnum>(json, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value)
                .ShouldBe(FlagsEnum.Baz);
        }

        [Test]
        public void CoreEnumJsonConverter_Deserializes_SingleValue_WhenNullable()
        {
            const String json1 = @"""Baz""";

            var value1 = JsonConvert.DeserializeObject<FlagsEnum?>(json1, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value1.Value)
                .ShouldBe(FlagsEnum.Baz);

            const String json2 = @"null";

            var value2 = JsonConvert.DeserializeObject<FlagsEnum?>(json2, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void CoreEnumJsonConverter_Serializes_ArrayOfFlags()
        {
            const FlagsEnum value = FlagsEnum.Foo | FlagsEnum.Bar;

            var json = JsonConvert.SerializeObject(value,
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"[""Bar"",""Foo""]");
        }

        [Test]
        public void CoreEnumJsonConverter_Serializes_ArrayOfFlags_WhenNullable()
        {
            const FlagsEnum value = FlagsEnum.Foo | FlagsEnum.Bar;

            var json = JsonConvert.SerializeObject((FlagsEnum?)value, 
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"[""Bar"",""Foo""]");
        }

        [Test]
        public void CoreEnumJsonConverter_Serializes_ArrayOfFlags_AsLargestCombinedValue()
        {
            const FlagsEnum value = FlagsEnum.Foo | FlagsEnum.Bar | FlagsEnum.Baz;

            var json = JsonConvert.SerializeObject(value, 
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"[""FooAndBaz"",""Bar""]");
        }

        [Test]
        public void CoreEnumJsonConverter_Serializes_ArrayOfFlags_AsLargestCombinedValue_WhenNullable()
        {
            const FlagsEnum value = FlagsEnum.Foo | FlagsEnum.Bar | FlagsEnum.Baz;

            var json = JsonConvert.SerializeObject((FlagsEnum?)value, 
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"[""FooAndBaz"",""Bar""]");
        }

        [Test]
        public void CoreEnumJsonConverter_Serializes_SingleValue()
        {
            const FlagsEnum value = FlagsEnum.Foo;

            var json = JsonConvert.SerializeObject(value, 
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"""Foo""");
        }

        [Test]
        public void CoreEnumJsonConverter_Serializes_SingleValue_WhenNullable()
        {
            const FlagsEnum value = FlagsEnum.Foo;

            var json = JsonConvert.SerializeObject((FlagsEnum?)value, 
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"""Foo""");
        }
    }
}

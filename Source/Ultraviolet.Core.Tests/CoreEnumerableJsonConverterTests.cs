using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests
{
    [TestFixture]
    public sealed class CoreEnumerableJsonConverterTests : CoreTestFramework
    {
        private class Foo
        {
            [JsonConverter(typeof(CoreEnumerableJsonConverter<String>))]
            public IEnumerable<String> Values { get; set; }
        }

        [Test]
        public void CoreEnumerableJsonConverter_Deserializes_ArrayOfValues()
        {
            const String json = @"{ ""values"": [ ""hello"", ""world"" ] }";

            var value = JsonConvert.DeserializeObject<Foo>(json, 
                CoreJsonSerializerSettings.Instance);

            TheResultingCollection(value.Values)
                .ShouldBe("hello", "world");
        }

        [Test]
        public void CoreEnumerableJsonConverter_Deserializes_SingleValue()
        {
            const String json = @"{ ""values"": ""hello"" }";

            var value = JsonConvert.DeserializeObject<Foo>(json, 
                CoreJsonSerializerSettings.Instance);

            TheResultingCollection(value.Values)
                .ShouldBe("hello");
        }

        [Test]
        public void CoreEnumerableJsonConverter_Serializes_ArrayOfValues()
        {
            var value = new Foo() { Values = new[] { "hello", "world" } };

            var json = JsonConvert.SerializeObject(value,
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"{""values"":[""hello"",""world""]}");
        }

        [Test]
        public void CoreEnumerableJsonConverter_Serializes_SingleValue()
        {
            var value = new Foo() { Values = new[] { "hello" } };

            var json = JsonConvert.SerializeObject(value,
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"{""values"":""hello""}");
        }
    }
}

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests
{
    [TestFixture]
    public sealed class NucleusEnumerableJsonConverterTests : NucleusTestFramework
    {
        private class Foo
        {
            [JsonProperty(PropertyName = "values")]
            [JsonConverter(typeof(NucleusEnumerableJsonConverter<String>))]
            public IEnumerable<String> Values { get; set; }
        }

        [Test]
        public void NucleusEnumerableJsonConverter_Deserializes_ArrayOfValues()
        {
            const String json = @"{ ""values"": [ ""hello"", ""world"" ] }";

            var value = JsonConvert.DeserializeObject<Foo>(json);

            TheResultingCollection(value.Values)
                .ShouldBe("hello", "world");
        }

        [Test]
        public void NucleusEnumerableJsonConverter_Deserializes_SingleValue()
        {
            const String json = @"{ ""values"": ""hello"" }";

            var value = JsonConvert.DeserializeObject<Foo>(json);

            TheResultingCollection(value.Values)
                .ShouldBe("hello");
        }

        [Test]
        public void NucleusEnumerableJsonConverter_Serializes_ArrayOfValues()
        {
            var value = new Foo() { Values = new[] { "hello", "world" } };

            var json = JsonConvert.SerializeObject(value);

            TheResultingString(json)
                .ShouldBe(@"{""values"":[""hello"",""world""]}");
        }

        [Test]
        public void NucleusEnumerableJsonConverter_Serializes_SingleValue()
        {
            var value = new Foo() { Values = new[] { "hello" } };

            var json = JsonConvert.SerializeObject(value);

            TheResultingString(json)
                .ShouldBe(@"{""values"":""hello""}");
        }
    }
}

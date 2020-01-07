using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests
{
    [TestFixture]
    public class MaskedUInt32Test : CoreTestFramework
    {
        [Test]
        public void MaskedUInt32_CastsFromUInt32Correctly()
        {
            var val1 = (MaskedUInt32)12345;
            TheResultingValue(val1.Value).ShouldBe(12345u);
            
            var val2 = (MaskedUInt32)67890;
            TheResultingValue(val2.Value).ShouldBe(67890u);
            
            var val3 = (MaskedUInt32)UInt32.MinValue;
            TheResultingValue(val3.Value).ShouldBe(UInt32.MinValue);

            var val4 = (MaskedUInt32)UInt32.MaxValue;
            TheResultingValue(val4.Value).ShouldBe(UInt32.MaxValue);
        }

        [Test]
        public void MaskedUInt32_CalculatesMasksCorrectly()
        {
            var val1 = (MaskedUInt32)12345;
            TheResultingValue(val1.GetMask()).ShouldBe(0x03);

            var val2 = (MaskedUInt32)67890;
            TheResultingValue(val2.GetMask()).ShouldBe(0x07);

            var val3 = (MaskedUInt32)UInt32.MinValue;
            TheResultingValue(val3.GetMask()).ShouldBe(0x00);
            
            var val4 = (MaskedUInt32)UInt32.MaxValue;
            TheResultingValue(val4.GetMask()).ShouldBe(0x0F);
        }

        [Test]
        public void MaskedUInt32_SerializesToJson()
        {
            var value = (MaskedUInt32)987654;
            
            var json = JsonConvert.SerializeObject(value, 
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"987654");
        }

        [Test]
        public void MaskedUInt32_SerializesToJson_WhenNullable()
        {
            var value = (MaskedUInt32?)987654;

            var json = JsonConvert.SerializeObject(value, 
                CoreJsonSerializerSettings.Instance);

            TheResultingString(json)
                .ShouldBe(@"987654");
        }

        [Test]
        public void MaskedUInt32_DeserializesFromJson()
        {
            const String json = @"123456";
            
            var value = JsonConvert.DeserializeObject<MaskedUInt32>(json, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value.Value)
                .ShouldBe(123456);
        }

        [Test]
        public void MaskedUInt32_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"123456";

            var value1 = JsonConvert.DeserializeObject<MaskedUInt32?>(json1, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value1.Value.Value)
                .ShouldBe(123456);

            const String json2 = @"null";

            var value2 = JsonConvert.DeserializeObject<MaskedUInt32?>(json2, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value2.HasValue)
                .ShouldBe(false);
        }

        [Test]
        public void MaskedUInt32_DeserializesFromJson_String()
        {
            const String json = @"""123456""";

            var value = JsonConvert.DeserializeObject<MaskedUInt32>(json,
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value.Value)
                .ShouldBe(123456);
        }

        [Test]
        public void MaskedUInt32_DeserializesFromJson_String_WhenNullable()
        {
            const String json1 = @"""123456""";

            var value1 = JsonConvert.DeserializeObject<MaskedUInt32?>(json1, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value1.Value.Value)
                .ShouldBe(123456);

            const String json2 = @"null";

            var value2 = JsonConvert.DeserializeObject<MaskedUInt32?>(json2, 
                CoreJsonSerializerSettings.Instance);

            TheResultingValue(value2.HasValue)
                .ShouldBe(false);
        }
    }
}

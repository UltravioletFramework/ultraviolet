using System;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests
{
    [TestFixture]
    public class MaskedUInt32Test : NucleusTestFramework
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

            var converter = new NucleusJsonConverter();
            var json = JsonConvert.SerializeObject(value, converter);

            TheResultingString(json).ShouldBe(@"987654");
        }
        
        [Test]
        public void MaskedUInt32_DeserializesFromJson()
        {
            const String json = @"123456";

            var converter = new NucleusJsonConverter();
            var value = JsonConvert.DeserializeObject<MaskedUInt32>(json, converter);

            TheResultingValue(value.Value).ShouldBe(123456);
        }

        [Test]
        public void MaskedUInt32_DeserializesFromJson_String()
        {
            const String json = @"""123456""";

            var converter = new NucleusJsonConverter();
            var value = JsonConvert.DeserializeObject<MaskedUInt32>(json, converter);

            TheResultingValue(value.Value).ShouldBe(123456);
        }
    }
}

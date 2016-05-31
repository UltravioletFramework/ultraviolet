using System;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Nucleus.Testing;

namespace TwistedLogik.Nucleus.Tests
{
    [TestFixture]
    public class MaskedUInt64Test : NucleusTestFramework
    {
        [Test]
        public void MaskedUInt64_CastsFromUInt64Correctly()
        {
            var val1 = (MaskedUInt64)12345;
            TheResultingValue(val1.Value).ShouldBe(12345ul);

            var val2 = (MaskedUInt64)67890;
            TheResultingValue(val2.Value).ShouldBe(67890ul);
            
            var val3 = (MaskedUInt64)UInt64.MinValue;
            TheResultingValue(val3.Value).ShouldBe(UInt64.MinValue);
            
            var val4 = (MaskedUInt64)UInt64.MaxValue;
            TheResultingValue(val4.Value).ShouldBe(UInt64.MaxValue);
        }

        [Test]
        public void MaskedUInt64_CalculatesMasksCorrectly()
        {
            var val1 = (MaskedUInt64)12345;
            TheResultingValue(val1.GetMask()).ShouldBe(0x03);

            var val2 = (MaskedUInt64)67890;
            TheResultingValue(val2.GetMask()).ShouldBe(0x07);
            
            var val3 = (MaskedUInt64)UInt64.MinValue;
            TheResultingValue(val3.GetMask()).ShouldBe(0x00);

            var val4 = (MaskedUInt64)UInt64.MaxValue;
            TheResultingValue(val4.GetMask()).ShouldBe(0xFF);
        }

        [Test]
        public void MaskedUInt64_SerializesToJson()
        {
            var value = (MaskedUInt64)987654;
            
            var json = JsonConvert.SerializeObject(value);

            TheResultingString(json).ShouldBe(@"987654");
        }

        [Test]
        public void MaskedUInt64_DeserializesFromJson()
        {
            const String json = @"123456";
            
            var value = JsonConvert.DeserializeObject<MaskedUInt64>(json);

            TheResultingValue(value.Value).ShouldBe(123456);
        }

        [Test]
        public void MaskedUInt64_DeserializesFromJson_String()
        {
            const String json = @"""18446744073709551615""";
            
            var value = JsonConvert.DeserializeObject<MaskedUInt64>(json);

            TheResultingValue(value.Value).ShouldBe(18446744073709551615u);
        }
    }
}

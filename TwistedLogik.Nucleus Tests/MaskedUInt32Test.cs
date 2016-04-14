using NUnit.Framework;
using System;
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
    }
}

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using TwistedLogik.Ultraviolet.Testing;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestFixture]
    public class RadiansTest : UltravioletTestFramework
    {
        [Test]
        public void Radians_Parse_RawValue()
        {
            var result = Radians.Parse("3.14159");

            TheResultingValue(result).ShouldBe(3.14159f);
        }

        [Test]
        public void Radians_Parse_FromPi()
        {
            var value1 = Radians.Parse("1 pi");
            TheResultingValue(value1).ShouldBe((float)Math.PI);
            
            var value1sym = Radians.Parse("1π");
            TheResultingValue(value1sym).ShouldBe((float)Math.PI);

            var value2 = Radians.Parse("2 pi");
            TheResultingValue(value2).ShouldBe((float)(2.0 * Math.PI));

            var value2sym = Radians.Parse("2π");
            TheResultingValue(value2sym).ShouldBe((float)(2.0 * Math.PI));

            var value3 = Radians.Parse("1/2 pi");
            TheResultingValue(value3).ShouldBe((float)(0.5 * Math.PI));

            var value3sym = Radians.Parse("1/2π");
            TheResultingValue(value3sym).ShouldBe((float)(0.5 * Math.PI));
        }

        [Test]
        public void Radians_Parse_FromPi_Negative()
        {
            var value1 = Radians.Parse("-1 pi");
            TheResultingValue(value1).ShouldBe(-(float)Math.PI);

            var value1sym = Radians.Parse("-1π");
            TheResultingValue(value1sym).ShouldBe(-(float)Math.PI);
            
            var value2 = Radians.Parse("-2 pi");
            TheResultingValue(value2).ShouldBe(-(float)(2.0 * Math.PI));
            
            var value2sym = Radians.Parse("-2π");
            TheResultingValue(value2sym).ShouldBe(-(float)(2.0 * Math.PI));
            
            var value3 = Radians.Parse("-1/2 pi");
            TheResultingValue(value3).ShouldBe(-(float)(0.5 * Math.PI));

            var value3sym = Radians.Parse("-1/2π");
            TheResultingValue(value3sym).ShouldBe(-(float)(0.5 * Math.PI));
        }

        [Test]
        public void Radians_Parse_FromTau()
        {
            var value1 = Radians.Parse("1 tau");
            TheResultingValue(value1).ShouldBe((float)(2.0 * Math.PI));

            var value1sym = Radians.Parse("1τ");
            TheResultingValue(value1sym).ShouldBe((float)(2.0 * Math.PI));

            var value2 = Radians.Parse("2 tau");
            TheResultingValue(value2).ShouldBe((float)(4.0 * Math.PI));

            var value2sym = Radians.Parse("2τ");
            TheResultingValue(value2sym).ShouldBe((float)(4.0 * Math.PI));

            var value3 = Radians.Parse("1/2 tau");
            TheResultingValue(value3).ShouldBe((float)Math.PI);

            var value3sym = Radians.Parse("1/2τ");
            TheResultingValue(value3sym).ShouldBe((float)Math.PI);
        }
        
        [Test]
        public void Radians_Parse_FromTau_Negative()
        {
            var value1 = Radians.Parse("-1 tau");
            TheResultingValue(value1).ShouldBe(-(float)(2.0 * Math.PI));

            var value1sym = Radians.Parse("-1τ");
            TheResultingValue(value1sym).ShouldBe(-(float)(2.0 * Math.PI));

            var value2 = Radians.Parse("-2 tau");
            TheResultingValue(value2).ShouldBe(-(float)(4.0 * Math.PI));
            
            var value2sym = Radians.Parse("-2τ");
            TheResultingValue(value2sym).ShouldBe(-(float)(4.0 * Math.PI));

            var value3 = Radians.Parse("-1/2 tau");
            TheResultingValue(value3).ShouldBe(-(float)Math.PI);

            var value3sym = Radians.Parse("-1/2τ");
            TheResultingValue(value3sym).ShouldBe(-(float)Math.PI);
        }
        
        [Test]
        public void Radians_SerializesToJson()
        {
            var converter = new UltravioletJsonConverter();
            var radians = 1.234f;
            var json = JsonConvert.SerializeObject(radians, converter);

            TheResultingString(json).ShouldBe(@"1.234");
        }

        [Test]
        public void Radians_DeserializesFromJson()
        {
            const String json = @"1.234";

            var converter = new UltravioletJsonConverter();
            var radians = JsonConvert.DeserializeObject<Radians>(json, converter);

            TheResultingValue(radians)
                .ShouldBe(1.234f);
        }
    }
}

using System;
using Newtonsoft.Json;
using NUnit.Framework;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Tests
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
            var radians = 1.234f;
            var json = JsonConvert.SerializeObject(radians);

            TheResultingString(json).ShouldBe(@"1.234");
        }

        [Test]
        public void Radians_SerializesToJson_WhenNullable()
        {
            var radians = 1.234f;
            var json = JsonConvert.SerializeObject((Radians?)radians, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingString(json).ShouldBe(@"1.234");
        }

        [Test]
        public void Radians_DeserializesFromJson()
        {
            const String json = @"1.234";
            
            var radians = JsonConvert.DeserializeObject<Radians>(json, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(radians)
                .ShouldBe(1.234f);
        }

        [Test]
        public void Radians_DeserializesFromJson_WhenNullable()
        {
            const String json1 = @"1.234";

            var radians1 = JsonConvert.DeserializeObject<Radians?>(json1, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(radians1.Value)
                .ShouldBe(1.234f);

            const String json2 = @"null";

            var radians2 = JsonConvert.DeserializeObject<Radians?>(json2, 
                UltravioletJsonSerializerSettings.Instance);

            TheResultingValue(radians2.HasValue)
                .ShouldBe(false);
        }
    }
}

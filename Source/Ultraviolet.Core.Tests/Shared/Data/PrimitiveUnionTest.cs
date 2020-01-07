using System;
using NUnit.Framework;
using Ultraviolet.Core.Data;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests.Data
{
    [TestFixture]
    public class PrimitiveUnionTest : CoreTestFramework
    {
        [Test]
        public void PrimitiveUnion_CorrectlyConvertsByteValues()
        {
            var union1 = new PrimitiveUnion((Byte)123);

            TheResultingValue(union1.AsByte())
                .ShouldBe(123);
        }

        [Test]
        public void PrimitiveUnion_CorrectlyConvertsSByteValues()
        {
            var union1 = new PrimitiveUnion((SByte)123);
            var union2 = new PrimitiveUnion((SByte)(-112));

            TheResultingValue(union1.AsByte())
                .ShouldBe(123);
            TheResultingValue(union2.AsSByte())
                .ShouldBe(-112);
        }

        [Test]
        public void PrimitiveUnion_CorrectlyConvertsCharValues()
        {
            var union1 = new PrimitiveUnion('@');

            TheResultingValue(union1.AsChar())
                .ShouldBe('@');
        }

        [Test]
        public void PrimitiveUnion_CorrectlyConvertsInt16Values()
        {
            var union1 = new PrimitiveUnion((Int16)1234);
            var union2 = new PrimitiveUnion((Int16)(-2345));

            TheResultingValue(union1.AsInt16())
                .ShouldBe(1234);
            TheResultingValue(union2.AsInt16())
                .ShouldBe(-2345);
        }

        [Test]
        public void PrimitiveUnion_CorrectlyConvertsUInt16Values()
        {
            var union1 = new PrimitiveUnion((UInt16)1234);

            TheResultingValue(union1.AsUInt16())
                .ShouldBe(1234);;

        }
        
        [Test]
        public void PrimitiveUnion_CorrectlyConvertsInt32Values()
        {
            var union1 = new PrimitiveUnion(12341234);
            var union2 = new PrimitiveUnion(-23452345);

            TheResultingValue(union1.AsInt32())
                .ShouldBe(12341234);
            TheResultingValue(union2.AsInt32())
                .ShouldBe(-23452345);
        }
        
        [Test]
        public void PrimitiveUnion_CorrectlyConvertsUInt32Values()
        {
            var union1 = new PrimitiveUnion((UInt32)12341234);

            TheResultingValue(union1.AsUInt32())
                .ShouldBe(12341234);
        }
        
        [Test]
        public void PrimitiveUnion_CorrectlyConvertsInt64Values()
        {
            var union1 = new PrimitiveUnion(123412341234);
            var union2 = new PrimitiveUnion(-234523452345);

            TheResultingValue(union1.AsInt64())
                .ShouldBe(123412341234);
            TheResultingValue(union2.AsInt64())
                .ShouldBe(-234523452345);
        }

        [Test]
        public void PrimitiveUnion_CorrectlyConvertsUInt64Values()
        {
            var union1 = new PrimitiveUnion((UInt64)123412341234);

            TheResultingValue(union1.AsUInt64())
                .ShouldBe(123412341234);
        }

        [Test]
        public void PrimitiveUnion_CorrectlyConvertsSingleValues()
        {
            var union1 = new PrimitiveUnion(12345.6789f);

            TheResultingValue(union1.AsSingle())
                .ShouldBe(12345.6789f);
        }

        [Test]
        public void PrimitiveUnion_CorrectlyConvertsDoubleValues()
        {
            var union1 = new PrimitiveUnion(1234567890.123456789);

            TheResultingValue(union1.AsDouble())
                .ShouldBe(1234567890.123456789);
        }
    }
}

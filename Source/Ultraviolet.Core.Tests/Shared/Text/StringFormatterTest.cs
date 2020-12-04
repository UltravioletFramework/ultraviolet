using System;
using System.Text;
using NUnit.Framework;
using Ultraviolet.Core.TestFramework;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Core.Tests.Text
{
    [TestFixture]
    public class StringFormatterTest : CoreTestFramework
    {
        [Test]
        public void StringFormatter_CanFormatStringArgument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument("world");
            formatter.Format("hello {0}", buffer);

            TheResultingString(buffer).ShouldBe("hello world");
        }

        [Test]
        public void StringFormatter_CanFormatBooleanArgument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(true);
            formatter.AddArgument(false);
            formatter.Format("this is {0} and this is {1}", buffer);

            TheResultingString(buffer).ShouldBe("this is True and this is False");
        }

        [Test]
        public void StringFormatter_CanFormatBooleanArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(true);
            formatter.AddArgument(false);
            formatter.Format("this is {0:hex} and this is {1:hex}", buffer);

            TheResultingString(buffer).ShouldBe("this is 01 and this is 00");
        }

        [Test]
        public void StringFormatter_CanFormatByteArgument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument((Byte)123);
            formatter.Format("this is a Byte value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Byte value: 123");
        }

        [Test]
        public void StringFormatter_CanFormatByteArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument((Byte)123);
            formatter.Format("this is a Byte value: 0x{0:hex} / 0x{0:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Byte value: 0x7B / 0x7b");
        }

        [Test]
        public void StringFormatter_CanFormatCharArgument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument('Z');
            formatter.Format("this is a Char value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Char value: Z");
        }

        [Test]
        public void StringFormatter_CanFormatCharArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument('Z');
            formatter.Format("this is a Char value: 0x{0:hex} / 0x{0:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Char value: 0x005A / 0x005a");
        }

        [Test]
        public void StringFormatter_CanFormatInt16Argument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument((Int16)31566);
            formatter.AddArgument((Int16)(-31566));
            formatter.Format("this is an Int16 value: {0} / {1}", buffer);

            TheResultingString(buffer).ShouldBe("this is an Int16 value: 31566 / -31566");
        }

        [Test]
        public void StringFormatter_CanFormatInt16ArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument((Int16)31566);
            formatter.AddArgument((Int16)(-31566));
            formatter.Format("this is an Int16 value: 0x{0:hex} / 0x{0:hex:l} / 0x{1:hex} / 0x{1:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is an Int16 value: 0x7B4E / 0x7b4e / 0x84B2 / 0x84b2");
        }

        [Test]
        public void StringFormatter_CanFormatInt32Argument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(12345);
            formatter.AddArgument(-12345);
            formatter.Format("this is an Int32 value: {0} / {1}", buffer);

            TheResultingString(buffer).ShouldBe("this is an Int32 value: 12345 / -12345");
        }

        [Test]
        public void StringFormatter_CanFormatInt32ArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123456);
            formatter.AddArgument(-123456);
            formatter.Format("this is an Int32 value: 0x{0:hex} / 0x{0:hex:l} / 0x{1:hex} / 0x{1:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is an Int32 value: 0x0001E240 / 0x0001e240 / 0xFFFE1DC0 / 0xfffe1dc0");
        }

        [Test]
        public void StringFormatter_CanFormatInt64Argument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(12345678901234);
            formatter.AddArgument(-12345678901234);
            formatter.Format("this is an Int64 value: {0} / {1}", buffer);

            TheResultingString(buffer).ShouldBe("this is an Int64 value: 12345678901234 / -12345678901234");
        }

        [Test]
        public void StringFormatter_CanFormatInt64ArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(12345678901234);
            formatter.AddArgument(-12345678901234);
            formatter.Format("this is an Int64 value: 0x{0:hex} / 0x{0:hex:l} / 0x{1:hex} / 0x{1:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is an Int64 value: 0x00000B3A73CE2FF2 / 0x00000b3a73ce2ff2 / 0xFFFFF4C58C31D00E / 0xfffff4c58c31d00e");
        }

        [Test]
        public void StringFormatter_CanFormatUInt16Argument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument((UInt16)54543);
            formatter.Format("this is a UInt16 value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a UInt16 value: 54543");
        }

        [Test]
        public void StringFormatter_CanFormatUInt16ArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument((UInt16)54543);
            formatter.Format("this is a UInt16 value: 0x{0:hex} / 0x{0:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is a UInt16 value: 0xD50F / 0xd50f");
        }

        [Test]
        public void StringFormatter_CanFormatUInt32Argument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(12345U);
            formatter.Format("this is a UInt32 value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a UInt32 value: 12345");
        }

        [Test]
        public void StringFormatter_CanFormatUInt32ArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123456U);
            formatter.Format("this is a UInt32 value: 0x{0:hex} / 0x{0:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is a UInt32 value: 0x0001E240 / 0x0001e240");
        }

        [Test]
        public void StringFormatter_CanFormatUInt64Argument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(12345678901234UL);
            formatter.Format("this is a UInt64 value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a UInt64 value: 12345678901234");
        }

        [Test]
        public void StringFormatter_CanFormatUInt64ArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(12345678901234UL);
            formatter.Format("this is a UInt64 value: 0x{0:hex} / 0x{0:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is a UInt64 value: 0x00000B3A73CE2FF2 / 0x00000b3a73ce2ff2");
        }

        [Test]
        public void StringFormatter_CanFormatSingleArgument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.45f);
            formatter.AddArgument(-123.45f);
            formatter.Format("this is a Single value: {0} / {1}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Single value: 123.45000 / -123.45000");
        }

        [Test]
        public void StringFormatter_CanFormatSingleArgumentWithDecimalsCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.456789f);
            formatter.AddArgument(-123.456789f);
            formatter.Format("this is a Single value: {0:decimals:2} / {1:decimals:2}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Single value: 123.46 / -123.46");
        }

        [Test]
        public void StringFormatter_CanFormatSingleArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.456789f);
            formatter.Format("this is a Single value: 0x{0:hex} / 0x{0:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Single value: 0x42F6E9E0 / 0x42f6e9e0");
        }

        [Test]
        public void StringFormatter_CanFormatDoubleArgument()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.45);
            formatter.AddArgument(-123.45);
            formatter.Format("this is a Double value: {0} / {1}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Double value: 123.45000 / -123.45000");
        }

        [Test]
        public void StringFormatter_CanFormatDoubleArgumentWithDecimalsCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.456789);
            formatter.AddArgument(-123.456789);
            formatter.Format("this is a Double value: {0:decimals:2} / {1:decimals:2}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Double value: 123.46 / -123.46");
        }

        [Test]
        public void StringFormatter_CanFormatDoubleArgumentWithHexCommand()
        {
            var formatter = new StringFormatter();
            var buffer = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.456789);
            formatter.Format("this is a Double value: 0x{0:hex} / 0x{0:hex:l}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Double value: 0x405EDD3C07EE0B0B / 0x405edd3c07ee0b0b");
        }
    }
}
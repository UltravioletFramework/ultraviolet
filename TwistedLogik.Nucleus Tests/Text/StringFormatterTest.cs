using NUnit.Framework;
using System;
using System.Text;
using TwistedLogik.Nucleus.Testing;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Nucleus.Tests.Text
{
    [TestFixture]
    public class StringFormatterTest : NucleusTestFramework
    {
        [Test]
        public void StringFormatter_CanFormatStringArgument()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument("world");
            formatter.Format("hello {0}", buffer);

            TheResultingString(buffer).ShouldBe("hello world");
        }

        [Test]
        public void StringFormatter_CanFormatBooleanArgument()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(true);
            formatter.AddArgument(false);
            formatter.Format("this is {0} and this is {1}", buffer);

            TheResultingString(buffer).ShouldBe("this is True and this is False");
        }

        [Test]
        public void StringFormatter_CanFormatByteArgument()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument((Byte)123);
            formatter.Format("this is a Byte value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Byte value: 123");
        }

        [Test]
        public void StringFormatter_CanFormatCharArgument()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument('Z');
            formatter.Format("this is a Char value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Char value: Z");
        }

        [Test]
        public void StringFormatter_CanFormatInt32Argument()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(12345);
            formatter.Format("this is an Int32 value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is an Int32 value: 12345");
        }

        [Test]
        public void StringFormatter_CanFormatSingleArgument()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.45f);
            formatter.Format("this is a Single value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Single value: 123.45000");
        }

        [Test]
        public void StringFormatter_CanFormatSingleArgumentWithDecimalsCommand()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.456789f);
            formatter.Format("this is a Single value: {0:decimals:2}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Single value: 123.46");
        }

        [Test]
        public void StringFormatter_CanFormatDoubleArgument()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.45);
            formatter.Format("this is a Double value: {0}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Double value: 123.45000");
        }

        [Test]
        public void StringFormatter_CanFormatDoubleArgumentWithDecimalsCommand()
        {
            var formatter = new StringFormatter();
            var buffer    = new StringBuilder();

            formatter.Reset();
            formatter.AddArgument(123.456789);
            formatter.Format("this is a Double value: {0:decimals:2}", buffer);

            TheResultingString(buffer).ShouldBe("this is a Double value: 123.46");
        }
    }
}

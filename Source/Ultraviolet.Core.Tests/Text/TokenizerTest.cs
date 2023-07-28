using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ultraviolet.Core.TestFramework;
using Ultraviolet.Core.Text;

namespace Ultraviolet.Core.Tests.IO
{
    [TestFixture]
    public class TokenizerTest : CoreTestFramework
    {
        [Test]
        public void Tokenizer_ShouldTokenizeString()
        {
            var tokens = new List<String>();
            var input = "Hello, world!  This is a test of the \"Ultraviolet String Tokenizer.\"";

            input.Tokenize(tokens);

            TheResultingCollection(tokens)
                .ShouldBeExactly("Hello,", "world!", "This", "is", "a", "test", "of", "the", "Ultraviolet String Tokenizer.");
        }

        [Test]
        public void Tokenizer_ShouldReturnLeftoverStringWhenTokenCountIsConstrained()
        {
            var remainder = String.Empty;
            var tokens = new List<String>();
            var input = "/cmd arg1 arg2 the rest is leftover";

            input.Tokenize(tokens, 3, out remainder);

            TheResultingCollection(tokens)
                .ShouldBeExactly("/cmd", "arg1", "arg2");

            TheResultingString(remainder)
                .ShouldBe("the rest is leftover");
        }
    }
}

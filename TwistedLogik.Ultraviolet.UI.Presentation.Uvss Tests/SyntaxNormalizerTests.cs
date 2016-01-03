using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Testing;
using static TwistedLogik.Ultraviolet.UI.Presentation.Uvss.SyntaxFactory;

namespace TwistedLogik.Ultraviolet.UI.Presentation.Uvss.Tests
{
    [TestClass]
    public class SyntaxNormalizerTests : NucleusTestFramework
    {
        [TestMethod]
        public void SyntaxNormalizer_CorrectlyNormalizesRule()
        {
            var node = Rule(
                PropertyName("foo", "bar"),
                PropertyValue("baz"),
                important: true);

            TheResultingString(node.NormalizeWhitespace().ToFullString()).ShouldBe(@"foo.bar: baz !important;");
        }
    }
}

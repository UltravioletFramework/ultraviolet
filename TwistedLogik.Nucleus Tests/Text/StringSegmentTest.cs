using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Testing;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Nucleus.Tests.Text
{
    [TestClass]
    public class StringSegmentTest : NucleusTestFramework
    {
        [TestMethod]
        public void StringSegment_CanBeCreatedFromString()
        {
            var source = "Hello, world!";
            var segment = new StringSegment(source, 2, 4);

            TheResultingString(segment.ToString())
                .ShouldBe("llo,");
        }
    }
}

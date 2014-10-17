using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TwistedLogik.Ultraviolet.Tests
{
    [TestClass]
    public static class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }
    }
}

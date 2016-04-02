using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Testing;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Nucleus.Tests.Text
{
    [TestClass]
    public class StringPtr16Test : NucleusTestFramework
    {
        [TestMethod]
        public void StringPtr16_HandlesNullTerminatedString()
        {
            unsafe
            {
                var data = new ushort[] { 'H', 'e', 'l', 'l', 'o', 0 };
                fixed (ushort* pdata = data)
                {
                    var ptr = new StringPtr16((IntPtr)pdata);
                    
                    TheResultingString(ptr.ToString())
                        .ShouldBe("Hello");
                }
            }
        }
    }
}

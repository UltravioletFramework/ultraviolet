using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Nucleus.Testing;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Nucleus.Tests.Text
{
    [TestClass]
    public class StringPtr8Test : NucleusTestFramework
    {
        [TestMethod]
        public void StringPtr8_HandlesNullTerminatedString()
        {
            unsafe
            {
                var data = new byte[] { (byte)'H', (byte)'e', (byte)'l', (byte)'l', (byte)'o', 0 };
                fixed (byte* pdata = data)
                {
                    var ptr = new StringPtr8((IntPtr)pdata);

                    TheResultingString(ptr.ToString())
                        .ShouldBe("Hello");
                }
            }
        }
    }
}

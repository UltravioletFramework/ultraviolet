using System;
using TwistedLogik.Nucleus;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    public sealed class Program
    {
        public static Int32 Main(string[] args)
        {
            LibraryLoader.Load("libcef");

            CefRuntime.Load();
            try
            {
                var cefArgs = new CefMainArgs(args);
                var cefApp = (CefApp)null;

                return CefRuntime.ExecuteProcess(cefArgs, cefApp, IntPtr.Zero);
            }
            finally
            {
                CefRuntime.Shutdown();
            }
        }
    }
}

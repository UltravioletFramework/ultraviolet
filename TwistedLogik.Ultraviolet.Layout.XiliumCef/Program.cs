using System;
using TwistedLogik.Nucleus;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Contains the entry point for the Chromium subprocess.
    /// </summary>
    internal sealed class Program
    {
        internal static Int32 Main(string[] args)
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

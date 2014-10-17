using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TwistedLogik.Nucleus
{
    /// <summary>
    /// Contains methods for loading native libraries dynamically based on the process' bitness.
    /// </summary>
    public static class NativeLibraryLoader
    {
        public static void LoadLibrary(String name)
        {
            var directory = Environment.Is64BitProcess ? "x64" : "x86";

        }

        private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);
        private static void LoadNativeLibrary()
        {

        }
    }
}

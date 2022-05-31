using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Contains native methods used by the Ultraviolet Framework.
    /// </summary>
    internal static unsafe class UltravioletNative
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct utsname_darwin
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String sysname;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String nodename;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String release;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String version;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String machine;
        }

        [DllImport("libc")]
        public static extern int uname(IntPtr buf);        
    }
}

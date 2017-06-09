using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.Shims.Desktop.Platform
{
    partial class DesktopScreenDensityService
    {
        /// <summary>
        /// Contains native Win32 methods used to calculate screen density on Windows.
        /// </summary>
        private static unsafe class Win32
        {            
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public Int32 left;
                public Int32 top;
                public Int32 right;
                public Int32 bottom;
            }

            public delegate Boolean EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, RECT* lprcMonitor, IntPtr dwData);

            [DllImport("user32")]
            public static extern Boolean EnumDisplayMonitors(IntPtr hdc, RECT* lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

            [DllImport("Shcore")]
            public static extern IntPtr GetDpiForMonitor(IntPtr hmonitor, Int32 dpiType, out UInt32 dpiX, out UInt32 dpiY);
        }
    }
}

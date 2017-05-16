using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Platform
{
    public partial class SDL2UltravioletWindow
    {
        /// <summary>
        /// Contains Win32 p/invoke functions.
        /// </summary>
        private static class Win32Native
        {
            public delegate IntPtr WndProcDelegate(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lparam);

            public const Int32 GWL_STYLE = -16;
            public const UInt32 WS_BORDER = 0x800000;
            public const UInt32 WS_DLGFRAME = 0x400000;

            [DllImport("user32.dll", EntryPoint = "CallWindowProc")]
            public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
            private static extern IntPtr SetWindowLong32(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

            public static IntPtr SetWindowLongPtr(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong)
            {
                return Environment.Is64BitProcess ?
                    SetWindowLongPtr64(hWnd, nIndex, dwNewLong) :
                    SetWindowLong32(hWnd, nIndex, dwNewLong);
            }

            [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
            private static extern IntPtr GetWindowLong32(IntPtr hWnd, Int32 nIndex);

            [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
            private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, Int32 nIndex);

            public static IntPtr GetWindowLongPtr(IntPtr hWnd, Int32 nIndex)
            {
                return Environment.Is64BitProcess ?
                    GetWindowLongPtr64(hWnd, nIndex) :
                    GetWindowLong32(hWnd, nIndex);
            }
        }
    }
}

using System;
using System.Runtime.InteropServices;

namespace TwistedLogik.Ultraviolet.OpenGL.Platform
{
    public unsafe partial class OpenGLUltravioletWindow
    {
        /// <summary>
        /// Contains Win32 p/invoke functions.
        /// </summary>
        private static class Win32Native
        {
            public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lparam);

            [DllImport("user32.dll", EntryPoint = "CallWindowProc")]
            public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
            private static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
            private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
            {
                return Environment.Is64BitProcess ? 
                    SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : 
                    SetWindowLong32(hWnd, nIndex, dwNewLong);
            }
        }
    }
}

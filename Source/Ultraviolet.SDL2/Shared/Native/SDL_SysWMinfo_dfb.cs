using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo_dfb
    {
        public IntPtr dfb;
        public IntPtr window;
        public IntPtr surface;
    }
#pragma warning restore 1591
}

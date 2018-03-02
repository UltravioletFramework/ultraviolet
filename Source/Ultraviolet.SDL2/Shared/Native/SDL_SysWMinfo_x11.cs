using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [Preserve]
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo_x11
    {
        public IntPtr display;
        public IntPtr window;
    }
#pragma warning restore 1591
}

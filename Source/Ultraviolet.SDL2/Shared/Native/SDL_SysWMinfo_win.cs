using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [Preserve]
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo_win
    {
        public IntPtr window;
        public IntPtr hdc;
    }
#pragma warning restore 1591
}

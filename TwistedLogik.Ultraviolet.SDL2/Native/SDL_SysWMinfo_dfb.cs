using System;
using System.Runtime.InteropServices;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo_dfb
    {
        public IntPtr dfb;
        public IntPtr window;
        public IntPtr surface;
    }
}

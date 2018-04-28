using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_Rect
    {
        public Int32 x, y;
        public Int32 w, h;
    }
#pragma warning restore 1591
}

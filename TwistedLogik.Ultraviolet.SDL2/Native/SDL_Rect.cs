using System;
using System.Runtime.InteropServices;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_Rect
    {
        public Int32 x, y;
        public Int32 w, h;
    }
}

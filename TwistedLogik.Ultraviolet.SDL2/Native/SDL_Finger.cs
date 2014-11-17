using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Finger
    {
        public Int64 id;
        public Single x;
        public Single y;
        public Single pressure;
    }
}

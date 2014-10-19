using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_MouseWheelEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public UInt32 windowID;
        public UInt32 which;
        public Int32 x;
        public Int32 y;
    }
}

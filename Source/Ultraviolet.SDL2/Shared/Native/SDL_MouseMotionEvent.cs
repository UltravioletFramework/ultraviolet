using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_MouseMotionEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public UInt32 windowID;
        public UInt32 which;
        public UInt32 state;
        public Int32 x;
        public Int32 y;
        public Int32 xrel;
        public Int32 yrel;
    }
#pragma warning restore 1591
}
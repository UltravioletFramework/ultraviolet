using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_UserEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public UInt32 windowID;
        public Int32 code;
        public void* data1;
        public void* data2;
    }
}

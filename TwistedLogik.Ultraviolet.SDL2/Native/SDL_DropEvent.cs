using System;
using System.Runtime.InteropServices;
using TwistedLogik.Nucleus;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Preserve]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_DropEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public char* file;
    }
}

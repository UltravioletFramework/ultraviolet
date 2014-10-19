using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_TextEditingEvent
    {
        public const Int32 TEXT_SIZE = 32;

        public UInt32 type;
        public UInt32 timestamp;
        public UInt32 windowID;
        public fixed char text[TEXT_SIZE];
        public Int32 start;
        public Int32 length;
    }
}

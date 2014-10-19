using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_KeyboardEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public UInt32 windowID;
        public Byte state;
        public Byte repeat;
        public Byte padding2;
        public Byte padding3;
        public SDL_Keysym keysym;
    }
}

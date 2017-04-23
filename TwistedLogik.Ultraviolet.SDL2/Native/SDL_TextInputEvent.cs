using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Preserve]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_TextInputEvent
    {
        public const Int32 TEXT_SIZE = 32;

        public UInt32 type;
        public UInt32 timestamp;
        public UInt32 windowID;
        public fixed byte text[TEXT_SIZE];
    }
}

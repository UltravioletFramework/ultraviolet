using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

#pragma warning disable 1591

namespace Ultraviolet.SDL2.Native
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

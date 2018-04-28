using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_version
    {
        public Byte major;
        public Byte minor;
        public Byte patch;
    }
#pragma warning restore 1591
}

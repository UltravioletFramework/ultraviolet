using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [Preserve]
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_version
    {
        public Byte major;
        public Byte minor;
        public Byte patch;
    }
#pragma warning restore 1591
}

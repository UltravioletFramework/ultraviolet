using System;
using System.Runtime.InteropServices;
using TwistedLogik.Nucleus;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Preserve]
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo_uikit
    {
        public IntPtr window;
        public UInt32 framebuffer;
        public UInt32 colorbuffer;
        public UInt32 resolveFramebuffer;
    }
}

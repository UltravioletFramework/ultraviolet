using System;
using System.Runtime.InteropServices;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Keysym
    {
        public SDL_Scancode scancode;
        public SDL_Keycode keycode;
        public SDL_Keymod mod;
        public UInt32 unused;
    }
}

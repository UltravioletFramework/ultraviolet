using System;
using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_DisplayMode
    {
        public SDL_PixelFormatEnum format;
        public Int32 w;
        public Int32 h;
        public Int32 refresh_rate;
        public void* driver_data;
    }
}

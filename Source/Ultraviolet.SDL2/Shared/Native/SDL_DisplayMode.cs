using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_DisplayMode
    {
        public SDL_PixelFormatEnum format;
        public Int32 w;
        public Int32 h;
        public Int32 refresh_rate;
        public void* driver_data;
    }
#pragma warning restore 1591
}

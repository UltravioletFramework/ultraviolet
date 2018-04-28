using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_Surface
    {
        public UInt32 flags;
        public SDL_PixelFormat* format;
        public Int32 w, h;
        public Int32 pitch;
        public void* pixels;
        public void* userdata;
        public Int32 locked;
        public void* lock_data;
        public SDL_Rect clip_rect;
        public IntPtr map;
        public Int32 refcount;
    }
#pragma warning restore 1591
}

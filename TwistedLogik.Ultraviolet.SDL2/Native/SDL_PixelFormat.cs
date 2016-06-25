﻿using System;
using System.Runtime.InteropServices;
using TwistedLogik.Nucleus;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Preserve]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_PixelFormat
    {
        public UInt32 format;
        public IntPtr palette;
        public Byte BitsPerPixel;
        public Byte BytesPerPixel;
        public Byte padding0;
        public Byte padding1;
        public UInt32 Rmask;
        public UInt32 Gmask;
        public UInt32 Bmask;
        public UInt32 Amask;
        public Byte Rloss;
        public Byte Gloss;
        public Byte Bloss;
        public Byte Aloss;
        public Byte Rshift;
        public Byte Gshift;
        public Byte Bshift;
        public Byte Ashift;
        public Int32 refcount;
        public IntPtr next;
    }
}

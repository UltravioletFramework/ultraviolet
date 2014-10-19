using System;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    /// <summary>
    /// Contains SDL2 preprocessor macros.
    /// </summary>
    public static unsafe class SDLMacro
    {
        public static void SDL_VERSION(SDL_version* version)
        {
            version->major = 2;
            version->minor = 0;
            version->patch = 3;
        }

        public static Boolean MUSTLOCK(SDL_Surface_Native* surface)
        {
            const uint SDL_RLEACCEL = 0x00000002;
            return (surface->flags & SDL_RLEACCEL) != 0;
        }

        public static Int32 BITSPERPIXEL(SDL_PixelFormatEnum format)
        {
            return (int)(((uint)(format) >> 8) & 0xFF);
        }

        public static UInt32 FOURCC(Byte A, Byte B, Byte C, Byte D)
        {
            return ((uint)A << 0) | ((uint)B << 8) | ((uint)C << 16) | ((uint)D << 24);
        }

        public static UInt32 DEFINE_PIXELFOURCC(Char A, Char B, Char C, Char D)
        {
            return FOURCC((Byte)A, (Byte)B, (Byte)C, (Byte)D);
        }

        public static Int32 DEFINE_PIXELFORMAT(Int32 type, Int32 order, Int32 layout, Int32 bits, Int32 bytes)
        {
            return ((1 << 28) | ((type) << 24) | ((order) << 20) | ((layout) << 16) | ((bits) << 8) | ((bytes) << 0));
        }

        public static readonly Int32 WINDOWPOS_UNDEFINED_MASK = 0x1FFF0000;

        public static Int32 WINDOWPOS_UNDEFINED()
        {
            return WINDOWPOS_UNDEFINED_DISPLAY(0);
        }

        public static Int32 WINDOWPOS_UNDEFINED_DISPLAY(Int32 displayIndex)
        {
            return WINDOWPOS_UNDEFINED_MASK | displayIndex;
        }

        public static readonly Int32 WINDOWPOS_CENTERED_MASK = 0x2FFF0000;

        public static Int32 WINDOWPOS_CENTERED()
        {
            return WINDOWPOS_CENTERED_DISPLAY(0);
        }

        public static Int32 WINDOWPOS_CENTERED_DISPLAY(Int32 displayIndex)
        {
            return WINDOWPOS_CENTERED_MASK | displayIndex;
        }
    }
}

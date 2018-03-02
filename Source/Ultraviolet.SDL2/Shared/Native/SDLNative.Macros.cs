using System;
using static Ultraviolet.SDL2.Native.SDL_PixelFormatEnum;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    /// <summary>
    /// Contains SDL2 preprocessor macros.
    /// </summary>
    unsafe static partial class SDLNative
    {
        public static void SDL_VERSION(SDL_version* version)
        {
            version->major = 2;
            version->minor = 0;
            version->patch = 7;
        }

        public const UInt32 SDL_SWSURFACE = 0;
        public const UInt32 SDL_PREALLOC = 0x00000001;
        public const UInt32 SDL_RLEACCEL = 0x00000002;
        public const UInt32 SDL_DONTFREE = 0x00000004;

        public static Boolean SDL_MUSTLOCK(SDL_Surface* surface) => (surface->flags & SDL_RLEACCEL) != 0;

        public static UInt32 SDL_PIXELFLAG(SDL_PixelFormatEnum format) => ((((UInt32)format) >> 28) & 0x0F);
        public static UInt32 SDL_PIXELTYPE(SDL_PixelFormatEnum format) => ((((UInt32)format) >> 24) & 0x0F);
        public static UInt32 SDL_PIXELORDER(SDL_PixelFormatEnum format) => ((((UInt32)format) >> 20) & 0x0F);
        public static UInt32 SDL_PIXELLAYOUT(SDL_PixelFormatEnum format) => ((((UInt32)format) >> 16) & 0x0F);
        public static UInt32 SDL_BITSPERPIXEL(SDL_PixelFormatEnum format) => ((((UInt32)format) >> 8) & 0xFF);
        public static UInt32 SDL_BYTESPERPIXEL(SDL_PixelFormatEnum format) =>
           (SDL_ISPIXELFORMAT_FOURCC(format) ?
                ((((format) == SDL_PIXELFORMAT_YUY2) ||
                  ((format) == SDL_PIXELFORMAT_UYVY) ||
                  ((format) == SDL_PIXELFORMAT_YVYU)) ? 2u : 1u) : ((((UInt32)format) >> 0) & 0xFF));

        public static UInt32 SDL_FOURCC(Byte A, Byte B, Byte C, Byte D) =>
            ((UInt32)A << 0) | ((UInt32)B << 8) | ((UInt32)C << 16) | ((UInt32)D << 24);

        public static UInt32 SDL_DEFINE_PIXELFOURCC(Char A, Char B, Char C, Char D) => 
            SDL_FOURCC((Byte)A, (Byte)B, (Byte)C, (Byte)D);

        public static UInt32 SDL_DEFINE_PIXELFORMAT(UInt32 type, UInt32 order, UInt32 layout, UInt32 bits, UInt32 bytes) =>
            ((1 << 28) | ((type) << 24) | ((order) << 20) | ((layout) << 16) | ((bits) << 8) | ((bytes) << 0));

        public static Boolean SDL_ISPIXELFORMAT_FOURCC(SDL_PixelFormatEnum format) =>
            ((format != 0) && (SDL_PIXELFLAG(format) != 1));

        public static readonly UInt32 SDL_WINDOWPOS_UNDEFINED_MASK = 0x1FFF0000;
        public static UInt32 SDL_WINDOWPOS_UNDEFINED() => SDL_WINDOWPOS_UNDEFINED_DISPLAY(0);
        public static UInt32 SDL_WINDOWPOS_UNDEFINED_DISPLAY(UInt32 displayIndex) => SDL_WINDOWPOS_UNDEFINED_MASK | displayIndex;

        public static readonly UInt32 SDL_WINDOWPOS_CENTERED_MASK = 0x2FFF0000;
        public static UInt32 SDL_WINDOWPOS_CENTERED() => SDL_WINDOWPOS_CENTERED_DISPLAY(0);
        public static UInt32 SDL_WINDOWPOS_CENTERED_DISPLAY(UInt32 displayIndex) => SDL_WINDOWPOS_CENTERED_MASK | displayIndex;
    }
#pragma warning restore 1591
}
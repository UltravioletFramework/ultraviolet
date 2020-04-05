using System;
using System.Runtime.CompilerServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    /// <summary>
    /// Contains SDL2 helper methods.
    /// </summary>
    unsafe static partial class SDLNative
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Surface* SDL_LoadBMP(String file) => SDL_LoadBMP_RW(SDL_RWFromFile(file, "r"), 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SaveBMP(SDL_Surface* surface, String file) => SDL_SaveBMP_RW(surface, SDL_RWFromFile(file, "wb"), 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GameControllerAddMappingsFromFile(String file) => SDL_GameControllerAddMappingsFromRW(SDL_RWFromFile(file, "rb"), 1);
    }
}

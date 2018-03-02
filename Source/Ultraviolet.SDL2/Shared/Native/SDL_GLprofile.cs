using System;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [Flags]
    public enum SDL_GLprofile
    {
        SDL_GL_CONTEXT_PROFILE_CORE = 0x0001,
        SDL_GL_CONTEXT_PROFILE_COMPATIBILITY = 0x0002,
        SDL_GL_CONTEXT_PROFILE_ES = 0x0004,
    }
#pragma warning restore 1591
}

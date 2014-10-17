using System;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Flags]
    public enum SDL_GLprofile
    {
        CORE          = 0x0001,
        COMPATIBILITY = 0x0002,
        ES            = 0x0004,
    }
}

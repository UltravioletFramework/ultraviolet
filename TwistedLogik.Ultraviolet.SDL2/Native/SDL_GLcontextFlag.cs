using System;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Flags]
    public enum SDL_GLcontextFlag
    {
        DEBUG = 0x0001,
        FORWARD_COMPATIBLE = 0x0002,
        ROBUST_ACCESS = 0x0004,
        RESET_ISOLATION = 0x0008,
    }
}

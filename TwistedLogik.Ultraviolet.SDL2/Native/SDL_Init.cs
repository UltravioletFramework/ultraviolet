using System;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Flags]
    public enum SDL_Init : uint
    {
        TIMER           = 0x00000001,
        AUDIO           = 0x00000010,
        VIDEO           = 0x00000020,
        JOYSTICK        = 0x00000200,
        HAPTIC          = 0x00001000,
        GAMECONTROLLER  = 0x00002000,
        EVENTS          = 0x00004000,
        NOPARACHUTE     = 0x00100000,
        EVERYTHING      = TIMER | AUDIO | VIDEO | EVENTS | JOYSTICK | HAPTIC | GAMECONTROLLER,
    }
}

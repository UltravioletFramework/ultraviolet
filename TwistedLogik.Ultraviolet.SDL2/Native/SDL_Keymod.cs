using System;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Flags]
    public enum SDL_Keymod
    {
        NONE = 0x0000,
        LSHIFT = 0x0001,
        RSHIFT = 0x0002,
        LCTRL = 0x0040,
        RCTRL = 0x0080,
        LALT = 0x0100,
        RALT = 0x0200,
        LGUI = 0x0400,
        RGUI = 0x0800,
        NUM = 0x1000,
        CAPS = 0x2000,
        MODE = 0x4000,
        CTRL = LCTRL | RCTRL,
        SHIFT = LSHIFT | RSHIFT,
        ALT = LALT | RALT,
        GUI = LGUI | RGUI
    }
}

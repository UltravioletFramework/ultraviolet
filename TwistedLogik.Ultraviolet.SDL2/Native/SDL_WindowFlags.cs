using System;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [Flags]
    public enum SDL_WindowFlags : uint
    {
        FULLSCREEN = 0x00000001,
        OPENGL = 0x00000002,
        SHOWN = 0x00000004,
        HIDDEN = 0x00000008,
        BORDERLESS = 0x00000010,
        RESIZABLE = 0x00000020,
        MINIMIZED = 0x00000040,
        MAXIMIZED = 0x00000080,
        INPUT_GRABBED = 0x00000100,
        INPUT_FOCUS = 0x00000200,
        MOUSE_FOCUS = 0x00000400,
        FULLSCREEN_DESKTOP = (FULLSCREEN | 0x00001000),
        FOREIGN = 0x00000800,
        ALLOW_HIGHDPI = 0x00002000,
    }
}

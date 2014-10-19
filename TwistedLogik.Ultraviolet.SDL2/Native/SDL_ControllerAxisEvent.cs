using System;
using System.Runtime.InteropServices;
using SDL_JoystickID = System.Int32;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_ControllerAxisEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public SDL_JoystickID which;
        public Byte axis;
        public Byte padding1;
        public Byte padding2;
        public Byte padding3;
        public Int16 value;
        public UInt16 padding4;
    }
}

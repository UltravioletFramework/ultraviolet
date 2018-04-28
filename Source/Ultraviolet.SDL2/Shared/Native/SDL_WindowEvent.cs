using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_WindowEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public UInt32 windowID;
        public SDL_WindowEventID @event;
        public Byte padding1;
        public Byte padding2;
        public Byte padding3;
        public Int32 data1;
        public Int32 data2;
    }
#pragma warning restore 1591
}
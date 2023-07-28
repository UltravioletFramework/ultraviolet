using System;
using System.Runtime.InteropServices;
using SDL_TouchID = System.Int64;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_MultiGestureEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public SDL_TouchID touchId;
        public Single dTheta;
        public Single dDist;
        public Single x;
        public Single y;
        public UInt16 numFingers;
        public UInt16 padding;
    }
#pragma warning restore 1591
}
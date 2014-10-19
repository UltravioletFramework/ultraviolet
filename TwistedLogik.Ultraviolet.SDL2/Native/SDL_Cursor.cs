using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_Cursor
    {
        public SDL_Cursor* next;
        public void* driverdata;
    }
}

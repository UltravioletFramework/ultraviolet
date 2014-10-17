using System.Runtime.InteropServices;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_Cursor
    {
        public SDL_Cursor* next;
        public void* driverdata;
    }
}

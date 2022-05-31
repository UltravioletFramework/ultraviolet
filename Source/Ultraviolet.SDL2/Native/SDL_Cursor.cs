using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_Cursor
    {
        public SDL_Cursor* next;
        public void* driverdata;
    }
#pragma warning restore 1591
}

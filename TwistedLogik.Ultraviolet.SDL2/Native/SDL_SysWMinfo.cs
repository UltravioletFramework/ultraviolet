using System.Runtime.InteropServices;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo
    {
        public SDL_version version;
        public SDL_SysWMType subsystem;
        public SDL_SysWMinfoUnion info;
    }
}

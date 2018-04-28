using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo
    {
        public SDL_version version;
        public SDL_SysWM_Type subsystem;
        public SDL_SysWMinfoUnion info;
    }
#pragma warning restore 1591
}

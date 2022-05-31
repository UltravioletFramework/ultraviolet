using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Vector32
    {
        public Int32 x;
        public Int32 y;
    }
#pragma warning restore 1591
}

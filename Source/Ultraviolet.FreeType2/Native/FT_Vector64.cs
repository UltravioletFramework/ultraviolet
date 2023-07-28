using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Vector64
    {
        public Int64 x;
        public Int64 y;
    }
#pragma warning restore 1591
}

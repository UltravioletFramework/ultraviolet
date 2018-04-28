using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Matrix64
    {
        public Int64 xx, xy;
        public Int64 yx, yy;
    }
#pragma warning restore 1591
}

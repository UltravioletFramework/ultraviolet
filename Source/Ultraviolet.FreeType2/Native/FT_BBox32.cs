using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_BBox32
    {
        public Int32 xMin, yMin;
        public Int32 xMax, yMax;
    }
#pragma warning restore 1591
}

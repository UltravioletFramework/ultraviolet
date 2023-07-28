using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Size_Metrics64
    {
        public UInt16 x_ppem;
        public UInt16 y_ppem;

        public Int64 x_scale;
        public Int64 y_scale;

        public Int64 ascender;
        public Int64 descender;
        public Int64 height;
        public Int64 max_advance;
    }
#pragma warning restore 1591
}

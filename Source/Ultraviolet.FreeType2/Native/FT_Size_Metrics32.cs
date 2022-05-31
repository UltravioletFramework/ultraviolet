using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Size_Metrics32
    {
        public UInt16 x_ppem;
        public UInt16 y_ppem;

        public Int32 x_scale;
        public Int32 y_scale;

        public Int32 ascender;
        public Int32 descender;
        public Int32 height;
        public Int32 max_advance;
    }
#pragma warning restore 1591
}

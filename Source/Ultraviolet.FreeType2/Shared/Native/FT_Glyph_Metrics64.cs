using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Glyph_Metrics64
    {
        public Int64 width;
        public Int64 height;

        public Int64 horiBearingX;
        public Int64 horiBearingY;
        public Int64 horiAdvance;

        public Int64 vertBearingX;
        public Int64 vertBearingY;
        public Int64 vertAdvance;
    }
#pragma warning restore 1591
}

using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Glyph_Metrics32
    {
        public Int32 width;
        public Int32 height;

        public Int32 horiBearingX;
        public Int32 horiBearingY;
        public Int32 horiAdvance;
    
        public Int32 vertBearingX;
        public Int32 vertBearingY;
        public Int32 vertAdvance;
    }
#pragma warning restore 1591
}

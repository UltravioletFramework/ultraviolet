using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphLoadRec64
    {
        public FT_Outline64 outline;
        public FT_Vector64* extra_points;
        public FT_Vector64* extra_points2;
        public UInt32 num_subglyphs;
        public FT_SubGlyphRec64* subglyphs;
    }
#pragma warning restore 1591
}

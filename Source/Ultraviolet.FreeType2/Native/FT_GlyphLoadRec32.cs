using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphLoadRec32
    {
        public FT_Outline32 outline;
        public FT_Vector32* extra_points;
        public FT_Vector32* extra_points2;
        public UInt32 num_subglyphs;
        public FT_SubGlyphRec32* subglyphs;
    }
#pragma warning restore 1591
}

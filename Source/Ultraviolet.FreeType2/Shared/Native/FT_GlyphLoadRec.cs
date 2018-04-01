using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_UInt = System.UInt32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphLoadRec
    {
        public FT_Outline outline;
        public FT_Vector* extra_points;
        public FT_Vector* extra_points2;
        public FT_UInt num_subglyphs;
        public FT_SubGlyphRec* subglyphs;
    }
#pragma warning restore 1591
}

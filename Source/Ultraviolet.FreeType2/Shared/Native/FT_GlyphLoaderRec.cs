using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Bool = System.Boolean;
using FT_UInt = System.UInt32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphLoaderRec
    {
        public FT_MemoryRec* memory;
        public FT_UInt max_points;
        public FT_UInt max_contours;
        public FT_UInt max_subglyphs;
        public FT_Bool use_extra;

        public FT_GlyphLoadRec @base;
        public FT_GlyphLoadRec current;

        public IntPtr other;
    }
#pragma warning restore 1591
}

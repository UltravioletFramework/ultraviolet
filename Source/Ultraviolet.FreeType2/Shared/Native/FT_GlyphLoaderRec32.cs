using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphLoaderRec32
    {
        public IntPtr memory;
        public UInt32 max_points;
        public UInt32 max_contours;
        public UInt32 max_subglyphs;
        public Boolean use_extra;

        public FT_GlyphLoadRec32 @base;
        public FT_GlyphLoadRec32 current;

        public IntPtr other;
    }
#pragma warning restore 1591
}

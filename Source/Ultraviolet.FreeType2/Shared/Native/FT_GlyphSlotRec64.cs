using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphSlotRec64
    {
        public FT_LibraryRec* library;
        public FT_FaceRec64* face;
        public FT_GlyphSlotRec64* next;
        public UInt32 reserved;
        public FT_Generic generic;

        public FT_Glyph_Metrics64 metrics;
        public Int64 linearHoriAdvance;
        public Int64 linearVertAdvance;
        public FT_Vector64 advance;

        public FT_Glyph_Format format;

        public FT_Bitmap bitmap;
        public Int32 bitmap_left;
        public Int32 bitmap_top;

        public FT_Outline64 outline;

        public UInt32 num_subglyphs;
        public FT_SubGlyphRec64* subglyphs;

        public IntPtr control_data;
        public Int64 control_len;

        public Int64 lsb_delta;
        public Int64 rsb_delta;

        public IntPtr other;

        public IntPtr @internal;
    }
#pragma warning restore 1591
}

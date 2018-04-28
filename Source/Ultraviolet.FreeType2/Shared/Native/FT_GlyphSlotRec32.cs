using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphSlotRec32
    {
        public IntPtr library;
        public FT_FaceRec32* face;
        public FT_GlyphSlotRec32* next;
        public UInt32 reserved;
        public FT_Generic generic;

        public FT_Glyph_Metrics32 metrics;
        public Int32 linearHoriAdvance;
        public Int32 linearVertAdvance;
        public FT_Vector32 advance;

        public FT_Glyph_Format format;

        public FT_Bitmap bitmap;
        public Int32 bitmap_left;
        public Int32 bitmap_top;

        public FT_Outline32 outline;

        public UInt32 num_subglyphs;
        public FT_SubGlyphRec32* subglyphs;

        public IntPtr control_data;
        public Int32 control_len;

        public Int32 lsb_delta;
        public Int32 rsb_delta;

        public IntPtr other;

        public IntPtr @internal;
    }
#pragma warning restore 1591
}

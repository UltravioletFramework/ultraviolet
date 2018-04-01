using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Fixed = System.Int32;
using FT_Int = System.Int32;
using FT_Pos = System.Int32;
using FT_UInt = System.UInt32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphSlotRec
    {
        public FT_LibraryRec* library;
        public FT_FaceRec* face;
        public FT_GlyphSlotRec* next;
        public FT_UInt reserved;
        public FT_Generic generic;

        public FT_Glyph_Metrics metrics;
        public FT_Fixed linearHoriAdvance;
        public FT_Fixed linearVertAdvance;
        public FT_Vector advance;

        public FT_Glyph_Format format;

        public FT_Bitmap bitmap;
        public FT_Int bitmap_left;
        public FT_Int bitmap_top;

        public FT_Outline outline;

        public FT_UInt num_subglyphs;
        public FT_SubGlyphRec* subglyphs;

        public IntPtr control_data;
        public Int32 control_len;

        public FT_Pos lsb_delta;
        public FT_Pos rsb_delta;

        public IntPtr other;

        public IntPtr @internal;
    }
#pragma warning restore 1591
}

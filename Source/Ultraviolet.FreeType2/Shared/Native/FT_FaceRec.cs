using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Int = System.Int32;
using FT_Long = System.Int32;
using FT_Short = System.Int16;
using FT_UShort = System.UInt16;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_FaceRec
    {
        public FT_Long num_faces;
        public FT_Long face_index;

        public FT_Long face_flags;
        public FT_Long style_flags;

        public FT_Long num_glyphs;

        public IntPtr family_name;
        public IntPtr style_name;

        public FT_Int num_fixed_sizes;
        public FT_Bitmap_Size* available_sizes;

        public FT_Int num_charmaps;
        public FT_CharMapRec* charmaps;

        public FT_Generic generic;

        public FT_BBox bbox;

        public FT_UShort units_per_EM;
        public FT_Short ascender;
        public FT_Short descender;
        public FT_Short height;

        public FT_Short max_advance_width;
        public FT_Short max_advance_height;

        public FT_Short underline_position;
        public FT_Short underline_thickness;

        public FT_GlyphSlotRec* glyph;
        public FT_SizeRec* size;
        public FT_CharMapRec* charmap;

        public IntPtr driver;
        public IntPtr memory;
        public IntPtr stream;

        public FT_ListRec sizes_list;

        public FT_Generic autohint;
        public IntPtr extensions;

        public IntPtr @internal;
    }
#pragma warning restore 1591
}

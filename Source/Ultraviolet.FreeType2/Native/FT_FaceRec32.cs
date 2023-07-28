using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_FaceRec32
    {
        public Int32 num_faces;
        public Int32 face_index;

        public Int32 face_flags;
        public Int32 style_flags;

        public Int32 num_glyphs;

        public IntPtr family_name;
        public IntPtr style_name;

        public Int32 num_fixed_sizes;
        public FT_Bitmap_Size32* available_sizes;

        public Int32 num_charmaps;
        public FT_CharMapRec32** charmaps;

        public FT_Generic generic;

        public FT_BBox32 bbox;

        public UInt16 units_per_EM;
        public Int16 ascender;
        public Int16 descender;
        public Int16 height;

        public Int16 max_advance_width;
        public Int16 max_advance_height;

        public Int16 underline_position;
        public Int16 underline_thickness;

        public FT_GlyphSlotRec32* glyph;
        public FT_SizeRec32* size;
        public FT_CharMapRec32* charmap;

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

using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    public abstract unsafe class FreeTypeNativeImpl
    {
        public abstract FT_Error FT_Init_FreeType(IntPtr alibrary);
        public abstract FT_Error FT_Done_FreeType(IntPtr library);
        public abstract FT_Error FT_New_Face32(IntPtr library, String filepathname, Int32 face_index, IntPtr aface);
        public abstract FT_Error FT_New_Face64(IntPtr library, String filepathname, Int64 face_index, IntPtr aface);
        public abstract FT_Error FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface);
        public abstract FT_Error FT_New_Memory_Face64(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface);
        public abstract FT_Error FT_Done_Face(IntPtr face);
        public abstract FT_Error FT_Set_Char_Size32(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        public abstract FT_Error FT_Set_Char_Size64(IntPtr face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        public abstract FT_Error FT_Select_Size(IntPtr face, Int32 strike_index);
        public abstract UInt32 FT_Get_Char_Index32(IntPtr face, UInt32 charcode);
        public abstract UInt32 FT_Get_Char_Index64(IntPtr face, UInt64 charcode);
        public abstract FT_Error FT_Load_Glyph(IntPtr face, UInt32 glyph_index, Int32 load_flags);
        public abstract FT_Error FT_Get_Kerning(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning);
        public abstract FT_Error FT_Get_Glyph(IntPtr slot, IntPtr aglyph);
        public abstract FT_Error FT_Done_Glyph(IntPtr glyph);
        public abstract FT_Error FT_Render_Glyph(IntPtr slot, FT_Render_Mode render_mode);
        public abstract FT_Error FT_Stroker_New(IntPtr library, IntPtr astroker);
        public abstract void FT_Stroker_Done(IntPtr stroker);
        public abstract void FT_Stroker_Set32(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit);
        public abstract void FT_Stroker_Set64(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit);
        public abstract FT_Error FT_Glyph_StrokeBorder(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy);
        public abstract FT_Error FT_Glyph_To_Bitmap(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy);
    }
#pragma warning restore 1591
}

using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class FreeTypeNativeImpl_Android : FreeTypeNativeImpl
    {
        [DllImport("freetype", EntryPoint = "FT_Init_FreeType", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Init_FreeType(IntPtr alibrary);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Init_FreeType(IntPtr alibrary) => INTERNAL_FT_Init_FreeType(alibrary);
        
        [DllImport("freetype", EntryPoint = "FT_Done_FreeType", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Done_FreeType(IntPtr library);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Done_FreeType(IntPtr library) => INTERNAL_FT_Done_FreeType(library);
        
        [DllImport("freetype", EntryPoint = "FT_New_Face", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_New_Face32(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int32 face_index, IntPtr aface);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_New_Face32(IntPtr library, String filepathname, Int32 face_index, IntPtr aface) => INTERNAL_FT_New_Face32(library, filepathname, face_index, aface);
        
        [DllImport("freetype", EntryPoint = "FT_New_Face", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_New_Face64(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, IntPtr aface);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_New_Face64(IntPtr library, String filepathname, Int64 face_index, IntPtr aface) => INTERNAL_FT_New_Face64(library, filepathname, face_index, aface);
        
        [DllImport("freetype", EntryPoint = "FT_New_Memory_Face", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface) => INTERNAL_FT_New_Memory_Face32(library, file_base, file_size, face_index, aface);
        
        [DllImport("freetype", EntryPoint = "FT_New_Memory_Face", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_New_Memory_Face64(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_New_Memory_Face64(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface) => INTERNAL_FT_New_Memory_Face64(library, file_base, file_size, face_index, aface);
        
        [DllImport("freetype", EntryPoint = "FT_Done_Face", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Done_Face(IntPtr face);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Done_Face(IntPtr face) => INTERNAL_FT_Done_Face(face);
        
        [DllImport("freetype", EntryPoint = "FT_Set_Char_Size", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Set_Char_Size32(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Set_Char_Size32(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => INTERNAL_FT_Set_Char_Size32(face, char_width, char_height, horz_resolution, vert_resolution);
        
        [DllImport("freetype", EntryPoint = "FT_Set_Char_Size", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Set_Char_Size64(IntPtr face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Set_Char_Size64(IntPtr face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => INTERNAL_FT_Set_Char_Size64(face, char_width, char_height, horz_resolution, vert_resolution);
        
        [DllImport("freetype", EntryPoint = "FT_Select_Size", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Select_Size(IntPtr face, Int32 strike_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Select_Size(IntPtr face, Int32 strike_index) => INTERNAL_FT_Select_Size(face, strike_index);
        
        [DllImport("freetype", EntryPoint = "FT_Get_Char_Index", CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 INTERNAL_FT_Get_Char_Index32(IntPtr face, UInt32 charcode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 FT_Get_Char_Index32(IntPtr face, UInt32 charcode) => INTERNAL_FT_Get_Char_Index32(face, charcode);
        
        [DllImport("freetype", EntryPoint = "FT_Get_Char_Index", CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 INTERNAL_FT_Get_Char_Index64(IntPtr face, UInt64 charcode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 FT_Get_Char_Index64(IntPtr face, UInt64 charcode) => INTERNAL_FT_Get_Char_Index64(face, charcode);
        
        [DllImport("freetype", EntryPoint = "FT_Load_Glyph", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Load_Glyph(IntPtr face, UInt32 glyph_index, Int32 load_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Load_Glyph(IntPtr face, UInt32 glyph_index, Int32 load_flags) => INTERNAL_FT_Load_Glyph(face, glyph_index, load_flags);
        
        [DllImport("freetype", EntryPoint = "FT_Get_Kerning", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Get_Kerning(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Get_Kerning(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning) => INTERNAL_FT_Get_Kerning(face, left_glyph, right_glyph, kern_mode, akerning);
        
        [DllImport("freetype", EntryPoint = "FT_Get_Glyph", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Get_Glyph(IntPtr slot, IntPtr aglyph);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Get_Glyph(IntPtr slot, IntPtr aglyph) => INTERNAL_FT_Get_Glyph(slot, aglyph);
        
        [DllImport("freetype", EntryPoint = "FT_Done_Glyph", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Done_Glyph(IntPtr glyph);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Done_Glyph(IntPtr glyph) => INTERNAL_FT_Done_Glyph(glyph);
        
        [DllImport("freetype", EntryPoint = "FT_Render_Glyph", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Render_Glyph(IntPtr slot, FT_Render_Mode render_mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Render_Glyph(IntPtr slot, FT_Render_Mode render_mode) => INTERNAL_FT_Render_Glyph(slot, render_mode);
        
        [DllImport("freetype", EntryPoint = "FT_Stroker_New", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Stroker_New(IntPtr library, IntPtr astroker);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Stroker_New(IntPtr library, IntPtr astroker) => INTERNAL_FT_Stroker_New(library, astroker);
        
        [DllImport("freetype", EntryPoint = "FT_Stroker_Done", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_FT_Stroker_Done(IntPtr stroker);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void FT_Stroker_Done(IntPtr stroker) => INTERNAL_FT_Stroker_Done(stroker);
        
        [DllImport("freetype", EntryPoint = "FT_Stroker_Set", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_FT_Stroker_Set32(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void FT_Stroker_Set32(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit) => INTERNAL_FT_Stroker_Set32(stroker, radius, line_cap, line_join, miter_limit);
        
        [DllImport("freetype", EntryPoint = "FT_Stroker_Set", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_FT_Stroker_Set64(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void FT_Stroker_Set64(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit) => INTERNAL_FT_Stroker_Set64(stroker, radius, line_cap, line_join, miter_limit);
        
        [DllImport("freetype", EntryPoint = "FT_Glyph_StrokeBorder", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Glyph_StrokeBorder(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Glyph_StrokeBorder(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy) => INTERNAL_FT_Glyph_StrokeBorder(pglyph, stroker, inside, destroy);
        
        [DllImport("freetype", EntryPoint = "FT_Glyph_To_Bitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern FT_Error INTERNAL_FT_Glyph_To_Bitmap(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Glyph_To_Bitmap(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy) => INTERNAL_FT_Glyph_To_Bitmap(the_glyph, render_mode, origin, destroy);
    }
#pragma warning restore 1591
}

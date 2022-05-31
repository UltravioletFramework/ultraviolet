using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    public static unsafe partial class FreeTypeNative
    {
        private static readonly FreeTypeNativeImpl impl;
        
        static FreeTypeNative()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Android:
                    impl = new FreeTypeNativeImpl_Android();
                    break;
                    
                default:
                    impl = new FreeTypeNativeImpl_Default();
                    break;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Init_FreeType(IntPtr alibrary) => impl.FT_Init_FreeType(alibrary);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Done_FreeType(IntPtr library) => impl.FT_Done_FreeType(library);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_New_Face32(IntPtr library, String filepathname, Int32 face_index, IntPtr aface) => impl.FT_New_Face32(library, filepathname, face_index, aface);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_New_Face64(IntPtr library, String filepathname, Int64 face_index, IntPtr aface) => impl.FT_New_Face64(library, filepathname, face_index, aface);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface) => impl.FT_New_Memory_Face32(library, file_base, file_size, face_index, aface);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_New_Memory_Face64(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface) => impl.FT_New_Memory_Face64(library, file_base, file_size, face_index, aface);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Done_Face(IntPtr face) => impl.FT_Done_Face(face);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Set_Char_Size32(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => impl.FT_Set_Char_Size32(face, char_width, char_height, horz_resolution, vert_resolution);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Set_Char_Size64(IntPtr face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => impl.FT_Set_Char_Size64(face, char_width, char_height, horz_resolution, vert_resolution);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Select_Size(IntPtr face, Int32 strike_index) => impl.FT_Select_Size(face, strike_index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 FT_Get_Char_Index32(IntPtr face, UInt32 charcode) => impl.FT_Get_Char_Index32(face, charcode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 FT_Get_Char_Index64(IntPtr face, UInt64 charcode) => impl.FT_Get_Char_Index64(face, charcode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Load_Glyph(IntPtr face, UInt32 glyph_index, Int32 load_flags) => impl.FT_Load_Glyph(face, glyph_index, load_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Get_Kerning(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning) => impl.FT_Get_Kerning(face, left_glyph, right_glyph, kern_mode, akerning);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Get_Glyph(IntPtr slot, IntPtr aglyph) => impl.FT_Get_Glyph(slot, aglyph);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Done_Glyph(IntPtr glyph) => impl.FT_Done_Glyph(glyph);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Render_Glyph(IntPtr slot, FT_Render_Mode render_mode) => impl.FT_Render_Glyph(slot, render_mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Stroker_New(IntPtr library, IntPtr astroker) => impl.FT_Stroker_New(library, astroker);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FT_Stroker_Done(IntPtr stroker) => impl.FT_Stroker_Done(stroker);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FT_Stroker_Set32(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit) => impl.FT_Stroker_Set32(stroker, radius, line_cap, line_join, miter_limit);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FT_Stroker_Set64(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit) => impl.FT_Stroker_Set64(stroker, radius, line_cap, line_join, miter_limit);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Glyph_StrokeBorder(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy) => impl.FT_Glyph_StrokeBorder(pglyph, stroker, inside, destroy);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FT_Error FT_Glyph_To_Bitmap(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy) => impl.FT_Glyph_To_Bitmap(the_glyph, render_mode, origin, destroy);
    }
#pragma warning restore 1591
}

using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class FreeTypeNativeImpl_Default : FreeTypeNativeImpl
    {
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Init_FreeTypeDelegate(IntPtr alibrary);
        private readonly FT_Init_FreeTypeDelegate pFT_Init_FreeType = SharedNativeLibraries.libfreetype.LoadFunction<FT_Init_FreeTypeDelegate>("FT_Init_FreeType");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Init_FreeType(IntPtr alibrary) => pFT_Init_FreeType(alibrary);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Done_FreeTypeDelegate(IntPtr library);
        private readonly FT_Done_FreeTypeDelegate pFT_Done_FreeType = SharedNativeLibraries.libfreetype.LoadFunction<FT_Done_FreeTypeDelegate>("FT_Done_FreeType");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Done_FreeType(IntPtr library) => pFT_Done_FreeType(library);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Face32Delegate(IntPtr library, String filepathname, Int32 face_index, IntPtr aface);
        private readonly FT_New_Face32Delegate pFT_New_Face32 = SharedNativeLibraries.libfreetype.LoadFunction<FT_New_Face32Delegate>("FT_New_Face");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_New_Face32(IntPtr library, String filepathname, Int32 face_index, IntPtr aface) => pFT_New_Face32(library, filepathname, face_index, aface);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Face64Delegate(IntPtr library, String filepathname, Int64 face_index, IntPtr aface);
        private readonly FT_New_Face64Delegate pFT_New_Face64 = SharedNativeLibraries.libfreetype.LoadFunction<FT_New_Face64Delegate>("FT_New_Face");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_New_Face64(IntPtr library, String filepathname, Int64 face_index, IntPtr aface) => pFT_New_Face64(library, filepathname, face_index, aface);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Memory_Face32Delegate(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface);
        private readonly FT_New_Memory_Face32Delegate pFT_New_Memory_Face32 = SharedNativeLibraries.libfreetype.LoadFunction<FT_New_Memory_Face32Delegate>("FT_New_Memory_Face");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface) => pFT_New_Memory_Face32(library, file_base, file_size, face_index, aface);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Memory_Face64Delegate(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface);
        private readonly FT_New_Memory_Face64Delegate pFT_New_Memory_Face64 = SharedNativeLibraries.libfreetype.LoadFunction<FT_New_Memory_Face64Delegate>("FT_New_Memory_Face");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_New_Memory_Face64(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface) => pFT_New_Memory_Face64(library, file_base, file_size, face_index, aface);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Done_FaceDelegate(IntPtr face);
        private readonly FT_Done_FaceDelegate pFT_Done_Face = SharedNativeLibraries.libfreetype.LoadFunction<FT_Done_FaceDelegate>("FT_Done_Face");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Done_Face(IntPtr face) => pFT_Done_Face(face);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Set_Char_Size32Delegate(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        private readonly FT_Set_Char_Size32Delegate pFT_Set_Char_Size32 = SharedNativeLibraries.libfreetype.LoadFunction<FT_Set_Char_Size32Delegate>("FT_Set_Char_Size");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Set_Char_Size32(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => pFT_Set_Char_Size32(face, char_width, char_height, horz_resolution, vert_resolution);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Set_Char_Size64Delegate(IntPtr face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        private readonly FT_Set_Char_Size64Delegate pFT_Set_Char_Size64 = SharedNativeLibraries.libfreetype.LoadFunction<FT_Set_Char_Size64Delegate>("FT_Set_Char_Size");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Set_Char_Size64(IntPtr face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => pFT_Set_Char_Size64(face, char_width, char_height, horz_resolution, vert_resolution);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Select_SizeDelegate(IntPtr face, Int32 strike_index);
        private readonly FT_Select_SizeDelegate pFT_Select_Size = SharedNativeLibraries.libfreetype.LoadFunction<FT_Select_SizeDelegate>("FT_Select_Size");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Select_Size(IntPtr face, Int32 strike_index) => pFT_Select_Size(face, strike_index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 FT_Get_Char_Index32Delegate(IntPtr face, UInt32 charcode);
        private readonly FT_Get_Char_Index32Delegate pFT_Get_Char_Index32 = SharedNativeLibraries.libfreetype.LoadFunction<FT_Get_Char_Index32Delegate>("FT_Get_Char_Index");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 FT_Get_Char_Index32(IntPtr face, UInt32 charcode) => pFT_Get_Char_Index32(face, charcode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 FT_Get_Char_Index64Delegate(IntPtr face, UInt64 charcode);
        private readonly FT_Get_Char_Index64Delegate pFT_Get_Char_Index64 = SharedNativeLibraries.libfreetype.LoadFunction<FT_Get_Char_Index64Delegate>("FT_Get_Char_Index");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 FT_Get_Char_Index64(IntPtr face, UInt64 charcode) => pFT_Get_Char_Index64(face, charcode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Load_GlyphDelegate(IntPtr face, UInt32 glyph_index, Int32 load_flags);
        private readonly FT_Load_GlyphDelegate pFT_Load_Glyph = SharedNativeLibraries.libfreetype.LoadFunction<FT_Load_GlyphDelegate>("FT_Load_Glyph");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Load_Glyph(IntPtr face, UInt32 glyph_index, Int32 load_flags) => pFT_Load_Glyph(face, glyph_index, load_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Get_KerningDelegate(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning);
        private readonly FT_Get_KerningDelegate pFT_Get_Kerning = SharedNativeLibraries.libfreetype.LoadFunction<FT_Get_KerningDelegate>("FT_Get_Kerning");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Get_Kerning(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning) => pFT_Get_Kerning(face, left_glyph, right_glyph, kern_mode, akerning);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Get_GlyphDelegate(IntPtr slot, IntPtr aglyph);
        private readonly FT_Get_GlyphDelegate pFT_Get_Glyph = SharedNativeLibraries.libfreetype.LoadFunction<FT_Get_GlyphDelegate>("FT_Get_Glyph");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Get_Glyph(IntPtr slot, IntPtr aglyph) => pFT_Get_Glyph(slot, aglyph);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Done_GlyphDelegate(IntPtr glyph);
        private readonly FT_Done_GlyphDelegate pFT_Done_Glyph = SharedNativeLibraries.libfreetype.LoadFunction<FT_Done_GlyphDelegate>("FT_Done_Glyph");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Done_Glyph(IntPtr glyph) => pFT_Done_Glyph(glyph);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Render_GlyphDelegate(IntPtr slot, FT_Render_Mode render_mode);
        private readonly FT_Render_GlyphDelegate pFT_Render_Glyph = SharedNativeLibraries.libfreetype.LoadFunction<FT_Render_GlyphDelegate>("FT_Render_Glyph");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Render_Glyph(IntPtr slot, FT_Render_Mode render_mode) => pFT_Render_Glyph(slot, render_mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Stroker_NewDelegate(IntPtr library, IntPtr astroker);
        private readonly FT_Stroker_NewDelegate pFT_Stroker_New = SharedNativeLibraries.libfreetype.LoadFunction<FT_Stroker_NewDelegate>("FT_Stroker_New");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Stroker_New(IntPtr library, IntPtr astroker) => pFT_Stroker_New(library, astroker);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FT_Stroker_DoneDelegate(IntPtr stroker);
        private readonly FT_Stroker_DoneDelegate pFT_Stroker_Done = SharedNativeLibraries.libfreetype.LoadFunction<FT_Stroker_DoneDelegate>("FT_Stroker_Done");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void FT_Stroker_Done(IntPtr stroker) => pFT_Stroker_Done(stroker);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FT_Stroker_Set32Delegate(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit);
        private readonly FT_Stroker_Set32Delegate pFT_Stroker_Set32 = SharedNativeLibraries.libfreetype.LoadFunction<FT_Stroker_Set32Delegate>("FT_Stroker_Set");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void FT_Stroker_Set32(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit) => pFT_Stroker_Set32(stroker, radius, line_cap, line_join, miter_limit);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FT_Stroker_Set64Delegate(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit);
        private readonly FT_Stroker_Set64Delegate pFT_Stroker_Set64 = SharedNativeLibraries.libfreetype.LoadFunction<FT_Stroker_Set64Delegate>("FT_Stroker_Set");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void FT_Stroker_Set64(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit) => pFT_Stroker_Set64(stroker, radius, line_cap, line_join, miter_limit);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Glyph_StrokeBorderDelegate(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy);
        private readonly FT_Glyph_StrokeBorderDelegate pFT_Glyph_StrokeBorder = SharedNativeLibraries.libfreetype.LoadFunction<FT_Glyph_StrokeBorderDelegate>("FT_Glyph_StrokeBorder");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Glyph_StrokeBorder(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy) => pFT_Glyph_StrokeBorder(pglyph, stroker, inside, destroy);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Glyph_To_BitmapDelegate(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy);
        private readonly FT_Glyph_To_BitmapDelegate pFT_Glyph_To_Bitmap = SharedNativeLibraries.libfreetype.LoadFunction<FT_Glyph_To_BitmapDelegate>("FT_Glyph_To_Bitmap");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FT_Error FT_Glyph_To_Bitmap(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy) => pFT_Glyph_To_Bitmap(the_glyph, render_mode, origin, destroy);
    }
#pragma warning restore 1591
}

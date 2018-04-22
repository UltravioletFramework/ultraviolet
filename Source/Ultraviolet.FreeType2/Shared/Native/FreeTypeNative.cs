using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    public static unsafe partial class FreeTypeNative
    {
        public static Boolean Use64BitInterface => UltravioletPlatformInfo.CurrentPlatform != UltravioletPlatform.Windows && Environment.Is64BitProcess;

#if ANDROID
        const String LIBRARY = "freetype";
#elif IOS
        const String LIBRARY = "__Internal";
#else
        private static readonly NativeLibrary lib = new NativeLibrary(
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "freetype" : "libfreetype");
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Init_FreeType", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Init_FreeType(IntPtr alibrary);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Init_FreeTypeDelegate(IntPtr alibrary);
        private static readonly FT_Init_FreeTypeDelegate pFT_Init_FreeType = lib.LoadFunction<FT_Init_FreeTypeDelegate>("FT_Init_FreeType");
        public static FT_Error FT_Init_FreeType(IntPtr alibrary) => pFT_Init_FreeType(alibrary);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Done_FreeType", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Done_FreeType(IntPtr library);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Done_FreeTypeDelegate(IntPtr library);
        private static readonly FT_Done_FreeTypeDelegate pFT_Done_FreeType = lib.LoadFunction<FT_Done_FreeTypeDelegate>("FT_Done_FreeType");
        public static FT_Error FT_Done_FreeType(IntPtr library) => pFT_Done_FreeType(library);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Face32(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int32 face_index, IntPtr aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Face32Delegate(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int32 face_index, IntPtr aface);
        private static readonly FT_New_Face32Delegate pFT_New_Face32 = lib.LoadFunction<FT_New_Face32Delegate>("FT_New_Face");
        public static FT_Error FT_New_Face32(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int32 face_index, IntPtr aface) => pFT_New_Face32(library, filepathname, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Face64(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, IntPtr aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Face64Delegate(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, IntPtr aface);
        private static readonly FT_New_Face64Delegate pFT_New_Face64 = lib.LoadFunction<FT_New_Face64Delegate>("FT_New_Face");
        public static FT_Error FT_New_Face64(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, IntPtr aface) => pFT_New_Face64(library, filepathname, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Memory_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Memory_Face32Delegate(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface);
        private static readonly FT_New_Memory_Face32Delegate pFT_New_Memory_Face32 = lib.LoadFunction<FT_New_Memory_Face32Delegate>("FT_New_Memory_Face");
        public static FT_Error FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface) => pFT_New_Memory_Face32(library, file_base, file_size, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Memory_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Memory_Face64(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Memory_Face64Delegate(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface);
        private static readonly FT_New_Memory_Face64Delegate pFT_New_Memory_Face64 = lib.LoadFunction<FT_New_Memory_Face64Delegate>("FT_New_Memory_Face");
        public static FT_Error FT_New_Memory_Face64(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, IntPtr aface) => pFT_New_Memory_Face64(library, file_base, file_size, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Done_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Done_Face(IntPtr face);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Done_FaceDelegate(IntPtr face);
        private static readonly FT_Done_FaceDelegate pFT_Done_Face = lib.LoadFunction<FT_Done_FaceDelegate>("FT_Done_Face");
        public static FT_Error FT_Done_Face(IntPtr face) => pFT_Done_Face(face);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Set_Char_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Set_Char_Size32(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Set_Char_Size32Delegate(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        private static readonly FT_Set_Char_Size32Delegate pFT_Set_Char_Size32 = lib.LoadFunction<FT_Set_Char_Size32Delegate>("FT_Set_Char_Size");
        public static FT_Error FT_Set_Char_Size32(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => pFT_Set_Char_Size32(face, char_width, char_height, horz_resolution, vert_resolution);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Set_Char_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Set_Char_Size64(IntPtr face, Int32 char_width, Int32 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Set_Char_Size64Delegate(IntPtr face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        private static readonly FT_Set_Char_Size64Delegate pFT_Set_Char_Size64 = lib.LoadFunction<FT_Set_Char_Size64Delegate>("FT_Set_Char_Size");
        public static FT_Error FT_Set_Char_Size64(IntPtr face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => pFT_Set_Char_Size64(face, char_width, char_height, horz_resolution, vert_resolution);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Select_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Select_Size(IntPtr face, Int32 strike_index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Select_SizeDelegate(IntPtr face, Int32 strike_index);
        private static readonly FT_Select_SizeDelegate pFT_Select_Size = lib.LoadFunction<FT_Select_SizeDelegate>("FT_Select_Size");
        public static FT_Error FT_Select_Size(IntPtr face, Int32 strike_index) => pFT_Select_Size(face, strike_index);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Get_Char_Index", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 FT_Get_Char_Index32(IntPtr face, UInt32 charcode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 FT_Get_Char_Index32Delegate(IntPtr face, UInt32 charcode);
        private static readonly FT_Get_Char_Index32Delegate pFT_Get_Char_Index32 = lib.LoadFunction<FT_Get_Char_Index32Delegate>("FT_Get_Char_Index");
        public static UInt32 FT_Get_Char_Index32(IntPtr face, UInt32 charcode) => pFT_Get_Char_Index32(face, charcode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Get_Char_Index", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 FT_Get_Char_Index64(IntPtr face, UInt64 charcode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 FT_Get_Char_Index64Delegate(IntPtr face, UInt64 charcode);
        private static readonly FT_Get_Char_Index64Delegate pFT_Get_Char_Index64 = lib.LoadFunction<FT_Get_Char_Index64Delegate>("FT_Get_Char_Index");
        public static UInt32 FT_Get_Char_Index64(IntPtr face, UInt64 charcode) => pFT_Get_Char_Index64(face, charcode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Load_Glyph", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Load_Glyph(IntPtr face, UInt32 glyph_index, Int32 load_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Load_GlyphDelegate(IntPtr face, UInt32 glyph_index, Int32 load_flags);
        private static readonly FT_Load_GlyphDelegate pFT_Load_Glyph = lib.LoadFunction<FT_Load_GlyphDelegate>("FT_Load_Glyph");
        public static FT_Error FT_Load_Glyph(IntPtr face, UInt32 glyph_index, Int32 load_flags) => pFT_Load_Glyph(face, glyph_index, load_flags);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Get_Kerning", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_Kerning(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Get_KerningDelegate(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning);
        private static readonly FT_Get_KerningDelegate pFT_Get_Kerning = lib.LoadFunction<FT_Get_KerningDelegate>("FT_Get_Kerning");
        public static FT_Error FT_Get_Kerning(IntPtr face, UInt32 left_glyph, UInt32 right_glyph, UInt32 kern_mode, IntPtr akerning) => pFT_Get_Kerning(face, left_glyph, right_glyph, kern_mode, akerning);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Get_Glyph", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Get_Glyph(IntPtr slot, IntPtr aglyph);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Get_GlyphDelegate(IntPtr slot, IntPtr aglyph);
        private static readonly FT_Get_GlyphDelegate pFT_Get_Glyph = lib.LoadFunction<FT_Get_GlyphDelegate>("FT_Get_Glyph");
        public static FT_Error FT_Get_Glyph(IntPtr slot, IntPtr aglyph) => pFT_Get_Glyph(slot, aglyph);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Done_Glyph", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Done_Glyph(IntPtr glyph);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FT_Done_GlyphDelegate(IntPtr glyph);
        private static readonly FT_Done_GlyphDelegate pFT_Done_Glyph = lib.LoadFunction<FT_Done_GlyphDelegate>("FT_Done_Glyph");
        public static void FT_Done_Glyph(IntPtr glyph) => pFT_Done_Glyph(glyph);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Render_Glyph", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Render_Glyph(IntPtr slot, FT_Render_Mode render_mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Render_GlyphDelegate(IntPtr slot, FT_Render_Mode render_mode);
        private static readonly FT_Render_GlyphDelegate pFT_Render_Glyph = lib.LoadFunction<FT_Render_GlyphDelegate>("FT_Render_Glyph");
        public static FT_Error FT_Render_Glyph(IntPtr slot, FT_Render_Mode render_mode) => pFT_Render_Glyph(slot, render_mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Stroker_New", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Stroker_New(IntPtr library, IntPtr astroker);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Stroker_NewDelegate(IntPtr library, IntPtr astroker);
        private static readonly FT_Stroker_NewDelegate pFT_Stroker_New = lib.LoadFunction<FT_Stroker_NewDelegate>("FT_Stroker_New");
        public static FT_Error FT_Stroker_New(IntPtr library, IntPtr astroker) => pFT_Stroker_New(library, astroker);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Stroker_Done", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Stroker_Done(IntPtr stroker);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FT_Stroker_DoneDelegate(IntPtr stroker);
        private static readonly FT_Stroker_DoneDelegate pFT_Stroker_Done = lib.LoadFunction<FT_Stroker_DoneDelegate>("FT_Stroker_Done");
        public static void FT_Stroker_Done(IntPtr stroker) => pFT_Stroker_Done(stroker);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Stroker_Set", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Stroker_Set32(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FT_Stroker_Set32Delegate(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit);
        private static readonly FT_Stroker_Set32Delegate pFT_Stroker_Set32 = lib.LoadFunction<FT_Stroker_Set32Delegate>("FT_Stroker_Set");
        public static void FT_Stroker_Set32(IntPtr stroker, Int32 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int32 miter_limit) => pFT_Stroker_Set32(stroker, radius, line_cap, line_join, miter_limit);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Stroker_Set", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Stroker_Set64(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FT_Stroker_Set64Delegate(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit);
        private static readonly FT_Stroker_Set64Delegate pFT_Stroker_Set64 = lib.LoadFunction<FT_Stroker_Set64Delegate>("FT_Stroker_Set");
        public static void FT_Stroker_Set64(IntPtr stroker, Int64 radius, FT_Stroker_LineCap line_cap, FT_Stroker_LineJoin line_join, Int64 miter_limit) => pFT_Stroker_Set64(stroker, radius, line_cap, line_join, miter_limit);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Glyph_StrokeBorder", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Glyph_StrokeBorder(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Glyph_StrokeBorderDelegate(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy);
        private static readonly FT_Glyph_StrokeBorderDelegate pFT_Glyph_StrokeBorder = lib.LoadFunction<FT_Glyph_StrokeBorderDelegate>("FT_Glyph_StrokeBorder");
        public static FT_Error FT_Glyph_StrokeBorder(IntPtr pglyph, IntPtr stroker, Boolean inside, Boolean destroy) => pFT_Glyph_StrokeBorder(pglyph, stroker, inside, destroy);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Glyph_To_Bitmap", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Glyph_To_Bitmap(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Glyph_To_BitmapDelegate(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy);
        private static readonly FT_Glyph_To_BitmapDelegate pFT_Glyph_To_Bitmap = lib.LoadFunction<FT_Glyph_To_BitmapDelegate>("FT_Glyph_To_Bitmap");
        public static FT_Error FT_Glyph_To_Bitmap(IntPtr the_glyph, FT_Render_Mode render_mode, IntPtr origin, Boolean destroy) => pFT_Glyph_To_Bitmap(the_glyph, render_mode, origin, destroy);
#endif
    }
#pragma warning restore 1591
}

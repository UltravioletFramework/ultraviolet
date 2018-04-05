using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

using FT_F26Dot6 = System.Int32;
using FT_Int32 = System.Int32;
using FT_Long = System.Int32;
using FT_UInt = System.UInt32;
using FT_ULong = System.UInt32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    public static unsafe partial class FreeTypeNative
    {
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
        public static extern FT_Error FT_Init_FreeType(FT_LibraryRec_** alibrary);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Init_FreeTypeDelegate(FT_LibraryRec** alibrary);
        private static readonly FT_Init_FreeTypeDelegate pFT_Init_FreeType = lib.LoadFunction<FT_Init_FreeTypeDelegate>("FT_Init_FreeType");
        public static FT_Error FT_Init_FreeType(FT_LibraryRec** alibrary) => pFT_Init_FreeType(alibrary);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Done_FreeType", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Done_FreeType(FT_LibraryRec_* library);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Done_FreeTypeDelegate(FT_LibraryRec* library);
        private static readonly FT_Done_FreeTypeDelegate pFT_Done_FreeType = lib.LoadFunction<FT_Done_FreeTypeDelegate>("FT_Done_FreeType");
        public static FT_Error FT_Done_FreeType(FT_LibraryRec* library) => pFT_Done_FreeType(library);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Face(FT_LibraryRec_* library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, FT_Long face_index, FT_FaceRec_** aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_FaceDelegate(FT_LibraryRec* library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, FT_Long face_index, FT_FaceRec** aface);
        private static readonly FT_New_FaceDelegate pFT_New_Face = lib.LoadFunction<FT_New_FaceDelegate>("FT_New_Face");
        public static FT_Error FT_New_Face(FT_LibraryRec* library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, FT_Long face_index, FT_FaceRec** aface) => pFT_New_Face(library, filepathname, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Memory_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Memory_Face(FT_LibraryRec_* library, IntPtr file_base, FT_Long file_size, FT_Long face_index, FT_FaceRec_** aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Memory_FaceDelegate(FT_LibraryRec* library, IntPtr file_base, FT_Long file_size, FT_Long face_index, FT_FaceRec** aface);
        private static readonly FT_New_Memory_FaceDelegate pFT_New_Memory_Face = lib.LoadFunction<FT_New_Memory_FaceDelegate>("FT_New_Memory_Face");
        public static FT_Error FT_New_Memory_Face(FT_LibraryRec* library, IntPtr file_base, FT_Long file_size, FT_Long face_index, FT_FaceRec** aface) => pFT_New_Memory_Face(library, file_base, file_size, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Done_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Done_Face(FT_FaceRec_* face);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Done_FaceDelegate(FT_FaceRec* face);
        private static readonly FT_Done_FaceDelegate pFT_Done_Face = lib.LoadFunction<FT_Done_FaceDelegate>("FT_Done_Face");
        public static FT_Error FT_Done_Face(FT_FaceRec* face) => pFT_Done_Face(face);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Set_Char_Size", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Set_Char_Size(FT_FaceRec_* face, FT_F26Dot6 char_width, FT_F26Dot6 char_height, FT_UInt horz_resolution, FT_UInt vert_resolution);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Set_Char_SizeDelegate(FT_FaceRec* face, FT_F26Dot6 char_width, FT_F26Dot6 char_height, FT_UInt horz_resolution, FT_UInt vert_resolution);
        private static readonly FT_Set_Char_SizeDelegate pFT_Set_Char_Size = lib.LoadFunction<FT_Set_Char_SizeDelegate>("FT_Set_Char_Size");
        public static FT_Error FT_Set_Char_Size(FT_FaceRec* face, FT_F26Dot6 char_width, FT_F26Dot6 char_height, FT_UInt horz_resolution, FT_UInt vert_resolution) => pFT_Set_Char_Size(face, char_width, char_height, horz_resolution, vert_resolution);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Get_Char_Index", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_UInt FT_Get_Char_Index(FT_FaceRec* face, FT_ULong charcode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_UInt FT_Get_Char_IndexDelegate(FT_FaceRec* face, FT_ULong charcode);
        private static readonly FT_Get_Char_IndexDelegate pFT_Get_Char_Index = lib.LoadFunction<FT_Get_Char_IndexDelegate>("FT_Get_Char_Index");
        public static FT_UInt FT_Get_Char_Index(FT_FaceRec* face, FT_ULong charcode) => pFT_Get_Char_Index(face, charcode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Load_Glyph", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Load_Glyph(FT_FaceRec* face, FT_UInt glyph_index, FT_Int32 load_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_Load_GlyphDelegate(FT_FaceRec* face, FT_UInt glyph_index, FT_Int32 load_flags);
        private static readonly FT_Load_GlyphDelegate pFT_Load_Glyph = lib.LoadFunction<FT_Load_GlyphDelegate>("FT_Load_Glyph");
        public static FT_Error FT_Load_Glyph(FT_FaceRec* face, FT_UInt glyph_index, FT_Int32 load_flags) => pFT_Load_Glyph(face, glyph_index, load_flags);
#endif
    }
#pragma warning restore 1591
}

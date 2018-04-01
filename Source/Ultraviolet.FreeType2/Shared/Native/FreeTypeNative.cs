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

        public const Int32 FT_LOAD_DEFAULT = 0x0;
        public const Int32 FT_LOAD_NO_SCALE = (1 << 0);
        public const Int32 FT_LOAD_NO_HINTING = (1 << 1);
        public const Int32 FT_LOAD_RENDER = (1 << 2);
        public const Int32 FT_LOAD_NO_BITMAP = (1 << 3);
        public const Int32 FT_LOAD_VERTICAL_LAYOUT = (1 << 4);
        public const Int32 FT_LOAD_FORCE_AUTOHINT = (1 << 5);
        public const Int32 FT_LOAD_CROP_BITMAP = (1 << 6);
        public const Int32 FT_LOAD_PEDANTIC = (1 << 7);
        public const Int32 FT_LOAD_IGNORE_GLOBAL_ADVANCE_WIDTH = (1 << 9);
        public const Int32 FT_LOAD_NO_RECURSE = (1 << 10);
        public const Int32 FT_LOAD_IGNORE_TRANSFORM = (1 << 11);
        public const Int32 FT_LOAD_MONOCHROME = (1 << 12);
        public const Int32 FT_LOAD_LINEAR_DESIGN = (1 << 13);
        public const Int32 FT_LOAD_NO_AUTOHINT = (1 << 15);
        public const Int32 FT_LOAD_COLOR = (1 << 20);
        public const Int32 FT_LOAD_COMPUTE_METRICS = (1 << 21);
        public const Int32 FT_LOAD_BITMAP_METRICS_ONLY = (1 << 22);
        public const Int32 FT_LOAD_ADVANCE_ONLY = (1 << 8);
        public const Int32 FT_LOAD_SBITS_ONLY = (1 << 14);

        public const Int32 FT_FACE_FLAG_SCALABLE = 1 << 0;
        public const Int32 FT_FACE_FLAG_FIXED_SIZES = 1 << 1;
        public const Int32 FT_FACE_FLAG_FIXED_WIDTH = 1 << 2;
        public const Int32 FT_FACE_FLAG_SFNT = 1 << 3;
        public const Int32 FT_FACE_FLAG_HORIZONTAL = 1 << 4;
        public const Int32 FT_FACE_FLAG_VERTICAL = 1 << 5;
        public const Int32 FT_FACE_FLAG_KERNING = 1 << 6;
        public const Int32 FT_FACE_FLAG_FAST_GLYPHS = 1 << 7;
        public const Int32 FT_FACE_FLAG_MULTIPLE_MASTERS = 1 << 8;
        public const Int32 FT_FACE_FLAG_GLYPH_NAMES = 1 << 9;
        public const Int32 FT_FACE_FLAG_EXTERNAL_STREAM = 1 << 10;
        public const Int32 FT_FACE_FLAG_HINTER = 1 << 11;
        public const Int32 FT_FACE_FLAG_CID_KEYED = 1 << 12;
        public const Int32 FT_FACE_FLAG_TRICKY = 1 << 13;
        public const Int32 FT_FACE_FLAG_COLOR = 1 << 14;
        public const Int32 FT_FACE_FLAG_VARIATION = 1 << 15;

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

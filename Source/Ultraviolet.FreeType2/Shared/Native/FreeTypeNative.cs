using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

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
        [DllImport(LIBRARY, EntryPoint="FT_Init_FreeType", CallingConvention = CallingConvention.StdCall)]
        public static extern FT_Err FT_Init_FreeType(FT_LibraryRec_** alibrary);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FT_Err FT_Init_FreeTypeDelegate(FT_LibraryRec_** alibrary);
        private static readonly FT_Init_FreeTypeDelegate pFT_Init_FreeType = lib.LoadFunction<FT_Init_FreeTypeDelegate>("FT_Init_FreeType");
        public static FT_Err FT_Init_FreeType(FT_LibraryRec_** alibrary) => pFT_Init_FreeType(alibrary);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Done_FreeType", CallingConvention = CallingConvention.StdCall)]
        public static extern FT_Err FT_Done_FreeType(FT_LibraryRec_* library);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FT_Err FT_Done_FreeTypeDelegate(FT_LibraryRec_* library);
        private static readonly FT_Done_FreeTypeDelegate pFT_Done_FreeType = lib.LoadFunction<FT_Done_FreeTypeDelegate>("FT_Done_FreeType");
        public static FT_Err FT_Done_FreeType(FT_LibraryRec_* library) => pFT_Done_FreeType(library);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Face", CallingConvention = CallingConvention.StdCall)]
        public static extern FT_Err FT_New_Face(FT_LibraryRec_* library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, FT_FaceRec_** aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FT_Err FT_New_FaceDelegate(FT_LibraryRec_* library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, FT_FaceRec_** aface);
        private static readonly FT_New_FaceDelegate pFT_New_Face = lib.LoadFunction<FT_New_FaceDelegate>("FT_New_Face");
        public static FT_Err FT_New_Face(FT_LibraryRec_* library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, FT_FaceRec_** aface) => pFT_New_Face(library, filepathname, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Memory_Face", CallingConvention = CallingConvention.StdCall)]
        public static extern FT_Err FT_New_Memory_Face(FT_LibraryRec_* library, IntPtr file_base, Int64 file_size, Int64 face_index, FT_FaceRec_** aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FT_Err FT_New_Memory_FaceDelegate(FT_LibraryRec_* library, IntPtr file_base, Int64 file_size, Int64 face_index, FT_FaceRec_** aface);
        private static readonly FT_New_Memory_FaceDelegate pFT_New_Memory_Face = lib.LoadFunction<FT_New_Memory_FaceDelegate>("FT_New_Memory_Face");
        public static FT_Err FT_New_Memory_Face(FT_LibraryRec_* library, IntPtr file_base, Int64 file_size, Int64 face_index, FT_FaceRec_** aface) => pFT_New_Memory_Face(library, file_base, file_size, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Done_Face", CallingConvention = CallingConvention.StdCall)]
        public static extern FT_Err FT_Done_Face(FT_FaceRec_* face);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FT_Err FT_Done_FaceDelegate(FT_FaceRec_* face);
        private static readonly FT_Done_FaceDelegate pFT_Done_Face = lib.LoadFunction<FT_Done_FaceDelegate>("FT_Done_Face");
        public static FT_Err FT_Done_Face(FT_FaceRec_* face) => pFT_Done_Face(face);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_Set_Char_Size", CallingConvention = CallingConvention.StdCall)]
        public static extern FT_Err FT_Set_Char_Size(FT_FaceRec_* face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FT_Err FT_Set_Char_SizeDelegate(FT_FaceRec_* face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution);
        private static readonly FT_Set_Char_SizeDelegate pFT_Set_Char_Size = lib.LoadFunction<FT_Set_Char_SizeDelegate>("FT_Set_Char_Size");
        public static FT_Err FT_Set_Char_Size(FT_FaceRec_* face, Int64 char_width, Int64 char_height, UInt32 horz_resolution, UInt32 vert_resolution) => pFT_Set_Char_Size(face, char_width, char_height, horz_resolution, vert_resolution);
#endif
    }
#pragma warning restore 1591
}

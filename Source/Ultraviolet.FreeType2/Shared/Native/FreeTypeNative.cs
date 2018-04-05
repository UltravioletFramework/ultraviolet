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
        public static extern FT_Error FT_New_Face32(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int32 face_index, FT_FaceRec* aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Face32Delegate(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int32 face_index, IntPtr aface);
        private static readonly FT_New_Face32Delegate pFT_New_Face32 = lib.LoadFunction<FT_New_Face32Delegate>("FT_New_Face");
        public static FT_Error FT_New_Face32(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int32 face_index, IntPtr aface) => pFT_New_Face32(library, filepathname, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Face64(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, FT_FaceRec* aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Face64Delegate(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, IntPtr aface);
        private static readonly FT_New_Face64Delegate pFT_New_Face64 = lib.LoadFunction<FT_New_Face64Delegate>("FT_New_Face");
        public static FT_Error FT_New_Face64(IntPtr library, [MarshalAs(UnmanagedType.LPStr)] String filepathname, Int64 face_index, IntPtr aface) => pFT_New_Face64(library, filepathname, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Memory_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, FT_FaceRec* aface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate FT_Error FT_New_Memory_Face32Delegate(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface);
        private static readonly FT_New_Memory_Face32Delegate pFT_New_Memory_Face32 = lib.LoadFunction<FT_New_Memory_Face32Delegate>("FT_New_Memory_Face");
        public static FT_Error FT_New_Memory_Face32(IntPtr library, IntPtr file_base, Int32 file_size, Int32 face_index, IntPtr aface) => pFT_New_Memory_Face32(library, file_base, file_size, face_index, aface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FT_New_Memory_Face", CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_New_Memory_Face64(IntPtr library, IntPtr file_base, Int64 file_size, Int64 face_index, FT_FaceRec* aface);
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
    }
#pragma warning restore 1591
}

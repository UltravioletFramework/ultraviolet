using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphRec64
    {
        public FT_LibraryRec* library;
        public IntPtr clazz;
        public FT_Glyph_Format format;
        public FT_Vector64 advance;
    }
#pragma warning restore 1591
}

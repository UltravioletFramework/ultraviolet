using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_SubGlyphRec32
    {
        public Int32 index;
        public UInt16 flags;
        public Int32 arg1;
        public Int32 arg2;
        public FT_Matrix32 transform;
    }
#pragma warning restore 1591
}

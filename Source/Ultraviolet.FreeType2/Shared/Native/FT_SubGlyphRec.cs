using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Int = System.Int32;
using FT_UShort = System.UInt16;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_SubGlyphRec
    {
        public FT_Int index;
        public FT_UShort flags;
        public FT_Int arg1;
        public FT_Int arg2;
        public FT_Matrix transform;
    }
#pragma warning restore 1591
}

using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Fixed = System.Int32;
using FT_Pos = System.Int32;
using FT_UShort = System.UInt16;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Size_Metrics
    {
        public FT_UShort x_ppem;
        public FT_UShort y_ppem;

        public FT_Fixed x_scale;
        public FT_Fixed y_scale;

        public FT_Pos ascender;
        public FT_Pos descender;
        public FT_Pos height;
        public FT_Pos max_advance;
    }
#pragma warning restore 1591
}

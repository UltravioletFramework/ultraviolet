using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Pos = System.Int32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Glyph_Metrics
    {
        public FT_Pos width;
        public FT_Pos height;

        public FT_Pos horiBearingX;
        public FT_Pos horiBearingY;
        public FT_Pos horiAdvance;

        public FT_Pos vertBearingX;
        public FT_Pos vertBearingY;
        public FT_Pos vertAdvance;
    }
#pragma warning restore 1591
}

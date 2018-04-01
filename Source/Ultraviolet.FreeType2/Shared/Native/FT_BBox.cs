using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Pos = System.Int32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_BBox
    {
        public FT_Pos xMin, yMin;
        public FT_Pos xMax, yMax;
    }
#pragma warning restore 1591
}

using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Pos = System.Int32;
using FT_Short = System.Int16;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Bitmap_Size
    {
        public FT_Short height;
        public FT_Short width;

        public FT_Pos size;

        public FT_Pos x_ppem;
        public FT_Pos y_ppem;
    }
#pragma warning restore 1591
}

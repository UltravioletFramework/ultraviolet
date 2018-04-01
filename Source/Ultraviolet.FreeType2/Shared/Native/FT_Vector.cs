using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Pos = System.Int32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Vector
    {
        public FT_Pos x;
        public FT_Pos y;
    }
#pragma warning restore 1591
}

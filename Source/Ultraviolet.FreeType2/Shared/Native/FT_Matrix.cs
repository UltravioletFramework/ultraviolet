using System.Runtime.InteropServices;
using Ultraviolet.Core;

using FT_Fixed = System.Int32;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Matrix
    {
        public FT_Fixed xx, xy;
        public FT_Fixed yx, yy;
    }
#pragma warning restore 1591
}

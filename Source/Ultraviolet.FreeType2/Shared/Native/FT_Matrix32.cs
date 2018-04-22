using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Matrix32
    {
        public Int32 xx, xy;
        public Int32 yx, yy;
    }
#pragma warning restore 1591
}

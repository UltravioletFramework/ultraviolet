using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_BBox64
    {
        public Int64 xMin, yMin;
        public Int64 xMax, yMax;
    }
#pragma warning restore 1591
}

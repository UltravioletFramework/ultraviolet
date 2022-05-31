using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_SizeRec32
    {
        public FT_FaceRec32* face;
        public FT_Generic generic;
        public FT_Size_Metrics32 metrics;
        public IntPtr @internal;
    }
#pragma warning restore 1591
}

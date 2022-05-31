using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Bitmap_Size64
    {
        public Int16 height;
        public Int16 width;

        public Int64 size;

        public Int64 x_ppem;
        public Int64 y_ppem;
    }
#pragma warning restore 1591
}

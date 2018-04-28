using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Bitmap_Size32
    {
        public Int16 height;
        public Int16 width;

        public Int32 size;

        public Int32 x_ppem;
        public Int32 y_ppem;
    }
#pragma warning restore 1591
}

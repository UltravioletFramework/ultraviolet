using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Bitmap
    {
        public UInt32 rows;
        public UInt32 width;
        public Int32 pitch;
        public IntPtr buffer;
        public UInt16 num_grays;
        public Byte pixel_mode;
        public Byte palette_mode;
        public IntPtr palette;
    }
#pragma warning restore 1591
}

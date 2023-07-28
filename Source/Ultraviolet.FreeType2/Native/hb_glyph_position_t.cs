using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct hb_glyph_position_t
    {
        public Int32 x_advance;
        public Int32 y_advance;
        public Int32 x_offset;
        public Int32 y_offset;

        private UInt32 var;
    }
#pragma warning restore 1591
}

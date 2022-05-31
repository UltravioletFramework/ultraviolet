using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct hb_glyph_info_t
    {
        public UInt32 codepoint;
        public UInt32 mask;
        public UInt32 cluster;

        private UInt32 var1;
        private UInt32 var2;
    }
#pragma warning restore 1591
}

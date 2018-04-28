using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_ListNodeRec
    {
        public FT_ListNodeRec* prev;
        public FT_ListNodeRec* next;
        public IntPtr data;
    }
#pragma warning restore 1591
}

using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_ListRec
    {
        public FT_ListNodeRec* head;
        public FT_ListNodeRec* tail;
    }
#pragma warning restore 1591
}

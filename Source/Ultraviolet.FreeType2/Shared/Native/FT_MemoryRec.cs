using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate IntPtr FT_Alloc_Func(FT_MemoryRec* memory, Int64 size);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate IntPtr FT_Free_Func(FT_MemoryRec* memory, IntPtr block);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate IntPtr FT_Realloc_Func(FT_MemoryRec* memory, Int64 cur_size, Int64 new_size, IntPtr block);

    [Preserve(AllMembers = true)]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_MemoryRec
    {
        public IntPtr user;
        public IntPtr alloc;
        public IntPtr free;
        public IntPtr realloc;
    }
#pragma warning restore 1591
}

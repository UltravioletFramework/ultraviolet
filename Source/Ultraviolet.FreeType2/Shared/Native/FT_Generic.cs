using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate void FT_Generic_Finalizer(IntPtr @object);
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Generic
    {
        public IntPtr data;
        public IntPtr finalizer;
    }
#pragma warning restore 1591
}

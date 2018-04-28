using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FMOD_ASYNCREADINFO
    {
        void* handle;
        UInt32 offset;
        UInt32 sizebytes;
        Int32 priority;
        void* userdata;
        void* buffer;
        UInt32 bytesread;
        IntPtr done;
    }
#pragma warning restore 1591
}

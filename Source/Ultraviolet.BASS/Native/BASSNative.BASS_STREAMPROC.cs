using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    public delegate UInt32 StreamProc(UInt32 handle, IntPtr buffer, UInt32 length, IntPtr user);

    static partial class BASSNative
    {
        public const UInt32 BASS_STREAMPROC_END = 0x80000000;

        // special STREAMPROCs
        public static readonly IntPtr STREAMPROC_DUMMY  = new IntPtr(0);
        public static readonly IntPtr STREAMPROC_PUSH   = new IntPtr(-1);
        public static readonly IntPtr STREAMPROC_DEVICE = new IntPtr(-2);
    }
#pragma warning restore 1591
}

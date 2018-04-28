using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct BASS_CHANNELINFO
    {
        public UInt32 freq;
        public UInt32 chans;
        public UInt32 flags;
        public UInt32 ctype;
        public UInt32 origres;
        public UInt32 plugin;
        public UInt32 sample;
        public IntPtr filename;
    }
#pragma warning restore 1591
}

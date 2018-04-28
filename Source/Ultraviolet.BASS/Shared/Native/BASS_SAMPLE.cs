using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public struct BASS_SAMPLE
    {
        public UInt32 freq;
        public Single volume;
        public Single pan;
        public UInt32 flags;
        public UInt32 length;
        public UInt32 max;
        public UInt32 origres;
        public UInt32 chans;
        public UInt32 mingap;
        public UInt32 mode3d;
        public Single mindist;
        public Single maxidst;
        public UInt32 iangle;
        public UInt32 oangle;
        public Single outvol;
        public UInt32 vam;
        public UInt32 priority;
    }
#pragma warning restore 1591
}
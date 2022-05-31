using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    public delegate void SyncProc(UInt32 handle, UInt32 channel, UInt32 data, IntPtr user);

    static partial class BASSNative
    {
        public const UInt32 BASS_SYNC_POS			= 0;
        public const UInt32 BASS_SYNC_END			= 2;
        public const UInt32 BASS_SYNC_META			= 4;
        public const UInt32 BASS_SYNC_SLIDE			= 5;
        public const UInt32 BASS_SYNC_STALL			= 6;
        public const UInt32 BASS_SYNC_DOWNLOAD		= 7;
        public const UInt32 BASS_SYNC_FREE			= 8;
        public const UInt32 BASS_SYNC_SETPOS		= 11;
        public const UInt32 BASS_SYNC_MUSICPOS		= 10;
        public const UInt32 BASS_SYNC_MUSICINST     = 1;
        public const UInt32 BASS_SYNC_MUSICFX		= 3;
        public const UInt32 BASS_SYNC_OGG_CHANGE	= 12;
        public const UInt32 BASS_SYNC_MIXTIME		= 0x40000000; // flag: sync at mixtime, else at playtime
        public const UInt32 BASS_SYNC_ONETIME		= 0x80000000; // flag: sync only once, else continuously
    }
#pragma warning restore 1591
}

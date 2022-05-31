using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    static partial class BASSNative
    {
        public const UInt32 BASS_ACTIVE_STOPPED = 0;
        public const UInt32 BASS_ACTIVE_PLAYING = 1;
        public const UInt32 BASS_ACTIVE_STALLED = 2;
        public const UInt32 BASS_ACTIVE_PAUSED  = 3;
    }
#pragma warning restore 1591
}

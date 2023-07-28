using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    static partial class BASSNative
    {
        public const UInt32 BASS_SAMPLE_8BITS       = 1;        // 8 bit
        public const UInt32 BASS_SAMPLE_FLOAT       = 256;      // 32 bit floating-point
        public const UInt32 BASS_SAMPLE_MONO        = 2;        // mono
        public const UInt32 BASS_SAMPLE_LOOP        = 4;        // looped
        public const UInt32 BASS_SAMPLE_3D          = 8;        // 3D functionality
        public const UInt32 BASS_SAMPLE_SOFTWARE    = 16;       // not using hardware mixing
        public const UInt32 BASS_SAMPLE_MUTEMAX     = 32;       // mute at max distance (3D only)
        public const UInt32 BASS_SAMPLE_VAM         = 64;       // DX7 voice allocation & management
        public const UInt32 BASS_SAMPLE_FX          = 128;      // old implementation of DX8 effects
        public const UInt32 BASS_SAMPLE_OVER_VOL    = 0x10000;  // override lowest volume
        public const UInt32 BASS_SAMPLE_OVER_POS    = 0x20000;  // override longest playing
        public const UInt32 BASS_SAMPLE_OVER_DIST   = 0x30000;  // override furthest from listener (3D only)
    }
#pragma warning restore 1591
}

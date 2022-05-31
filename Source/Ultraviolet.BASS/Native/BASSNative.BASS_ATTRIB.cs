using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    static partial class BASSNative
    {
        public const UInt32 BASS_ATTRIB_FREQ = 1;
        public const UInt32 BASS_ATTRIB_VOL = 2;
        public const UInt32 BASS_ATTRIB_PAN = 3;
        public const UInt32 BASS_ATTRIB_EAXMIX = 4;
        public const UInt32 BASS_ATTRIB_NOBUFFER = 5;
        public const UInt32 BASS_ATTRIB_CPU = 7;
        public const UInt32 BASS_ATTRIB_SRC = 8;
        public const UInt32 BASS_ATTRIB_MUSIC_AMPLIFY = 0x100;
        public const UInt32 BASS_ATTRIB_MUSIC_PANSEP = 0x101;
        public const UInt32 BASS_ATTRIB_MUSIC_PSCALER = 0x102;
        public const UInt32 BASS_ATTRIB_MUSIC_BPM = 0x103;
        public const UInt32 BASS_ATTRIB_MUSIC_SPEED = 0x104;
        public const UInt32 BASS_ATTRIB_MUSIC_VOL_GLOBAL = 0x105;
        public const UInt32 BASS_ATTRIB_MUSIC_VOL_CHAN = 0x200;
        public const UInt32 BASS_ATTRIB_MUSIC_VOL_INST = 0x300;

        // BASS_FX
        public const UInt32 BASS_ATTRIB_TEMPO = 0x10000;
        public const UInt32 BASS_ATTRIB_TEMPO_PITCH = 0x10001;
        public const UInt32 BASS_ATTRIB_TEMP_FREQ = 0x10002;
    }
#pragma warning restore 1591
}

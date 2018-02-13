using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    static partial class BASSNative
    {
        // BASS_Init flags
        public const UInt32 BASS_DEVICE_8BITS       = 1;		// 8 bit
        public const UInt32 BASS_DEVICE_MONO        = 2;		// mono
        public const UInt32 BASS_DEVICE_3D          = 4;		// enable 3D functionality
        public const UInt32 BASS_DEVICE_16BITS      = 8;		// limit output to 16 bit
        public const UInt32 BASS_DEVICE_LATENCY     = 0x100;	// calculate device latency (BASS_INFO struct)
        public const UInt32 BASS_DEVICE_CPSPEAKERS  = 0x400;	// detect speakers via Windows control panel
        public const UInt32 BASS_DEVICE_SPEAKERS    = 0x800;	// force enabling of speaker assignment
        public const UInt32 BASS_DEVICE_NOSPEAKER   = 0x1000;	// ignore speaker arrangement
        public const UInt32 BASS_DEVICE_DMIX        = 0x2000;	// use ALSA "dmix" plugin
        public const UInt32 BASS_DEVICE_FREQ        = 0x4000;	// set device sample rate
        public const UInt32 BASS_DEVICE_STEREO      = 0x8000;	// limit output to stereo
        public const UInt32 BASS_DEVICE_HOG         = 0x10000;	// hog/exclusive mode
        public const UInt32 BASS_DEVICE_AUDIOTRACK  = 0x20000;	// use AudioTrack output
        public const UInt32 BASS_DEVICE_DSOUND      = 0x40000;   // use DirectSound output

        // BASS_DEVICEINFO flags
        public const UInt32 BASS_DEVICE_ENABLED     = 1;
        public const UInt32 BASS_DEVICE_DEFAULT     = 2;
        public const UInt32 BASS_DEVICE_INIT        = 4;
        public const UInt32 BASS_DEVICE_LOOPBACK    = 8;
    }
#pragma warning restore 1591
}

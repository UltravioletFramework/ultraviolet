using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    static partial class BASSNative
    {
        // BASS_SetConfig options
        public const UInt32 BASS_CONFIG_BUFFER			    = 0;
        public const UInt32 BASS_CONFIG_UPDATEPERIOD	    = 1;
        public const UInt32 BASS_CONFIG_GVOL_SAMPLE		    = 4;
        public const UInt32 BASS_CONFIG_GVOL_STREAM		    = 5;
        public const UInt32 BASS_CONFIG_GVOL_MUSIC		    = 6;
        public const UInt32 BASS_CONFIG_CURVE_VOL		    = 7;
        public const UInt32 BASS_CONFIG_CURVE_PAN		    = 8;
        public const UInt32 BASS_CONFIG_FLOATDSP		    = 9;
        public const UInt32 BASS_CONFIG_3DALGORITHM		    = 10;
        public const UInt32 BASS_CONFIG_NET_TIMEOUT		    = 11;
        public const UInt32 BASS_CONFIG_NET_BUFFER		    = 12;
        public const UInt32 BASS_CONFIG_PAUSE_NOPLAY	    = 13;
        public const UInt32 BASS_CONFIG_NET_PREBUF		    = 15;
        public const UInt32 BASS_CONFIG_NET_PASSIVE		    = 18;
        public const UInt32 BASS_CONFIG_REC_BUFFER		    = 19;
        public const UInt32 BASS_CONFIG_NET_PLAYLIST	    = 21;
        public const UInt32 BASS_CONFIG_MUSIC_VIRTUAL	    = 22;
        public const UInt32 BASS_CONFIG_VERIFY			    = 23;
        public const UInt32 BASS_CONFIG_UPDATETHREADS	    = 24;
        public const UInt32 BASS_CONFIG_DEV_BUFFER		    = 27;
        public const UInt32 BASS_CONFIG_VISTA_TRUEPOS	    = 30;
        public const UInt32 BASS_CONFIG_IOS_MIXAUDIO	    = 34;
        public const UInt32 BASS_CONFIG_DEV_DEFAULT		    = 36;
        public const UInt32 BASS_CONFIG_NET_READTIMEOUT	    = 37;
        public const UInt32 BASS_CONFIG_VISTA_SPEAKERS	    = 38;
        public const UInt32 BASS_CONFIG_IOS_SPEAKER		    = 39;
        public const UInt32 BASS_CONFIG_MF_DISABLE		    = 40;
        public const UInt32 BASS_CONFIG_HANDLES			    = 41;
        public const UInt32 BASS_CONFIG_UNICODE			    = 42;
        public const UInt32 BASS_CONFIG_SRC				    = 43;
        public const UInt32 BASS_CONFIG_SRC_SAMPLE		    = 44;
        public const UInt32 BASS_CONFIG_ASYNCFILE_BUFFER    = 45;
        public const UInt32 BASS_CONFIG_OGG_PRESCAN		    = 47;
        public const UInt32 BASS_CONFIG_MF_VIDEO		    = 48;
        public const UInt32 BASS_CONFIG_AIRPLAY			    = 49;
        public const UInt32 BASS_CONFIG_DEV_NONSTOP		    = 50;
        public const UInt32 BASS_CONFIG_IOS_NOCATEGORY	    = 51;
        public const UInt32 BASS_CONFIG_VERIFY_NET		    = 52;
        public const UInt32 BASS_CONFIG_DEV_PERIOD		    = 53;
        public const UInt32 BASS_CONFIG_FLOAT			    = 54;
        public const UInt32 BASS_CONFIG_NET_SEEK		    = 56;
        public const UInt32 BASS_CONFIG_AM_DISABLE		    = 58;
        public const UInt32 BASS_CONFIG_NET_PLAYLIST_DEPTH  = 59;
        public const UInt32 BASS_CONFIG_NET_PREBUF_WAIT	    = 60;

        // BASS_SetConfigPtr options
        public const UInt32 BASS_CONFIG_NET_AGENT		    = 16;
        public const UInt32 BASS_CONFIG_NET_PROXY		    = 17;
        public const UInt32 BASS_CONFIG_IOS_NOTIFY		    = 46;
    }
#pragma warning restore 1591
}

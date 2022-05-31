using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    static partial class BASSNative
    {
        public const UInt32 BASS_STREAM_PRESCAN     = 0x20000;  // enable pin-point seeking/length (MP3/MP2/MP1)
        public const UInt32 BASS_STREAM_AUTOFREE    = 0x40000;	// automatically free the stream when it stop/ends
        public const UInt32 BASS_STREAM_RESTRATE    = 0x80000;	// restrict the download rate of internet file streams
        public const UInt32 BASS_STREAM_BLOCK       = 0x100000; // download/play internet file stream in small blocks
        public const UInt32 BASS_STREAM_DECODE      = 0x200000; // don't play the stream, only decode (BASS_ChannelGetData)
        public const UInt32 BASS_STREAM_STATUS      = 0x800000; // give server status info (HTTP/ICY tags) in DOWNLOADPROC
    }
#pragma warning restore 1591
}

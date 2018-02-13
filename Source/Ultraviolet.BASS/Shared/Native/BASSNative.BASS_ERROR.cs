using System;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    static partial class BASSNative
    {
        public const Int32 BASS_OK				= 0;	// all is OK
        public const Int32 BASS_ERROR_MEM		= 1;	// memory error
        public const Int32 BASS_ERROR_FILEOPEN	= 2;	// can't open the file
        public const Int32 BASS_ERROR_DRIVER	= 3;	// can't find a free/valid driver
        public const Int32 BASS_ERROR_BUFLOST	= 4;	// the sample buffer was lost
        public const Int32 BASS_ERROR_HANDLE	= 5;	// invalid handle
        public const Int32 BASS_ERROR_FORMAT	= 6;	// unsupported sample format
        public const Int32 BASS_ERROR_POSITION	= 7;	// invalid position
        public const Int32 BASS_ERROR_INIT		= 8;	// BASS_Init has not been successfully called
        public const Int32 BASS_ERROR_START	    = 9;	// BASS_Start has not been successfully called
        public const Int32 BASS_ERROR_SSL		= 10;	// SSL/HTTPS support isn't available
        public const Int32 BASS_ERROR_ALREADY	= 14;	// already initialized/paused/whatever
        public const Int32 BASS_ERROR_NOCHAN	= 18;	// can't get a free channel
        public const Int32 BASS_ERROR_ILLTYPE	= 19;	// an illegal type was specified
        public const Int32 BASS_ERROR_ILLPARAM	= 20;	// an illegal parameter was specified
        public const Int32 BASS_ERROR_NO3D		= 21;	// no = 3;D support
        public const Int32 BASS_ERROR_NOEAX	    = 22;	// no EAX support
        public const Int32 BASS_ERROR_DEVICE	= 23;	// illegal device number
        public const Int32 BASS_ERROR_NOPLAY	= 24;	// not playing
        public const Int32 BASS_ERROR_FREQ		= 25;	// illegal sample rate
        public const Int32 BASS_ERROR_NOTFILE	= 27;	// the stream is not a file stream
        public const Int32 BASS_ERROR_NOHW		= 29;	// no hardware voices available
        public const Int32 BASS_ERROR_EMPTY	    = 31;	// the MOD music has no sequence data
        public const Int32 BASS_ERROR_NONET	    = 32;	// no internet connection could be opened
        public const Int32 BASS_ERROR_CREATE	= 33;	// couldn't create the file
        public const Int32 BASS_ERROR_NOFX		= 34;	// effects are not available
        public const Int32 BASS_ERROR_NOTAVAIL	= 37;	// requested data/action is not available
        public const Int32 BASS_ERROR_DECODE	= 38;	// the channel is/isn't a "decoding channel"
        public const Int32 BASS_ERROR_DX		= 39;	// a sufficient DirectX version is not installed
        public const Int32 BASS_ERROR_TIMEOUT	= 40;	// connection timedout
        public const Int32 BASS_ERROR_FILEFORM	= 41;	// unsupported file format
        public const Int32 BASS_ERROR_SPEAKER	= 42;	// unavailable speaker
        public const Int32 BASS_ERROR_VERSION	= 43;	// invalid BASS version (used by add-ons)
        public const Int32 BASS_ERROR_CODEC	    = 44;	// codec is not available/supported
        public const Int32 BASS_ERROR_ENDED	    = 45;	// the channel/file has ended
        public const Int32 BASS_ERROR_BUSY		= 46;	// the device is busy
        public const Int32 BASS_ERROR_UNKNOWN	= -1;	// some other mystery problem
    }
#pragma warning restore 1591
}

using System;
using System.Runtime.InteropServices;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FMOD_CREATESOUNDEXINFO
    {
        public Int32 cbsize;
        public UInt32 length;
        public UInt32 fileoffset;
        public Int32 numchannels;
        public Int32 defaultfrequency;
        public FMOD_SOUND_FORMAT format;
        public UInt32 decodebuffersize;
        public Int32 initialsubsound;
        public Int32 numsubsounds;
        public Int32* inclusionlist;
        public Int32 inclusionlistnum;
        public IntPtr pcmreadcallback;
        public IntPtr pcmsetposcallback;
        public IntPtr nonblockcallback;
        public IntPtr dlsname;
        public IntPtr encryptionkey;
        public Int32 maxpolyphony;
        public IntPtr userdata;
        public FMOD_SOUND_TYPE suggestedsoundtype;
        public IntPtr fileuseropen;
        public IntPtr fileuserclose;
        public IntPtr fileuserread;
        public IntPtr fileuserasyncread;
        public IntPtr fileuserasynccancel;
        public IntPtr fileuserdata;
        public Int32 filebuffersize;
        public FMOD_CHANNELORDER channelorder;
        public FMOD_CHANNELMASK channelmask;
        public FMOD_SOUNDGROUP* initialsoundgroup;
        public UInt32 initialseekposition;
        public FMOD_TIMEUNIT initialseekpostype;
        public Int32 ignoresetfilesystem;
        public UInt32 audioqueuepolicy;
        public UInt32 minmidigranularity;
        public Int32 nonblockthreadid;
        public IntPtr fsbguid;
    }
#pragma warning restore 1591
}

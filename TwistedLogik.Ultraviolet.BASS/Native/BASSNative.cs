using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.BASS.Native
{
    internal delegate UInt32 StreamProc(UInt32 handle, IntPtr buffer, UInt32 length, IntPtr user);

    internal delegate void SyncProc(UInt32 handle, UInt32 channel, UInt32 data, IntPtr user);

    [SuppressUnmanagedCodeSecurity]
    internal static unsafe class BASSNative
    {
#if IOS
        const String LibraryPath = "__Internal";
#else
        const String LibraryPath = "bass";
#endif

        static BASSNative()
        {
            LibraryLoader.Load("bass");
        }

        public static readonly IntPtr STREAMPROC_DUMMY = new IntPtr(0);
        public static readonly IntPtr STREAMPROC_PUSH = new IntPtr(-1);

        public const UInt32 BASS_OK = 0;
        public const UInt32 BASS_ERROR_MEM = 1;
        public const UInt32 BASS_ERROR_FILEOPEN = 2;
        public const UInt32 BASS_ERROR_DRIVER = 3;
        public const UInt32 BASS_ERROR_BUFLOST = 4;
        public const UInt32 BASS_ERROR_HANDLE = 5;
        public const UInt32 BASS_ERROR_FORMAT = 6;
        public const UInt32 BASS_ERROR_POSITION = 7;
        public const UInt32 BASS_ERROR_INIT = 8;
        public const UInt32 BASS_ERROR_START = 9;
        public const UInt32 BASS_ERROR_ALREADY = 14;
        public const UInt32 BASS_ERROR_NOCHAN = 18;
        public const UInt32 BASS_ERROR_ILLTYPE = 19;
        public const UInt32 BASS_ERROR_ILLPARAM = 20;
        public const UInt32 BASS_ERROR_NO3D = 21;
        public const UInt32 BASS_ERROR_NOEAX = 22;
        public const UInt32 BASS_ERROR_DEVICE = 23;
        public const UInt32 BASS_ERROR_NOPLAY = 24;
        public const UInt32 BASS_ERROR_FREQ = 25;
        public const UInt32 BASS_ERROR_NOTFILE = 27;
        public const UInt32 BASS_ERROR_NOHW = 29;
        public const UInt32 BASS_ERROR_EMPTY = 31;
        public const UInt32 BASS_ERROR_NONET = 32;
        public const UInt32 BASS_ERROR_CREATE = 33;
        public const UInt32 BASS_ERROR_NOFX = 34;
        public const UInt32 BASS_ERROR_NOTAVAIL = 37;
        public const UInt32 BASS_ERROR_DECODE = 38;
        public const UInt32 BASS_ERROR_DX = 39;
        public const UInt32 BASS_ERROR_TIMEOUT = 40;
        public const UInt32 BASS_ERROR_FILEFORM = 41;
        public const UInt32 BASS_ERROR_SPEAKER = 42;
        public const UInt32 BASS_ERROR_VERSION = 43;
        public const UInt32 BASS_ERROR_CODEC = 44;
        public const UInt32 BASS_ERROR_ENDED = 45;
        public const UInt32 BASS_ERROR_BUSY = 46;
        public const UInt32 BASS_ERROR_UNKNOWN = unchecked((UInt32)(-1));

        public const UInt32 BASS_ACTIVE_STOPPED = 0;
        public const UInt32 BASS_ACTIVE_PLAYING = 1;
        public const UInt32 BASS_ACTIVE_STALLED = 2;
        public const UInt32 BASS_ACTIVE_PAUSED = 3;

        public const UInt32 BASS_STREAMPROC_END = 0x80000000;

        public const UInt32 BASS_STREAM_AUTOFREE = 0x40000;
        public const UInt32 BASS_STREAM_DECODE = 0x200000;

        public const UInt32 BASS_FX_FREESOURCE = 0x10000;

        public const UInt32 BASS_SAMPLE_LOOP = 4;
        public const UInt32 BASS_SAMPLE_OVER_VOL = 0x10000;
        public const UInt32 BASS_SAMPLE_OVER_POS = 0x20000;
        public const UInt32 BASS_SAMPLE_OVER_DIST = 0x30000;

        public const UInt32 BASS_TAG_ID3 = 0;
        public const UInt32 BASS_TAG_ID3V2 = 1;
        public const UInt32 BASS_TAG_OGG = 2;
        public const UInt32 BASS_TAG_HTTP = 3;
        public const UInt32 BASS_TAG_ICY = 4;
        public const UInt32 BASS_TAG_META = 5;
        public const UInt32 BASS_TAG_APE = 6;
        public const UInt32 BASS_TAG_MP4 = 7;
        public const UInt32 BASS_TAG_WMA = 8;
        public const UInt32 BASS_TAG_VENDOR = 9;
        public const UInt32 BASS_TAG_LYRICS3 = 10;
        public const UInt32 BASS_TAG_CA_CODEC = 11;
        public const UInt32 BASS_TAG_MF = 13;
        public const UInt32 BASS_TAG_WAVEFORMAT = 14;
        public const UInt32 BASS_TAG_RIFF_INFO = 0x100;
        public const UInt32 BASS_TAG_RIFF_BEXT = 0x101;
        public const UInt32 BASS_TAG_RIFF_CART = 0x102;
        public const UInt32 BASS_TAG_RIFF_DISP = 0x103;
        public const UInt32 BASS_TAG_APE_BINARY = 0x1000;
        public const UInt32 BASS_TAG_MUSIC_NAME = 0x10000;
        public const UInt32 BASS_TAG_MUSIC_MESSAGE = 0x10001;
        public const UInt32 BASS_TAG_MUSIC_ORDERS = 0x10002;
        public const UInt32 BASS_TAG_MUSIC_AUTH = 0x10003;
        public const UInt32 BASS_TAG_MUSIC_INST = 0x10100;
        public const UInt32 BASS_TAG_MUSIC_SAMPLE = 0x10300;

        [DllImport(LibraryPath, EntryPoint = "BASS_ErrorGetCode")]
        public static extern Int32 ErrorGetCode();

        [DllImport(LibraryPath, EntryPoint = "BASS_Init")]
        public static extern Boolean Init(Int32 device, UInt32 freq, UInt32 flags, IntPtr win, IntPtr clsid);

        [DllImport(LibraryPath, EntryPoint = "BASS_Free")]
        public static extern Boolean Free();

        [DllImport(LibraryPath, EntryPoint = "BASS_Update")]
        public static extern Boolean Update(UInt32 length);

        [DllImport(LibraryPath, EntryPoint = "BASS_PluginLoad", BestFitMapping = false)]
        public static extern UInt32 PluginLoad([MarshalAs(UnmanagedType.LPStr)] string file, UInt32 flags);

        [DllImport(LibraryPath, EntryPoint = "BASS_PluginFree")]
        public static extern Boolean PluginFree(UInt32 handle);

        [DllImport(LibraryPath, EntryPoint = "BASS_GetConfig")]
        public static extern UInt32 GetConfig(BASSConfig option);

        [DllImport(LibraryPath, EntryPoint = "BASS_SetConfig")]
        public static extern Boolean SetConfig(BASSConfig option, UInt32 value);

        [DllImport(LibraryPath, EntryPoint = "BASS_GetVolume")]
        public static extern Single GetVolume();

        [DllImport(LibraryPath, EntryPoint = "BASS_SetVolume")]
        public static extern Boolean SetVolume(Single volume);

        [DllImport(LibraryPath, EntryPoint = "BASS_StreamCreate")]
        public static extern UInt32 StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, StreamProc proc, IntPtr user);

        [DllImport(LibraryPath, EntryPoint = "BASS_StreamCreate")]
        public static extern UInt32 StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, IntPtr proc, IntPtr user);

        [DllImport(LibraryPath, EntryPoint = "BASS_StreamCreateFile")]
        private static extern UInt32 StreamCreateFile(Boolean mem, IntPtr file, UInt64 offset, UInt64 length, UInt32 flags);

        public static UInt32 StreamCreateFile(String file, UInt32 flags)
        {
            var fileptr = Marshal.StringToHGlobalAnsi(file);
            try
            {
                return StreamCreateFile(false, fileptr, 0, 0, flags);
            }
            finally
            {
                Marshal.FreeHGlobal(fileptr);
            }
        }

        public static UInt32 StreamCreateFile(IntPtr file, UInt64 offset, UInt64 length, UInt32 flags)
        {
            return StreamCreateFile(true, file, offset, length, flags);
        }

        [DllImport(LibraryPath, EntryPoint = "BASS_StreamCreateFileUser")]
        public static extern UInt32 StreamCreateFileUser(UInt32 system, UInt32 flags, BASS_FILEPROCS* procs, IntPtr user);

        [DllImport(LibraryPath, EntryPoint = "BASS_StreamPutData")]
        public static extern UInt32 StreamPutData(UInt32 handle, IntPtr buffer, UInt32 length);

        [DllImport(LibraryPath, EntryPoint = "BASS_StreamFree")]
        public static extern Boolean StreamFree(UInt32 handle);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelIsActive")]
        public static extern UInt32 ChannelIsActive(UInt32 handle);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelIsSliding")]
        public static extern Boolean ChannelIsSliding(UInt32 handle, BASSAttrib attrib);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelFlags")]
        public static extern UInt32 ChannelFlags(UInt32 handle, UInt32 flags, UInt32 mask);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelGetInfo")]
        public static extern Boolean ChannelGetInfo(UInt32 handle, out BASS_CHANNELINFO info);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelBytes2Seconds")]
        public static extern Double ChannelBytes2Seconds(UInt32 handle, UInt64 pos);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelSeconds2Bytes")]
        public static extern UInt64 ChannelSeconds2Bytes(UInt32 handle, Double pos);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelUpdate")]
        public static extern Boolean ChannelUpdate(UInt32 handle, UInt32 length);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelPlay")]
        public static extern Boolean ChannelPlay(UInt32 handle, Boolean restart);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelStop")]
        public static extern Boolean ChannelStop(UInt32 handle);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelPause")]
        public static extern Boolean ChannelPause(UInt32 handle);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelGetData")]
        public static extern UInt32 ChannelGetData(UInt32 handle, IntPtr buffer, UInt32 length);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelGetAttribute")]
        public static extern Boolean ChannelGetAttribute(UInt32 handle, BASSAttrib attrib, Single* value);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelSetAttribute")]
        public static extern Boolean ChannelSetAttribute(UInt32 handle, BASSAttrib attrib, Single value);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelSlideAttribute")]
        public static extern Boolean ChannelSlideAttribute(UInt32 handle, BASSAttrib attrib, Single value, UInt32 time);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelGetPosition")]
        public static extern UInt64 ChannelGetPosition(UInt32 handle, UInt32 mode);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelSetPosition")]
        public static extern Boolean ChannelSetPosition(UInt32 handle, UInt64 pos, UInt32 mode);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelGetLength")]
        public static extern UInt64 ChannelGetLength(UInt32 handle, UInt32 mode);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelSetSync")]
        public static extern UInt32 ChannelSetSync(UInt32 handle, BASSSync type, UInt64 param, SyncProc proc, IntPtr user);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelRemoveSync")]
        public static extern Boolean ChannelRemoveSync(UInt32 handle, UInt32 sync);

        [DllImport(LibraryPath, EntryPoint = "BASS_ChannelGetTags")]
        public static extern void* ChannelGetTags(UInt32 handle, UInt32 tags);
        
        public static Boolean ChannelGetTags_Ogg(UInt32 handle, out IDictionary<String, String> tags)
        {
            var ptr = (Byte*)ChannelGetTags(handle, BASS_TAG_OGG);
            if (ptr == null)
            {
                var error = ErrorGetCode();
                if (error == BASS_ERROR_NOTAVAIL)
                {
                    tags = null;
                    return false;
                }

                throw new BASSException(error);
            }

            var result = new Dictionary<String, String>();

            while (*ptr != 0)
            {
                var str = Marshal.PtrToStringAnsi((IntPtr)ptr);

                var tag = str.Split('=');
                var key = tag[0];
                var value = tag[1];

                result[key] = value;

                ptr += str.Length + 1;
            }

            tags = result;
            return true;
        }

        [DllImport(LibraryPath, EntryPoint = "BASS_SampleLoad")]
        private static extern UInt32 SampleLoad(Boolean mem, IntPtr file, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags);

        public static UInt32 SampleLoad(String file, UInt32 max, UInt32 flags)
        {
            var pFile = Marshal.StringToHGlobalAnsi(file);
            try
            {
                return SampleLoad(false, pFile, 0, 0, max, flags);
            }
            finally
            {
                Marshal.FreeHGlobal(pFile);
            }
        }

        public static UInt32 SampleLoad(Byte[] data, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags)
        {
            fixed (Byte* pData = data)
            {
                return SampleLoad(true, (IntPtr)pData, offset, length, max, flags);
            }            
        }

        [DllImport(LibraryPath, EntryPoint = "BASS_SampleFree")]
        public static extern Boolean SampleFree(UInt32 handle);

        [DllImport(LibraryPath, EntryPoint = "BASS_SampleGetChannel")]
        public static extern UInt32 SampleGetChannel(UInt32 handle, Boolean onlynew);

        [DllImport(LibraryPath, EntryPoint = "BASS_SampleGetInfo")]
        public static extern Boolean SampleGetInfo(UInt32 handle, out BASS_SAMPLE info);

        [DllImport(LibraryPath, EntryPoint = "BASS_SampleGetData")]
        public static extern Boolean SampleGetData(UInt32 handle, IntPtr buffer);

        public static Boolean SampleGetData(UInt32 handle, Byte[] buffer)
        {
            fixed (Byte* pBuffer = buffer)
            {
                return SampleGetData(handle, (IntPtr)pBuffer);
            }
        }

        [DllImport(LibraryPath, EntryPoint = "BASS_Pause")]
        public static extern Boolean Pause();

        [DllImport(LibraryPath, EntryPoint = "BASS_Start")]
        public static extern Boolean Start();
    }
}

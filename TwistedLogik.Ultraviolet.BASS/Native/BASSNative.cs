using System;
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
        static BASSNative()
        {
            LibraryLoader.Load("bass");
        }

        public static readonly IntPtr STREAMPROC_DUMMY = new IntPtr(0);
        public static readonly IntPtr STREAMPROC_PUSH  = new IntPtr(-1);
        
        public const UInt32 BASS_OK			    = 0;	
        public const UInt32 BASS_ERROR_MEM	    = 1;	
        public const UInt32 BASS_ERROR_FILEOPEN = 2;	
        public const UInt32 BASS_ERROR_DRIVER   = 3;	
        public const UInt32 BASS_ERROR_BUFLOST  = 4;	
        public const UInt32 BASS_ERROR_HANDLE   = 5;	
        public const UInt32 BASS_ERROR_FORMAT   = 6;	
        public const UInt32 BASS_ERROR_POSITION = 7;	
        public const UInt32 BASS_ERROR_INIT	    = 8;	
        public const UInt32 BASS_ERROR_START    = 9;	
        public const UInt32 BASS_ERROR_ALREADY  = 14;	
        public const UInt32 BASS_ERROR_NOCHAN   = 18;	
        public const UInt32 BASS_ERROR_ILLTYPE  = 19;	
        public const UInt32 BASS_ERROR_ILLPARAM = 20;	
        public const UInt32 BASS_ERROR_NO3D	    = 21;	
        public const UInt32 BASS_ERROR_NOEAX    = 22;	
        public const UInt32 BASS_ERROR_DEVICE   = 23;	
        public const UInt32 BASS_ERROR_NOPLAY   = 24;	
        public const UInt32 BASS_ERROR_FREQ	    = 25;	
        public const UInt32 BASS_ERROR_NOTFILE  = 27;	
        public const UInt32 BASS_ERROR_NOHW	    = 29;	
        public const UInt32 BASS_ERROR_EMPTY    = 31;	
        public const UInt32 BASS_ERROR_NONET    = 32;	
        public const UInt32 BASS_ERROR_CREATE   = 33;	
        public const UInt32 BASS_ERROR_NOFX	    = 34;	
        public const UInt32 BASS_ERROR_NOTAVAIL = 37;	
        public const UInt32 BASS_ERROR_DECODE   = 38;	
        public const UInt32 BASS_ERROR_DX	    = 39;	
        public const UInt32 BASS_ERROR_TIMEOUT  = 40;	
        public const UInt32 BASS_ERROR_FILEFORM = 41;	
        public const UInt32 BASS_ERROR_SPEAKER  = 42;	
        public const UInt32 BASS_ERROR_VERSION  = 43;	
        public const UInt32 BASS_ERROR_CODEC    = 44;	
        public const UInt32 BASS_ERROR_ENDED    = 45;	
        public const UInt32 BASS_ERROR_BUSY	    = 46;
        public const UInt32 BASS_ERROR_UNKNOWN  = unchecked((UInt32)(-1));	

        public const UInt32 BASS_ACTIVE_STOPPED	= 0;
        public const UInt32 BASS_ACTIVE_PLAYING	= 1;
        public const UInt32 BASS_ACTIVE_STALLED	= 2;
        public const UInt32 BASS_ACTIVE_PAUSED	= 3;

        public const UInt32 BASS_STREAMPROC_END = 0x80000000;

        public const UInt32 BASS_STREAM_AUTOFREE = 0x40000;
        public const UInt32 BASS_STREAM_DECODE   = 0x200000;

        public const UInt32 BASS_FX_FREESOURCE = 0x10000;

        public const UInt32 BASS_SAMPLE_LOOP = 4;
        public const UInt32 BASS_SAMPLE_OVER_VOL  = 0x10000;
        public const UInt32 BASS_SAMPLE_OVER_POS  = 0x20000;
        public const UInt32 BASS_SAMPLE_OVER_DIST = 0x30000;

        [DllImport("bass", EntryPoint = "BASS_ErrorGetCode", CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ErrorGetCode();

        [DllImport("bass", EntryPoint = "BASS_Init", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean Init(Int32 device, UInt32 freq, UInt32 flags, IntPtr win, IntPtr clsid);

        [DllImport("bass", EntryPoint = "BASS_Free", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean Free();

        [DllImport("bass", EntryPoint = "BASS_Update", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean Update(UInt32 length);

        [DllImport("bass", EntryPoint = "BASS_PluginLoad", CallingConvention = CallingConvention.StdCall, BestFitMapping = false)]
        public static extern UInt32 PluginLoad([MarshalAs(UnmanagedType.LPStr)] string file, UInt32 flags);

        [DllImport("bass", EntryPoint = "BASS_PluginFree", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean PluginFree(UInt32 handle);

        [DllImport("bass", EntryPoint = "BASS_GetConfig", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 GetConfig(BASSConfig option);

        [DllImport("bass", EntryPoint = "BASS_SetConfig", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean SetConfig(BASSConfig option, UInt32 value);

        [DllImport("bass", EntryPoint = "BASS_GetVolume", CallingConvention = CallingConvention.StdCall)]
        public static extern Single GetVolume();

        [DllImport("bass", EntryPoint = "BASS_SetVolume", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean SetVolume(Single volume);

        [DllImport("bass", EntryPoint = "BASS_StreamCreate", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, StreamProc proc, IntPtr user);

        [DllImport("bass", EntryPoint = "BASS_StreamCreate", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, IntPtr proc, IntPtr user);

        [DllImport("bass", EntryPoint = "BASS_StreamCreateFile", CallingConvention = CallingConvention.StdCall)]
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

        [DllImport("bass", EntryPoint = "BASS_StreamCreateFileUser", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 StreamCreateFileUser(UInt32 system, UInt32 flags, BASS_FILEPROCS* procs, IntPtr user);

        [DllImport("bass", EntryPoint = "BASS_StreamPutData", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 StreamPutData(UInt32 handle, IntPtr buffer, UInt32 length);

        [DllImport("bass", EntryPoint = "BASS_StreamFree", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean StreamFree(UInt32 handle);

        [DllImport("bass", EntryPoint = "BASS_ChannelIsActive", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 ChannelIsActive(UInt32 handle);

        [DllImport("bass", EntryPoint = "BASS_ChannelIsSliding", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelIsSliding(UInt32 handle, BASSAttrib attrib);

        [DllImport("bass", EntryPoint = "BASS_ChannelFlags", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 ChannelFlags(UInt32 handle, UInt32 flags, UInt32 mask);

        [DllImport("bass", EntryPoint = "BASS_ChannelGetInfo", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelGetInfo(UInt32 handle, out BASS_CHANNELINFO info);

        [DllImport("bass", EntryPoint = "BASS_ChannelBytes2Seconds", CallingConvention = CallingConvention.StdCall)]
        public static extern Double ChannelBytes2Seconds(UInt32 handle, UInt64 pos);

        [DllImport("bass", EntryPoint = "BASS_ChannelSeconds2Bytes", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt64 ChannelSeconds2Bytes(UInt32 handle, Double pos);

        [DllImport("bass", EntryPoint = "BASS_ChannelUpdate", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelUpdate(UInt32 handle, UInt32 length);

        [DllImport("bass", EntryPoint = "BASS_ChannelPlay", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelPlay(UInt32 handle, Boolean restart);

        [DllImport("bass", EntryPoint = "BASS_ChannelStop", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelStop(UInt32 handle);

        [DllImport("bass", EntryPoint = "BASS_ChannelPause", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelPause(UInt32 handle);

        [DllImport("bass", EntryPoint = "BASS_ChannelGetData", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 ChannelGetData(UInt32 handle, IntPtr buffer, UInt32 length);

        [DllImport("bass", EntryPoint = "BASS_ChannelGetAttribute", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelGetAttribute(UInt32 handle, BASSAttrib attrib, Single* value);

        [DllImport("bass", EntryPoint = "BASS_ChannelSetAttribute", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelSetAttribute(UInt32 handle, BASSAttrib attrib, Single value);

        [DllImport("bass", EntryPoint = "BASS_ChannelSlideAttribute", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelSlideAttribute(UInt32 handle, BASSAttrib attrib, Single value, UInt32 time);

        [DllImport("bass", EntryPoint = "BASS_ChannelGetPosition", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt64 ChannelGetPosition(UInt32 handle, UInt32 mode);

        [DllImport("bass", EntryPoint = "BASS_ChannelSetPosition", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelSetPosition(UInt32 handle, UInt64 pos, UInt32 mode);

        [DllImport("bass", EntryPoint = "BASS_ChannelGetLength", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt64 ChannelGetLength(UInt32 handle, UInt32 mode);

        [DllImport("bass", EntryPoint = "BASS_ChannelSetSync", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 ChannelSetSync(UInt32 handle, BASSSync type, UInt64 param, SyncProc proc, IntPtr user);

        [DllImport("bass", EntryPoint = "BASS_ChannelRemoveSync", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ChannelRemoveSync(UInt32 handle, UInt32 sync);

        [DllImport("bass", EntryPoint = "BASS_SampleLoad", CallingConvention = CallingConvention.StdCall)]
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

        [DllImport("bass", EntryPoint = "BASS_SampleFree", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean SampleFree(UInt32 handle);

        [DllImport("bass", EntryPoint = "BASS_SampleGetChannel", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 SampleGetChannel(UInt32 handle, Boolean onlynew);

        [DllImport("bass", EntryPoint = "BASS_SampleGetInfo", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean SampleGetInfo(UInt32 handle, out BASS_SAMPLE info);

        [DllImport("bass", EntryPoint = "BASS_SampleGetData", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean SampleGetData(UInt32 handle, IntPtr buffer);

        public static Boolean SampleGetData(UInt32 handle, Byte[] buffer)
        {
            fixed (Byte* pBuffer = buffer)
            {
                return SampleGetData(handle, (IntPtr)pBuffer);
            }
        }

        [DllImport("bass", EntryPoint = "BASS_Pause", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean Pause();

        [DllImport("bass", EntryPoint = "BASS_Start", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean Start();
    }
}

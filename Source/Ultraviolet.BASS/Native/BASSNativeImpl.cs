using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    public abstract unsafe class BASSNativeImpl
    {
        public abstract Int32 BASS_ErrorGetCode();
        public abstract Boolean BASS_Init(Int32 device, UInt32 freq, UInt32 flags, IntPtr win, IntPtr clsid);
        public abstract Boolean BASS_Free();
        public abstract Boolean BASS_Update(UInt32 length);
        public abstract Boolean BASS_SetDevice(UInt32 device);
        public abstract UInt32 BASS_GetDevice();
        public abstract UInt32 BASS_PluginLoad(String file, UInt32 flags);
        public abstract Boolean BASS_PluginFree(UInt32 handle);
        public abstract UInt32 BASS_GetConfig(UInt32 option);
        public abstract Boolean BASS_SetConfig(UInt32 option, UInt32 value);
        public abstract Single BASS_GetVolume();
        public abstract Boolean BASS_SetVolume(Single volume);
        public abstract UInt32 BASS_StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, IntPtr proc, IntPtr user);
        public abstract UInt32 BASS_StreamCreateFile(Boolean mem, String file, UInt64 offset, UInt64 length, UInt32 flags);
        public abstract UInt32 BASS_StreamCreateFileUser(UInt32 system, UInt32 flags, BASS_FILEPROCS* procs, IntPtr user);
        public abstract UInt32 BASS_StreamPutData(UInt32 handle, IntPtr buffer, UInt32 length);
        public abstract Boolean BASS_StreamFree(UInt32 handle);
        public abstract Boolean BASS_ChannelSetDevice(UInt32 handle, UInt32 device);
        public abstract UInt32 BASS_ChannelIsActive(UInt32 handle);
        public abstract Boolean BASS_ChannelIsSliding(UInt32 handle, UInt32 attrib);
        public abstract UInt32 BASS_ChannelFlags(UInt32 handle, UInt32 flags, UInt32 mask);
        public abstract Boolean BASS_ChannelGetInfo(UInt32 handle, out BASS_CHANNELINFO info);
        public abstract Double BASS_ChannelBytes2Seconds(UInt32 handle, UInt64 pos);
        public abstract UInt64 BASS_ChannelSeconds2Bytes(UInt32 handle, Double pos);
        public abstract Boolean BASS_ChannelUpdate(UInt32 handle, UInt32 length);
        public abstract Boolean BASS_ChannelPlay(UInt32 handle, Boolean restart);
        public abstract Boolean BASS_ChannelStop(UInt32 handle);
        public abstract Boolean BASS_ChannelPause(UInt32 handle);
        public abstract UInt32 BASS_ChannelGetData(UInt32 handle, IntPtr buffer, UInt32 length);
        public abstract Boolean BASS_ChannelGetAttribute(UInt32 handle, UInt32 attrib, Single* value);
        public abstract Boolean BASS_ChannelSetAttribute(UInt32 handle, UInt32 attrib, Single value);
        public abstract Boolean BASS_ChannelSlideAttribute(UInt32 handle, UInt32 attrib, Single value, UInt32 time);
        public abstract UInt64 BASS_ChannelGetPosition(UInt32 handle, UInt32 mode);
        public abstract Boolean BASS_ChannelSetPosition(UInt32 handle, UInt64 pos, UInt32 mode);
        public abstract UInt64 BASS_ChannelGetLength(UInt32 handle, UInt32 mode);
        public abstract UInt32 BASS_ChannelSetSync(UInt32 handle, UInt32 type, UInt64 param, SyncProc proc, IntPtr user);
        public abstract Boolean BASS_ChannelRemoveSync(UInt32 handle, UInt32 sync);
        public abstract void* BASS_ChannelGetTags(UInt32 handle, UInt32 tags);
        public abstract UInt32 BASS_SampleLoad(Boolean mem, IntPtr file, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags);
        public abstract Boolean BASS_SampleFree(UInt32 handle);
        public abstract UInt32 BASS_SampleGetChannel(UInt32 handle, Boolean onlynew);
        public abstract Boolean BASS_SampleGetInfo(UInt32 handle, out BASS_SAMPLE info);
        public abstract Boolean BASS_SampleGetData(UInt32 handle, IntPtr buffer);
        public abstract Boolean BASS_Pause();
        public abstract Boolean BASS_Start();
        public abstract Boolean BASS_GetDeviceInfo(UInt32 device, BASS_DEVICEINFO* info);
    }
#pragma warning restore 1591
}

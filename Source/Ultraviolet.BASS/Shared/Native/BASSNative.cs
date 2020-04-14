using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    public static unsafe partial class BASSNative
    {
        private static readonly BASSNativeImpl impl;
        
        static BASSNative()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Android:
                    impl = new BASSNativeImpl_Android();
                    break;
                    
                default:
                    impl = new BASSNativeImpl_Default();
                    break;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 BASS_ErrorGetCode() => impl.BASS_ErrorGetCode();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_Init(Int32 device, UInt32 freq, UInt32 flags, IntPtr win, IntPtr clsid) => impl.BASS_Init(device, freq, flags, win, clsid);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_Free() => impl.BASS_Free();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_Update(UInt32 length) => impl.BASS_Update(length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_SetDevice(UInt32 device) => impl.BASS_SetDevice(device);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_GetDevice() => impl.BASS_GetDevice();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_PluginLoad(String file, UInt32 flags) => impl.BASS_PluginLoad(file, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_PluginFree(UInt32 handle) => impl.BASS_PluginFree(handle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_GetConfig(UInt32 option) => impl.BASS_GetConfig(option);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_SetConfig(UInt32 option, UInt32 value) => impl.BASS_SetConfig(option, value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Single BASS_GetVolume() => impl.BASS_GetVolume();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_SetVolume(Single volume) => impl.BASS_SetVolume(volume);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, IntPtr proc, IntPtr user) => impl.BASS_StreamCreate(freq, chans, flags, proc, user);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_StreamCreateFile(Boolean mem, String file, UInt64 offset, UInt64 length, UInt32 flags) => impl.BASS_StreamCreateFile(mem, file, offset, length, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_StreamCreateFileUser(UInt32 system, UInt32 flags, BASS_FILEPROCS* procs, IntPtr user) => impl.BASS_StreamCreateFileUser(system, flags, procs, user);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_StreamPutData(UInt32 handle, IntPtr buffer, UInt32 length) => impl.BASS_StreamPutData(handle, buffer, length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_StreamFree(UInt32 handle) => impl.BASS_StreamFree(handle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelSetDevice(UInt32 handle, UInt32 device) => impl.BASS_ChannelSetDevice(handle, device);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_ChannelIsActive(UInt32 handle) => impl.BASS_ChannelIsActive(handle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelIsSliding(UInt32 handle, UInt32 attrib) => impl.BASS_ChannelIsSliding(handle, attrib);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_ChannelFlags(UInt32 handle, UInt32 flags, UInt32 mask) => impl.BASS_ChannelFlags(handle, flags, mask);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelGetInfo(UInt32 handle, out BASS_CHANNELINFO info) => impl.BASS_ChannelGetInfo(handle, out info);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double BASS_ChannelBytes2Seconds(UInt32 handle, UInt64 pos) => impl.BASS_ChannelBytes2Seconds(handle, pos);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 BASS_ChannelSeconds2Bytes(UInt32 handle, Double pos) => impl.BASS_ChannelSeconds2Bytes(handle, pos);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelUpdate(UInt32 handle, UInt32 length) => impl.BASS_ChannelUpdate(handle, length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelPlay(UInt32 handle, Boolean restart) => impl.BASS_ChannelPlay(handle, restart);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelStop(UInt32 handle) => impl.BASS_ChannelStop(handle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelPause(UInt32 handle) => impl.BASS_ChannelPause(handle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_ChannelGetData(UInt32 handle, IntPtr buffer, UInt32 length) => impl.BASS_ChannelGetData(handle, buffer, length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelGetAttribute(UInt32 handle, UInt32 attrib, Single* value) => impl.BASS_ChannelGetAttribute(handle, attrib, value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelSetAttribute(UInt32 handle, UInt32 attrib, Single value) => impl.BASS_ChannelSetAttribute(handle, attrib, value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelSlideAttribute(UInt32 handle, UInt32 attrib, Single value, UInt32 time) => impl.BASS_ChannelSlideAttribute(handle, attrib, value, time);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 BASS_ChannelGetPosition(UInt32 handle, UInt32 mode) => impl.BASS_ChannelGetPosition(handle, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelSetPosition(UInt32 handle, UInt64 pos, UInt32 mode) => impl.BASS_ChannelSetPosition(handle, pos, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 BASS_ChannelGetLength(UInt32 handle, UInt32 mode) => impl.BASS_ChannelGetLength(handle, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_ChannelSetSync(UInt32 handle, UInt32 type, UInt64 param, SyncProc proc, IntPtr user) => impl.BASS_ChannelSetSync(handle, type, param, proc, user);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_ChannelRemoveSync(UInt32 handle, UInt32 sync) => impl.BASS_ChannelRemoveSync(handle, sync);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* BASS_ChannelGetTags(UInt32 handle, UInt32 tags) => impl.BASS_ChannelGetTags(handle, tags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_SampleLoad(Boolean mem, IntPtr file, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags) => impl.BASS_SampleLoad(mem, file, offset, length, max, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_SampleFree(UInt32 handle) => impl.BASS_SampleFree(handle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_SampleGetChannel(UInt32 handle, Boolean onlynew) => impl.BASS_SampleGetChannel(handle, onlynew);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_SampleGetInfo(UInt32 handle, out BASS_SAMPLE info) => impl.BASS_SampleGetInfo(handle, out info);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_SampleGetData(UInt32 handle, IntPtr buffer) => impl.BASS_SampleGetData(handle, buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_Pause() => impl.BASS_Pause();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_Start() => impl.BASS_Start();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean BASS_GetDeviceInfo(UInt32 device, BASS_DEVICEINFO* info) => impl.BASS_GetDeviceInfo(device, info);
    }
#pragma warning restore 1591
}

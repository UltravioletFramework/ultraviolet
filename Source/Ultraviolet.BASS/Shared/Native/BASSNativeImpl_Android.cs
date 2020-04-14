using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class BASSNativeImpl_Android : BASSNativeImpl
    {
        [DllImport("bass", EntryPoint = "BASS_ErrorGetCode", CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 INTERNAL_BASS_ErrorGetCode();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 BASS_ErrorGetCode() => INTERNAL_BASS_ErrorGetCode();
        
        [DllImport("bass", EntryPoint = "BASS_Init", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_Init(Int32 device, UInt32 freq, UInt32 flags, IntPtr win, IntPtr clsid);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Init(Int32 device, UInt32 freq, UInt32 flags, IntPtr win, IntPtr clsid) => INTERNAL_BASS_Init(device, freq, flags, win, clsid);
        
        [DllImport("bass", EntryPoint = "BASS_Free", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_Free();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Free() => INTERNAL_BASS_Free();
        
        [DllImport("bass", EntryPoint = "BASS_Update", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_Update(UInt32 length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Update(UInt32 length) => INTERNAL_BASS_Update(length);
        
        [DllImport("bass", EntryPoint = "BASS_SetDevice", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_SetDevice(UInt32 device);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SetDevice(UInt32 device) => INTERNAL_BASS_SetDevice(device);
        
        [DllImport("bass", EntryPoint = "BASS_GetDevice", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_GetDevice();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_GetDevice() => INTERNAL_BASS_GetDevice();
        
        [DllImport("bass", EntryPoint = "BASS_PluginLoad", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_PluginLoad([MarshalAs(UnmanagedType.LPStr)] String file, UInt32 flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_PluginLoad(String file, UInt32 flags) => INTERNAL_BASS_PluginLoad(file, flags);
        
        [DllImport("bass", EntryPoint = "BASS_PluginFree", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_PluginFree(UInt32 handle);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_PluginFree(UInt32 handle) => INTERNAL_BASS_PluginFree(handle);
        
        [DllImport("bass", EntryPoint = "BASS_GetConfig", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_GetConfig(UInt32 option);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_GetConfig(UInt32 option) => INTERNAL_BASS_GetConfig(option);
        
        [DllImport("bass", EntryPoint = "BASS_SetConfig", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_SetConfig(UInt32 option, UInt32 value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SetConfig(UInt32 option, UInt32 value) => INTERNAL_BASS_SetConfig(option, value);
        
        [DllImport("bass", EntryPoint = "BASS_GetVolume", CallingConvention = CallingConvention.StdCall)]
        private static extern Single INTERNAL_BASS_GetVolume();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Single BASS_GetVolume() => INTERNAL_BASS_GetVolume();
        
        [DllImport("bass", EntryPoint = "BASS_SetVolume", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_SetVolume(Single volume);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SetVolume(Single volume) => INTERNAL_BASS_SetVolume(volume);
        
        [DllImport("bass", EntryPoint = "BASS_StreamCreate", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, IntPtr proc, IntPtr user);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, IntPtr proc, IntPtr user) => INTERNAL_BASS_StreamCreate(freq, chans, flags, proc, user);
        
        [DllImport("bass", EntryPoint = "BASS_StreamCreateFile", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_StreamCreateFile(Boolean mem, [MarshalAs(UnmanagedType.LPStr)] String file, UInt64 offset, UInt64 length, UInt32 flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_StreamCreateFile(Boolean mem, String file, UInt64 offset, UInt64 length, UInt32 flags) => INTERNAL_BASS_StreamCreateFile(mem, file, offset, length, flags);
        
        [DllImport("bass", EntryPoint = "BASS_StreamCreateFileUser", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_StreamCreateFileUser(UInt32 system, UInt32 flags, BASS_FILEPROCS* procs, IntPtr user);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_StreamCreateFileUser(UInt32 system, UInt32 flags, BASS_FILEPROCS* procs, IntPtr user) => INTERNAL_BASS_StreamCreateFileUser(system, flags, procs, user);
        
        [DllImport("bass", EntryPoint = "BASS_StreamPutData", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_StreamPutData(UInt32 handle, IntPtr buffer, UInt32 length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_StreamPutData(UInt32 handle, IntPtr buffer, UInt32 length) => INTERNAL_BASS_StreamPutData(handle, buffer, length);
        
        [DllImport("bass", EntryPoint = "BASS_StreamFree", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_StreamFree(UInt32 handle);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_StreamFree(UInt32 handle) => INTERNAL_BASS_StreamFree(handle);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelSetDevice", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelSetDevice(UInt32 handle, UInt32 device);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelSetDevice(UInt32 handle, UInt32 device) => INTERNAL_BASS_ChannelSetDevice(handle, device);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelIsActive", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_ChannelIsActive(UInt32 handle);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_ChannelIsActive(UInt32 handle) => INTERNAL_BASS_ChannelIsActive(handle);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelIsSliding", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelIsSliding(UInt32 handle, UInt32 attrib);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelIsSliding(UInt32 handle, UInt32 attrib) => INTERNAL_BASS_ChannelIsSliding(handle, attrib);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelFlags", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_ChannelFlags(UInt32 handle, UInt32 flags, UInt32 mask);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_ChannelFlags(UInt32 handle, UInt32 flags, UInt32 mask) => INTERNAL_BASS_ChannelFlags(handle, flags, mask);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelGetInfo", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelGetInfo(UInt32 handle, out BASS_CHANNELINFO info);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelGetInfo(UInt32 handle, out BASS_CHANNELINFO info) => INTERNAL_BASS_ChannelGetInfo(handle, out info);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelBytes2Seconds", CallingConvention = CallingConvention.StdCall)]
        private static extern Double INTERNAL_BASS_ChannelBytes2Seconds(UInt32 handle, UInt64 pos);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Double BASS_ChannelBytes2Seconds(UInt32 handle, UInt64 pos) => INTERNAL_BASS_ChannelBytes2Seconds(handle, pos);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelSeconds2Bytes", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt64 INTERNAL_BASS_ChannelSeconds2Bytes(UInt32 handle, Double pos);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt64 BASS_ChannelSeconds2Bytes(UInt32 handle, Double pos) => INTERNAL_BASS_ChannelSeconds2Bytes(handle, pos);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelUpdate", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelUpdate(UInt32 handle, UInt32 length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelUpdate(UInt32 handle, UInt32 length) => INTERNAL_BASS_ChannelUpdate(handle, length);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelPlay", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelPlay(UInt32 handle, Boolean restart);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelPlay(UInt32 handle, Boolean restart) => INTERNAL_BASS_ChannelPlay(handle, restart);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelStop", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelStop(UInt32 handle);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelStop(UInt32 handle) => INTERNAL_BASS_ChannelStop(handle);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelPause", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelPause(UInt32 handle);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelPause(UInt32 handle) => INTERNAL_BASS_ChannelPause(handle);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelGetData", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_ChannelGetData(UInt32 handle, IntPtr buffer, UInt32 length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_ChannelGetData(UInt32 handle, IntPtr buffer, UInt32 length) => INTERNAL_BASS_ChannelGetData(handle, buffer, length);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelGetAttribute", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelGetAttribute(UInt32 handle, UInt32 attrib, Single* value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelGetAttribute(UInt32 handle, UInt32 attrib, Single* value) => INTERNAL_BASS_ChannelGetAttribute(handle, attrib, value);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelSetAttribute", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelSetAttribute(UInt32 handle, UInt32 attrib, Single value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelSetAttribute(UInt32 handle, UInt32 attrib, Single value) => INTERNAL_BASS_ChannelSetAttribute(handle, attrib, value);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelSlideAttribute", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelSlideAttribute(UInt32 handle, UInt32 attrib, Single value, UInt32 time);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelSlideAttribute(UInt32 handle, UInt32 attrib, Single value, UInt32 time) => INTERNAL_BASS_ChannelSlideAttribute(handle, attrib, value, time);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelGetPosition", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt64 INTERNAL_BASS_ChannelGetPosition(UInt32 handle, UInt32 mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt64 BASS_ChannelGetPosition(UInt32 handle, UInt32 mode) => INTERNAL_BASS_ChannelGetPosition(handle, mode);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelSetPosition", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelSetPosition(UInt32 handle, UInt64 pos, UInt32 mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelSetPosition(UInt32 handle, UInt64 pos, UInt32 mode) => INTERNAL_BASS_ChannelSetPosition(handle, pos, mode);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelGetLength", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt64 INTERNAL_BASS_ChannelGetLength(UInt32 handle, UInt32 mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt64 BASS_ChannelGetLength(UInt32 handle, UInt32 mode) => INTERNAL_BASS_ChannelGetLength(handle, mode);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelSetSync", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_ChannelSetSync(UInt32 handle, UInt32 type, UInt64 param, SyncProc proc, IntPtr user);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_ChannelSetSync(UInt32 handle, UInt32 type, UInt64 param, SyncProc proc, IntPtr user) => INTERNAL_BASS_ChannelSetSync(handle, type, param, proc, user);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelRemoveSync", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_ChannelRemoveSync(UInt32 handle, UInt32 sync);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelRemoveSync(UInt32 handle, UInt32 sync) => INTERNAL_BASS_ChannelRemoveSync(handle, sync);
        
        [DllImport("bass", EntryPoint = "BASS_ChannelGetTags", CallingConvention = CallingConvention.StdCall)]
        private static extern void* INTERNAL_BASS_ChannelGetTags(UInt32 handle, UInt32 tags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void* BASS_ChannelGetTags(UInt32 handle, UInt32 tags) => INTERNAL_BASS_ChannelGetTags(handle, tags);
        
        [DllImport("bass", EntryPoint = "BASS_SampleLoad", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_SampleLoad(Boolean mem, IntPtr file, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_SampleLoad(Boolean mem, IntPtr file, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags) => INTERNAL_BASS_SampleLoad(mem, file, offset, length, max, flags);
        
        [DllImport("bass", EntryPoint = "BASS_SampleFree", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_SampleFree(UInt32 handle);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SampleFree(UInt32 handle) => INTERNAL_BASS_SampleFree(handle);
        
        [DllImport("bass", EntryPoint = "BASS_SampleGetChannel", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_SampleGetChannel(UInt32 handle, Boolean onlynew);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_SampleGetChannel(UInt32 handle, Boolean onlynew) => INTERNAL_BASS_SampleGetChannel(handle, onlynew);
        
        [DllImport("bass", EntryPoint = "BASS_SampleGetInfo", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_SampleGetInfo(UInt32 handle, out BASS_SAMPLE info);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SampleGetInfo(UInt32 handle, out BASS_SAMPLE info) => INTERNAL_BASS_SampleGetInfo(handle, out info);
        
        [DllImport("bass", EntryPoint = "BASS_SampleGetData", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_SampleGetData(UInt32 handle, IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SampleGetData(UInt32 handle, IntPtr buffer) => INTERNAL_BASS_SampleGetData(handle, buffer);
        
        [DllImport("bass", EntryPoint = "BASS_Pause", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_Pause();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Pause() => INTERNAL_BASS_Pause();
        
        [DllImport("bass", EntryPoint = "BASS_Start", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_Start();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Start() => INTERNAL_BASS_Start();
        
        [DllImport("bass", EntryPoint = "BASS_GetDeviceInfo", CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean INTERNAL_BASS_GetDeviceInfo(UInt32 device, BASS_DEVICEINFO* info);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_GetDeviceInfo(UInt32 device, BASS_DEVICEINFO* info) => INTERNAL_BASS_GetDeviceInfo(device, info);
    }
#pragma warning restore 1591
}

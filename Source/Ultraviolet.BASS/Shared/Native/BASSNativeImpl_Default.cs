using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class BASSNativeImpl_Default : BASSNativeImpl
    {
        private static readonly NativeLibrary lib;
        
        static BASSNativeImpl_Default()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Linux:
                    lib = new NativeLibrary("libbass");
                    break;
                case UltravioletPlatform.macOS:
                    lib = new NativeLibrary("libbass");
                    break;
                default:
                    lib = new NativeLibrary("bass");
                    break;
            }
        }
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Int32 BASS_ErrorGetCodeDelegate();
        private readonly BASS_ErrorGetCodeDelegate pBASS_ErrorGetCode = lib.LoadFunction<BASS_ErrorGetCodeDelegate>("BASS_ErrorGetCode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 BASS_ErrorGetCode() => pBASS_ErrorGetCode();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_InitDelegate(Int32 device, UInt32 freq, UInt32 flags, IntPtr win, IntPtr clsid);
        private readonly BASS_InitDelegate pBASS_Init = lib.LoadFunction<BASS_InitDelegate>("BASS_Init");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Init(Int32 device, UInt32 freq, UInt32 flags, IntPtr win, IntPtr clsid) => pBASS_Init(device, freq, flags, win, clsid);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_FreeDelegate();
        private readonly BASS_FreeDelegate pBASS_Free = lib.LoadFunction<BASS_FreeDelegate>("BASS_Free");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Free() => pBASS_Free();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_UpdateDelegate(UInt32 length);
        private readonly BASS_UpdateDelegate pBASS_Update = lib.LoadFunction<BASS_UpdateDelegate>("BASS_Update");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Update(UInt32 length) => pBASS_Update(length);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_SetDeviceDelegate(UInt32 device);
        private readonly BASS_SetDeviceDelegate pBASS_SetDevice = lib.LoadFunction<BASS_SetDeviceDelegate>("BASS_SetDevice");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SetDevice(UInt32 device) => pBASS_SetDevice(device);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_GetDeviceDelegate();
        private readonly BASS_GetDeviceDelegate pBASS_GetDevice = lib.LoadFunction<BASS_GetDeviceDelegate>("BASS_GetDevice");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_GetDevice() => pBASS_GetDevice();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_PluginLoadDelegate(String file, UInt32 flags);
        private readonly BASS_PluginLoadDelegate pBASS_PluginLoad = lib.LoadFunction<BASS_PluginLoadDelegate>("BASS_PluginLoad");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_PluginLoad(String file, UInt32 flags) => pBASS_PluginLoad(file, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_PluginFreeDelegate(UInt32 handle);
        private readonly BASS_PluginFreeDelegate pBASS_PluginFree = lib.LoadFunction<BASS_PluginFreeDelegate>("BASS_PluginFree");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_PluginFree(UInt32 handle) => pBASS_PluginFree(handle);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_GetConfigDelegate(UInt32 option);
        private readonly BASS_GetConfigDelegate pBASS_GetConfig = lib.LoadFunction<BASS_GetConfigDelegate>("BASS_GetConfig");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_GetConfig(UInt32 option) => pBASS_GetConfig(option);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_SetConfigDelegate(UInt32 option, UInt32 value);
        private readonly BASS_SetConfigDelegate pBASS_SetConfig = lib.LoadFunction<BASS_SetConfigDelegate>("BASS_SetConfig");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SetConfig(UInt32 option, UInt32 value) => pBASS_SetConfig(option, value);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Single BASS_GetVolumeDelegate();
        private readonly BASS_GetVolumeDelegate pBASS_GetVolume = lib.LoadFunction<BASS_GetVolumeDelegate>("BASS_GetVolume");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Single BASS_GetVolume() => pBASS_GetVolume();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_SetVolumeDelegate(Single volume);
        private readonly BASS_SetVolumeDelegate pBASS_SetVolume = lib.LoadFunction<BASS_SetVolumeDelegate>("BASS_SetVolume");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SetVolume(Single volume) => pBASS_SetVolume(volume);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_StreamCreateDelegate(UInt32 freq, UInt32 chans, UInt32 flags, IntPtr proc, IntPtr user);
        private readonly BASS_StreamCreateDelegate pBASS_StreamCreate = lib.LoadFunction<BASS_StreamCreateDelegate>("BASS_StreamCreate");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_StreamCreate(UInt32 freq, UInt32 chans, UInt32 flags, IntPtr proc, IntPtr user) => pBASS_StreamCreate(freq, chans, flags, proc, user);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_StreamCreateFileDelegate(Boolean mem, String file, UInt64 offset, UInt64 length, UInt32 flags);
        private readonly BASS_StreamCreateFileDelegate pBASS_StreamCreateFile = lib.LoadFunction<BASS_StreamCreateFileDelegate>("BASS_StreamCreateFile");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_StreamCreateFile(Boolean mem, String file, UInt64 offset, UInt64 length, UInt32 flags) => pBASS_StreamCreateFile(mem, file, offset, length, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_StreamCreateFileUserDelegate(UInt32 system, UInt32 flags, BASS_FILEPROCS* procs, IntPtr user);
        private readonly BASS_StreamCreateFileUserDelegate pBASS_StreamCreateFileUser = lib.LoadFunction<BASS_StreamCreateFileUserDelegate>("BASS_StreamCreateFileUser");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_StreamCreateFileUser(UInt32 system, UInt32 flags, BASS_FILEPROCS* procs, IntPtr user) => pBASS_StreamCreateFileUser(system, flags, procs, user);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_StreamPutDataDelegate(UInt32 handle, IntPtr buffer, UInt32 length);
        private readonly BASS_StreamPutDataDelegate pBASS_StreamPutData = lib.LoadFunction<BASS_StreamPutDataDelegate>("BASS_StreamPutData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_StreamPutData(UInt32 handle, IntPtr buffer, UInt32 length) => pBASS_StreamPutData(handle, buffer, length);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_StreamFreeDelegate(UInt32 handle);
        private readonly BASS_StreamFreeDelegate pBASS_StreamFree = lib.LoadFunction<BASS_StreamFreeDelegate>("BASS_StreamFree");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_StreamFree(UInt32 handle) => pBASS_StreamFree(handle);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelSetDeviceDelegate(UInt32 handle, UInt32 device);
        private readonly BASS_ChannelSetDeviceDelegate pBASS_ChannelSetDevice = lib.LoadFunction<BASS_ChannelSetDeviceDelegate>("BASS_ChannelSetDevice");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelSetDevice(UInt32 handle, UInt32 device) => pBASS_ChannelSetDevice(handle, device);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_ChannelIsActiveDelegate(UInt32 handle);
        private readonly BASS_ChannelIsActiveDelegate pBASS_ChannelIsActive = lib.LoadFunction<BASS_ChannelIsActiveDelegate>("BASS_ChannelIsActive");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_ChannelIsActive(UInt32 handle) => pBASS_ChannelIsActive(handle);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelIsSlidingDelegate(UInt32 handle, UInt32 attrib);
        private readonly BASS_ChannelIsSlidingDelegate pBASS_ChannelIsSliding = lib.LoadFunction<BASS_ChannelIsSlidingDelegate>("BASS_ChannelIsSliding");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelIsSliding(UInt32 handle, UInt32 attrib) => pBASS_ChannelIsSliding(handle, attrib);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_ChannelFlagsDelegate(UInt32 handle, UInt32 flags, UInt32 mask);
        private readonly BASS_ChannelFlagsDelegate pBASS_ChannelFlags = lib.LoadFunction<BASS_ChannelFlagsDelegate>("BASS_ChannelFlags");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_ChannelFlags(UInt32 handle, UInt32 flags, UInt32 mask) => pBASS_ChannelFlags(handle, flags, mask);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelGetInfoDelegate(UInt32 handle, out BASS_CHANNELINFO info);
        private readonly BASS_ChannelGetInfoDelegate pBASS_ChannelGetInfo = lib.LoadFunction<BASS_ChannelGetInfoDelegate>("BASS_ChannelGetInfo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelGetInfo(UInt32 handle, out BASS_CHANNELINFO info) => pBASS_ChannelGetInfo(handle, out info);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Double BASS_ChannelBytes2SecondsDelegate(UInt32 handle, UInt64 pos);
        private readonly BASS_ChannelBytes2SecondsDelegate pBASS_ChannelBytes2Seconds = lib.LoadFunction<BASS_ChannelBytes2SecondsDelegate>("BASS_ChannelBytes2Seconds");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Double BASS_ChannelBytes2Seconds(UInt32 handle, UInt64 pos) => pBASS_ChannelBytes2Seconds(handle, pos);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt64 BASS_ChannelSeconds2BytesDelegate(UInt32 handle, Double pos);
        private readonly BASS_ChannelSeconds2BytesDelegate pBASS_ChannelSeconds2Bytes = lib.LoadFunction<BASS_ChannelSeconds2BytesDelegate>("BASS_ChannelSeconds2Bytes");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt64 BASS_ChannelSeconds2Bytes(UInt32 handle, Double pos) => pBASS_ChannelSeconds2Bytes(handle, pos);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelUpdateDelegate(UInt32 handle, UInt32 length);
        private readonly BASS_ChannelUpdateDelegate pBASS_ChannelUpdate = lib.LoadFunction<BASS_ChannelUpdateDelegate>("BASS_ChannelUpdate");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelUpdate(UInt32 handle, UInt32 length) => pBASS_ChannelUpdate(handle, length);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelPlayDelegate(UInt32 handle, Boolean restart);
        private readonly BASS_ChannelPlayDelegate pBASS_ChannelPlay = lib.LoadFunction<BASS_ChannelPlayDelegate>("BASS_ChannelPlay");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelPlay(UInt32 handle, Boolean restart) => pBASS_ChannelPlay(handle, restart);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelStopDelegate(UInt32 handle);
        private readonly BASS_ChannelStopDelegate pBASS_ChannelStop = lib.LoadFunction<BASS_ChannelStopDelegate>("BASS_ChannelStop");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelStop(UInt32 handle) => pBASS_ChannelStop(handle);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelPauseDelegate(UInt32 handle);
        private readonly BASS_ChannelPauseDelegate pBASS_ChannelPause = lib.LoadFunction<BASS_ChannelPauseDelegate>("BASS_ChannelPause");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelPause(UInt32 handle) => pBASS_ChannelPause(handle);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_ChannelGetDataDelegate(UInt32 handle, IntPtr buffer, UInt32 length);
        private readonly BASS_ChannelGetDataDelegate pBASS_ChannelGetData = lib.LoadFunction<BASS_ChannelGetDataDelegate>("BASS_ChannelGetData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_ChannelGetData(UInt32 handle, IntPtr buffer, UInt32 length) => pBASS_ChannelGetData(handle, buffer, length);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelGetAttributeDelegate(UInt32 handle, UInt32 attrib, Single* value);
        private readonly BASS_ChannelGetAttributeDelegate pBASS_ChannelGetAttribute = lib.LoadFunction<BASS_ChannelGetAttributeDelegate>("BASS_ChannelGetAttribute");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelGetAttribute(UInt32 handle, UInt32 attrib, Single* value) => pBASS_ChannelGetAttribute(handle, attrib, value);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelSetAttributeDelegate(UInt32 handle, UInt32 attrib, Single value);
        private readonly BASS_ChannelSetAttributeDelegate pBASS_ChannelSetAttribute = lib.LoadFunction<BASS_ChannelSetAttributeDelegate>("BASS_ChannelSetAttribute");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelSetAttribute(UInt32 handle, UInt32 attrib, Single value) => pBASS_ChannelSetAttribute(handle, attrib, value);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelSlideAttributeDelegate(UInt32 handle, UInt32 attrib, Single value, UInt32 time);
        private readonly BASS_ChannelSlideAttributeDelegate pBASS_ChannelSlideAttribute = lib.LoadFunction<BASS_ChannelSlideAttributeDelegate>("BASS_ChannelSlideAttribute");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelSlideAttribute(UInt32 handle, UInt32 attrib, Single value, UInt32 time) => pBASS_ChannelSlideAttribute(handle, attrib, value, time);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt64 BASS_ChannelGetPositionDelegate(UInt32 handle, UInt32 mode);
        private readonly BASS_ChannelGetPositionDelegate pBASS_ChannelGetPosition = lib.LoadFunction<BASS_ChannelGetPositionDelegate>("BASS_ChannelGetPosition");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt64 BASS_ChannelGetPosition(UInt32 handle, UInt32 mode) => pBASS_ChannelGetPosition(handle, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelSetPositionDelegate(UInt32 handle, UInt64 pos, UInt32 mode);
        private readonly BASS_ChannelSetPositionDelegate pBASS_ChannelSetPosition = lib.LoadFunction<BASS_ChannelSetPositionDelegate>("BASS_ChannelSetPosition");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelSetPosition(UInt32 handle, UInt64 pos, UInt32 mode) => pBASS_ChannelSetPosition(handle, pos, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt64 BASS_ChannelGetLengthDelegate(UInt32 handle, UInt32 mode);
        private readonly BASS_ChannelGetLengthDelegate pBASS_ChannelGetLength = lib.LoadFunction<BASS_ChannelGetLengthDelegate>("BASS_ChannelGetLength");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt64 BASS_ChannelGetLength(UInt32 handle, UInt32 mode) => pBASS_ChannelGetLength(handle, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_ChannelSetSyncDelegate(UInt32 handle, UInt32 type, UInt64 param, SyncProc proc, IntPtr user);
        private readonly BASS_ChannelSetSyncDelegate pBASS_ChannelSetSync = lib.LoadFunction<BASS_ChannelSetSyncDelegate>("BASS_ChannelSetSync");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_ChannelSetSync(UInt32 handle, UInt32 type, UInt64 param, SyncProc proc, IntPtr user) => pBASS_ChannelSetSync(handle, type, param, proc, user);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_ChannelRemoveSyncDelegate(UInt32 handle, UInt32 sync);
        private readonly BASS_ChannelRemoveSyncDelegate pBASS_ChannelRemoveSync = lib.LoadFunction<BASS_ChannelRemoveSyncDelegate>("BASS_ChannelRemoveSync");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_ChannelRemoveSync(UInt32 handle, UInt32 sync) => pBASS_ChannelRemoveSync(handle, sync);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void* BASS_ChannelGetTagsDelegate(UInt32 handle, UInt32 tags);
        private readonly BASS_ChannelGetTagsDelegate pBASS_ChannelGetTags = lib.LoadFunction<BASS_ChannelGetTagsDelegate>("BASS_ChannelGetTags");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void* BASS_ChannelGetTags(UInt32 handle, UInt32 tags) => pBASS_ChannelGetTags(handle, tags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_SampleLoadDelegate(Boolean mem, IntPtr file, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags);
        private readonly BASS_SampleLoadDelegate pBASS_SampleLoad = lib.LoadFunction<BASS_SampleLoadDelegate>("BASS_SampleLoad");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_SampleLoad(Boolean mem, IntPtr file, UInt64 offset, UInt32 length, UInt32 max, UInt32 flags) => pBASS_SampleLoad(mem, file, offset, length, max, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_SampleFreeDelegate(UInt32 handle);
        private readonly BASS_SampleFreeDelegate pBASS_SampleFree = lib.LoadFunction<BASS_SampleFreeDelegate>("BASS_SampleFree");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SampleFree(UInt32 handle) => pBASS_SampleFree(handle);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_SampleGetChannelDelegate(UInt32 handle, Boolean onlynew);
        private readonly BASS_SampleGetChannelDelegate pBASS_SampleGetChannel = lib.LoadFunction<BASS_SampleGetChannelDelegate>("BASS_SampleGetChannel");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_SampleGetChannel(UInt32 handle, Boolean onlynew) => pBASS_SampleGetChannel(handle, onlynew);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_SampleGetInfoDelegate(UInt32 handle, out BASS_SAMPLE info);
        private readonly BASS_SampleGetInfoDelegate pBASS_SampleGetInfo = lib.LoadFunction<BASS_SampleGetInfoDelegate>("BASS_SampleGetInfo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SampleGetInfo(UInt32 handle, out BASS_SAMPLE info) => pBASS_SampleGetInfo(handle, out info);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_SampleGetDataDelegate(UInt32 handle, IntPtr buffer);
        private readonly BASS_SampleGetDataDelegate pBASS_SampleGetData = lib.LoadFunction<BASS_SampleGetDataDelegate>("BASS_SampleGetData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_SampleGetData(UInt32 handle, IntPtr buffer) => pBASS_SampleGetData(handle, buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_PauseDelegate();
        private readonly BASS_PauseDelegate pBASS_Pause = lib.LoadFunction<BASS_PauseDelegate>("BASS_Pause");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Pause() => pBASS_Pause();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_StartDelegate();
        private readonly BASS_StartDelegate pBASS_Start = lib.LoadFunction<BASS_StartDelegate>("BASS_Start");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_Start() => pBASS_Start();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Boolean BASS_GetDeviceInfoDelegate(UInt32 device, BASS_DEVICEINFO* info);
        private readonly BASS_GetDeviceInfoDelegate pBASS_GetDeviceInfo = lib.LoadFunction<BASS_GetDeviceInfoDelegate>("BASS_GetDeviceInfo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean BASS_GetDeviceInfo(UInt32 device, BASS_DEVICEINFO* info) => pBASS_GetDeviceInfo(device, info);
    }
#pragma warning restore 1591
}

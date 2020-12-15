using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;
using System.Text;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class FMODNativeImpl_Default : FMODNativeImpl
    {
        private static readonly NativeLibrary lib;
        
        static FMODNativeImpl_Default()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Linux:
                    lib = new NativeLibrary(new[] { "libfmodL", "libfmod" });
                    break;
                case UltravioletPlatform.macOS:
                    lib = new NativeLibrary(new[] { "libfmodL", "libfmod" });
                    break;
                default:
                    lib = new NativeLibrary(new[] { "fmodL", "fmod" });
                    break;
            }
        }
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Debug_InitializeDelegate(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, String filename);
        private readonly FMOD_Debug_InitializeDelegate pFMOD_Debug_Initialize = lib.LoadFunction<FMOD_Debug_InitializeDelegate>("FMOD_Debug_Initialize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Debug_Initialize(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, String filename) => pFMOD_Debug_Initialize(flags, mode, callback, filename);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CreateDelegate(FMOD_SYSTEM** system);
        private readonly FMOD_System_CreateDelegate pFMOD_System_Create = lib.LoadFunction<FMOD_System_CreateDelegate>("FMOD_System_Create");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Create(FMOD_SYSTEM** system) => pFMOD_System_Create(system);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_ReleaseDelegate(FMOD_SYSTEM* system);
        private readonly FMOD_System_ReleaseDelegate pFMOD_System_Release = lib.LoadFunction<FMOD_System_ReleaseDelegate>("FMOD_System_Release");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Release(FMOD_SYSTEM* system) => pFMOD_System_Release(system);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_GetVersionDelegate(FMOD_SYSTEM* system, UInt32* version);
        private readonly FMOD_System_GetVersionDelegate pFMOD_System_GetVersion = lib.LoadFunction<FMOD_System_GetVersionDelegate>("FMOD_System_GetVersion");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_GetVersion(FMOD_SYSTEM* system, UInt32* version) => pFMOD_System_GetVersion(system, version);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_GetNumDriversDelegate(FMOD_SYSTEM* system, Int32* numdrivers);
        private readonly FMOD_System_GetNumDriversDelegate pFMOD_System_GetNumDrivers = lib.LoadFunction<FMOD_System_GetNumDriversDelegate>("FMOD_System_GetNumDrivers");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_GetNumDrivers(FMOD_SYSTEM* system, Int32* numdrivers) => pFMOD_System_GetNumDrivers(system, numdrivers);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_GetDriverInfoDelegate(FMOD_SYSTEM* system, Int32 id, StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels);
        private readonly FMOD_System_GetDriverInfoDelegate pFMOD_System_GetDriverInfo = lib.LoadFunction<FMOD_System_GetDriverInfoDelegate>("FMOD_System_GetDriverInfo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_GetDriverInfo(FMOD_SYSTEM* system, Int32 id, StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels) => pFMOD_System_GetDriverInfo(system, id, name, namelen, guid, systemrate, speakermode, speakermodechannels);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_SetDriverDelegate(FMOD_SYSTEM* system, Int32 driver);
        private readonly FMOD_System_SetDriverDelegate pFMOD_System_SetDriver = lib.LoadFunction<FMOD_System_SetDriverDelegate>("FMOD_System_SetDriver");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_SetDriver(FMOD_SYSTEM* system, Int32 driver) => pFMOD_System_SetDriver(system, driver);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_GetDriverDelegate(FMOD_SYSTEM* system, Int32* driver);
        private readonly FMOD_System_GetDriverDelegate pFMOD_System_GetDriver = lib.LoadFunction<FMOD_System_GetDriverDelegate>("FMOD_System_GetDriver");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_GetDriver(FMOD_SYSTEM* system, Int32* driver) => pFMOD_System_GetDriver(system, driver);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_InitDelegate(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata);
        private readonly FMOD_System_InitDelegate pFMOD_System_Init = lib.LoadFunction<FMOD_System_InitDelegate>("FMOD_System_Init");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Init(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata) => pFMOD_System_Init(system, maxchannels, flags, extradriverdata);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_PlaySoundDelegate(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel);
        private readonly FMOD_System_PlaySoundDelegate pFMOD_System_PlaySound = lib.LoadFunction<FMOD_System_PlaySoundDelegate>("FMOD_System_PlaySound");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_PlaySound(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel) => pFMOD_System_PlaySound(system, sound, channelgroup, paused, channel);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CloseDelegate(FMOD_SYSTEM* system);
        private readonly FMOD_System_CloseDelegate pFMOD_System_Close = lib.LoadFunction<FMOD_System_CloseDelegate>("FMOD_System_Close");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Close(FMOD_SYSTEM* system) => pFMOD_System_Close(system);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_UpdateDelegate(FMOD_SYSTEM* system);
        private readonly FMOD_System_UpdateDelegate pFMOD_System_Update = lib.LoadFunction<FMOD_System_UpdateDelegate>("FMOD_System_Update");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Update(FMOD_SYSTEM* system) => pFMOD_System_Update(system);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_SetCallbackDelegate(FMOD_SYSTEM* system, FMOD_SYSTEM_CALLBACK callback, FMOD_SYSTEM_CALLBACK_TYPE callbackmask);
        private readonly FMOD_System_SetCallbackDelegate pFMOD_System_SetCallback = lib.LoadFunction<FMOD_System_SetCallbackDelegate>("FMOD_System_SetCallback");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_SetCallback(FMOD_SYSTEM* system, FMOD_SYSTEM_CALLBACK callback, FMOD_SYSTEM_CALLBACK_TYPE callbackmask) => pFMOD_System_SetCallback(system, callback, callbackmask);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_MixerSuspendDelegate(FMOD_SYSTEM* system);
        private readonly FMOD_System_MixerSuspendDelegate pFMOD_System_MixerSuspend = lib.LoadFunction<FMOD_System_MixerSuspendDelegate>("FMOD_System_MixerSuspend");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_MixerSuspend(FMOD_SYSTEM* system) => pFMOD_System_MixerSuspend(system);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_MixerResumeDelegate(FMOD_SYSTEM* system);
        private readonly FMOD_System_MixerResumeDelegate pFMOD_System_MixerResume = lib.LoadFunction<FMOD_System_MixerResumeDelegate>("FMOD_System_MixerResume");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_MixerResume(FMOD_SYSTEM* system) => pFMOD_System_MixerResume(system);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CreateSoundDelegate(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
        private readonly FMOD_System_CreateSoundDelegate pFMOD_System_CreateSound = lib.LoadFunction<FMOD_System_CreateSoundDelegate>("FMOD_System_CreateSound");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_CreateSound(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound) => pFMOD_System_CreateSound(system, name_or_data, mode, exinfo, sound);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CreateStreamDelegate(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
        private readonly FMOD_System_CreateStreamDelegate pFMOD_System_CreateStream = lib.LoadFunction<FMOD_System_CreateStreamDelegate>("FMOD_System_CreateStream");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_CreateStream(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound) => pFMOD_System_CreateStream(system, name_or_data, mode, exinfo, sound);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CreateChannelGroupDelegate(FMOD_SYSTEM* system, String name, FMOD_CHANNELGROUP** channelgroup);
        private readonly FMOD_System_CreateChannelGroupDelegate pFMOD_System_CreateChannelGroup = lib.LoadFunction<FMOD_System_CreateChannelGroupDelegate>("FMOD_System_CreateChannelGroup");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_CreateChannelGroup(FMOD_SYSTEM* system, String name, FMOD_CHANNELGROUP** channelgroup) => pFMOD_System_CreateChannelGroup(system, name, channelgroup);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_SetFileSystemDelegate(FMOD_SYSTEM* system, FMOD_FILE_OPEN_CALLBACK useropen, FMOD_FILE_CLOSE_CALLBACK userclose, FMOD_FILE_READ_CALLBACK userread, FMOD_FILE_SEEK_CALLBACK userseek, FMOD_FILE_ASYNCREAD_CALLBACK userasyncread, FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 bloackalign);
        private readonly FMOD_System_SetFileSystemDelegate pFMOD_System_SetFileSystem = lib.LoadFunction<FMOD_System_SetFileSystemDelegate>("FMOD_System_SetFileSystem");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_SetFileSystem(FMOD_SYSTEM* system, FMOD_FILE_OPEN_CALLBACK useropen, FMOD_FILE_CLOSE_CALLBACK userclose, FMOD_FILE_READ_CALLBACK userread, FMOD_FILE_SEEK_CALLBACK userseek, FMOD_FILE_ASYNCREAD_CALLBACK userasyncread, FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 bloackalign) => pFMOD_System_SetFileSystem(system, useropen, userclose, userread, userseek, userasyncread, userasynccancel, bloackalign);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Sound_GetLengthDelegate(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype);
        private readonly FMOD_Sound_GetLengthDelegate pFMOD_Sound_GetLength = lib.LoadFunction<FMOD_Sound_GetLengthDelegate>("FMOD_Sound_GetLength");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Sound_GetLength(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype) => pFMOD_Sound_GetLength(sound, length, lengthtype);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Sound_GetNumTagsDelegate(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated);
        private readonly FMOD_Sound_GetNumTagsDelegate pFMOD_Sound_GetNumTags = lib.LoadFunction<FMOD_Sound_GetNumTagsDelegate>("FMOD_Sound_GetNumTags");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Sound_GetNumTags(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated) => pFMOD_Sound_GetNumTags(sound, numtags, numtagsupdated);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Sound_GetTagDelegate(FMOD_SOUND* sound, String name, Int32 index, FMOD_TAG* tag);
        private readonly FMOD_Sound_GetTagDelegate pFMOD_Sound_GetTag = lib.LoadFunction<FMOD_Sound_GetTagDelegate>("FMOD_Sound_GetTag");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Sound_GetTag(FMOD_SOUND* sound, String name, Int32 index, FMOD_TAG* tag) => pFMOD_Sound_GetTag(sound, name, index, tag);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Sound_ReleaseDelegate(FMOD_SOUND* sound);
        private readonly FMOD_Sound_ReleaseDelegate pFMOD_Sound_Release = lib.LoadFunction<FMOD_Sound_ReleaseDelegate>("FMOD_Sound_Release");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Sound_Release(FMOD_SOUND* sound) => pFMOD_Sound_Release(sound);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_ChannelGroup_SetVolumeDelegate(FMOD_CHANNELGROUP* channelgroup, Single volume);
        private readonly FMOD_ChannelGroup_SetVolumeDelegate pFMOD_ChannelGroup_SetVolume = lib.LoadFunction<FMOD_ChannelGroup_SetVolumeDelegate>("FMOD_ChannelGroup_SetVolume");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_ChannelGroup_SetVolume(FMOD_CHANNELGROUP* channelgroup, Single volume) => pFMOD_ChannelGroup_SetVolume(channelgroup, volume);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_ChannelGroup_ReleaseDelegate(FMOD_CHANNELGROUP* channelgroup);
        private readonly FMOD_ChannelGroup_ReleaseDelegate pFMOD_ChannelGroup_Release = lib.LoadFunction<FMOD_ChannelGroup_ReleaseDelegate>("FMOD_ChannelGroup_Release");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_ChannelGroup_Release(FMOD_CHANNELGROUP* channelgroup) => pFMOD_ChannelGroup_Release(channelgroup);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_StopDelegate(FMOD_CHANNEL* channel);
        private readonly FMOD_Channel_StopDelegate pFMOD_Channel_Stop = lib.LoadFunction<FMOD_Channel_StopDelegate>("FMOD_Channel_Stop");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_Stop(FMOD_CHANNEL* channel) => pFMOD_Channel_Stop(channel);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetPausedDelegate(FMOD_CHANNEL* channel, Boolean paused);
        private readonly FMOD_Channel_SetPausedDelegate pFMOD_Channel_SetPaused = lib.LoadFunction<FMOD_Channel_SetPausedDelegate>("FMOD_Channel_SetPaused");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetPaused(FMOD_CHANNEL* channel, Boolean paused) => pFMOD_Channel_SetPaused(channel, paused);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetPausedDelegate(FMOD_CHANNEL* channel, Boolean* paused);
        private readonly FMOD_Channel_GetPausedDelegate pFMOD_Channel_GetPaused = lib.LoadFunction<FMOD_Channel_GetPausedDelegate>("FMOD_Channel_GetPaused");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetPaused(FMOD_CHANNEL* channel, Boolean* paused) => pFMOD_Channel_GetPaused(channel, paused);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetVolumeDelegate(FMOD_CHANNEL* channel, Single volume);
        private readonly FMOD_Channel_SetVolumeDelegate pFMOD_Channel_SetVolume = lib.LoadFunction<FMOD_Channel_SetVolumeDelegate>("FMOD_Channel_SetVolume");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetVolume(FMOD_CHANNEL* channel, Single volume) => pFMOD_Channel_SetVolume(channel, volume);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetVolumeDelegate(FMOD_CHANNEL* channel, Single* volume);
        private readonly FMOD_Channel_GetVolumeDelegate pFMOD_Channel_GetVolume = lib.LoadFunction<FMOD_Channel_GetVolumeDelegate>("FMOD_Channel_GetVolume");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetVolume(FMOD_CHANNEL* channel, Single* volume) => pFMOD_Channel_GetVolume(channel, volume);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetVolumeRampDelegate(FMOD_CHANNEL* channel, Boolean ramp);
        private readonly FMOD_Channel_SetVolumeRampDelegate pFMOD_Channel_SetVolumeRamp = lib.LoadFunction<FMOD_Channel_SetVolumeRampDelegate>("FMOD_Channel_SetVolumeRamp");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetVolumeRamp(FMOD_CHANNEL* channel, Boolean ramp) => pFMOD_Channel_SetVolumeRamp(channel, ramp);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetVolumeRampDelegate(FMOD_CHANNEL* channel, Boolean* ramp);
        private readonly FMOD_Channel_GetVolumeRampDelegate pFMOD_Channel_GetVolumeRamp = lib.LoadFunction<FMOD_Channel_GetVolumeRampDelegate>("FMOD_Channel_GetVolumeRamp");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetVolumeRamp(FMOD_CHANNEL* channel, Boolean* ramp) => pFMOD_Channel_GetVolumeRamp(channel, ramp);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetAudibilityDelegate(FMOD_CHANNEL* channel, Single* audibility);
        private readonly FMOD_Channel_GetAudibilityDelegate pFMOD_Channel_GetAudibility = lib.LoadFunction<FMOD_Channel_GetAudibilityDelegate>("FMOD_Channel_GetAudibility");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetAudibility(FMOD_CHANNEL* channel, Single* audibility) => pFMOD_Channel_GetAudibility(channel, audibility);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetPitchDelegate(FMOD_CHANNEL* channel, Single pitch);
        private readonly FMOD_Channel_SetPitchDelegate pFMOD_Channel_SetPitch = lib.LoadFunction<FMOD_Channel_SetPitchDelegate>("FMOD_Channel_SetPitch");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetPitch(FMOD_CHANNEL* channel, Single pitch) => pFMOD_Channel_SetPitch(channel, pitch);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetPitchDelegate(FMOD_CHANNEL* channel, Single* pitch);
        private readonly FMOD_Channel_GetPitchDelegate pFMOD_Channel_GetPitch = lib.LoadFunction<FMOD_Channel_GetPitchDelegate>("FMOD_Channel_GetPitch");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetPitch(FMOD_CHANNEL* channel, Single* pitch) => pFMOD_Channel_GetPitch(channel, pitch);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetMuteDelegate(FMOD_CHANNEL* channel, Boolean mute);
        private readonly FMOD_Channel_SetMuteDelegate pFMOD_Channel_SetMute = lib.LoadFunction<FMOD_Channel_SetMuteDelegate>("FMOD_Channel_SetMute");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetMute(FMOD_CHANNEL* channel, Boolean mute) => pFMOD_Channel_SetMute(channel, mute);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetMuteDelegate(FMOD_CHANNEL* channel, Boolean* mute);
        private readonly FMOD_Channel_GetMuteDelegate pFMOD_Channel_GetMute = lib.LoadFunction<FMOD_Channel_GetMuteDelegate>("FMOD_Channel_GetMute");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetMute(FMOD_CHANNEL* channel, Boolean* mute) => pFMOD_Channel_GetMute(channel, mute);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetModeDelegate(FMOD_CHANNEL* channel, FMOD_MODE mode);
        private readonly FMOD_Channel_SetModeDelegate pFMOD_Channel_SetMode = lib.LoadFunction<FMOD_Channel_SetModeDelegate>("FMOD_Channel_SetMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetMode(FMOD_CHANNEL* channel, FMOD_MODE mode) => pFMOD_Channel_SetMode(channel, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetModeDelegate(FMOD_CHANNEL* channel, FMOD_MODE* mode);
        private readonly FMOD_Channel_GetModeDelegate pFMOD_Channel_GetMode = lib.LoadFunction<FMOD_Channel_GetModeDelegate>("FMOD_Channel_GetMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetMode(FMOD_CHANNEL* channel, FMOD_MODE* mode) => pFMOD_Channel_GetMode(channel, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_IsPlayingDelegate(FMOD_CHANNEL* channel, Boolean* isplaying);
        private readonly FMOD_Channel_IsPlayingDelegate pFMOD_Channel_IsPlaying = lib.LoadFunction<FMOD_Channel_IsPlayingDelegate>("FMOD_Channel_IsPlaying");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_IsPlaying(FMOD_CHANNEL* channel, Boolean* isplaying) => pFMOD_Channel_IsPlaying(channel, isplaying);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetPanDelegate(FMOD_CHANNEL* channel, Single pan);
        private readonly FMOD_Channel_SetPanDelegate pFMOD_Channel_SetPan = lib.LoadFunction<FMOD_Channel_SetPanDelegate>("FMOD_Channel_SetPan");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetPan(FMOD_CHANNEL* channel, Single pan) => pFMOD_Channel_SetPan(channel, pan);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetPositionDelegate(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype);
        private readonly FMOD_Channel_SetPositionDelegate pFMOD_Channel_SetPosition = lib.LoadFunction<FMOD_Channel_SetPositionDelegate>("FMOD_Channel_SetPosition");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetPosition(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype) => pFMOD_Channel_SetPosition(channel, position, postype);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetPositionDelegate(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype);
        private readonly FMOD_Channel_GetPositionDelegate pFMOD_Channel_GetPosition = lib.LoadFunction<FMOD_Channel_GetPositionDelegate>("FMOD_Channel_GetPosition");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetPosition(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype) => pFMOD_Channel_GetPosition(channel, position, postype);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetLoopPointsDelegate(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype);
        private readonly FMOD_Channel_SetLoopPointsDelegate pFMOD_Channel_SetLoopPoints = lib.LoadFunction<FMOD_Channel_SetLoopPointsDelegate>("FMOD_Channel_SetLoopPoints");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetLoopPoints(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype) => pFMOD_Channel_SetLoopPoints(channel, loopstart, loopstarttype, loopend, loopendtype);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetLoopPointsDelegate(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype);
        private readonly FMOD_Channel_GetLoopPointsDelegate pFMOD_Channel_GetLoopPoints = lib.LoadFunction<FMOD_Channel_GetLoopPointsDelegate>("FMOD_Channel_GetLoopPoints");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetLoopPoints(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype) => pFMOD_Channel_GetLoopPoints(channel, loopstart, loopstarttype, loopend, loopendtype);
    }
#pragma warning restore 1591
}

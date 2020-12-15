using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class FMODNativeImpl_Android : FMODNativeImpl
    {
        [DllImport("fmod", EntryPoint = "FMOD_Debug_Initialize", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Debug_Initialize(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, [MarshalAs(UnmanagedType.LPStr)] String filename);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Debug_Initialize(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, String filename) => INTERNAL_FMOD_Debug_Initialize(flags, mode, callback, filename);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_Create", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_Create(FMOD_SYSTEM** system);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Create(FMOD_SYSTEM** system) => INTERNAL_FMOD_System_Create(system);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_Release", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_Release(FMOD_SYSTEM* system);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Release(FMOD_SYSTEM* system) => INTERNAL_FMOD_System_Release(system);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_GetVersion", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_GetVersion(FMOD_SYSTEM* system, UInt32* version);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_GetVersion(FMOD_SYSTEM* system, UInt32* version) => INTERNAL_FMOD_System_GetVersion(system, version);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_GetNumDrivers", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_GetNumDrivers(FMOD_SYSTEM* system, Int32* numdrivers);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_GetNumDrivers(FMOD_SYSTEM* system, Int32* numdrivers) => INTERNAL_FMOD_System_GetNumDrivers(system, numdrivers);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_GetDriverInfo", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_GetDriverInfo(FMOD_SYSTEM* system, Int32 id, [MarshalAs(UnmanagedType.LPStr)] StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_GetDriverInfo(FMOD_SYSTEM* system, Int32 id, StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels) => INTERNAL_FMOD_System_GetDriverInfo(system, id, name, namelen, guid, systemrate, speakermode, speakermodechannels);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_SetDriver", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_SetDriver(FMOD_SYSTEM* system, Int32 driver);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_SetDriver(FMOD_SYSTEM* system, Int32 driver) => INTERNAL_FMOD_System_SetDriver(system, driver);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_GetDriver", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_GetDriver(FMOD_SYSTEM* system, Int32* driver);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_GetDriver(FMOD_SYSTEM* system, Int32* driver) => INTERNAL_FMOD_System_GetDriver(system, driver);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_Init", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_Init(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Init(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata) => INTERNAL_FMOD_System_Init(system, maxchannels, flags, extradriverdata);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_PlaySound", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_PlaySound(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_PlaySound(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel) => INTERNAL_FMOD_System_PlaySound(system, sound, channelgroup, paused, channel);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_Close", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_Close(FMOD_SYSTEM* system);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Close(FMOD_SYSTEM* system) => INTERNAL_FMOD_System_Close(system);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_Update", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_Update(FMOD_SYSTEM* system);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_Update(FMOD_SYSTEM* system) => INTERNAL_FMOD_System_Update(system);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_SetCallback", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_SetCallback(FMOD_SYSTEM* system, FMOD_SYSTEM_CALLBACK callback, FMOD_SYSTEM_CALLBACK_TYPE callbackmask);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_SetCallback(FMOD_SYSTEM* system, FMOD_SYSTEM_CALLBACK callback, FMOD_SYSTEM_CALLBACK_TYPE callbackmask) => INTERNAL_FMOD_System_SetCallback(system, callback, callbackmask);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_MixerSuspend", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_MixerSuspend(FMOD_SYSTEM* system);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_MixerSuspend(FMOD_SYSTEM* system) => INTERNAL_FMOD_System_MixerSuspend(system);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_MixerResume", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_MixerResume(FMOD_SYSTEM* system);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_MixerResume(FMOD_SYSTEM* system) => INTERNAL_FMOD_System_MixerResume(system);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_CreateSound", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_CreateSound(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_CreateSound(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound) => INTERNAL_FMOD_System_CreateSound(system, name_or_data, mode, exinfo, sound);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_CreateStream", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_CreateStream(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_CreateStream(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound) => INTERNAL_FMOD_System_CreateStream(system, name_or_data, mode, exinfo, sound);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_CreateChannelGroup", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_CreateChannelGroup(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name, FMOD_CHANNELGROUP** channelgroup);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_CreateChannelGroup(FMOD_SYSTEM* system, String name, FMOD_CHANNELGROUP** channelgroup) => INTERNAL_FMOD_System_CreateChannelGroup(system, name, channelgroup);
        
        [DllImport("fmod", EntryPoint = "FMOD_System_SetFileSystem", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_System_SetFileSystem(FMOD_SYSTEM* system, FMOD_FILE_OPEN_CALLBACK useropen, FMOD_FILE_CLOSE_CALLBACK userclose, FMOD_FILE_READ_CALLBACK userread, FMOD_FILE_SEEK_CALLBACK userseek, FMOD_FILE_ASYNCREAD_CALLBACK userasyncread, FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 bloackalign);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_System_SetFileSystem(FMOD_SYSTEM* system, FMOD_FILE_OPEN_CALLBACK useropen, FMOD_FILE_CLOSE_CALLBACK userclose, FMOD_FILE_READ_CALLBACK userread, FMOD_FILE_SEEK_CALLBACK userseek, FMOD_FILE_ASYNCREAD_CALLBACK userasyncread, FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 bloackalign) => INTERNAL_FMOD_System_SetFileSystem(system, useropen, userclose, userread, userseek, userasyncread, userasynccancel, bloackalign);
        
        [DllImport("fmod", EntryPoint = "FMOD_Sound_GetLength", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Sound_GetLength(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Sound_GetLength(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype) => INTERNAL_FMOD_Sound_GetLength(sound, length, lengthtype);
        
        [DllImport("fmod", EntryPoint = "FMOD_Sound_GetNumTags", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Sound_GetNumTags(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Sound_GetNumTags(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated) => INTERNAL_FMOD_Sound_GetNumTags(sound, numtags, numtagsupdated);
        
        [DllImport("fmod", EntryPoint = "FMOD_Sound_GetTag", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Sound_GetTag(FMOD_SOUND* sound, [MarshalAs(UnmanagedType.LPStr)] String name, Int32 index, FMOD_TAG* tag);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Sound_GetTag(FMOD_SOUND* sound, String name, Int32 index, FMOD_TAG* tag) => INTERNAL_FMOD_Sound_GetTag(sound, name, index, tag);
        
        [DllImport("fmod", EntryPoint = "FMOD_Sound_Release", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Sound_Release(FMOD_SOUND* sound);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Sound_Release(FMOD_SOUND* sound) => INTERNAL_FMOD_Sound_Release(sound);
        
        [DllImport("fmod", EntryPoint = "FMOD_ChannelGroup_SetVolume", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_ChannelGroup_SetVolume(FMOD_CHANNELGROUP* channelgroup, Single volume);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_ChannelGroup_SetVolume(FMOD_CHANNELGROUP* channelgroup, Single volume) => INTERNAL_FMOD_ChannelGroup_SetVolume(channelgroup, volume);
        
        [DllImport("fmod", EntryPoint = "FMOD_ChannelGroup_Release", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_ChannelGroup_Release(FMOD_CHANNELGROUP* channelgroup);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_ChannelGroup_Release(FMOD_CHANNELGROUP* channelgroup) => INTERNAL_FMOD_ChannelGroup_Release(channelgroup);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_Stop", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_Stop(FMOD_CHANNEL* channel);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_Stop(FMOD_CHANNEL* channel) => INTERNAL_FMOD_Channel_Stop(channel);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetPaused", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetPaused(FMOD_CHANNEL* channel, Boolean paused);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetPaused(FMOD_CHANNEL* channel, Boolean paused) => INTERNAL_FMOD_Channel_SetPaused(channel, paused);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetPaused", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetPaused(FMOD_CHANNEL* channel, Boolean* paused);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetPaused(FMOD_CHANNEL* channel, Boolean* paused) => INTERNAL_FMOD_Channel_GetPaused(channel, paused);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetVolume", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetVolume(FMOD_CHANNEL* channel, Single volume);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetVolume(FMOD_CHANNEL* channel, Single volume) => INTERNAL_FMOD_Channel_SetVolume(channel, volume);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetVolume", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetVolume(FMOD_CHANNEL* channel, Single* volume);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetVolume(FMOD_CHANNEL* channel, Single* volume) => INTERNAL_FMOD_Channel_GetVolume(channel, volume);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetVolumeRamp", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetVolumeRamp(FMOD_CHANNEL* channel, Boolean ramp);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetVolumeRamp(FMOD_CHANNEL* channel, Boolean ramp) => INTERNAL_FMOD_Channel_SetVolumeRamp(channel, ramp);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetVolumeRamp", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetVolumeRamp(FMOD_CHANNEL* channel, Boolean* ramp);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetVolumeRamp(FMOD_CHANNEL* channel, Boolean* ramp) => INTERNAL_FMOD_Channel_GetVolumeRamp(channel, ramp);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetAudibility", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetAudibility(FMOD_CHANNEL* channel, Single* audibility);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetAudibility(FMOD_CHANNEL* channel, Single* audibility) => INTERNAL_FMOD_Channel_GetAudibility(channel, audibility);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetPitch", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetPitch(FMOD_CHANNEL* channel, Single pitch);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetPitch(FMOD_CHANNEL* channel, Single pitch) => INTERNAL_FMOD_Channel_SetPitch(channel, pitch);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetPitch", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetPitch(FMOD_CHANNEL* channel, Single* pitch);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetPitch(FMOD_CHANNEL* channel, Single* pitch) => INTERNAL_FMOD_Channel_GetPitch(channel, pitch);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetMute", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetMute(FMOD_CHANNEL* channel, Boolean mute);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetMute(FMOD_CHANNEL* channel, Boolean mute) => INTERNAL_FMOD_Channel_SetMute(channel, mute);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetMute", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetMute(FMOD_CHANNEL* channel, Boolean* mute);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetMute(FMOD_CHANNEL* channel, Boolean* mute) => INTERNAL_FMOD_Channel_GetMute(channel, mute);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetMode", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetMode(FMOD_CHANNEL* channel, FMOD_MODE mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetMode(FMOD_CHANNEL* channel, FMOD_MODE mode) => INTERNAL_FMOD_Channel_SetMode(channel, mode);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetMode", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetMode(FMOD_CHANNEL* channel, FMOD_MODE* mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetMode(FMOD_CHANNEL* channel, FMOD_MODE* mode) => INTERNAL_FMOD_Channel_GetMode(channel, mode);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_IsPlaying", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_IsPlaying(FMOD_CHANNEL* channel, Boolean* isplaying);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_IsPlaying(FMOD_CHANNEL* channel, Boolean* isplaying) => INTERNAL_FMOD_Channel_IsPlaying(channel, isplaying);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetPan", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetPan(FMOD_CHANNEL* channel, Single pan);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetPan(FMOD_CHANNEL* channel, Single pan) => INTERNAL_FMOD_Channel_SetPan(channel, pan);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetPosition", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetPosition(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetPosition(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype) => INTERNAL_FMOD_Channel_SetPosition(channel, position, postype);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetPosition", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetPosition(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetPosition(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype) => INTERNAL_FMOD_Channel_GetPosition(channel, position, postype);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_SetLoopPoints", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_SetLoopPoints(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_SetLoopPoints(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype) => INTERNAL_FMOD_Channel_SetLoopPoints(channel, loopstart, loopstarttype, loopend, loopendtype);
        
        [DllImport("fmod", EntryPoint = "FMOD_Channel_GetLoopPoints", CallingConvention = CallingConvention.StdCall)]
        private static extern FMOD_RESULT INTERNAL_FMOD_Channel_GetLoopPoints(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed FMOD_RESULT FMOD_Channel_GetLoopPoints(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype) => INTERNAL_FMOD_Channel_GetLoopPoints(channel, loopstart, loopstarttype, loopend, loopendtype);
    }
#pragma warning restore 1591
}

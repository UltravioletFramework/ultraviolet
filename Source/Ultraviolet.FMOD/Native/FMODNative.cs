using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;
using System.Text;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    public static unsafe partial class FMODNative
    {
        private static readonly FMODNativeImpl impl;
        
        static FMODNative()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Android:
                    impl = new FMODNativeImpl_Android();
                    break;
                    
                default:
                    impl = new FMODNativeImpl_Default();
                    break;
            }
        }
        
        public const UInt32 FMOD_VERSION = 0x00020106;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Debug_Initialize(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, String filename) => impl.FMOD_Debug_Initialize(flags, mode, callback, filename);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_Create(FMOD_SYSTEM** system) => impl.FMOD_System_Create(system);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_Release(FMOD_SYSTEM* system) => impl.FMOD_System_Release(system);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_GetVersion(FMOD_SYSTEM* system, UInt32* version) => impl.FMOD_System_GetVersion(system, version);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_GetNumDrivers(FMOD_SYSTEM* system, Int32* numdrivers) => impl.FMOD_System_GetNumDrivers(system, numdrivers);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_GetDriverInfo(FMOD_SYSTEM* system, Int32 id, StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels) => impl.FMOD_System_GetDriverInfo(system, id, name, namelen, guid, systemrate, speakermode, speakermodechannels);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_SetDriver(FMOD_SYSTEM* system, Int32 driver) => impl.FMOD_System_SetDriver(system, driver);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_GetDriver(FMOD_SYSTEM* system, Int32* driver) => impl.FMOD_System_GetDriver(system, driver);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_Init(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata) => impl.FMOD_System_Init(system, maxchannels, flags, extradriverdata);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_PlaySound(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel) => impl.FMOD_System_PlaySound(system, sound, channelgroup, paused, channel);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_Close(FMOD_SYSTEM* system) => impl.FMOD_System_Close(system);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_Update(FMOD_SYSTEM* system) => impl.FMOD_System_Update(system);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_SetCallback(FMOD_SYSTEM* system, FMOD_SYSTEM_CALLBACK callback, FMOD_SYSTEM_CALLBACK_TYPE callbackmask) => impl.FMOD_System_SetCallback(system, callback, callbackmask);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_MixerSuspend(FMOD_SYSTEM* system) => impl.FMOD_System_MixerSuspend(system);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_MixerResume(FMOD_SYSTEM* system) => impl.FMOD_System_MixerResume(system);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_CreateSound(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound) => impl.FMOD_System_CreateSound(system, name_or_data, mode, exinfo, sound);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_CreateStream(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound) => impl.FMOD_System_CreateStream(system, name_or_data, mode, exinfo, sound);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_CreateChannelGroup(FMOD_SYSTEM* system, String name, FMOD_CHANNELGROUP** channelgroup) => impl.FMOD_System_CreateChannelGroup(system, name, channelgroup);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_System_SetFileSystem(FMOD_SYSTEM* system, FMOD_FILE_OPEN_CALLBACK useropen, FMOD_FILE_CLOSE_CALLBACK userclose, FMOD_FILE_READ_CALLBACK userread, FMOD_FILE_SEEK_CALLBACK userseek, FMOD_FILE_ASYNCREAD_CALLBACK userasyncread, FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 bloackalign) => impl.FMOD_System_SetFileSystem(system, useropen, userclose, userread, userseek, userasyncread, userasynccancel, bloackalign);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Sound_GetLength(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype) => impl.FMOD_Sound_GetLength(sound, length, lengthtype);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Sound_GetNumTags(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated) => impl.FMOD_Sound_GetNumTags(sound, numtags, numtagsupdated);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Sound_GetTag(FMOD_SOUND* sound, String name, Int32 index, FMOD_TAG* tag) => impl.FMOD_Sound_GetTag(sound, name, index, tag);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Sound_Release(FMOD_SOUND* sound) => impl.FMOD_Sound_Release(sound);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_ChannelGroup_SetVolume(FMOD_CHANNELGROUP* channelgroup, Single volume) => impl.FMOD_ChannelGroup_SetVolume(channelgroup, volume);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_ChannelGroup_Release(FMOD_CHANNELGROUP* channelgroup) => impl.FMOD_ChannelGroup_Release(channelgroup);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_Stop(FMOD_CHANNEL* channel) => impl.FMOD_Channel_Stop(channel);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetPaused(FMOD_CHANNEL* channel, Boolean paused) => impl.FMOD_Channel_SetPaused(channel, paused);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetPaused(FMOD_CHANNEL* channel, Boolean* paused) => impl.FMOD_Channel_GetPaused(channel, paused);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetVolume(FMOD_CHANNEL* channel, Single volume) => impl.FMOD_Channel_SetVolume(channel, volume);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetVolume(FMOD_CHANNEL* channel, Single* volume) => impl.FMOD_Channel_GetVolume(channel, volume);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetVolumeRamp(FMOD_CHANNEL* channel, Boolean ramp) => impl.FMOD_Channel_SetVolumeRamp(channel, ramp);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetVolumeRamp(FMOD_CHANNEL* channel, Boolean* ramp) => impl.FMOD_Channel_GetVolumeRamp(channel, ramp);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetAudibility(FMOD_CHANNEL* channel, Single* audibility) => impl.FMOD_Channel_GetAudibility(channel, audibility);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetPitch(FMOD_CHANNEL* channel, Single pitch) => impl.FMOD_Channel_SetPitch(channel, pitch);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetPitch(FMOD_CHANNEL* channel, Single* pitch) => impl.FMOD_Channel_GetPitch(channel, pitch);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetMute(FMOD_CHANNEL* channel, Boolean mute) => impl.FMOD_Channel_SetMute(channel, mute);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetMute(FMOD_CHANNEL* channel, Boolean* mute) => impl.FMOD_Channel_GetMute(channel, mute);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetMode(FMOD_CHANNEL* channel, FMOD_MODE mode) => impl.FMOD_Channel_SetMode(channel, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetMode(FMOD_CHANNEL* channel, FMOD_MODE* mode) => impl.FMOD_Channel_GetMode(channel, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_IsPlaying(FMOD_CHANNEL* channel, Boolean* isplaying) => impl.FMOD_Channel_IsPlaying(channel, isplaying);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetPan(FMOD_CHANNEL* channel, Single pan) => impl.FMOD_Channel_SetPan(channel, pan);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetPosition(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype) => impl.FMOD_Channel_SetPosition(channel, position, postype);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetPosition(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype) => impl.FMOD_Channel_GetPosition(channel, position, postype);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_SetLoopPoints(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype) => impl.FMOD_Channel_SetLoopPoints(channel, loopstart, loopstarttype, loopend, loopendtype);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FMOD_RESULT FMOD_Channel_GetLoopPoints(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype) => impl.FMOD_Channel_GetLoopPoints(channel, loopstart, loopstarttype, loopend, loopendtype);
    }
#pragma warning restore 1591
}

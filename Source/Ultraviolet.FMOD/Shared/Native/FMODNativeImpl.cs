using System;
using System.Security;
using System.Runtime.InteropServices;
using System.Text;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate FMOD_RESULT FMOD_DEBUG_CALLBACK(FMOD_DEBUG_FLAGS flags, String file, Int32 line, String func, String message);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_OPEN_CALLBACK(String name, UInt32* filesize, void** handle, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_CLOSE_CALLBACK(void* handle, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_READ_CALLBACK(void* handle, void* buffer, UInt32 sizebytes, UInt32* bytesread, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_SEEK_CALLBACK(void* handle, UInt32 pos, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_ASYNCREAD_CALLBACK(FMOD_ASYNCREADINFO* info, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_ASYNCCANCEL_CALLBACK(FMOD_ASYNCREADINFO* info, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void FMOD_FILE_ASYNCDONE_FUNC(FMOD_ASYNCREADINFO* info);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate FMOD_RESULT FMOD_SYSTEM_CALLBACK(void* system, FMOD_SYSTEM_CALLBACK_TYPE type, void* commanddata1, void* commanddata2, void* userdata);
    
    public struct FMOD_SYSTEM { }
    public struct FMOD_SOUND { }
    public struct FMOD_CHANNELCONTROL { }
    public struct FMOD_CHANNEL { }
    public struct FMOD_CHANNELGROUP { }
    public struct FMOD_SOUNDGROUP { }
    public struct FMOD_REVERB3D { }
    public struct FMOD_DSP { }
    public struct FMOD_DSPCONNECTION { }
    public struct FMOD_POLYGON { }
    public struct FMOD_GEOMETRY { }
    public struct FMOD_SYNCPOINT { }
    
    [SuppressUnmanagedCodeSecurity]
    public abstract unsafe class FMODNativeImpl
    {
        public abstract FMOD_RESULT FMOD_Debug_Initialize(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, String filename);
        public abstract FMOD_RESULT FMOD_System_Create(FMOD_SYSTEM** system);
        public abstract FMOD_RESULT FMOD_System_Release(FMOD_SYSTEM* system);
        public abstract FMOD_RESULT FMOD_System_GetVersion(FMOD_SYSTEM* system, UInt32* version);
        public abstract FMOD_RESULT FMOD_System_GetNumDrivers(FMOD_SYSTEM* system, Int32* numdrivers);
        public abstract FMOD_RESULT FMOD_System_GetDriverInfo(FMOD_SYSTEM* system, Int32 id, StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels);
        public abstract FMOD_RESULT FMOD_System_SetDriver(FMOD_SYSTEM* system, Int32 driver);
        public abstract FMOD_RESULT FMOD_System_GetDriver(FMOD_SYSTEM* system, Int32* driver);
        public abstract FMOD_RESULT FMOD_System_Init(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata);
        public abstract FMOD_RESULT FMOD_System_PlaySound(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel);
        public abstract FMOD_RESULT FMOD_System_Close(FMOD_SYSTEM* system);
        public abstract FMOD_RESULT FMOD_System_Update(FMOD_SYSTEM* system);
        public abstract FMOD_RESULT FMOD_System_SetCallback(FMOD_SYSTEM* system, FMOD_SYSTEM_CALLBACK callback, FMOD_SYSTEM_CALLBACK_TYPE callbackmask);
        public abstract FMOD_RESULT FMOD_System_MixerSuspend(FMOD_SYSTEM* system);
        public abstract FMOD_RESULT FMOD_System_MixerResume(FMOD_SYSTEM* system);
        public abstract FMOD_RESULT FMOD_System_CreateSound(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
        public abstract FMOD_RESULT FMOD_System_CreateStream(FMOD_SYSTEM* system, String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
        public abstract FMOD_RESULT FMOD_System_CreateChannelGroup(FMOD_SYSTEM* system, String name, FMOD_CHANNELGROUP** channelgroup);
        public abstract FMOD_RESULT FMOD_System_SetFileSystem(FMOD_SYSTEM* system, FMOD_FILE_OPEN_CALLBACK useropen, FMOD_FILE_CLOSE_CALLBACK userclose, FMOD_FILE_READ_CALLBACK userread, FMOD_FILE_SEEK_CALLBACK userseek, FMOD_FILE_ASYNCREAD_CALLBACK userasyncread, FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 bloackalign);
        public abstract FMOD_RESULT FMOD_Sound_GetLength(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype);
        public abstract FMOD_RESULT FMOD_Sound_GetNumTags(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated);
        public abstract FMOD_RESULT FMOD_Sound_GetTag(FMOD_SOUND* sound, String name, Int32 index, FMOD_TAG* tag);
        public abstract FMOD_RESULT FMOD_Sound_Release(FMOD_SOUND* sound);
        public abstract FMOD_RESULT FMOD_ChannelGroup_SetVolume(FMOD_CHANNELGROUP* channelgroup, Single volume);
        public abstract FMOD_RESULT FMOD_ChannelGroup_Release(FMOD_CHANNELGROUP* channelgroup);
        public abstract FMOD_RESULT FMOD_Channel_Stop(FMOD_CHANNEL* channel);
        public abstract FMOD_RESULT FMOD_Channel_SetPaused(FMOD_CHANNEL* channel, Boolean paused);
        public abstract FMOD_RESULT FMOD_Channel_GetPaused(FMOD_CHANNEL* channel, Boolean* paused);
        public abstract FMOD_RESULT FMOD_Channel_SetVolume(FMOD_CHANNEL* channel, Single volume);
        public abstract FMOD_RESULT FMOD_Channel_GetVolume(FMOD_CHANNEL* channel, Single* volume);
        public abstract FMOD_RESULT FMOD_Channel_SetVolumeRamp(FMOD_CHANNEL* channel, Boolean ramp);
        public abstract FMOD_RESULT FMOD_Channel_GetVolumeRamp(FMOD_CHANNEL* channel, Boolean* ramp);
        public abstract FMOD_RESULT FMOD_Channel_GetAudibility(FMOD_CHANNEL* channel, Single* audibility);
        public abstract FMOD_RESULT FMOD_Channel_SetPitch(FMOD_CHANNEL* channel, Single pitch);
        public abstract FMOD_RESULT FMOD_Channel_GetPitch(FMOD_CHANNEL* channel, Single* pitch);
        public abstract FMOD_RESULT FMOD_Channel_SetMute(FMOD_CHANNEL* channel, Boolean mute);
        public abstract FMOD_RESULT FMOD_Channel_GetMute(FMOD_CHANNEL* channel, Boolean* mute);
        public abstract FMOD_RESULT FMOD_Channel_SetMode(FMOD_CHANNEL* channel, FMOD_MODE mode);
        public abstract FMOD_RESULT FMOD_Channel_GetMode(FMOD_CHANNEL* channel, FMOD_MODE* mode);
        public abstract FMOD_RESULT FMOD_Channel_IsPlaying(FMOD_CHANNEL* channel, Boolean* isplaying);
        public abstract FMOD_RESULT FMOD_Channel_SetPan(FMOD_CHANNEL* channel, Single pan);
        public abstract FMOD_RESULT FMOD_Channel_SetPosition(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype);
        public abstract FMOD_RESULT FMOD_Channel_GetPosition(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype);
        public abstract FMOD_RESULT FMOD_Channel_SetLoopPoints(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype);
        public abstract FMOD_RESULT FMOD_Channel_GetLoopPoints(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype);
    }
#pragma warning restore 1591
}

using System;
using System.Runtime.InteropServices;
using System.Security;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
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
    public struct FMOD_ASYNCREADINFO { }

    [SuppressUnmanagedCodeSecurity]
    public static unsafe partial class FMODNative
    {
        // NOTE: The #ifdefs everywhere are necessary because I haven't yet found a way to make
        // the new dynamic loader work on mobile platforms, particularly Android, where dlopen()
        // sometimes maps the same library to multiple address spaces for reasons that I haven't
        // yet been able to discern. My hope is that if the proposed .NET Standard API for dynamic
        // library loading ever makes it to Xamarin Android/iOS, we can standardize all supported
        // platforms on a single declaration type. For now, though, this nonsense seems necessary.

#if ANDROID
        const String LIBRARY = "fmod";
#elif IOS
        const String LIBRARY = "__Internal";
#else
        private static readonly NativeLibrary lib = new NativeLibrary(
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "fmod" : "libfmod");
#endif

        public const UInt32 FMOD_VERSION = 0x00011003;

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_Create", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_Create(FMOD_SYSTEM** system);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CreateDelegate(FMOD_SYSTEM** system);
        private static readonly FMOD_System_CreateDelegate pFMOD_System_Create = lib.LoadFunction<FMOD_System_CreateDelegate>("FMOD_System_Create");
        public static FMOD_RESULT FMOD_System_Create(FMOD_SYSTEM** system) => pFMOD_System_Create(system);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_Release", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_Release(FMOD_SYSTEM* system);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_ReleaseDelegate(FMOD_SYSTEM* system);
        private static readonly FMOD_System_ReleaseDelegate pFMOD_System_Release = lib.LoadFunction<FMOD_System_ReleaseDelegate>("FMOD_System_Release");
        public static FMOD_RESULT FMOD_System_Release(FMOD_SYSTEM* system) => pFMOD_System_Release(system);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_GetVersion", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_GetVersion(FMOD_SYSTEM* system, UInt32* version);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_GetVersionDelegate(FMOD_SYSTEM* system, UInt32* version);
        private static readonly FMOD_System_GetVersionDelegate pFMOD_System_GetVersion = lib.LoadFunction<FMOD_System_GetVersionDelegate>("FMOD_System_GetVersion");
        public static FMOD_RESULT FMOD_System_GetVersion(FMOD_SYSTEM* system, UInt32* version) => pFMOD_System_GetVersion(system, version);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_Init", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_Init(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_InitDelegate(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata);
        private static readonly FMOD_System_InitDelegate pFMOD_System_Init = lib.LoadFunction<FMOD_System_InitDelegate>("FMOD_System_Init");
        public static FMOD_RESULT FMOD_System_Init(FMOD_SYSTEM* system, Int32 maxchannels, FMOD_INITFLAGS flags, void* extradriverdata) => pFMOD_System_Init(system, maxchannels, flags, extradriverdata);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_Close", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_Close(FMOD_SYSTEM* system);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CloseDelegate(FMOD_SYSTEM* system);
        private static readonly FMOD_System_CloseDelegate pFMOD_System_Close = lib.LoadFunction<FMOD_System_CloseDelegate>("FMOD_System_Close");
        public static FMOD_RESULT FMOD_System_Close(FMOD_SYSTEM* system) => pFMOD_System_Close(system);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_Update", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_Update(FMOD_SYSTEM* system);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_UpdateDelegate(FMOD_SYSTEM* system);
        private static readonly FMOD_System_UpdateDelegate pFMOD_System_Update = lib.LoadFunction<FMOD_System_UpdateDelegate>("FMOD_System_Update");
        public static FMOD_RESULT FMOD_System_Update(FMOD_SYSTEM* system) => pFMOD_System_Update(system);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_MixerSuspend", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_MixerSuspend(FMOD_SYSTEM* system);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_MixerSuspendDelegate(FMOD_SYSTEM* system);
        private static readonly FMOD_System_MixerSuspendDelegate pFMOD_System_MixerSuspend = lib.LoadFunction<FMOD_System_MixerSuspendDelegate>("FMOD_System_MixerSuspend");
        public static FMOD_RESULT FMOD_System_MixerSuspend(FMOD_SYSTEM* system) => pFMOD_System_MixerSuspend(system);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_MixerResume", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_MixerResume(FMOD_SYSTEM* system);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_MixerResumeDelegate(FMOD_SYSTEM* system);
        private static readonly FMOD_System_MixerResumeDelegate pFMOD_System_MixerResume = lib.LoadFunction<FMOD_System_MixerResumeDelegate>("FMOD_System_MixerResume");
        public static FMOD_RESULT FMOD_System_MixerResume(FMOD_SYSTEM* system) => pFMOD_System_MixerResume(system);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_CreateSound", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_CreateSound(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CreateSoundDelegate(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
        private static readonly FMOD_System_CreateSoundDelegate pFMOD_System_CreateSound = lib.LoadFunction<FMOD_System_CreateSoundDelegate>("FMOD_System_CreateSound");
        public static FMOD_RESULT FMOD_System_CreateSound(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound) => pFMOD_System_CreateSound(system, name_or_data, mode, exinfo, sound);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Sound_GetLength", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Sound_GetLength(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Sound_GetLengthDelegate(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype);
        private static readonly FMOD_Sound_GetLengthDelegate pFMOD_Sound_GetLength = lib.LoadFunction<FMOD_Sound_GetLengthDelegate>("FMOD_Sound_GetLength");
        public static FMOD_RESULT FMOD_Sound_GetLength(FMOD_SOUND* sound, UInt32* length, FMOD_TIMEUNIT lengthtype) => pFMOD_Sound_GetLength(sound, length, lengthtype);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Sound_GetNumTags", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Sound_GetNumTags(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Sound_GetNumTagsDelegate(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated);
        private static readonly FMOD_Sound_GetNumTagsDelegate pFMOD_Sound_GetNumTags = lib.LoadFunction<FMOD_Sound_GetNumTagsDelegate>("FMOD_Sound_GetNumTags");
        public static FMOD_RESULT FMOD_Sound_GetNumTags(FMOD_SOUND* sound, Int32* numtags, Int32* numtagsupdated) => pFMOD_Sound_GetNumTags(sound, numtags, numtagsupdated);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Sound_GetTag", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Sound_GetTag(FMOD_SOUND* sound, [MarshalAs(UnmanagedType.LPStr)] String name, Int32 index, FMOD_TAG* tag);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Sound_GetTagDelegate(FMOD_SOUND* sound, [MarshalAs(UnmanagedType.LPStr)] String name, Int32 index, FMOD_TAG* tag);
        private static readonly FMOD_Sound_GetTagDelegate pFMOD_Sound_GetTag = lib.LoadFunction<FMOD_Sound_GetTagDelegate>("FMOD_Sound_GetTag");
        public static FMOD_RESULT FMOD_Sound_GetTag(FMOD_SOUND* sound, [MarshalAs(UnmanagedType.LPStr)] String name, Int32 index, FMOD_TAG* tag) => pFMOD_Sound_GetTag(sound, name, index, tag);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Sound_Release", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Sound_Release(FMOD_SOUND* sound);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Sound_ReleaseDelegate(FMOD_SOUND* sound);
        private static readonly FMOD_Sound_ReleaseDelegate pFMOD_Sound_Release = lib.LoadFunction<FMOD_Sound_ReleaseDelegate>("FMOD_Sound_Release");
        public static FMOD_RESULT FMOD_Sound_Release(FMOD_SOUND* sound) => pFMOD_Sound_Release(sound);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_CreateChannelGroup", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_CreateChannelGroup(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name, FMOD_CHANNELGROUP** channelgroup);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CreateChannelGroupDelegate(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name, FMOD_CHANNELGROUP** channelgroup);
        private static readonly FMOD_System_CreateChannelGroupDelegate pFMOD_System_CreateChannelGroup = lib.LoadFunction<FMOD_System_CreateChannelGroupDelegate>("FMOD_System_CreateChannelGroup");
        public static FMOD_RESULT FMOD_System_CreateChannelGroup(FMOD_SYSTEM* system, String name, FMOD_CHANNELGROUP** channelgroup) => pFMOD_System_CreateChannelGroup(system, name, channelgroup);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_ChannelGroup_SetVolume", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_ChannelGroup_SetVolume(FMOD_CHANNELGROUP* channelgroup, Single volume);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_ChannelGroup_SetVolumeDelegate(FMOD_CHANNELGROUP* channelgroup, Single volume);
        private static readonly FMOD_ChannelGroup_SetVolumeDelegate pFMOD_ChannelGroup_SetVolume = lib.LoadFunction<FMOD_ChannelGroup_SetVolumeDelegate>("FMOD_ChannelGroup_SetVolume");
        public static FMOD_RESULT FMOD_ChannelGroup_SetVolume(FMOD_CHANNELGROUP* channelgroup, Single volume) => pFMOD_ChannelGroup_SetVolume(channelgroup, volume);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_ChannelGroup_Release", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_ChannelGroup_Release(FMOD_CHANNELGROUP* channelgroup);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_ChannelGroup_ReleaseDelegate(FMOD_CHANNELGROUP* channelgroup);
        private static readonly FMOD_ChannelGroup_ReleaseDelegate pFMOD_ChannelGroup_Release = lib.LoadFunction<FMOD_ChannelGroup_ReleaseDelegate>("FMOD_ChannelGroup_Release");
        public static FMOD_RESULT FMOD_ChannelGroup_Release(FMOD_CHANNELGROUP* channelgroup) => pFMOD_ChannelGroup_Release(channelgroup);
#endif
    }
}

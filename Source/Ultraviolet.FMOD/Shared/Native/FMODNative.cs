using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.FMOD.Native
{
#pragma warning disable 1591
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate FMOD_RESULT FMOD_DEBUG_CALLBACK(FMOD_DEBUG_FLAGS flags, [MarshalAs(UnmanagedType.LPStr)] String file, Int32 line, [MarshalAs(UnmanagedType.LPStr)] String func, [MarshalAs(UnmanagedType.LPStr)] String message);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_OPEN_CALLBACK([MarshalAs(UnmanagedType.LPStr)] String name, UInt32* filesize, void** handle, void* userdadata);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_CLOSE_CALLBACK(void* handle, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_READ_CALLBACK(void* handle, void* buffer, UInt32 sizebytes, UInt32* bytesread, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_SEEK_CALLBACK(void* handle, UInt32 pos, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_ASYNCREAD_CALLBACK(FMOD_ASYNCREADINFO* info, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate FMOD_RESULT FMOD_FILE_ASYNCCANCEL_CALLBACK(FMOD_ASYNCREADINFO* info, void* userdata);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public unsafe delegate void FMOD_FILE_ASYNCDONE_FUNC(FMOD_ASYNCREADINFO* info, FMOD_RESULT result);

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
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? new[] { "fmodL", "fmod" } : new[] { "libfmodL", "libfmod" });
#endif
        
        public const UInt32 FMOD_VERSION = 0x00011003;

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Debug_Initialize", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Debug_Initialize(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, [MarshalAs(UnmanagedType.LPStr)] String filename);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Debug_InitializeDelegate(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, [MarshalAs(UnmanagedType.LPStr)] String filename);
        private static readonly FMOD_Debug_InitializeDelegate pFMOD_Debug_Initialize = lib.LoadFunction<FMOD_Debug_InitializeDelegate>("FMOD_Debug_Initialize");
        public static FMOD_RESULT FMOD_Debug_Initialize(FMOD_DEBUG_FLAGS flags, FMOD_DEBUG_MODE mode, FMOD_DEBUG_CALLBACK callback, [MarshalAs(UnmanagedType.LPStr)] String filename) => pFMOD_Debug_Initialize(flags, mode, callback, filename);
#endif

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
        [DllImport(LIBRARY, EntryPoint="FMOD_System_GetNumDrivers", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_GetNumDrivers(FMOD_SYSTEM* system, Int32* numdrivers);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_GetNumDriversDelegate(FMOD_SYSTEM* system, Int32* numdrivers);
        private static readonly FMOD_System_GetNumDriversDelegate pFMOD_System_GetNumDrivers = lib.LoadFunction<FMOD_System_GetNumDriversDelegate>("FMOD_System_GetNumDrivers");
        public static FMOD_RESULT FMOD_System_GetNumDrivers(FMOD_SYSTEM* system, Int32* numdrivers) => pFMOD_System_GetNumDrivers(system, numdrivers);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_GetDriverInfo", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_GetDriverInfo(FMOD_SYSTEM* system, Int32 id, [MarshalAs(UnmanagedType.LPStr)] StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_GetDriverInfoDelegate(FMOD_SYSTEM* system, Int32 id, [MarshalAs(UnmanagedType.LPStr)] StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels);
        private static readonly FMOD_System_GetDriverInfoDelegate pFMOD_System_GetDriverInfo = lib.LoadFunction<FMOD_System_GetDriverInfoDelegate>("FMOD_System_GetDriverInfo");
        public static FMOD_RESULT FMOD_System_GetDriverInfo(FMOD_SYSTEM* system, Int32 id, [MarshalAs(UnmanagedType.LPStr)] StringBuilder name, Int32 namelen, Guid* guid, Int32* systemrate, FMOD_SPEAKERMODE* speakermode, Int32* speakermodechannels) => pFMOD_System_GetDriverInfo(system, id, name, namelen, guid, systemrate, speakermode, speakermodechannels);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_SetDriver", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_SetDriver(FMOD_SYSTEM* system, Int32 driver);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_SetDriverDelegate(FMOD_SYSTEM* system, Int32 driver);
        private static readonly FMOD_System_SetDriverDelegate pFMOD_System_SetDriver = lib.LoadFunction<FMOD_System_SetDriverDelegate>("FMOD_System_SetDriver");
        public static FMOD_RESULT FMOD_System_SetDriver(FMOD_SYSTEM* system, Int32 driver) => pFMOD_System_SetDriver(system, driver);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_System_GetDriver", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_GetDriver(FMOD_SYSTEM* system, Int32* driver);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_GetDriverDelegate(FMOD_SYSTEM* system, Int32* driver);
        private static readonly FMOD_System_GetDriverDelegate pFMOD_System_GetDriver = lib.LoadFunction<FMOD_System_GetDriverDelegate>("FMOD_System_GetDriver");
        public static FMOD_RESULT FMOD_System_GetDriver(FMOD_SYSTEM* system, Int32* driver) => pFMOD_System_GetDriver(system, driver);
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
        [DllImport(LIBRARY, EntryPoint="FMOD_System_PlaySound", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_PlaySound(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_PlaySoundDelegate(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel);
        private static readonly FMOD_System_PlaySoundDelegate pFMOD_System_PlaySound = lib.LoadFunction<FMOD_System_PlaySoundDelegate>("FMOD_System_PlaySound");
        public static FMOD_RESULT FMOD_System_PlaySound(FMOD_SYSTEM* system, FMOD_SOUND* sound, FMOD_CHANNELGROUP* channelgroup, Boolean paused, FMOD_CHANNEL** channel) => pFMOD_System_PlaySound(system, sound, channelgroup, paused, channel);
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
        [DllImport(LIBRARY, EntryPoint="FMOD_System_CreateStream", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_CreateStream(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_CreateStreamDelegate(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound);
        private static readonly FMOD_System_CreateStreamDelegate pFMOD_System_CreateStream = lib.LoadFunction<FMOD_System_CreateStreamDelegate>("FMOD_System_CreateStream");
        public static FMOD_RESULT FMOD_System_CreateStream(FMOD_SYSTEM* system, [MarshalAs(UnmanagedType.LPStr)] String name_or_data, FMOD_MODE mode, FMOD_CREATESOUNDEXINFO* exinfo, FMOD_SOUND** sound) => pFMOD_System_CreateStream(system, name_or_data, mode, exinfo, sound);
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
        [DllImport(LIBRARY, EntryPoint = "FMOD_System_SetFileSystem", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_System_SetFileSystem(FMOD_SYSTEM* system,
            FMOD_FILE_OPEN_CALLBACK useropen,
            FMOD_FILE_CLOSE_CALLBACK userclose,
            FMOD_FILE_READ_CALLBACK userread,
            FMOD_FILE_SEEK_CALLBACK userseek,
            FMOD_FILE_ASYNCREAD_CALLBACK userasyncread,
            FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 blockalign);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_System_SetFileSystemDelegate(FMOD_SYSTEM* system, FMOD_FILE_OPEN_CALLBACK useropen, FMOD_FILE_CLOSE_CALLBACK userclose, FMOD_FILE_READ_CALLBACK userread, FMOD_FILE_SEEK_CALLBACK userseek, FMOD_FILE_ASYNCREAD_CALLBACK userasyncread, FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 blockalign);
        private static readonly FMOD_System_SetFileSystemDelegate pFMOD_System_SetFileSystem = lib.LoadFunction<FMOD_System_SetFileSystemDelegate>("FMOD_System_SetFileSystem");
        public static FMOD_RESULT FMOD_System_SetFileSystem(FMOD_SYSTEM* system, FMOD_FILE_OPEN_CALLBACK useropen, FMOD_FILE_CLOSE_CALLBACK userclose, FMOD_FILE_READ_CALLBACK userread, FMOD_FILE_SEEK_CALLBACK userseek, FMOD_FILE_ASYNCREAD_CALLBACK userasyncread, FMOD_FILE_ASYNCCANCEL_CALLBACK userasynccancel, Int32 blockalign) => pFMOD_System_SetFileSystem(system, useropen, userclose, userread, userseek, userasyncread, userasynccancel, blockalign);
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

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_Stop", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_Stop(FMOD_CHANNEL* channel);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_StopDelegate(FMOD_CHANNEL* channel);
        private static readonly FMOD_Channel_StopDelegate pFMOD_Channel_Stop = lib.LoadFunction<FMOD_Channel_StopDelegate>("FMOD_Channel_Stop");
        public static FMOD_RESULT FMOD_Channel_Stop(FMOD_CHANNEL* channel) => pFMOD_Channel_Stop(channel);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetPaused", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetPaused(FMOD_CHANNEL* channel, Boolean paused);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetPausedDelegate(FMOD_CHANNEL* channel, Boolean paused);
        private static readonly FMOD_Channel_SetPausedDelegate pFMOD_Channel_SetPaused = lib.LoadFunction<FMOD_Channel_SetPausedDelegate>("FMOD_Channel_SetPaused");
        public static FMOD_RESULT FMOD_Channel_SetPaused(FMOD_CHANNEL* channel, Boolean paused) => pFMOD_Channel_SetPaused(channel, paused);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetPaused", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetPaused(FMOD_CHANNEL* channel, Boolean* paused);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetPausedDelegate(FMOD_CHANNEL* channel, Boolean* paused);
        private static readonly FMOD_Channel_GetPausedDelegate pFMOD_Channel_GetPaused = lib.LoadFunction<FMOD_Channel_GetPausedDelegate>("FMOD_Channel_GetPaused");
        public static FMOD_RESULT FMOD_Channel_GetPaused(FMOD_CHANNEL* channel, Boolean* paused) => pFMOD_Channel_GetPaused(channel, paused);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetVolume", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetVolume(FMOD_CHANNEL* channel, Single volume);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetVolumeDelegate(FMOD_CHANNEL* channel, Single volume);
        private static readonly FMOD_Channel_SetVolumeDelegate pFMOD_Channel_SetVolume = lib.LoadFunction<FMOD_Channel_SetVolumeDelegate>("FMOD_Channel_SetVolume");
        public static FMOD_RESULT FMOD_Channel_SetVolume(FMOD_CHANNEL* channel, Single volume) => pFMOD_Channel_SetVolume(channel, volume);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetVolume", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetVolume(FMOD_CHANNEL* channel, Single* volume);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetVolumeDelegate(FMOD_CHANNEL* channel, Single* volume);
        private static readonly FMOD_Channel_GetVolumeDelegate pFMOD_Channel_GetVolume = lib.LoadFunction<FMOD_Channel_GetVolumeDelegate>("FMOD_Channel_GetVolume");
        public static FMOD_RESULT FMOD_Channel_GetVolume(FMOD_CHANNEL* channel, Single* volume) => pFMOD_Channel_GetVolume(channel, volume);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetVolumeRamp", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetVolumeRamp(FMOD_CHANNEL* channel, Boolean ramp);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetVolumeRampDelegate(FMOD_CHANNEL* channel, Boolean ramp);
        private static readonly FMOD_Channel_SetVolumeRampDelegate pFMOD_Channel_SetVolumeRamp = lib.LoadFunction<FMOD_Channel_SetVolumeRampDelegate>("FMOD_Channel_SetVolumeRamp");
        public static FMOD_RESULT FMOD_Channel_SetVolumeRamp(FMOD_CHANNEL* channel, Boolean ramp) => pFMOD_Channel_SetVolumeRamp(channel, ramp);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetVolumeRamp", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetVolumeRamp(FMOD_CHANNEL* channel, Boolean* ramp);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetVolumeRampDelegate(FMOD_CHANNEL* channel, Boolean* ramp);
        private static readonly FMOD_Channel_GetVolumeRampDelegate pFMOD_Channel_GetVolumeRamp = lib.LoadFunction<FMOD_Channel_GetVolumeRampDelegate>("FMOD_Channel_GetVolumeRamp");
        public static FMOD_RESULT FMOD_Channel_GetVolumeRamp(FMOD_CHANNEL* channel, Boolean* ramp) => pFMOD_Channel_GetVolumeRamp(channel, ramp);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetAudibility", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetAudibility(FMOD_CHANNEL* channel, Single* audibility);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetAudibilityDelegate(FMOD_CHANNEL* channel, Single* audibility);
        private static readonly FMOD_Channel_GetAudibilityDelegate pFMOD_Channel_GetAudibility = lib.LoadFunction<FMOD_Channel_GetAudibilityDelegate>("FMOD_Channel_GetAudibility");
        public static FMOD_RESULT FMOD_Channel_GetAudibility(FMOD_CHANNEL* channel, Single* audibility) => pFMOD_Channel_GetAudibility(channel, audibility);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetPitch", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetPitch(FMOD_CHANNEL* channel, Single pitch);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetPitchDelegate(FMOD_CHANNEL* channel, Single pitch);
        private static readonly FMOD_Channel_SetPitchDelegate pFMOD_Channel_SetPitch = lib.LoadFunction<FMOD_Channel_SetPitchDelegate>("FMOD_Channel_SetPitch");
        public static FMOD_RESULT FMOD_Channel_SetPitch(FMOD_CHANNEL* channel, Single pitch) => pFMOD_Channel_SetPitch(channel, pitch);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetPitch", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetPitch(FMOD_CHANNEL* channel, Single* pitch);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetPitchDelegate(FMOD_CHANNEL* channel, Single* pitch);
        private static readonly FMOD_Channel_GetPitchDelegate pFMOD_Channel_GetPitch = lib.LoadFunction<FMOD_Channel_GetPitchDelegate>("FMOD_Channel_GetPitch");
        public static FMOD_RESULT FMOD_Channel_GetPitch(FMOD_CHANNEL* channel, Single* pitch) => pFMOD_Channel_GetPitch(channel, pitch);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetMute", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetMute(FMOD_CHANNEL* channel, Boolean mute);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetMuteDelegate(FMOD_CHANNEL* channel, Boolean mute);
        private static readonly FMOD_Channel_SetMuteDelegate pFMOD_Channel_SetMute = lib.LoadFunction<FMOD_Channel_SetMuteDelegate>("FMOD_Channel_SetMute");
        public static FMOD_RESULT FMOD_Channel_SetMute(FMOD_CHANNEL* channel, Boolean mute) => pFMOD_Channel_SetMute(channel, mute);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetMute", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetMute(FMOD_CHANNEL* channel, Boolean* mute);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetMuteDelegate(FMOD_CHANNEL* channel, Boolean* mute);
        private static readonly FMOD_Channel_GetMuteDelegate pFMOD_Channel_GetMute = lib.LoadFunction<FMOD_Channel_GetMuteDelegate>("FMOD_Channel_GetMute");
        public static FMOD_RESULT FMOD_Channel_GetMute(FMOD_CHANNEL* channel, Boolean* mute) => pFMOD_Channel_GetMute(channel, mute);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetMode", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetMode(FMOD_CHANNEL* channel, FMOD_MODE mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetModeDelegate(FMOD_CHANNEL* channel, FMOD_MODE mode);
        private static readonly FMOD_Channel_SetModeDelegate pFMOD_Channel_SetMode = lib.LoadFunction<FMOD_Channel_SetModeDelegate>("FMOD_Channel_SetMode");
        public static FMOD_RESULT FMOD_Channel_SetMode(FMOD_CHANNEL* channel, FMOD_MODE mode) => pFMOD_Channel_SetMode(channel, mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetMode", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetMode(FMOD_CHANNEL* channel, FMOD_MODE* mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetModeDelegate(FMOD_CHANNEL* channel, FMOD_MODE* mode);
        private static readonly FMOD_Channel_GetModeDelegate pFMOD_Channel_GetMode = lib.LoadFunction<FMOD_Channel_GetModeDelegate>("FMOD_Channel_GetMode");
        public static FMOD_RESULT FMOD_Channel_GetMode(FMOD_CHANNEL* channel, FMOD_MODE* mode) => pFMOD_Channel_GetMode(channel, mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_IsPlaying", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_IsPlaying(FMOD_CHANNEL* channel, Boolean* isplaying);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_IsPlayingDelegate(FMOD_CHANNEL* channel, Boolean* isplaying);
        private static readonly FMOD_Channel_IsPlayingDelegate pFMOD_Channel_IsPlaying = lib.LoadFunction<FMOD_Channel_IsPlayingDelegate>("FMOD_Channel_IsPlaying");
        public static FMOD_RESULT FMOD_Channel_IsPlaying(FMOD_CHANNEL* channel, Boolean* isplaying) => pFMOD_Channel_IsPlaying(channel, isplaying);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetPan", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetPan(FMOD_CHANNEL* channel, Single pan);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetPanDelegate(FMOD_CHANNEL* channel, Single pan);
        private static readonly FMOD_Channel_SetPanDelegate pFMOD_Channel_SetPan = lib.LoadFunction<FMOD_Channel_SetPanDelegate>("FMOD_Channel_SetPan");
        public static FMOD_RESULT FMOD_Channel_SetPan(FMOD_CHANNEL* channel, Single pan) => pFMOD_Channel_SetPan(channel, pan);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetPosition", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetPosition(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetPositionDelegate(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype);
        private static readonly FMOD_Channel_SetPositionDelegate pFMOD_Channel_SetPosition = lib.LoadFunction<FMOD_Channel_SetPositionDelegate>("FMOD_Channel_SetPosition");
        public static FMOD_RESULT FMOD_Channel_SetPosition(FMOD_CHANNEL* channel, UInt32 position, FMOD_TIMEUNIT postype) => pFMOD_Channel_SetPosition(channel, position, postype);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetPosition", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetPosition(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetPositionDelegate(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype);
        private static readonly FMOD_Channel_GetPositionDelegate pFMOD_Channel_GetPosition = lib.LoadFunction<FMOD_Channel_GetPositionDelegate>("FMOD_Channel_GetPosition");
        public static FMOD_RESULT FMOD_Channel_GetPosition(FMOD_CHANNEL* channel, UInt32* position, FMOD_TIMEUNIT postype) => pFMOD_Channel_GetPosition(channel, position, postype);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_SetLoopPoints", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_SetLoopPoints(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_SetLoopPointsDelegate(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype);
        private static readonly FMOD_Channel_SetLoopPointsDelegate pFMOD_Channel_SetLoopPoints = lib.LoadFunction<FMOD_Channel_SetLoopPointsDelegate>("FMOD_Channel_SetLoopPoints");
        public static FMOD_RESULT FMOD_Channel_SetLoopPoints(FMOD_CHANNEL* channel, UInt32 loopstart, FMOD_TIMEUNIT loopstarttype, UInt32 loopend, FMOD_TIMEUNIT loopendtype) => pFMOD_Channel_SetLoopPoints(channel, loopstart, loopstarttype, loopend, loopendtype);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="FMOD_Channel_GetLoopPoints", CallingConvention = CallingConvention.StdCall)]
        public static extern FMOD_RESULT FMOD_Channel_GetLoopPoints(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate FMOD_RESULT FMOD_Channel_GetLoopPointsDelegate(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype);
        private static readonly FMOD_Channel_GetLoopPointsDelegate pFMOD_Channel_GetLoopPoints = lib.LoadFunction<FMOD_Channel_GetLoopPointsDelegate>("FMOD_Channel_GetLoopPoints");
        public static FMOD_RESULT FMOD_Channel_GetLoopPoints(FMOD_CHANNEL* channel, UInt32* loopstart, FMOD_TIMEUNIT loopstarttype, UInt32* loopend, FMOD_TIMEUNIT loopendtype) => pFMOD_Channel_GetLoopPoints(channel, loopstart, loopstarttype, loopend, loopendtype);
#endif
    }
}

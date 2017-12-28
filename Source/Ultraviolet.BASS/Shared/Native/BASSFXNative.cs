using System;
using System.Security;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.BASS.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal static class BASSFXNative
    {
        private static readonly NativeLibrary lib = new NativeLibrary(
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.iOS ? "__Internal" :
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "bass_fx" : "libbass_fx");

        [MonoNativeFunctionWrapper]
        private delegate UInt32 BASS_FX_GetVersionDelegate();
        private static readonly BASS_FX_GetVersionDelegate pBASS_FX_GetVersion = lib.LoadFunction<BASS_FX_GetVersionDelegate>("BASS_FX_GetVersion");
        public static UInt32 GetVersion() => pBASS_FX_GetVersion();

        [MonoNativeFunctionWrapper]
        private delegate UInt32 BASS_FX_TempoCreateDelegate(UInt32 chan, UInt32 flags);
        private static readonly BASS_FX_TempoCreateDelegate pBASS_FX_TempoCreate = lib.LoadFunction<BASS_FX_TempoCreateDelegate>("BASS_FX_TempoCreate");
        public static UInt32 TempoCreate(UInt32 chan, UInt32 flags) => pBASS_FX_TempoCreate(chan, flags);
    }
}

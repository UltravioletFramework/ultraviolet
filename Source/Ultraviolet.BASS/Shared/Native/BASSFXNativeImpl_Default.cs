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
    internal sealed unsafe class BASSFXNativeImpl_Default : BASSFXNativeImpl
    {
        private static readonly NativeLibrary lib;
        
        static BASSFXNativeImpl_Default()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Linux:
                    lib = new NativeLibrary("libbass_fx");
                    break;
                case UltravioletPlatform.macOS:
                    lib = new NativeLibrary("libbass_fx");
                    break;
                default:
                    lib = new NativeLibrary("bass_fx");
                    break;
            }
        }
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt32 BASS_FX_TempoCreateDelegate(UInt32 chan, UInt32 flags);
        private readonly BASS_FX_TempoCreateDelegate pBASS_FX_TempoCreate = lib.LoadFunction<BASS_FX_TempoCreateDelegate>("BASS_FX_TempoCreate");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_FX_TempoCreate(UInt32 chan, UInt32 flags) => pBASS_FX_TempoCreate(chan, flags);
    }
#pragma warning restore 1591
}

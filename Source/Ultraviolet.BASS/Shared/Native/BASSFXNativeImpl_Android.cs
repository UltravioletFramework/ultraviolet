using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class BASSFXNativeImpl_Android : BASSFXNativeImpl
    {
        [DllImport("bass_fx", EntryPoint = "BASS_FX_TempoCreate", CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 INTERNAL_BASS_FX_TempoCreate(UInt32 chan, UInt32 flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 BASS_FX_TempoCreate(UInt32 chan, UInt32 flags) => INTERNAL_BASS_FX_TempoCreate(chan, flags);
    }
#pragma warning restore 1591
}

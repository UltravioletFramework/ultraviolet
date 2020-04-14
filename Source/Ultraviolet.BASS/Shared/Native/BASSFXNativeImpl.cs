using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    public abstract unsafe class BASSFXNativeImpl
    {
        public abstract UInt32 BASS_FX_TempoCreate(UInt32 chan, UInt32 flags);
    }
#pragma warning restore 1591
}

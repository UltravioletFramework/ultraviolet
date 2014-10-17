using System;
using System.Runtime.InteropServices;
using System.Security;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.BASS.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal static class BASSFXNative
    {
        static BASSFXNative()
        {
            LibraryLoader.Load("bass_fx");
        }

        [DllImport("bass_fx", EntryPoint = "BASS_FX_GetVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 GetVersion();

        [DllImport("bass_fx", EntryPoint = "BASS_FX_TempoCreate", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 TempoCreate(UInt32 chan, UInt32 flags);
    }
}

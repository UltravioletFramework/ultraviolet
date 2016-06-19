using System;
using System.Runtime.InteropServices;
using System.Security;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.BASS.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal static class BASSFXNative
    {
#if IOS
        const String LibraryPath = "__Internal";
#else
        const String LibraryPath = "bass_fx";
#endif

        static BASSFXNative()
        {
            LibraryLoader.Load("bass_fx");
        }

        [DllImport(LibraryPath, EntryPoint = "BASS_FX_GetVersion")]
        public static extern UInt32 GetVersion();

        [DllImport(LibraryPath, EntryPoint = "BASS_FX_TempoCreate")]
        public static extern UInt32 TempoCreate(UInt32 chan, UInt32 flags);
    }
}

using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;

namespace Ultraviolet.BASS.Native
{
#pragma warning disable 1591
    public static unsafe partial class BASSFXNative
    {
        private static readonly BASSFXNativeImpl impl;
        
        static BASSFXNative()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Android:
                    impl = new BASSFXNativeImpl_Android();
                    break;
                    
                default:
                    impl = new BASSFXNativeImpl_Default();
                    break;
            }
        }
        
        public const UInt32 BASS_FX_FREESOURCE = 0x10000;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BASS_FX_TempoCreate(UInt32 chan, UInt32 flags) => impl.BASS_FX_TempoCreate(chan, flags);
    }
#pragma warning restore 1591
}

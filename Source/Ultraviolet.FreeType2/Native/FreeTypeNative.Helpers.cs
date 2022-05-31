using System;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    /// <summary>
    /// Contains FreeType2 helper methods.
    /// </summary>
    unsafe static partial class FreeTypeNative
    {
        public static Boolean Use64BitInterface => UltravioletPlatformInfo.CurrentPlatform != UltravioletPlatform.Windows && Environment.Is64BitProcess;
    }
#pragma warning restore 1591
}

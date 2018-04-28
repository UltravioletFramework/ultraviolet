#if !ANDROID && !IOS
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    internal static class NativeLibs
    {
        public static readonly NativeLibrary libpng = new NativeLibrary(
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "libpng16" : "libpng");
        public static readonly NativeLibrary libharfbuzz = new NativeLibrary(
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "harfbuzz" : "libharfbuzz");
        public static readonly NativeLibrary libfreetype = new NativeLibrary(
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "freetype" : "libfreetype");
    }
#pragma warning restore 1591
}
#endif

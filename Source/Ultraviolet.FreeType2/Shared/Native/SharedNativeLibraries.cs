using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    internal sealed class SharedNativeLibraries
    {
        public static readonly NativeLibrary libpng;
        public static readonly NativeLibrary libharfbuzz;
        public static readonly NativeLibrary libfreetype;
        
        static SharedNativeLibraries()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                default:
                    libpng = new NativeLibrary("libpng16");
                    break;
            }
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Linux:
                    libharfbuzz = new NativeLibrary("libharfbuzz");
                    break;
                case UltravioletPlatform.macOS:
                    libharfbuzz = new NativeLibrary("libharfbuzz");
                    break;
                default:
                    libharfbuzz = new NativeLibrary("harfbuzz");
                    break;
            }
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Linux:
                    libfreetype = new NativeLibrary("libfreetype");
                    break;
                case UltravioletPlatform.macOS:
                    libfreetype = new NativeLibrary("libfreetype");
                    break;
                default:
                    libfreetype = new NativeLibrary("freetype");
                    break;
            }
        }
    }
#pragma warning restore 1591
}

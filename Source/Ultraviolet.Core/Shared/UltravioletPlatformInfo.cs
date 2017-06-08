using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ultraviolet.Core
{
    /// <summary>
    /// Contains information relating to the current platform.
    /// </summary>
    public static class UltravioletPlatformInfo
    {
        /// <summary>
        /// Initializes the <see cref="UltravioletPlatformInfo"/> type.
        /// </summary>
        static UltravioletPlatformInfo()
        {
            CurrentPlatformMachineHardwareName = Environment.OSVersion.Platform.ToString();
            CurrentPlatformVersion = Environment.OSVersion.VersionString;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    CurrentPlatform = UltravioletPlatform.Windows;
                    break;

                case PlatformID.Unix:
                    {
                        var buf = IntPtr.Zero;
                        try
                        {
                            buf = Marshal.AllocHGlobal(8192);
                            if (UltravioletNative.uname(buf) == 0)
                            {
                                var os = Marshal.PtrToStringAnsi(buf);
                                if (String.Equals("Darwin", os, StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Assembly.GetEntryAssembly().GetReferencedAssemblies().Where(x => String.Equals("Xamarin.iOS", x.Name, StringComparison.OrdinalIgnoreCase)).Any())
                                    {
                                        CurrentPlatform = UltravioletPlatform.iOS;
                                    }
                                    else
                                    {
                                        CurrentPlatform = UltravioletPlatform.macOS;
                                    }
                                }
                                else
                                {
                                    if (Assembly.GetEntryAssembly().GetReferencedAssemblies().Where(x => String.Equals("Xamarin.Android", x.Name, StringComparison.OrdinalIgnoreCase)).Any())
                                    {
                                        CurrentPlatform = UltravioletPlatform.Android;
                                    }
                                    else
                                    {
                                        CurrentPlatform = UltravioletPlatform.Linux;
                                    }
                                }

                                CurrentPlatformMachineHardwareName = os;
                            }
                        }
                        finally
                        {
                            if (buf != IntPtr.Zero)
                                Marshal.FreeHGlobal(buf);
                        }
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether runtime code generation is supported on the current platform.
        /// </summary>
        /// <returns><see langword="true"/> if the current platform supports runtime code generation; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsRuntimeCodeGenerationSupported()
        {
            return IsRuntimeCodeGenerationSupported(CurrentPlatform);
        }

        /// <summary>
        /// Gets a value indicating whether runtime code generation is supported on the specified platform.
        /// </summary>
        /// <param name="platform">The platform to evaluate.</param>
        /// <returns><see langword="true"/> if the specified platform supports runtime code generation; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsRuntimeCodeGenerationSupported(UltravioletPlatform platform)
        {
            if (platform == UltravioletPlatform.iOS)
                return false;

            return true;
        }

        /// <summary>
        /// Gets an <see cref="UltravioletPlatform"/> value indicating which of Ultraviolet's supported
        /// platforms is currently executing this application.
        /// </summary>
        public static UltravioletPlatform CurrentPlatform { get; }

        /// <summary>
        /// Gets the string which contains the machine hardware name for the current platform.
        /// </summary>
        public static String CurrentPlatformMachineHardwareName { get; }

        /// <summary>
        /// Gets the string which contains the version information for the current platform.
        /// </summary>
        public static String CurrentPlatformVersion { get; }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Ultraviolet.Core;
using System.Reflection;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Contains methods for finding dependencies of our compiled assemblies.
    /// </summary>
    internal static class DependencyFinder
    {
        /// <summary>
        /// Gets a value indicating whether NuGet is available on the current platform.
        /// </summary>
        /// <returns><see langword="true"/> if NuGet is available; otherwise, <see langword="false"/>.</returns>
        public static Boolean IsNuGetAvailable()
        {
            return UltravioletPlatformInfo.CurrentRuntime != UltravioletRuntime.CoreCLR;
        }

        /// <summary>
        /// Downloads the NuGet executable into the current working directory,
        /// if it does not already exist there.
        /// </summary>
        /// <returns><see langword="true"/> if the package was downloaded; otherwise, <see langword="false"/>.</returns>
        public static Boolean DownloadNuGetExecutable()
        {
            if (!IsNuGetAvailable())
                throw new PlatformNotSupportedException();

            if (File.Exists("nuget.exe"))
                return false;

            Debug.Write("NuGet: Downloading client from nuget.org... ");

            var dir = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var exe = Path.Combine(dir, "nuget.exe");

            using (var client = new WebClient())
                client.DownloadFile("https://dist.nuget.org/win-x86-commandline/latest/nuget.exe", exe);

            Debug.WriteLine("done.");

            return true;
        }

        /// <summary>
        /// Installs the specified NuGet package into the current working directory.
        /// </summary>
        /// <param name="state">The compiler's current state.</param>
        /// <param name="packageID">The identifier of the package to install.</param>
        /// <param name="version">The version of the package to install, or <see langword="null"/> to install the newest version.</param>
        /// <returns><see langword="true"/> if the package was installed successfully; otherwise, <see langword="false"/>.</returns>
        public static Boolean InstallNuGetPackage(IExpressionCompilerState state, String packageID, String version = null)
        {
            Contract.Require(state, nameof(state));
            Contract.RequireNotEmpty(packageID, nameof(packageID));

            if (!IsNuGetAvailable())
                throw new PlatformNotSupportedException();

            Debug.Write("NuGet: Installing package " + packageID + "... ");

            var dir = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var exe = Path.Combine(dir, "nuget.exe");

            var args = String.IsNullOrEmpty(version) ? $"install {packageID}" : $"install {packageID} -Version {version}";
            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = exe,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    WorkingDirectory = state.GetWorkingDirectory()
                }
            };
            proc.Start();
            proc.WaitForExit();

            Debug.WriteLine("done.");

            return proc.ExitCode == 0;
        }

        /// <summary>
        /// Gets the path to the NuGet cache directory, if it exists.
        /// </summary>
        /// <returns>The path to the NuGet cache directory, or <see langword="null"/> if it doesn't exist.</returns>
        public static String GetNuGetCacheDirectory()
        {
            var home = UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ?
                Environment.GetEnvironmentVariable("USERPROFILE") :
                Environment.GetEnvironmentVariable("HOME");
            var dir = Path.Combine(home, ".nuget", "packages");

            return Directory.Exists(dir) ? dir : null;
        }
        
        /// <summary>
        /// Gets the path to the .NET Standard 2.0 reference assemblies contained within the 
        /// NuGet cache, if they exist there.
        /// </summary>
        /// <returns>The path to the reference assemblies, or <see langword="null"/> if they don't exist.</returns>
        public static String GetNetStandardLibraryDirFromNuGetCache()
        {
            var cache = GetNuGetCacheDirectory();
            if (cache == null)
                return null;

            var dir = Path.Combine(cache, "netstandard.library", "2.0.1", "build", "netstandard2.0", "ref");

            return Directory.Exists(dir) ? dir : null;
        }

        /// <summary>
        /// Gets the path to the .NET Standard 2.0 reference assemblies contained within the SDK
        /// fallback directory, if they exist there.
        /// </summary>
        /// <returns>The path to the reference assemblies, or <see langword="null"/> if they don't exist.</returns>
        public static String GetNetStandardLibraryDirFromFallback()
        {
            // On Windows, check for an installation of .NET Framework 4.7.1, which includes support
            // for the entirety of .NET Standard 2.0 as part of the base installation.
            if (UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows)
            {
                var netFramework471Dir = Path.Combine(Environment.GetEnvironmentVariable("PROGRAMFILES(X86)"), "Reference Assemblies", "Framework", ".NETFramework", "v4.7.1", "Facades");
                if (Directory.Exists(netFramework471Dir))
                    return netFramework471Dir;
            }

            // If the .NET Core SDK is installed, we can try the NuGetFallbackFolder, which is in a 
            // couple of different places depending on the current platform...
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Windows:
                    {
                        var fallbackDir = Path.Combine(Environment.GetEnvironmentVariable("PROGRAMFILES(X86)"), "dotnet", "sdk", "NuGetFallbackFolder", "microsoft.netcore.app", "2.0.0", "ref", "netcoreapp2.0");
                        if (Directory.Exists(fallbackDir))
                            return fallbackDir;
                    }
                    break;

                case UltravioletPlatform.Linux:
                case UltravioletPlatform.macOS:
                    {
                        var fallbackDirLocal = Path.Combine("usr", "local", "share", "dotnet", "sdk", "NuGetFallbackFolder", "microsoft.netcore.app", "2.0.0", "ref", "netcoreapp2.0");
                        if (Directory.Exists(fallbackDirLocal))
                            return fallbackDirLocal;

                        var fallbackDir = Path.Combine("usr", "share", "dotnet", "sdk", "NuGetFallbackFolder", "microsoft.netcore.app", "2.0.0", "ref", "netcoreapp2.0");
                        if (Directory.Exists(fallbackDir))
                            return fallbackDir;
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// Gets the path to the .NET Standard 2.0 reference assemblies contained within the
        /// current working directory, if they exist there.
        /// </summary>
        /// <param name="workingDir">The compiler's current working directory.</param>
        /// <returns>The path to the reference assemblies, or <see langword="null"/> if they don't exist.</returns>
        public static String GetNetStandardLibraryDirFromWorkingDir(String workingDir)
        {
            var dir = Path.Combine(workingDir ?? Directory.GetCurrentDirectory(), "NETStandard.Library.2.0.1", "build", "netstandard2.0", "ref");
            if (Directory.Exists(dir))
                return dir;

            return null;
        }
    }
}

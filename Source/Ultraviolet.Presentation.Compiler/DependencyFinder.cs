using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Ultraviolet.Core;

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

            var args = String.IsNullOrEmpty(version) ? $"install {packageID} -Source nuget.org" : $"install {packageID} -Source nuget.org -Version {version}";
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
            var exited = proc.WaitForExit((Int32)TimeSpan.FromMinutes(5).TotalSeconds);

            Debug.WriteLine("done.");

            return exited && proc.ExitCode == 0;
        }

        /// <summary>
        /// Gets the path to the NuGet cache directory, if it exists.
        /// </summary>
        /// <returns>The path to the NuGet cache directory, or <see langword="null"/> if it doesn't exist.</returns>
        public static String GetNuGetCacheDirectory()
        {
            var nugetPackagesDir = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
            if (Directory.Exists(nugetPackagesDir))
            {
                return nugetPackagesDir;
            }

            var home = UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ?
                Environment.GetEnvironmentVariable("USERPROFILE") :
                Environment.GetEnvironmentVariable("HOME");
            var dir = Path.Combine(home, ".nuget", "packages");

            return Directory.Exists(dir) ? dir : null;
        }
        
        /// <summary>
        /// Gets the path to the .NET Standard reference assemblies contained within the 
        /// NuGet cache, if they exist there.
        /// </summary>
        /// <returns>The path to the reference assemblies, or <see langword="null"/> if they don't exist.</returns>
        public static String GetNetStandardLibraryDirFromNuGetCache(IList<String> additionalPaths)
        {
            additionalPaths.Clear();

            return GetNetStandardLibraryDirFromNuGetCache_Standard21(additionalPaths);
        }

        /// <summary>
        /// Gets the path to the .NET Standard 2.1 assemblies.
        /// </summary>
        private static String GetNetStandardLibraryDirFromNuGetCache_Standard21(IList<String> additionalPaths)
        {
            var cache = GetNuGetCacheDirectory();
            if (cache == null)
                return null;

            var dir = new DirectoryInfo(Path.Combine(cache, "netstandard.library.ref"));
            if (dir.Exists)
            {
                var best = dir.EnumerateDirectories().Select(x => new { Directory = x, Target = new DirectoryInfo(Path.Combine(x.FullName, "ref", "netstandard2.1")), Version = TryParseVersion(x.Name) })
                    .Where(x => x.Version != null && x.Version >= new Version(2, 1, 0) && x.Target.Exists)
                    .OrderByDescending(x => x.Version)
                    .FirstOrDefault();
                if (best != null)
                {
                    return best.Target.FullName;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the path to the .NET Standard reference assemblies contained within the SDK
        /// fallback directory, if they exist there.
        /// </summary>
        /// <returns>The path to the reference assemblies, or <see langword="null"/> if they don't exist.</returns>
        public static String GetNetStandardLibraryDirFromFallback(IList<String> additionalPaths)
        {
            additionalPaths.Clear();

            return GetNetStandardLibraryDirFromFallback_Standard21(additionalPaths);
        }

        /// <summary>
        /// Gets the path to the .NET Standard 2.1 reference assemblies.
        /// </summary>
        private static String GetNetStandardLibraryDirFromFallback_Standard21(IList<String> additionalPaths)
        {
            // If the .NET Core SDK is installed, we can try the NETStandard.Library.Ref metapackage, which is in a 
            // couple of different places depending on the current platform...
            var refFolderDir = default(DirectoryInfo);
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Windows:
                    {
                        refFolderDir = new DirectoryInfo(Path.Combine(Environment.GetEnvironmentVariable("PROGRAMW6432"), "dotnet", "packs", "NETStandard.Library.Ref"));
                    }
                    break;

                case UltravioletPlatform.Linux:
                case UltravioletPlatform.macOS:
                    {
                        refFolderDir = new DirectoryInfo(Path.Combine("/", "usr", "local", "share", "dotnet", "packs", "NETStandard.Library.Ref"));
                        if (refFolderDir.Exists)
                            break;

                        refFolderDir = new DirectoryInfo(Path.Combine("/", "usr", "share", "dotnet", "packs", "NETStandard.Library.Ref"));
                    }
                    break;
            }

            if (refFolderDir.Exists)
            {
                var refFolderBest = refFolderDir.EnumerateDirectories().Select(x => new { Directory = x, Target = new DirectoryInfo(Path.Combine(x.FullName, "ref", "netstandard2.1")), Version = TryParseVersion(x.Name) })
                    .Where(x => x.Version != null && x.Version >= new Version(2, 1, 0) && x.Target.Exists)
                    .OrderByDescending(x => x.Version).FirstOrDefault();
                if (refFolderBest != null)
                {
                    return refFolderBest.Target.FullName;
                }
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
            var root = workingDir ?? Directory.GetCurrentDirectory();
            var dirRel = Path.Combine(root, "NETStandard.Library.2.0.1", "build", "netstandard2.0", "ref");
            var dirAbs = Path.GetFullPath(dirRel);

            if (Directory.Exists(dirAbs))
                return dirAbs;

            return null;
        }

        /// <summary>
        /// Attempts to parse a version string.,
        /// </summary>
        private static Version TryParseVersion(String str) => Version.TryParse(str, out var result) ? result : null;
    }
}

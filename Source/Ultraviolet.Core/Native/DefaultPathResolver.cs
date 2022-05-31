using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace Ultraviolet.Core.Native
{
    /// <summary>
    /// Represents the default algorithm for resolving load targets from a native library name.
    /// This implementation returns the following load targets:
    /// 1. The library contained in the application's base folder.
    /// 2. The library contained in the application's platform-specific dependency folder (i.e. x64/win32nt, x64/unix, etc.)
    /// 3. The simple name, unchanged.
    /// 4. The library as resolved via the default DependencyContext, in the default NuGet package cache folder.
    /// </summary>
    /// <remarks>This code is based on a prototype by Eric Mellinoe (https://github.com/mellinoe/nativelibraryloader/tree/master/NativeLibraryLoader).</remarks>
    public class DefaultPathResolver : PathResolver
    {
        private static readonly String[] KnownExtensions_Win = new[] { "dll" };
        private static readonly String[] KnownExtensions_Mac = new[] { "so", "dylib" };
        private static readonly String[] KnownExtensions_Unix = new[] { "so" };

        /// <inheritdoc/>
        public override IEnumerable<String> EnumeratePossibleLibraryLoadTargets(String name)
        {
            yield return Path.Combine(AppContext.BaseDirectory, name);
            if (TryLocateNativeAssetInPlatformFolder(name, out var platformResolvedPath))
            {
                yield return platformResolvedPath;
            }
            yield return name;
            if (TryLocateNativeAssetFromDeps(name, out var depsResolvedPath))
            {
                yield return depsResolvedPath;
            }
        }

        /// <summary>
        /// Determines whether the specified string is either an integer or one of the known library extensions for the current platform.
        /// </summary>
        private static Boolean IsNumberOrKnownExtension(String str)
        {
            if (Int32.TryParse(str, out _))
                return true;

            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Windows:
                    return KnownExtensions_Win.Contains(str, StringComparer.InvariantCultureIgnoreCase);

                case UltravioletPlatform.macOS:
                    return KnownExtensions_Mac.Contains(str, StringComparer.InvariantCultureIgnoreCase);

                default:
                    return KnownExtensions_Unix.Contains(str, StringComparer.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Attempts to locate a native asset with the given name in the application's platform-specific dependency folder.
        /// </summary>
        private Boolean TryLocateNativeAssetInPlatformFolder(String name, out String platformResolvedPath)
        {
            var dir = Path.Combine(AppContext.BaseDirectory);
            if (UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.macOS && UltravioletPlatformInfo.CurrentRuntime != UltravioletRuntime.CoreCLR)
                dir = Path.Combine("..", "Resources");

            dir = Path.Combine(dir, Environment.Is64BitProcess ? "x64" : "x86");

            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Windows:
                    dir = Path.Combine(dir, "win32nt");
                    break;
                    
                case UltravioletPlatform.Linux:
                    dir = Path.Combine(dir, "unix");
                    break;

                case UltravioletPlatform.macOS:
                    dir = Path.Combine(dir, "osx");
                    break;
            }

            var nameFile = Path.GetFileNameWithoutExtension(name);
            var nameExt = Path.GetExtension(name);

            foreach (var file in Directory.GetFiles(dir))
            {
                var fileName = Path.GetFileName(file);
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                if (fileName == name || fileNameWithoutExt == name)
                {
                    platformResolvedPath = Path.Combine(dir, fileName);
                    return true;
                }

                if (fileName.StartsWith(nameFile))
                {
                    var fileNameParts = fileName.Substring(nameFile.Length).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fileNameParts.All(x => IsNumberOrKnownExtension(x)))
                    {
                        platformResolvedPath = Path.Combine(dir, fileName);
                        return true;
                    }
                }
            }

            platformResolvedPath = null;
            return false;
        }

        /// <summary>
        /// Attempts to locate a native asset with the given name in the default NuGet package cache folder.
        /// </summary>
        private Boolean TryLocateNativeAssetFromDeps(String name, out String depsResolvedPath)
        {
            depsResolvedPath = null;

            var defaultContext = DependencyContext.Default;
            if (defaultContext == null)
                return false;

            var userHomeDirectory = UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ?
                Environment.GetEnvironmentVariable("USERPROFILE") : Environment.GetEnvironmentVariable("HOME");
            var nugetPackagesRootDir = Path.Combine(userHomeDirectory, ".nuget", "packages");

            var runtimeIdentifier = RuntimeEnvironment.GetRuntimeIdentifier();
            foreach (var runtimeLib in defaultContext.RuntimeLibraries)
            {
                foreach (var nativeAsset in runtimeLib.GetRuntimeNativeAssets(defaultContext, runtimeIdentifier))
                {
                    if (Path.GetFileName(nativeAsset) == name || Path.GetFileNameWithoutExtension(nativeAsset) == name)
                    {
                        var fullPath = Path.Combine(nugetPackagesRootDir, runtimeLib.Name.ToLowerInvariant(), runtimeLib.Version, nativeAsset);
                        fullPath = Path.GetFullPath(fullPath);
                        depsResolvedPath = fullPath;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

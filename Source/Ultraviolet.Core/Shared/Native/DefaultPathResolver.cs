using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// Attempts to locate a native asset with the given name in the application's platform-specific dependency folder.
        /// </summary>
        private Boolean TryLocateNativeAssetInPlatformFolder(String name, out String platformResolvedPath)
        {
            platformResolvedPath = Path.Combine(AppContext.BaseDirectory, Environment.Is64BitProcess ? "x64" : "x86");

            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Windows:
                    platformResolvedPath = Path.Combine(platformResolvedPath, "win32nt");
                    return true;

                case UltravioletPlatform.Android:
                case UltravioletPlatform.Linux:
                case UltravioletPlatform.macOS:
                    platformResolvedPath = Path.Combine(platformResolvedPath, "unix");
                    return true;
            }
            
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

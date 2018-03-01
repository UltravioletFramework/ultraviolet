using System;
using System.IO;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet
{
    /// <summary>
    /// Contains useful debugging methods.
    /// </summary>
    public static class UltravioletDebugUtil
    {
        /// <summary>
        /// Evaluates whether the specified path is beneath the specified root content directory, and if so, attempts to
        /// determine the path of the project file which produced it. 
        /// </summary>
        /// <param name="root">The root content directory.</param>
        /// <param name="path">The path for which to find the original content path.</param>
        /// <returns>The original content path, if it was found; otherwise, <paramref name="path"/>.</returns>
        public static String GetOriginalContentFilePath(String root, String path)
        {
            Contract.Require(root, nameof(root));
            Contract.RequireNotEmpty(path, nameof(path));

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
                return path;

            // Produce a URI which is relative to the directory that contains our application executable.
            var uriExe = GetDirectoryUri(AppContext.BaseDirectory);
            var uriRoot = new Uri(uriExe, root);
            var uriPathAbs = new Uri(Path.GetFullPath(path), UriKind.Absolute);

            if (!uriRoot.IsBaseOf(uriPathAbs))
                return path;            

            var uriPathRel = uriExe.MakeRelativeUri(uriPathAbs);

            // Get out of "Debug" (or "Release", etc.)...
            var dir = new DirectoryInfo(uriExe.LocalPath);
            if (dir.Parent == null)
                return path;

            // .NET Core 2.0 apps have an extra directory in the way
            if (String.Equals("netcoreapp2.0", dir.Name, StringComparison.Ordinal))
                dir = dir.Parent;

            dir = dir.Parent;

            // Get out of "bin"...
            if (dir.Parent == null)
                return path;

            dir = dir.Parent;

            // If we found a valid file, return its path, otherwise return the original path.
            var uriOriginalContentDir = GetDirectoryUri(dir.FullName);
            var uriOriginalContentFile = new Uri(uriOriginalContentDir, uriPathRel);
            return File.Exists(uriOriginalContentFile.LocalPath) ? uriOriginalContentFile.LocalPath : path;
        }

        /// <summary>
        /// Gets a value indicating whether the Ultraviolet core assembly was compiled in Debug mode.
        /// </summary>
        public static Boolean WasCompiledAsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Ultraviolet core assembly was compiled in Release mode.
        /// </summary>
        public static Boolean WasCompiledAsRelease
        {
            get
            {
#if !DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Gets a URI which represents the absolute specified directory path.
        /// </summary>
        private static Uri GetDirectoryUri(String path)
        {
            if (!path.EndsWith(DirectorySeparatorChar) && !path.EndsWith(AltDirectorySeparatorChar))
                path += DirectorySeparatorChar;

            return new Uri(path, UriKind.Absolute);
        }

        // Directory separator characters as strings.
        private static readonly String DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();
        private static readonly String AltDirectorySeparatorChar = Path.AltDirectorySeparatorChar.ToString();
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Contains methods for discovering content files.
    /// </summary>
    internal static class ContentDiscovery
    {
        /// <summary>
        /// Attempts to find the directory that contains the application solution file, if 
        /// we have reason to believe that we're running in debug mode.
        /// </summary>
        /// <returns>The solution directory, if it was found; otherwise, <see langword="null"/>.</returns>
        public static String FindSolutionDirectory(UltravioletContext uv, String searchRootDirectory)
        {
            Contract.Require(uv, nameof(uv));
            Contract.Require(searchRootDirectory, nameof(searchRootDirectory));

            if (solutionDirectory != null)
                return solutionDirectory == String.Empty ? null : solutionDirectory;

            if (uv.Platform == UltravioletPlatform.Android || uv.Platform == UltravioletPlatform.iOS)
            {
                solutionDirectory = String.Empty;
                return null;
            }

            var asm = Assembly.GetEntryAssembly();
            if (asm == null)
            {
                solutionDirectory = String.Empty;
                return null;
            }

            var asmDebuggableAttr = (DebuggableAttribute)asm.GetCustomAttributes(typeof(DebuggableAttribute), false).FirstOrDefault();
            if (asmDebuggableAttr == null || !asmDebuggableAttr.IsJITOptimizerDisabled)
            {
                solutionDirectory = String.Empty;
                return null;
            }

            var asmDir = new DirectoryInfo(Path.GetDirectoryName(AppContext.BaseDirectory));
            if (asmDir == null || !asmDir.Exists)
            {
                solutionDirectory = String.Empty;
                return null;
            }

            // Break out of the bin directory. There's a different number of steps depending on our platform.
            var depth =
                (uv.Runtime == UltravioletRuntime.CoreCLR) ? 3 :
                (uv.Platform == UltravioletPlatform.macOS) ? 5 : 2;
            for (int i = 0; i < depth; i++)
            {
                asmDir = asmDir.Parent;
                if (asmDir == null || !asmDir.Exists)
                {
                    solutionDirectory = String.Empty;
                    return null;
                }
            }

            // Is there a directory here with the same structure as our content root?
            var projectContentRoot = new DirectoryInfo(Path.Combine(asmDir.FullName, searchRootDirectory));
            if (projectContentRoot == null || !projectContentRoot.Exists)
            {
                // If we're on a Mac, check for a Desktop version of the project by
                // going up another level and looking for a "Desktop" directory.
                // If your app doesn't follow the Ultraviolet convention here, then
                // unfortunately you're out of luck.
                if (uv.Runtime == UltravioletRuntime.Mono && uv.Platform == UltravioletPlatform.macOS)
                {
                    asmDir = asmDir.Parent;
                    if (asmDir != null && asmDir.Exists)
                    {
                        var desktopRoot = new DirectoryInfo(Path.Combine(asmDir.FullName, "Desktop"));
                        if (desktopRoot != null && desktopRoot.Exists)
                        {
                            // Check again for the content root...
                            projectContentRoot = new DirectoryInfo(Path.Combine(desktopRoot.FullName, searchRootDirectory));
                            if (projectContentRoot != null && projectContentRoot.Exists)
                            {
                                solutionDirectory = projectContentRoot.FullName;
                                return solutionDirectory;
                            }
                        }
                    }
                }
                solutionDirectory = String.Empty;
                return null;
            }

            solutionDirectory = projectContentRoot.FullName;
            return solutionDirectory;
        }

        // Cached solution directory.
        private static String solutionDirectory;
    }
}

using System;
using System.Linq;

namespace Ultraviolet.Core.Native
{
    /// <summary>
    /// Contains methods for loading shared libraries on Unix-based platforms.
    /// </summary>
    internal abstract class UnixLibraryLoader : LibraryLoader
    {
        /// <summary>
        /// Adds the specified path to the LD_LIBRARY_PATH environment variable.
        /// </summary>
        /// <param name="path">The path to add.</param>
        protected void AddPathToLibraryPath(String path)
        {
            var existing = (Environment.GetEnvironmentVariable("LD_LIBRARY_PATH", EnvironmentVariableTarget.Process) ?? String.Empty)
                .Split(new [] { ':' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (existing.Contains(path))
                return;

            existing.Add(path);
            Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", String.Join(":", existing), EnvironmentVariableTarget.Process);
        }
    }
}

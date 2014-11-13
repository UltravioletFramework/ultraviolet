using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Content.Res;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Android.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="FileSystemService"/> class for the Android platform.
    /// </summary>
    public sealed class AndroidFileSystemService : FileSystemService
    {
        /// <inheritdoc/>
        public override IEnumerable<String> ListFiles(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            return
                (from a in Assets.List(path)
                 let fullpath = Path.Combine(path, a)
                 where IsFile(fullpath)
                 select fullpath).ToList();
        }

        /// <inheritdoc/>
        public override IEnumerable<String> ListDirectories(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            return
                (from a in Assets.List(path)
                 let fullpath = Path.Combine(path, a)
                 where IsDirectory(fullpath)
                 select fullpath).ToList();
        }

        /// <inheritdoc/>
        public override Stream OpenRead(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            return Assets.Open(path);
        }

        /// <summary>
        /// Gets the application's asset manager.
        /// </summary>
        public static AssetManager Assets
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset path represents a file.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns><c>true</c> if the specified path represents a file; otherwise, <c>false</c>.</returns>
        private Boolean IsFile(String path)
        {
            return Assets.List(path).Length == 0;
        }

        /// <summary>
        /// Gets a value indicating whether the specified asset path represents a directory.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns><c>true</c> if the specified path represents a directory; otherwise, <c>false</c>.</returns>
        private Boolean IsDirectory(String path)
        {
            return Assets.List(path).Length > 0;
        }
    }
}

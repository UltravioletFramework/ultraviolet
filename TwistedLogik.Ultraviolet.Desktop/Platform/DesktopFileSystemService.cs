using System;
using System.Collections.Generic;
using System.IO;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Desktop.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="FileSystemService"/> class for desktop platforms.
    /// </summary>
    public sealed class DesktopFileSystemService : FileSystemService
    {
        /// <inheritdoc/>
        public override Boolean FileExists(String path)
        {
            return File.Exists(path);
        }

        /// <inheritdoc/>
        public override Boolean DirectoryExists(String path)
        {
            return Directory.Exists(path);
        }

        /// <inheritdoc/>
        public override IEnumerable<String> ListFiles(String path, String searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        /// <inheritdoc/>
        public override IEnumerable<String> ListDirectories(String path, String searchPattern)
        {
            return Directory.GetDirectories(path, searchPattern);
        }

        /// <inheritdoc/>
        public override Stream OpenRead(String path)
        {
            return File.OpenRead(path);
        }
    }
}

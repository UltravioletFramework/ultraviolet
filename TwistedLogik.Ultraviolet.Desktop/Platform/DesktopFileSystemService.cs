using System;
using System.Collections.Generic;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.Desktop.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="FileSystemService"/> class for desktop platforms.
    /// </summary>
    public sealed class DesktopFileSystemService : FileSystemService
    {
        /// <inheritdoc/>
        public override IEnumerable<String> ListFiles(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            return Directory.GetFiles(path);
        }

        /// <inheritdoc/>
        public override IEnumerable<String> ListDirectories(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            return Directory.GetDirectories(path);
        }

        /// <inheritdoc/>
        public override Stream OpenRead(String path)
        {
            Contract.RequireNotEmpty(path, "path");

            return File.OpenRead(path);
        }
    }
}

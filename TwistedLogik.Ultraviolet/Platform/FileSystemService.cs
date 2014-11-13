using System;
using System.Collections.Generic;
using System.IO;

namespace TwistedLogik.Ultraviolet.Platform
{
    /// <summary>
    /// Represents a factory method which constructs instances of the <see cref="FileSystemService"/> class.
    /// </summary>
    /// <returns>The instance of <see cref="FileSystemService"/> that was created.</returns>
    public delegate FileSystemService FileSystemServiceFactory();

    /// <summary>
    /// Contains methods for interacting with the file system.
    /// </summary>
    public abstract class FileSystemService
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FileSystemService"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="FileSystemService"/> that was created.</returns>
        public static FileSystemService Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<FileSystemServiceFactory>()();
        }

        /// <summary>
        /// Lists the files at the specified path.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns>A list of directories at the specified path.</returns>
        public abstract IEnumerable<String> ListFiles(String path);

        /// <summary>
        /// Lists the directories at the specified path.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns>A list of directories at the specified path.</returns>
        public abstract IEnumerable<String> ListDirectories(String path);

        /// <summary>
        /// Opens the specified file for reading.
        /// </summary>
        /// <param name="path">The path of the file to open.</param>
        /// <returns>A <see cref="Stream"/> on the file at <paramref name="path"/>.</returns>
        public abstract Stream OpenRead(String path);
    }
}

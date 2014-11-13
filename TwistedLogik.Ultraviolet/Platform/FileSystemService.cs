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
        /// Gets a value indicating whether the specified path exists and is a file.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns><c>true</c> if the specified path exists and is a file; otherwise, <c>false</c>.</returns>
        public abstract Boolean FileExists(String path);

        /// <summary>
        /// Gets a value indicating whether the specified path exists and is a directory.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <returns><c>true</c> if the specified path exists and is a directory; otherwise, <c>false</c>.</returns>
        public abstract Boolean DirectoryExists(String path);

        /// <summary>
        /// Lists the files at the specified path.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>.</param>
        /// <returns>A list of directories at the specified path.</returns>
        public abstract IEnumerable<String> ListFiles(String path, String searchPattern = "*");

        /// <summary>
        /// Lists the directories at the specified path.
        /// </summary>
        /// <param name="path">The path to evaluate.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path"/>.</param>
        /// <returns>A list of directories at the specified path.</returns>
        public abstract IEnumerable<String> ListDirectories(String path, String searchPattern = "*");

        /// <summary>
        /// Opens the specified file for reading.
        /// </summary>
        /// <param name="path">The path of the file to open.</param>
        /// <returns>A <see cref="Stream"/> on the file at <paramref name="path"/>.</returns>
        public abstract Stream OpenRead(String path);
    }
}

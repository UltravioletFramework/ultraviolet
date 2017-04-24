using System;
using System.IO;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a source of file data.
    /// </summary>
    public abstract class FileSource
    {
        /// <summary>
        /// Finds the archive node with the specified path.
        /// </summary>
        /// <param name="path">The relative path of the node to find.</param>
        /// <param name="throwIfNotFound">A value indicating whether to throw a <see cref="FileNotFoundException"/> if the path does not exist.</param>
        /// <returns>The archive node with the specified path, or <see langword="null"/> if no such node exists and <paramref name="throwIfNotFound"/> is <see langword="false"/>.</returns>
        public abstract FileSourceNode Find(String path, Boolean throwIfNotFound = true);

        /// <summary>
        /// Extracts the specified file.
        /// </summary>
        /// <param name="path">The relative path of the file to load.</param>
        /// <returns>A <see cref="Stream"/> that represents the extracted data.</returns>
        public abstract Stream Extract(String path);
    }
}

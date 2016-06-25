using System;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents a collection of content override directories.
    /// </summary>
    public sealed class ContentOverrideDirectoryCollection : UltravioletCollection<String>
    {
        /// <summary>
        /// Clears the collection.
        /// </summary>
        [Preserve]
        public void Clear()
        {
            ClearInternal();
        }

        /// <summary>
        /// Adds a directory to the collection.
        /// </summary>
        /// <param name="directory">The directory to add to the collection.</param>
        [Preserve]
        public void Add(String directory)
        {
            AddInternal(directory);
        }

        /// <summary>
        /// Removes a directory from the collection.
        /// </summary>
        /// <param name="directory">The directory to remove from the collection.</param>
        /// <returns><see langword="true"/> if the directory was removed from the collection; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Remove(String directory)
        {
            return RemoveInternal(directory);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified directory.
        /// </summary>
        /// <param name="directory">The directory to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified directory; otherwise, <see langword="false"/>.</returns>
        [Preserve]
        public Boolean Contains(String directory)
        {
            return ContainsInternal(directory);
        }
    }
}

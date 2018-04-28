using System;
using System.Collections.Generic;
using System.IO;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a collection of content override directories.
    /// </summary>
    public sealed class ContentOverrideDirectoryCollection : UltravioletCollection<String>, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentOverrideDirectoryCollection"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="ContentManager"/> which owns the collection.</param>
        internal ContentOverrideDirectoryCollection(ContentManager owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Releases resources associated with the collection.
        /// </summary>
        public void Dispose()
        {
            if (watchers != null)
            {
                foreach (var watcher in watchers)
                    watcher.Value.Dispose();

                watchers.Clear();
            }
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        public void Clear()
        {
            ClearInternal();
        }

        /// <summary>
        /// Adds a directory to the collection.
        /// </summary>
        /// <param name="directory">The directory to add to the collection.</param>
        public void Add(String directory)
        {
            AddInternal(directory);
        }

        /// <summary>
        /// Removes a directory from the collection.
        /// </summary>
        /// <param name="directory">The directory to remove from the collection.</param>
        /// <returns><see langword="true"/> if the directory was removed from the collection; otherwise, <see langword="false"/>.</returns>
        public Boolean Remove(String directory)
        {
            return RemoveInternal(directory);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified directory.
        /// </summary>
        /// <param name="directory">The directory to evaluate.</param>
        /// <returns><see langword="true"/> if the collection contains the specified directory; otherwise, <see langword="false"/>.</returns>
        public Boolean Contains(String directory)
        {
            return ContainsInternal(directory);
        }

        /// <summary>
        /// Gets the <see cref="FileSystemWatcher"/> instance for the specified
        /// directory, if one has been created.
        /// </summary>
        /// <param name="directory">The directory for which to retrieve a file system watcher.</param>
        /// <returns>The <see cref="FileSystemWatcher"/> for the specified directory,
        /// or <see langword="null"/> if no watcher exists for the directory.</returns>
        internal FileSystemWatcher GetFileSystemWatcherForDirectory(String directory)
        {
            return watchers?[directory];
        }

        /// <summary>
        /// Creates file system watchers for all of the directories which are added to the collection,
        /// as well as any directories which are subsequently added to the collection.
        /// </summary>
        internal void CreateFileSystemWatchers()
        {
            if (watchers != null)
                return;

            watchers = new Dictionary<String, FileSystemWatcher>();
            CreateFileSystemWatchersForKnownDirectories();
        }

        /// <summary>
        /// Destroys any file system watchers which have been created by this collection.
        /// </summary>
        internal void DestroyFileSystemWatchers()
        {
            if (watchers != null)
            {
                foreach (var kvp in watchers)
                    kvp.Value.Dispose();

                watchers = null;
            }
        }

        /// <inheritdoc/>
        protected override void ClearInternal()
        {
            if (watchers != null)
            {
                foreach (var kvp in watchers)
                    kvp.Value.Dispose();

                watchers.Clear();
            }
            base.ClearInternal();
        }

        /// <inheritdoc/>
        protected override void AddInternal(String item)
        {
            if (IsWatchingFileSystem)
                CreateFileSystemWatcherForDirectory(item);

            base.AddInternal(item);
        }

        /// <inheritdoc/>
        protected override Boolean RemoveInternal(String item)
        {
            if (watchers != null)
            {
                FileSystemWatcher watcher;
                if (watchers.TryGetValue(item, out watcher))
                {
                    watcher.Dispose();
                    watchers.Remove(item);
                }
            }
            return base.RemoveInternal(item);
        }

        /// <summary>
        /// Creates a file system watcher for any known directory which doesn't
        /// already have a watcher.
        /// </summary>
        private void CreateFileSystemWatchersForKnownDirectories()
        {
            foreach (var directory in this)
            {
                if (!watchers.ContainsKey(directory))
                    CreateFileSystemWatcherForDirectory(directory);
            }
        }

        /// <summary>
        /// Creates a file system watcher for the specified directory.
        /// </summary>
        private void CreateFileSystemWatcherForDirectory(String directory)
        {
            var watcher = new FileSystemWatcher(directory);
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += owner.OnFileSystemChanged;

            watchers.Add(directory, watcher);
        }

        /// <summary>
        /// Gets a value indicating whether the collection is watching the file system
        /// for changes to the directories which it contains.
        /// </summary>
        private Boolean IsWatchingFileSystem => watchers != null;

        // State values.
        private readonly ContentManager owner;
        private Dictionary<String, FileSystemWatcher> watchers;
    }
}

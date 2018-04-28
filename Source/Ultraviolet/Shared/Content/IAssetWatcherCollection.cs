using System;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents the type-agnostic interface for an <see cref="AssetWatcher{T}"/> instance.
    /// </summary>
    internal interface IAssetWatcherCollection
    {
        /// <summary>
        /// Adds an asset watcher to the collection.
        /// </summary>
        /// <param name="watcher">The asset watcher to add to the collection.</param>
        void Add(IAssetWatcher watcher);

        /// <summary>
        /// Removes an asset watcher from the collection.
        /// </summary>
        /// <param name="watcher">The asset watcher to remove from the collection.</param>
        /// <returns><see langword="true"/> if the asset watcher was removed; otherwise, <see langword="false"/>.</returns>
        bool Remove(IAssetWatcher watcher);

        /// <summary>
        /// Gets the asset watcher at the specified index within the collection.
        /// </summary>
        /// <param name="index">The index of the asset watcher to retrieve.</param>
        /// <returns>The asset watcher at the specified index within the collection.</returns>
        IAssetWatcher this[Int32 index] { get; }

        /// <summary>
        /// Gets the content manager that owns the collection.
        /// </summary>
        ContentManager Owner { get; }

        /// <summary>
        /// Gets the normalized asset path of the asset which is being watched.
        /// </summary>
        String AssetPath { get; }

        /// <summary>
        /// Gets the asset file path of the asset which is being watched.
        /// </summary>
        String AssetFilePath { get; }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        Int32 Count { get; }
    }
}

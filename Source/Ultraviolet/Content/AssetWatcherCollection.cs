using System;
using System.Collections.Generic;
using Ultraviolet.Core;

namespace Ultraviolet.Content
{
    /// <summary>
    /// Represents a collection of <see cref="AssetWatcher{T}"/> instances for a particular asset.
    /// </summary>
    internal class AssetWatcherCollection<T> : IAssetWatcherCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetWatcherCollection{T}"/> class.
        /// </summary>
        /// <param name="owner">The content manager that owns the collection.</param>
        /// <param name="assetPath">The normalized asset path of the asset which is being watched.</param>
        /// <param name="assetFilePath">The asset file path of the asset which is being watched.</param>
        public AssetWatcherCollection(ContentManager owner, String assetPath, String assetFilePath)
        {
            Contract.Require(owner, nameof(owner));
            Contract.Require(assetPath, nameof(assetPath));
            Contract.Require(assetFilePath, nameof(assetFilePath));

            this.Owner = owner;
            this.AssetPath = assetPath;
            this.AssetFilePath = assetFilePath;
        }
        
        /// <inheritdoc/>
        void IAssetWatcherCollection.Add(IAssetWatcher watcher) => Add((AssetWatcher<T>)watcher);

        /// <inheritdoc/>
        bool IAssetWatcherCollection.Remove(IAssetWatcher watcher) => Remove((AssetWatcher<T>)watcher);

        /// <summary>
        /// Adds an asset watcher to the collection.
        /// </summary>
        /// <param name="watcher">The asset watcher to add to the collection.</param>
        public void Add(AssetWatcher<T> watcher)
        {
            Contract.Require(watcher, nameof(watcher));
            
            storage.Add(watcher);
        }

        /// <summary>
        /// Removes an asset watcher from the collection.
        /// </summary>
        /// <param name="watcher">The asset watcher to remove from the collection.</param>
        /// <returns><see langword="true"/> if the asset watcher was removed; otherwise, <see langword="false"/>.</returns>
        public bool Remove(AssetWatcher<T> watcher)
        {
            Contract.Require(watcher, nameof(watcher));

            return storage.Remove(watcher);
        }

        /// <inheritdoc/>
        public IAssetWatcher this[Int32 index] => storage[index];

        /// <inheritdoc/>
        public ContentManager Owner { get; }

        /// <inheritdoc/>
        public String AssetPath { get; }

        /// <inheritdoc/>
        public String AssetFilePath { get; }

        /// <inheritdoc/>
        public Int32 Count => storage.Count;

        // State values.
        private readonly List<AssetWatcher<T>> storage = new List<AssetWatcher<T>>();
    }
}
